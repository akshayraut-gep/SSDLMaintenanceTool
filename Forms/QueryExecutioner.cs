using OfficeOpenXml;
using Ookii.Dialogs.WinForms;
using SSDLMaintenanceTool.Constants;
using SSDLMaintenanceTool.Helpers;
using SSDLMaintenanceTool.Implementations;
using SSDLMaintenanceTool.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SSDLMaintenanceTool.Forms
{
    public partial class QueryExecutioner : Form
    {
        public List<EnvironmentSetting> Environments { get; set; }
        public DataSet DomainsTableSet { get; set; }
        public DAO DAO { get; set; }
        public ConnectionStringHandler _connectionStringHandler;
        public List<Domain> Domains { get; set; }
        public List<Domain> BackupDomains { get; set; }
        private readonly SynchronizationContext synchronizationContext;
        public delegate bool MethodInvokerWithDataSetResult();
        public delegate void QueryCompletion();
        public Dictionary<string, QueryCompletion> QueryCompleted { get; set; }
        public ConcurrentDictionary<string, DataSet> QueryResultDataSet { get; set; }
        public bool IsAllDomainsSelected { get; set; }

        public QueryExecutioner()
        {
            InitializeComponent();
            DAO = new DAO();
            _connectionStringHandler = new ConnectionStringHandler();
            synchronizationContext = SynchronizationContext.Current; //context from UI thread
        }

        private void QueryExecutioner_Load(object sender, EventArgs e)
        {
            this.connectionStringsComboBox.DataSource = ConnectionStringHandler.ConnectionStrings;
            this.connectionStringsComboBox.ValueMember = "Name";
            this.connectionStringsComboBox.DisplayMember = "DisplayName";
            this.connectionStringsComboBox.SelectedIndex = -1;
            this.connectionStringsComboBox.Text = "Select a connection";
            this.connectionStringsComboBox.SelectedIndexChanged += connectionStringsComboBox_SelectedIndexChanged;
            this.exportDomainsButton.Enabled = false;
            this.queryTextBox.ScrollBars = ScrollBars.Both;

            List<QueryTemplate> queryTemplates = new List<QueryTemplate>();
            queryTemplates.Add(new QueryTemplate { Name = "Select a template", Value = "" });
            queryTemplates.Add(new QueryTemplate
            {
                Name = "Publish Predefined Queries Migration",
                Value = GlobalConstants.PublishPredefinedQueriesMigration,
                QueryTemplateFilePath = Path.Combine(Environment.CurrentDirectory.Replace(@"bin\Debug", ""), @"QueryTemplates\Publish-Predefined-Migration-Part-01.sql")
            });

            this.savedTemplatesComboBox.Enabled = false;
            this.savedTemplatesComboBox.DataSource = queryTemplates;
            this.savedTemplatesComboBox.DisplayMember = "Name";
            this.savedTemplatesComboBox.ValueMember = "Value";
            this.savedTemplatesComboBox.SelectedIndex = 0;

            this.QueryCompleted = new Dictionary<string, QueryCompletion>();
            this.QueryCompleted.Add(GlobalConstants.PublishPredefinedQueriesMigration, PredefinedQueriesCompleted);
            this.QueryCompleted.Add(GlobalConstants.GeneralQueries, QueryExecutionCompleted);

            ConvertToDomainModel();
            PopulateDomainsCheckListBox();
            this.queryOutputTabControl.Width = this.Width - 20;
            this.queryOutputTabControl.Height = this.Height - this.queryOutputTabControl.Top - 50;
        }

        private void executeQueryButton_Click(object sender, EventArgs e)
        {
            var connectionDetails = GetSelectedConnectionDetails(out var valiationMessage);
            if (valiationMessage.HasContent())
            {
                MessageBox.Show(valiationMessage);
                return;
            }

            if (queryTextBox.Text == null || queryTextBox.Text.Trim() == "")
            {
                MessageBox.Show("Query Text is mandatory");
                return;
            }

            List<Domain> domains = new List<Domain>();

            if (connectionDetails.IsMultiTenant)
            {
                if (domainsCheckListBox.CheckedItems.Count == 0)
                {
                    MessageBox.Show("Select at least one SSDL domain");
                    return;
                }
                domains = ConvertToDomainDatabases();
            }
            EnableUIForQueryExecution(false);
            queryOutputTabControl.TabPages.Clear();

            if (asyncCheckBox.Checked)
            {
                StartQueryExecutionAsync(connectionDetails, domains);
            }
            else
            {
                StartQueryExecution(connectionDetails, domains);
            }
        }

        private void StartQueryExecutionAsync(ConnectionDetails connectionDetails, List<Domain> domains)
        {
            Task.Run(() =>
            {
                try
                {
                    //QueryResultDataSet = ExecuteQuery(connectionDetails, checkDomains, out var errorMessage);
                    ExecuteQueryAsync(connectionDetails, domains, out var errorMessage); ;

                    //Send the update to our UI thread
                    synchronizationContext.Post(new SendOrPostCallback(o =>
                    {
                        if (errorMessage.HasContent())
                        {
                            MessageBox.Show(errorMessage);
                            return;
                        }
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

        private void StartQueryExecution(ConnectionDetails connectionDetails, List<Domain> domains)
        {
            Task.Run(() =>
            {
                try
                {
                    QueryResultDataSet = ExecuteQuery(connectionDetails, domains, out var errorMessage);

                    //Send the update to our UI thread
                    synchronizationContext.Post(new SendOrPostCallback(o =>
                    {
                        EnableUIForQueryExecution();
                        if (errorMessage.HasContent())
                        {
                            MessageBox.Show(errorMessage);
                            return;
                        }
                        QueryExecutionCompleted();
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

        private void QueryFailed(Exception ex)
        {
            MessageBox.Show(ex.Message);
        }

        private void QueryExecutionCompleted()
        {
            if (this.useSavedTemplateCheckBox.Checked && this.savedTemplatesComboBox.SelectedIndex > 0 && this.savedTemplatesComboBox.SelectedValue.HasContent())
            {
                var selectedQueryTemplate = (this.savedTemplatesComboBox.SelectedItem as QueryTemplate);
                if (selectedQueryTemplate == null)
                {
                    MessageBox.Show("Template is not available");
                    return;
                }
                this.QueryCompleted[selectedQueryTemplate.Value]();
            }
            else
            {
                if (QueryResultDataSet == null || QueryResultDataSet.Count == 0)
                {
                    MessageBox.Show("No records found in any of the SSDL domains");
                    return;
                }
                if (canExportToExcelCheckBox.Checked)
                {
                    VistaFolderBrowserDialog vistaFolderBrowserDialog = new VistaFolderBrowserDialog();
                    vistaFolderBrowserDialog.Description = "Export Query Result - Select folder to export the file";
                    vistaFolderBrowserDialog.UseDescriptionForTitle = true;
                    var saveFileDialogResult = vistaFolderBrowserDialog.ShowDialog(this);
                    if (saveFileDialogResult == DialogResult.OK)
                    {
                        ExportDataSetCollectionToExcel(QueryResultDataSet, vistaFolderBrowserDialog.SelectedPath, GlobalConstants.QueryExecutionerQueryResultDirectory);
                    }
                }
                if (displayQueryOutputCheckBox.Checked)
                    DisplayOutputInTabs(QueryResultDataSet);
            }
        }

        private void QueryExecutionCompletedAsync(string databaseName, DataSet dataSet)
        {
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                if (canExportToExcelCheckBox.Checked)
                {
                    VistaFolderBrowserDialog vistaFolderBrowserDialog = new VistaFolderBrowserDialog();
                    vistaFolderBrowserDialog.Description = "Export Query Result - Select folder to export the file";
                    vistaFolderBrowserDialog.UseDescriptionForTitle = true;
                    var saveFileDialogResult = vistaFolderBrowserDialog.ShowDialog(this);
                    if (saveFileDialogResult == DialogResult.OK)
                    {
                        ExportDataSetToExcel(dataSet, vistaFolderBrowserDialog.SelectedPath, GlobalConstants.QueryExecutionerQueryResultDirectory);
                    }
                }
                if (displayQueryOutputCheckBox.Checked)
                    DisplayOutputInTabsAsync(databaseName, dataSet);
            }
        }

        private void DisplayOutputInTabs(ConcurrentDictionary<string, DataSet> dataSetCollection)
        {
            queryOutputTabControl.Height = this.Height - queryOutputTabControl.Top;
            foreach (var item in dataSetCollection)
            {
                queryOutputTabControl.TabPages.Add(item.Key, item.Key);
                if (item.Value.Tables.Count > 0)
                {
                    if (multiTabOutputCheckBox.Checked)
                    {
                        var thisTabPage = queryOutputTabControl.TabPages[item.Key];
                        var thisTabControl = new TabControl();
                        thisTabPage.Controls.Add(thisTabControl);

                        foreach (DataTable dataTable in item.Value.Tables)
                        {
                            thisTabControl.TabPages.Add(dataTable.TableName, dataTable.TableName);

                            var newTabPage = thisTabControl.TabPages[dataTable.TableName];

                            DataGridView dataGridView = new DataGridView();
                            dataGridView.DataSource = item.Value.Tables[0];

                            dataGridView.Height = thisTabPage.Height;
                            dataGridView.Width = thisTabPage.Width;

                            newTabPage.Controls.Add(dataGridView);
                            newTabPage.AutoScroll = true;
                        }
                        thisTabPage.AutoScroll = true;
                    }
                    else
                    {
                        var thisTabPage = queryOutputTabControl.TabPages[item.Key];
                        DataGridView dataGridView = new DataGridView();
                        dataGridView.DataSource = item.Value.Tables[0];
                        thisTabPage.Controls.Add(dataGridView);
                        dataGridView.Height = thisTabPage.Height;
                        dataGridView.Width = thisTabPage.Width;
                        thisTabPage.AutoScroll = true;
                    }
                }
            }
        }

        private void DisplayOutputInTabsAsync(string databaseName, DataSet dataSetWithKey)
        {
            queryOutputTabControl.Height = this.Height - queryOutputTabControl.Top;
            queryOutputTabControl.TabPages.Add(databaseName, databaseName);
            if (dataSetWithKey.Tables.Count > 0)
            {
                if (multiTabOutputCheckBox.Checked)
                {
                    var thisTabPage = queryOutputTabControl.TabPages[databaseName];
                    var thisTabControl = new TabControl();
                    thisTabPage.Controls.Add(thisTabControl);

                    foreach (DataTable dataTable in dataSetWithKey.Tables)
                    {
                        thisTabControl.TabPages.Add(dataTable.TableName, dataTable.TableName);

                        var newTabPage = thisTabControl.TabPages[dataTable.TableName];

                        DataGridView dataGridView = new DataGridView();
                        dataGridView.DataSource = dataSetWithKey.Tables[0];

                        dataGridView.Height = thisTabPage.Height;
                        dataGridView.Width = thisTabPage.Width;

                        newTabPage.Controls.Add(dataGridView);
                        newTabPage.AutoScroll = true;
                    }
                    thisTabPage.AutoScroll = true;
                }
                else
                {
                    var thisTabPage = queryOutputTabControl.TabPages[databaseName];
                    DataGridView dataGridView = new DataGridView();
                    dataGridView.DataSource = dataSetWithKey.Tables[0];
                    thisTabPage.Controls.Add(dataGridView);
                    dataGridView.Height = thisTabPage.Height;
                    dataGridView.Width = thisTabPage.Width;
                    thisTabPage.AutoScroll = true;
                }
            }
        }

        private void PredefinedQueriesCompleted()
        {
            if (QueryResultDataSet == null || QueryResultDataSet.Count == 0)
            {
                MessageBox.Show("No records found in any of the SSDL domains");
                return;
            }
            VistaFolderBrowserDialog vistaFolderBrowserDialog = new VistaFolderBrowserDialog();
            vistaFolderBrowserDialog.Description = "Export Query Result - Select folder to export the file";
            vistaFolderBrowserDialog.UseDescriptionForTitle = true;
            var saveFileDialogResult = vistaFolderBrowserDialog.ShowDialog(this);
            if (saveFileDialogResult == DialogResult.OK)
            {
                ExportPublishPredefinedQueries(QueryResultDataSet, vistaFolderBrowserDialog.SelectedPath, GlobalConstants.QueryExecutionerQueryResultDirectory);
            }
        }

        private List<Domain> ConvertToDomainDatabases()
        {
            List<Domain> checkedDomains = new List<Domain>();
            foreach (var checkedDomainItem in domainsCheckListBox.CheckedItems)
            {
                var checkedDomain = checkedDomainItem as Domain;
                if (checkedDomain != null && checkedDomain.Name != "SelectAll")
                    checkedDomains.Add(checkedDomain);
            }
            return checkedDomains;
        }

        public ConcurrentDictionary<string, DataSet> ExecuteQuery(ConnectionDetails connectionDetails, List<Domain> checkedDomains, out string errorMessage)
        {
            var domainDataSet = new ConcurrentDictionary<string, DataSet>();
            errorMessage = "";

            if (connectionDetails.IsMultiTenant)
            {
                var parallelOptions = new ParallelOptions();
                parallelOptions.MaxDegreeOfParallelism = 10;
                Parallel.ForEach(checkedDomains, parallelOptions, (checkedDomain) =>
                {
                    var copyConnection = _connectionStringHandler.GetDeepCopy(connectionDetails);
                    copyConnection.Database = checkedDomain.DatabaseName;
                    var resultSet = DAO.GetData(queryTextBox.Text, copyConnection);
                    if (resultSet != null && resultSet.Tables != null && resultSet.Tables.Count != 0 && resultSet.Tables[0] != null && resultSet.Tables[0].Rows.Count != 0)
                    {
                        resultSet.Tables[0].TableName = copyConnection.Database;
                        domainDataSet.TryAdd(copyConnection.Database, resultSet.Copy());
                    }
                });
            }
            else
            {
                var resultSet = DAO.GetData(queryTextBox.Text, connectionDetails);
                if (resultSet == null || resultSet.Tables == null || resultSet.Tables.Count == 0 || resultSet.Tables[0] == null || resultSet.Tables[0].Rows.Count == 0)
                {
                    errorMessage = "No data found";
                    return null;
                }
                domainDataSet.TryAdd(connectionDetails.Database, resultSet.Copy());
            }
            return domainDataSet;
        }

        public void ExecuteQueryAsync(ConnectionDetails connectionDetails, List<Domain> checkedDomains, out string errorMessage)
        {
            errorMessage = "";

            if (connectionDetails.IsMultiTenant)
            {
                var parallelOptions = new ParallelOptions();
                parallelOptions.MaxDegreeOfParallelism = 2;
                Parallel.ForEach(checkedDomains, parallelOptions, (checkedDomain) =>
                {
                    var copyConnection = _connectionStringHandler.GetDeepCopy(connectionDetails);
                    copyConnection.Database = checkedDomain.DatabaseName;
                    var resultSet = DAO.GetData(queryTextBox.Text, copyConnection);
                    if (resultSet != null && resultSet.Tables != null && resultSet.Tables.Count != 0 && resultSet.Tables[0] != null && resultSet.Tables[0].Rows.Count != 0)
                    {
                        resultSet.Tables[0].TableName = copyConnection.Database;

                        //Send the update to our UI thread
                        synchronizationContext.Post(new SendOrPostCallback(o =>
                        {
                            QueryExecutionCompletedAsync(copyConnection.Database, resultSet.Copy());
                        }), null);
                    }
                });
            }
            else
            {
                var resultSet = DAO.GetData(queryTextBox.Text, connectionDetails);
                if (resultSet == null || resultSet.Tables == null || resultSet.Tables.Count == 0 || resultSet.Tables[0] == null || resultSet.Tables[0].Rows.Count == 0)
                {
                    errorMessage = "No data found";
                    return;
                }
                //Send the update to our UI thread
                synchronizationContext.Post(new SendOrPostCallback(o =>
                {
                    QueryExecutionCompletedAsync(connectionDetails.Database, resultSet.Copy());
                }), null);
            }
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
            copyConnection.Database = "master";

            if (copyConnection.IsInputCredentialsRequired)
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

            EnableUIForDomainList(false);

            //Send the update to our UI thread
            Task.Run(() =>
            {
                try
                {
                    LoadSSDLDomains(copyConnection);
                    ConvertToDomainModel();
                    synchronizationContext.Post(new SendOrPostCallback(o =>
                    {
                        EnableUIForDomainList();
                        PopulateDomainsCheckListBox();
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

        private void LoadingDomainsFailed(Exception ex)
        {
            MessageBox.Show(ex.Message);
        }

        private void EnableUIForDomainList(bool value = true)
        {
            connectionStringsComboBox.Enabled = value;
            loadDomainsButton.Enabled = value;
            domainsCheckListBox.SelectionMode = value ? SelectionMode.One : SelectionMode.None;
            executeQueryButton.Enabled = value;
        }

        private void EnableUIForQueryExecution(bool value = true)
        {
            EnableUIForDomainList(value);
            queryTextBox.ReadOnly = !value;
        }

        private void LoadSSDLDomains(ConnectionDetails copyConnection)
        {
            var resultSet = DAO.GetData("SELECT Name FROM sys.databases WHERE Name LIKE '%[_]SSDL'", copyConnection);
            if (resultSet == null || resultSet.Tables == null || resultSet.Tables.Count == 0 || resultSet.Tables[0] == null || resultSet.Tables[0].Rows.Count == 0)
            {
                MessageBox.Show("No SSDL domains found");
                return;
            }

            DomainsTableSet = resultSet;
        }

        private void ConvertToDomainModel()
        {
            BackupDomains = new List<Domain>();
            BackupDomains.Insert(0, new Domain() { Name = "SelectAll", DisplayName = "Select all" });
            if (DomainsTableSet != null && DomainsTableSet.Tables.Count > 0 && DomainsTableSet.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in DomainsTableSet.Tables[0].Rows)
                {
                    Domain domain = new Domain();
                    domain.Name = row["Name"].ToString();
                    domain.DisplayName = row["Name"].ToString();
                    domain.DatabaseName = row["Name"].ToString();
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
            this.domainsCheckListBox.SelectedIndexChanged -= DomainsCheckListBox_SelectedIndexChanged;
            this.domainsCheckListBox.DataSource = Domains;
            this.domainsCheckListBox.ValueMember = "Name";
            this.domainsCheckListBox.DisplayMember = "DisplayName";
            this.exportDomainsButton.Enabled = true;
            this.domainsCheckListBox.SelectedIndexChanged += DomainsCheckListBox_SelectedIndexChanged;

            var checkedDomains = Domains.Where(a => a.IsChecked).ToList();
            if (checkedDomains.Count > 0)
            {
                foreach (var checkedDomain in checkedDomains)
                {
                    for (int i = 0; i < this.domainsCheckListBox.Items.Count; i++)
                    {
                        if ((this.domainsCheckListBox.Items[i] as Domain).Name == checkedDomain.Name)
                        {
                            this.domainsCheckListBox.SetItemChecked(i, true);
                        }
                    }
                }
            }
        }

        public ConnectionDetails GetSelectedConnectionDetails(out string validationMessage)
        {
            validationMessage = "";
            if (this.connectionStringsComboBox.SelectedValue == null)
            {
                validationMessage = "Select a connection";
                return null;
            }

            var connectionDetail = ConnectionStringHandler.ConnectionStrings.FirstOrDefault(a => a.Name == this.connectionStringsComboBox.SelectedValue.ToString());
            if (connectionDetail == null)
            {
                validationMessage = "Connection string not available";
                return null;
            }
            return connectionDetail;
        }

        private void DomainsCheckListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.domainsCheckListBox.Items != null && this.domainsCheckListBox.Items.Count > 0)
            {
                for (int j = 0; j < this.domainsCheckListBox.Items.Count; j++)
                {
                    var isDomainSelected = this.domainsCheckListBox.GetItemChecked(j);
                    var itemDomain = this.domainsCheckListBox.Items[j] as Domain;
                    if (itemDomain != null)
                    {
                        itemDomain.IsChecked = isDomainSelected;

                        var matchedBackupDomain = BackupDomains.FirstOrDefault(a => a.Name == itemDomain.Name);
                        if (matchedBackupDomain != null)
                            matchedBackupDomain.IsChecked = isDomainSelected;

                        if (itemDomain.Name == "SelectAll" && IsAllDomainsSelected != isDomainSelected)
                        {
                            for (int i = 0; i < this.domainsCheckListBox.Items.Count; i++)
                            {
                                this.domainsCheckListBox.SetItemChecked(i, isDomainSelected);
                            }
                            IsAllDomainsSelected = isDomainSelected;
                        }
                    }
                }
            }
        }

        private void connectionStringsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.domainsCheckListBox.DataSource = null;
        }

        private void exportDomainsButton_Click(object sender, EventArgs e)
        {
            VistaFolderBrowserDialog vistaFolderBrowserDialog = new VistaFolderBrowserDialog();
            vistaFolderBrowserDialog.Description = "Export Domains - Select folder to export the file(s)";
            vistaFolderBrowserDialog.UseDescriptionForTitle = true;
            var saveFileDialogResult = vistaFolderBrowserDialog.ShowDialog(this);

            if (saveFileDialogResult == DialogResult.OK)
            {
                var connectionDetails = GetSelectedConnectionDetails(out var validationMessage);
                ExportDataTableToExcel(DomainsTableSet.Tables[0], vistaFolderBrowserDialog.SelectedPath, GlobalConstants.QueryExecutionerDomainExportDirectory, connectionDetails.Environment + "-" + connectionDetails.Region + "-" + connectionDetails.Instance);
            }
        }

        void ExportDataTableToExcel(DataTable dataTable, string usersSaveDirectoryPath, string rootDirectoryName, string fileName = null)
        {
            var resolvedDirectoryPath = usersSaveDirectoryPath + @"\" + GlobalConstants.QueryExecutionerDomainExportDirectory + @"\";
            Directory.CreateDirectory(resolvedDirectoryPath);

            var resolvedFilePath = resolvedDirectoryPath + (fileName ?? dataTable.TableName) + ".xlsx";

            using (ExcelPackage pck = new ExcelPackage(resolvedFilePath))
            {
                try
                {
                    if (pck.Workbook.Worksheets.Count > 0)
                    {
                        for (int i = 0; i < pck.Workbook.Worksheets.Count; i++)
                        {
                            pck.Workbook.Worksheets.Delete(i);
                        }
                    }
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add(dataTable.TableName);
                    ws.Cells["A1"].LoadFromDataTable(dataTable, true);
                    var totalColumns = ws.Columns.Count();
                    //ws.InsertColumn(totalColumns, 1);
                    var lastColumn = ws.Dimension.End.Column;
                    ws.Cells[1, lastColumn + 1].Value = "New Name";

                    ws.Cells[2, lastColumn + 1].Value = "Akshay";
                    var testAdd = ws.Cells[2, lastColumn + 1];
                    ws.Cells[3, lastColumn + 1].Formula = "=" + ws.Cells[2, lastColumn + 1].Address;
                }
                catch (Exception ex)
                {

                }
                pck.Save();
            }
            var successDialogResult = MessageBox.Show("File has been exported.\nClick on Yes to open the file location.", "Success", MessageBoxButtons.YesNo);
            if (successDialogResult == DialogResult.Yes)
                Process.Start("explorer.exe", resolvedDirectoryPath);
        }

        void ExportDataSetToExcel(DataSet dataSet, string usersSaveDirectoryPath, string rootDirectoryName)
        {
            var resolvedDirectoryPath = usersSaveDirectoryPath + @"\" + rootDirectoryName + @"\";
            Directory.CreateDirectory(resolvedDirectoryPath);

            foreach (DataTable dataTable in dataSet.Tables)
            {
                var resolvedFilePath = resolvedDirectoryPath + dataTable.TableName + ".xlsx";

                using (ExcelPackage pck = new ExcelPackage(resolvedFilePath))
                {
                    try
                    {
                        if (pck.Workbook.Worksheets.Count > 0)
                        {
                            for (int i = 0; i < pck.Workbook.Worksheets.Count; i++)
                            {
                                pck.Workbook.Worksheets.Delete(i);
                            }
                        }
                        ExcelWorksheet ws = pck.Workbook.Worksheets.Add(dataTable.TableName);
                        ws.Cells["A1"].LoadFromDataTable(dataTable, true);
                    }
                    catch (Exception ex)
                    {

                    }
                    pck.Save();
                }
            }
            var successDialogResult = MessageBox.Show("File(s) have been exported.\nClick on Yes to open the file location.", "Success", MessageBoxButtons.YesNo);
            if (successDialogResult == DialogResult.Yes)
                Process.Start("explorer.exe", resolvedDirectoryPath);
        }

        void ExportDataSetCollectionToExcel(ConcurrentDictionary<string, DataSet> dataSetCollection, string usersSaveDirectoryPath, string rootDirectoryName)
        {
            var resolvedDirectoryPath = usersSaveDirectoryPath + @"\" + rootDirectoryName + @"\";
            Directory.CreateDirectory(resolvedDirectoryPath);

            foreach (var item in dataSetCollection)
            {
                var resolvedFilePath = resolvedDirectoryPath + item.Key + ".xlsx";
                using (ExcelPackage pck = new ExcelPackage(resolvedFilePath))
                {
                    if (pck.Workbook.Worksheets.Count > 0)
                    {
                        for (int i = 0; i < pck.Workbook.Worksheets.Count; i++)
                        {
                            pck.Workbook.Worksheets.Delete(i);
                        }
                    }
                    foreach (DataTable dataTable in item.Value.Tables)
                    {
                        try
                        {
                            ExcelWorksheet ws = pck.Workbook.Worksheets.Add(dataTable.TableName);
                            ws.Cells["A1"].LoadFromDataTable(dataTable, true);
                        }
                        catch (Exception ex)
                        {

                        }
                        pck.Save();
                    }
                }
            }
            var successDialogResult = MessageBox.Show("File(s) have been exported.\nClick on Yes to open the file location.", "Success", MessageBoxButtons.YesNo);
            if (successDialogResult == DialogResult.Yes)
                Process.Start("explorer.exe", resolvedDirectoryPath);
        }

        void ExportPublishPredefinedQueries(ConcurrentDictionary<string, DataSet> dataSetCollection, string usersSaveDirectoryPath, string rootDirectoryName)
        {
            var resolvedDirectoryPath = usersSaveDirectoryPath + @"\" + rootDirectoryName + @"\";
            Directory.CreateDirectory(resolvedDirectoryPath);

            foreach (var item in dataSetCollection)
            {
                var resolvedFilePath = resolvedDirectoryPath + item.Key + ".xlsx";
                using (ExcelPackage pck = new ExcelPackage(resolvedFilePath))
                {
                    if (pck.Workbook.Worksheets.Count > 0)
                    {
                        for (int i = 0; i < pck.Workbook.Worksheets.Count; i++)
                        {
                            pck.Workbook.Worksheets.Delete(i);
                        }
                    }
                    foreach (DataTable dataTable in item.Value.Tables)
                    {
                        try
                        {
                            ExcelWorksheet worksheet = pck.Workbook.Worksheets.Add(dataTable.TableName);
                            worksheet.Cells["B1"].LoadFromDataTable(dataTable, true);

                            var childJobQueries = dataTable.Select("ParentJobId IS NOT NULL");

                            if (childJobQueries != null && childJobQueries.Length > 0)
                            {
                                int rowStart = worksheet.Dimension.Start.Row;
                                int rowEnd = worksheet.Dimension.End.Row;

                                int columnStart = worksheet.Dimension.Start.Column;
                                int columnEnd = worksheet.Dimension.End.Column;

                                var dataExcelRange = worksheet.Cells[rowStart, columnStart, rowEnd, columnEnd];

                                var jobIdColumn = from cell in dataExcelRange //you can define your own range of cells for lookup
                                                  where cell.Value != null && cell.Value.ToString() == "JobId"
                                                  select cell.Start.Column;

                                var jobIdColumnNumber = jobIdColumn.FirstOrDefault();

                                var queryNameColumn = from cell in dataExcelRange //you can define your own range of cells for lookup
                                                      where cell.Value != null && cell.Value.ToString() == "QueryName"
                                                      select cell.Start.Column;

                                var queryNameColumnNumber = queryNameColumn.FirstOrDefault();

                                worksheet.Cells["A" + 1].Value = "New Query Name";

                                foreach (DataRow childQueryRow in childJobQueries)
                                {
                                    var childJobId = childQueryRow["JobId"].ToString();
                                    var parentJobId = childQueryRow["ParentJobId"].ToString();
                                    var queryName = childQueryRow["QueryName"].ToString();

                                    for (int parentQueryIterator = 2; parentQueryIterator <= rowEnd; parentQueryIterator++)
                                    {
                                        var parentJobIdCell = worksheet.Cells[parentQueryIterator, jobIdColumnNumber];
                                        var parentQueryNameCell = worksheet.Cells[parentQueryIterator, queryNameColumnNumber];
                                        if (parentJobIdCell != null && parentJobIdCell.Value != null && parentJobIdCell.Value.ToString() == parentJobId && parentQueryNameCell != null && parentQueryNameCell.Value != null && parentQueryNameCell.Value.ToString() == queryName)
                                        {
                                            for (int childQueryIterator = 2; childQueryIterator <= rowEnd; childQueryIterator++)
                                            {
                                                var childJobIdCell = worksheet.Cells[childQueryIterator, jobIdColumnNumber];
                                                var childQueryNameCell = worksheet.Cells[childQueryIterator, queryNameColumnNumber];

                                                if (childJobIdCell != null && childJobIdCell.Value != null && childJobIdCell.Value.ToString() == childJobId && childQueryNameCell != null && childQueryNameCell.Value != null && childQueryNameCell.Value.ToString() == queryName)
                                                {
                                                    worksheet.Cells["A" + childJobIdCell.Start.Row].Formula = "=" + worksheet.Cells["A" + parentJobIdCell.Start.Row].Address;
                                                    break;
                                                }
                                            }
                                        }
                                    }

                                    //worksheet.Cells[2, columnEnd + 1].Value = "Akshay";
                                    //var testAdd = worksheet.Cells[2, columnEnd + 1];
                                    //worksheet.Cells[3, columnEnd + 1].Formula = "=" + worksheet.Cells[2, columnEnd + 1].Address;
                                }
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                        pck.Save();
                    }
                }
            }
            var successDialogResult = MessageBox.Show("File(s) have been exported.\nClick on Yes to open the file location.", "Success", MessageBoxButtons.YesNo);
            if (successDialogResult == DialogResult.Yes)
                Process.Start("explorer.exe", resolvedDirectoryPath);
        }

        private void exportQueryResultButton_Click(object sender, EventArgs e)
        {

        }

        private void useSavedTemplateCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.savedTemplatesComboBox.Enabled = this.useSavedTemplateCheckBox.Checked;
            this.queryTextBox.ReadOnly = this.useSavedTemplateCheckBox.Checked;
        }

        private void savedTemplatesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.useSavedTemplateCheckBox.Checked && this.savedTemplatesComboBox.SelectedIndex > 0 && this.savedTemplatesComboBox.SelectedValue.HasContent())
            {
                var selectedQueryTemplate = (this.savedTemplatesComboBox.SelectedItem as QueryTemplate);
                if (selectedQueryTemplate == null)
                {
                    MessageBox.Show("Template is not available");
                    return;
                }
                queryTextBox.Text = File.ReadAllText(selectedQueryTemplate.QueryTemplateFilePath);
            }
        }

        private void filterDomainsTextBox_TextChanged(object sender, EventArgs e)
        {
            var filterText = filterDomainsTextBox.Text.ToLower();
            var filteredDomains = new List<Domain>();
            if (filterDomainsTextBox.Text.HasContent())
            {
                filteredDomains = BackupDomains.FindAll(a => a.DisplayName.ToLower().Contains(filterText) && a.Name != "SelectAll")
                ;
                filteredDomains.Insert(0, new Domain() { Name = "SelectAll", DisplayName = "Select all" });
            }
            else
                filteredDomains = BackupDomains.FindAll(a => a.DisplayName.ToLower().Contains(filterText));

            Domains = filteredDomains.Select(a => new Domain()
            {
                Name = a.Name,
                DatabaseName = a.DatabaseName,
                DisplayName = a.DisplayName,
                IsChecked = a.IsChecked
            }).ToList(); ;
            PopulateDomainsCheckListBox();
        }

        private void QueryExecutioner_SizeChanged(object sender, EventArgs e)
        {
            this.queryOutputTabControl.Width = this.Width - 20;
            this.queryOutputTabControl.Height = this.Height - this.queryOutputTabControl.Top - 50;
        }
    }
}