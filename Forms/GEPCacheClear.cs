using SSDLMaintenanceTool.Helpers;
using SSDLMaintenanceTool.Implementations;
using SSDLMaintenanceTool.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SSDLMaintenanceTool.Forms
{
    public partial class GEPCacheClear : Form
    {
        public DataTable DomainsTableSet { get; set; }
        public List<Domain> BackupDomains { get; set; }
        public List<Domain> Domains { get; set; }
        public DAO dao;
        public ConnectionStringHandler _connectionStringHandler;
        private readonly SynchronizationContext synchronizationContext;
        public bool IsAllDomainsSelected { get; set; }
        public int SuccessDomainsCount { get; set; }
        public int FailureDomainsCount { get; set; }

        public GEPCacheClear()
        {
            InitializeComponent();
            dao = new DAO();
            _connectionStringHandler = new ConnectionStringHandler(@"Assets\cache-clear-connection-strings.json");
            synchronizationContext = SynchronizationContext.Current; //context from UI thread
        }

        private void GEPCacheClear_Load(object sender, EventArgs e)
        {
            environmentComboBox.DataSource = ConnectionStringHandler.ConnectionStrings;
            environmentComboBox.ValueMember = "Name";
            environmentComboBox.DisplayMember = "DisplayName";
            environmentComboBox.SelectedIndex = -1;
            environmentComboBox.Text = "Select an environment";

            modeComboBox.Items.Add(new NameValueModel { Name = "Single", Value = "Single" });
            modeComboBox.Items.Add(new NameValueModel { Name = "Bulk", Value = "Bulk" });
            modeComboBox.DisplayMember = "Name";
            modeComboBox.ValueMember = "Value";
            modeComboBox.SelectedIndex = 0;

            domainFilterOptionsComboBox.Items.Add(new NameValueModel { Name = "SSDL Domains", Value = "SSDLDomains" });
            domainFilterOptionsComboBox.Items.Add(new NameValueModel { Name = "All Domains", Value = "AllDomains" });
            domainFilterOptionsComboBox.DisplayMember = "Name";
            domainFilterOptionsComboBox.ValueMember = "Value";
            domainFilterOptionsComboBox.SelectedIndex = 0;
        }

        private void LoadSSDLDomains(ConnectionDetails copyConnection, string domainNameFilter = "")
        {
            if (!domainNameFilter.HasContent() || domainNameFilter == "SSDLDomains")
            {
                foreach (var connectionStringByRegion in copyConnection.ConnectionStringByRegions)
                {
                    var resultSet = dao.GetData($"SELECT PartnerCode FROM SPEND_PROJECT_MST", connectionStringByRegion);
                    if (resultSet == null || resultSet.Tables == null || resultSet.Tables.Count == 0 || resultSet.Tables[0] == null || resultSet.Tables[0].Rows.Count == 0)
                    {
                        domainsCheckListBox.DataSource = null;
                        MessageBox.Show("No SSDL domains found");
                        return;
                    }
                    if (DomainsTableSet == null)
                        DomainsTableSet = resultSet.Tables[0];
                    DomainsTableSet.Merge(resultSet.Tables[0]);
                }
            }
            else
            {

            }
        }

        private void ConvertToDomainModel()
        {
            BackupDomains = new List<Domain>();
            BackupDomains.Insert(0, new Domain() { Name = "SelectAll", DisplayName = "Select all" });
            if (DomainsTableSet != null && DomainsTableSet.Rows.Count > 0)
            {
                foreach (DataRow row in DomainsTableSet.Rows)
                {
                    Domain domain = new Domain();
                    domain.Name = row["PartnerCode"].ToString();
                    domain.DisplayName = row["PartnerCode"].ToString();
                    domain.DatabaseName = row["PartnerCode"].ToString();
                    BackupDomains.Add(domain);
                }
            }
            Domains = BackupDomains.Select(a => new Domain()
            {
                Name = a.Name,
                DatabaseName = a.DatabaseName,
                DisplayName = a.DisplayName,
                IsChecked = a.IsChecked
            }).ToList();
        }

        private void PopulateDomainsCheckListBox()
        {
            domainsCheckListBox.SelectedIndexChanged -= DomainsCheckListBox_SelectedIndexChanged;
            domainsCheckListBox.DataSource = Domains;
            domainsCheckListBox.ValueMember = "Name";
            domainsCheckListBox.DisplayMember = "DisplayName";
            domainsCheckListBox.SelectedIndexChanged += DomainsCheckListBox_SelectedIndexChanged;

            var checkedDomains = Domains.Where(a => a.IsChecked).ToList();
            if (checkedDomains.Count > 0)
            {
                foreach (var checkedDomain in checkedDomains)
                {
                    for (int i = 0; i < domainsCheckListBox.Items.Count; i++)
                    {
                        if ((domainsCheckListBox.Items[i] as Domain).Name == checkedDomain.Name)
                        {
                            domainsCheckListBox.SetItemChecked(i, true);
                        }
                    }
                }
            }
        }

        private void DomainsCheckListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (domainsCheckListBox.Items != null && domainsCheckListBox.Items.Count > 0)
            {
                for (int j = 0; j < domainsCheckListBox.Items.Count; j++)
                {
                    var isDomainSelected = domainsCheckListBox.GetItemChecked(j);
                    var itemDomain = domainsCheckListBox.Items[j] as Domain;
                    if (itemDomain != null)
                    {
                        itemDomain.IsChecked = isDomainSelected;

                        var matchedBackupDomain = BackupDomains.FirstOrDefault(a => a.Name == itemDomain.Name);
                        if (matchedBackupDomain != null)
                            matchedBackupDomain.IsChecked = isDomainSelected;

                        if (itemDomain.Name == "SelectAll" && IsAllDomainsSelected != isDomainSelected)
                        {
                            for (int i = 0; i < domainsCheckListBox.Items.Count; i++)
                            {
                                domainsCheckListBox.SetItemChecked(i, isDomainSelected);
                            }
                            IsAllDomainsSelected = isDomainSelected;
                        }
                    }
                }
            }
        }

        private void environmentComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            domainsCheckListBox.DataSource = null;
            IsAllDomainsSelected = false;
        }

        private void modeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedMode = (modeComboBox.SelectedItem as NameValueModel).Value;
            if (selectedMode == "Single")
            {
                singleDomainBPCTextBox.Enabled = true;
                loadDomainsButton.Enabled = false;
                filterDomainsTextBox.ReadOnly = true;
                domainsCheckListBox.Enabled = false;
            }
            else if (selectedMode == "Bulk")
            {
                singleDomainBPCTextBox.Enabled = false;
                loadDomainsButton.Enabled = true;
                filterDomainsTextBox.ReadOnly = false;
                domainsCheckListBox.Enabled = true;
            }
        }

        private async void clearCacheButton_Click(object sender, EventArgs e)
        {
            var listOfBPCs = new List<string>();
            var selectedMode = (modeComboBox.SelectedItem as NameValueModel).Value;

            var selectedEnvironment = GetSelectedEnvironmentName(out var valiationMessage);
            if (valiationMessage.HasContent())
            {
                MessageBox.Show(valiationMessage);
                return;
            }

            if (selectedMode == "Single")
            {
                if (!singleDomainBPCTextBox.Text.HasContent())
                {
                    MessageBox.Show("Buyer Partner Code is empty in the text box");
                    return;
                }
                listOfBPCs.Add(singleDomainBPCTextBox.Text);
            }
            else if (selectedMode == "Bulk")
            {
                if (domainsCheckListBox.CheckedItems.Count == 0)
                {
                    MessageBox.Show("Select at least one domain");
                    return;
                }
                listOfBPCs = ConvertToDomainBPCs();
            }
            StartClearingCache(selectedEnvironment.Name, listOfBPCs, asyncCheckBox.Checked, parallelismDegreeNumericUpDown.Value);
        }

        public ConnectionDetails GetSelectedEnvironmentName(out string validationMessage)
        {
            validationMessage = "";
            if (environmentComboBox.SelectedValue == null)
            {
                validationMessage = "Select a domain";
                return null;
            }

            var connectionDetail = ConnectionStringHandler.ConnectionStrings.FirstOrDefault(a => a.Name == environmentComboBox.SelectedValue.ToString());
            if (connectionDetail == null)
            {
                validationMessage = "BPC is not available";
                return null;
            }
            return connectionDetail;
        }

        private void StartClearingCache(string environment, List<string> listOfBPCs, bool isAsync, decimal parallelismDegree)
        {
            Task.Run(() =>
            {
                try
                {
                    ClearCacheForBPCs(environment, listOfBPCs, parallelismDegree);
                    //Send the update to our UI thread
                    synchronizationContext.Post(new SendOrPostCallback(o =>
                    {
                        EnableUIForQueryExecution();
                        ClearingCacheCompleted();
                    }), null);
                }
                catch (Exception ex)
                {
                    //Send the update to our UI thread
                    synchronizationContext.Post(new SendOrPostCallback(o =>
                    {
                        QueryFailed(ex);
                    }), null);
                }
                finally
                {
                    //Send the update to our UI thread
                    synchronizationContext.Post(new SendOrPostCallback(o =>
                    {
                        EnableUIForQueryExecution();
                    }), null);
                }
            });
        }

        private void ClearCacheForBPCs(string environment, List<string> listOfBPCs, decimal parallelismDegree)
        {
            var parallelOptions = new ParallelOptions();
            parallelOptions.MaxDegreeOfParallelism = ((int)parallelismDegree) == 0 ? 10 : ((int)parallelismDegree);
            Parallel.For(0, listOfBPCs.Count, parallelOptions, (async (BPC) =>
             {
                 try
                 {
                     var formDataDictionary = new Dictionary<string, string>();
                     formDataDictionary.Add("environment", environment);
                     formDataDictionary.Add("bpc", listOfBPCs[BPC]);
                     formDataDictionary.Add("cacheKey", "");

                     var httpClient = new HttpClient();

                     var multipartFormDataContent = new FormUrlEncodedContent(formDataDictionary);

                     var response = await httpClient.PostAsync("https://devops.gep.com/CacheClear/Cache", multipartFormDataContent);
                     if (response != null)
                     {
                         if (response.StatusCode == HttpStatusCode.OK)
                         {
                             //Send the update to our UI thread
                             synchronizationContext.Post(new SendOrPostCallback(o =>
                              {
                                  SuccessDomainsCount++;
                                  var percentage = ((double)SuccessDomainsCount / (double)listOfBPCs.Count) * 100;
                                  queryProgressBarToolStrip.Value = (int)percentage;
                              }), null);
                         }
                     }
                     else
                     {
                         //Send the update to our UI thread
                         synchronizationContext.Post(new SendOrPostCallback(o =>
                          {
                              FailureDomainsCount++;
                          }), null);
                     }
                 }
                 catch (Exception ex)
                 {
                     //Send the update to our UI thread
                     synchronizationContext.Post(new SendOrPostCallback(o =>
                      {
                          FailureDomainsCount++;
                      }), null);
                 }
             }));
        }

        private void QueryFailed(Exception ex)
        {
            queryStatusLabelToolStrip.Text = "Failed";
            loadDomainStatusLabelToolStrip.Text = "Ready";
            MessageBox.Show(ex.Message);
        }

        private void EnableUIForQueryExecution(bool value = true)
        {
            EnableUIForDomainList(value);
        }

        private void loadDomainsButton_Click(object sender, EventArgs e)
        {
            var connectionDetails = GetSelectedConnectionDetails(out var valiationMessage);
            if (valiationMessage.HasContent())
            {
                MessageBox.Show(valiationMessage);
                return;
            }

            var copyConnection = _connectionStringHandler.GetDeepCopy(connectionDetails);

            if (copyConnection.IsInputCredentialsRequired && !copyConnection.IsMFA)
            {
                var credentialsPrompt = new CredentialsPrompt();
                credentialsPrompt.ShowDialog(this);
                if (!credentialsPrompt.UserName.HasContent() || !credentialsPrompt.Password.HasContent())
                {
                    MessageBox.Show("Credentials are required");
                    return;
                }
                copyConnection.UserName = credentialsPrompt.UserName;
                copyConnection.Password = credentialsPrompt.Password;
            }

            var domainFilterBeforeSearch = (domainFilterOptionsComboBox.SelectedItem as NameValueModel).Value;

            EnableUIForDomainList(false);
            loadDomainsProgressBarToolStrip.Value = 0;
            loadDomainStatusLabelToolStrip.Text = "Running";
            queryProgressBarToolStrip.Value = 0;
            queryStatusLabelToolStrip.Text = "Cache Clearing is blocked - till domains are being loaded";

            //Send the update to our UI thread
            Task.Run(() =>
            {
                try
                {
                    LoadSSDLDomains(copyConnection, domainFilterBeforeSearch);
                    ConvertToDomainModel();
                    synchronizationContext.Post(new SendOrPostCallback(o =>
                    {
                        EnableUIForDomainList();
                        PopulateDomainsCheckListBox();
                        loadDomainsProgressBarToolStrip.Value = 100;
                        loadDomainStatusLabelToolStrip.Text = "Completed";
                        queryStatusLabelToolStrip.Text = "Ready";
                    }), null);
                }
                catch (Exception ex)
                {
                    synchronizationContext.Post(new SendOrPostCallback(o =>
                    {
                        LoadingDomainsFailed(ex);
                    }), null);
                }
                finally
                {
                    synchronizationContext.Post(new SendOrPostCallback(o =>
                    {
                        EnableUIForDomainList();
                    }), null);
                }
            });
        }

        private void EnableUIForDomainList(bool value = true)
        {
            environmentComboBox.Enabled = value;
            loadDomainsButton.Enabled = value;
            domainsCheckListBox.SelectionMode = value ? SelectionMode.One : SelectionMode.None;
            clearCacheButton.Enabled = value;
        }

        private void LoadingDomainsFailed(Exception ex)
        {
            MessageBox.Show(ex.Message);
        }

        private List<string> ConvertToDomainBPCs()
        {
            List<string> checkedDomains = new List<string>();
            foreach (var checkedDomainItem in domainsCheckListBox.CheckedItems)
            {
                var checkedDomain = checkedDomainItem as Domain;
                if (checkedDomain != null && checkedDomain.Name != "SelectAll")
                    checkedDomains.Add(checkedDomain.BuyerPartnerCode);
            }
            return checkedDomains;
        }

        private void ClearingCacheCompleted()
        {
            queryStatusLabelToolStrip.Text = "Completed";
            loadDomainStatusLabelToolStrip.Text = "Ready";
            successDomainsToolStrip.Text = SuccessDomainsCount + " successful";
            failureDomainsToolStrip.Text = FailureDomainsCount + " failed";
        }

        public ConnectionDetails GetSelectedConnectionDetails(out string validationMessage)
        {
            validationMessage = "";
            if (environmentComboBox.SelectedValue == null)
            {
                validationMessage = "Select a connection";
                return null;
            }

            var connectionDetail = ConnectionStringHandler.ConnectionStrings.FirstOrDefault(a => a.Name == environmentComboBox.SelectedValue.ToString());
            if (connectionDetail == null)
            {
                validationMessage = "Connection string not available";
                return null;
            }
            return connectionDetail;
        }
    }
}
