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
        public int SuccessDomainsCount { get; set; }
        public int FailureDomainsCount { get; set; }
        public List<TabPage> OutputTabPages { get; set; }
        public List<string> DomainsWithResults { get; set; }

        public QueryExecutioner()
        {
            InitializeComponent();
            DAO = new DAO();
            _connectionStringHandler = new ConnectionStringHandler();
            synchronizationContext = SynchronizationContext.Current; //context from UI thread
        }

        private void QueryExecutioner_Load(object sender, EventArgs e)
        {
            connectionStringsComboBox.DataSource = ConnectionStringHandler.ConnectionStrings;
            connectionStringsComboBox.ValueMember = "Name";
            connectionStringsComboBox.DisplayMember = "DisplayName";
            connectionStringsComboBox.SelectedIndex = -1;
            connectionStringsComboBox.Text = "Select a connection";
            connectionStringsComboBox.SelectedIndexChanged += connectionStringsComboBox_SelectedIndexChanged;
            exportDomainsButton.Enabled = false;
            queryRichTextBox.ScrollBars = RichTextBoxScrollBars.Both;

            List<QueryTemplate> queryTemplates = new List<QueryTemplate>();
            queryTemplates.Add(new QueryTemplate { Name = "Select a template", Value = "" });
            queryTemplates.Add(new QueryTemplate
            {
                Name = "Publish Predefined Queries Migration",
                Value = GlobalConstants.PublishPredefinedQueriesMigration,
                QueryTemplateFilePath = Path.Combine(Environment.CurrentDirectory.Replace(@"bin\Debug", ""), @"QueryTemplates\Publish-Predefined-Migration-Part-01.sql")
            });

            savedTemplatesComboBox.Enabled = false;
            savedTemplatesComboBox.DataSource = queryTemplates;
            savedTemplatesComboBox.DisplayMember = "Name";
            savedTemplatesComboBox.ValueMember = "Value";
            savedTemplatesComboBox.SelectedIndex = 0;

            displayOptionsComboBox.Items.Add(new NameValueModel { Name = "Single result single tab", Value = "SingleResultSingleTab" });
            displayOptionsComboBox.Items.Add(new NameValueModel { Name = "Multi result single tab", Value = "MultiResultSingleTab" });
            displayOptionsComboBox.Items.Add(new NameValueModel { Name = "Only list domains with affected records", Value = "OnlyListDomainsWithAffectedRecords" });
            displayOptionsComboBox.Items.Add(new NameValueModel { Name = "Only list domains with data", Value = "OnlyListDomainsWithData" });
            displayOptionsComboBox.DisplayMember = "Name";
            displayOptionsComboBox.ValueMember = "Value";
            displayOptionsComboBox.SelectedIndex = 0;

            exportOptionsComboBox.Items.Add(new NameValueModel { Name = "No Export", Value = "NoExport" });
            exportOptionsComboBox.Items.Add(new NameValueModel { Name = "Export one file per domain", Value = "ExportOneFilePerDomain" });
            exportOptionsComboBox.Items.Add(new NameValueModel { Name = "Export all domains in one file", Value = "ExportAllDomainsInOneFile" });
            exportOptionsComboBox.DisplayMember = "Name";
            exportOptionsComboBox.ValueMember = "Value";
            exportOptionsComboBox.SelectedIndex = 0;

            QueryCompleted = new Dictionary<string, QueryCompletion>();
            QueryCompleted.Add(GlobalConstants.PublishPredefinedQueriesMigration, PredefinedQueriesCompleted);
            QueryCompleted.Add(GlobalConstants.GeneralQueries, QueryExecutionCompleted);

            ConvertToDomainModel();
            PopulateDomainsCheckListBox();
            queryOutputTabControl.Height = 200;

            queryProgressBarToolStrip.Minimum = 0;
            queryProgressBarToolStrip.Maximum = 100;
            successDomainsToolStrip.Text = "";
            failureDomainsToolStrip.Text = "";

            loadDomainsProgressBarToolStrip.Minimum = 0;
            loadDomainsProgressBarToolStrip.Maximum = 100;

            DomainsWithResults = new List<string>();
            mainPanel.AutoScroll = true;

            mainPanel.Width = this.Width - 30;
            mainPanel.Height = this.Height - 70;

            if (mainPanel.VerticalScroll.Visible)
                queryOutputTabControl.Width = Width - 55;
            else
                queryOutputTabControl.Width = Width - 40;

            parallelismDegreeNumericUpDown.Minimum = 1;
            parallelismDegreeNumericUpDown.Maximum = 20;
            parallelismDegreeNumericUpDown.Value = 1;
        }

        private void executeQueryButton_Click(object sender, EventArgs e)
        {
            var connectionDetails = GetSelectedConnectionDetails(out var valiationMessage);
            if (valiationMessage.HasContent())
            {
                MessageBox.Show(valiationMessage);
                return;
            }

            if (queryRichTextBox.Text == null || queryRichTextBox.Text.Trim() == "")
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

            var selectedExportOption = (exportOptionsComboBox.SelectedItem as NameValueModel);
            if (selectedExportOption != null && (selectedExportOption.Value == "ExportAllDomainsInOneFile"))
            {
                exportFileNameInputDialog.WindowTitle = "Save as XLSX";
                exportFileNameInputDialog.MainInstruction = "Enter name of file being exported";
                var dialogResult = exportFileNameInputDialog.ShowDialog(this);
                if (dialogResult != DialogResult.OK)
                {
                    return;
                }
            }

            EnableUIForQueryExecution(false);
            queryOutputTabControl.TabPages.Clear();
            queryProgressBarToolStrip.Value = 0;
            queryStatusLabelToolStrip.Text = "Running";
            loadDomainsProgressBarToolStrip.Value = 0;
            loadDomainStatusLabelToolStrip.Text = "Loading domains is blocked - till query is being executed";
            SuccessDomainsCount = 0;
            FailureDomainsCount = 0;
            OutputTabPages = new List<TabPage>();
            DomainsWithResults = new List<string>();
            successDomainsToolStrip.Text = "";
            failureDomainsToolStrip.Text = "";

            var selectedDisplayOption = displayOptionsComboBox.SelectedItem as NameValueModel;

            StartExecution(connectionDetails, domains, queryRichTextBox.Text, asyncCheckBox.Checked, selectedDisplayOption.Value);
        }

        private void StartExecution(ConnectionDetails connectionDetails, List<Domain> domains, string query, bool isAsync, string displayOption)
        {
            Task.Run(() =>
            {
                try
                {
                    if (isAsync)
                    {
                        StartQueryExecutionAsync(connectionDetails, domains, query, displayOption);
                    }
                    else
                    {
                        StartQueryExecution(connectionDetails, domains, query, displayOption);
                    }
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

        private void StartQueryExecutionAsync(ConnectionDetails connectionDetails, List<Domain> domains, string query, string displayOption)
        {
            QueryResultDataSet = ExecuteQueryAsync(connectionDetails, domains, query, displayOption, out var errorMessage);

            //Send the update to our UI thread
            synchronizationContext.Post(new SendOrPostCallback(o =>
            {
                if (errorMessage.HasContent())
                {
                    MessageBox.Show(errorMessage);
                    return;
                }

                var selectedExportOption = (exportOptionsComboBox.SelectedItem as NameValueModel);
                if (selectedExportOption != null && (selectedExportOption.Value == "ExportOneFilePerDomain" || selectedExportOption.Value == "ExportAllDomainsInOneFile"))
                {
                    var queryResultsList = QueryResultDataSet.Select(a => a).ToList();
                    queryResultsList.Sort((s1, s2) => s1.Key.CompareTo(s2.Key));

                    VistaFolderBrowserDialog vistaFolderBrowserDialog = new VistaFolderBrowserDialog();
                    vistaFolderBrowserDialog.Description = "Export Query Result - Select folder to export the file";
                    vistaFolderBrowserDialog.UseDescriptionForTitle = true;
                    var saveFileDialogResult = vistaFolderBrowserDialog.ShowDialog(this);
                    if (saveFileDialogResult == DialogResult.OK)
                    {
                        ExportDataSetCollectionToExcel(queryResultsList, vistaFolderBrowserDialog.SelectedPath, GlobalConstants.QueryExecutionerQueryResultDirectory, selectedExportOption.Value, exportFileNameInputDialog.Input);
                    }
                }
            }), null);
        }

        private void StartQueryExecution(ConnectionDetails connectionDetails, List<Domain> domains, string query, string displayOption)
        {
            QueryResultDataSet = ExecuteQuery(connectionDetails, domains, query, displayOption, out var errorMessage);

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

        private void QueryFailed(Exception ex)
        {
            queryStatusLabelToolStrip.Text = "Failed";
            loadDomainStatusLabelToolStrip.Text = "Ready";
            MessageBox.Show(ex.Message);
        }

        private void QueryExecutionCompleted()
        {
            queryStatusLabelToolStrip.Text = "Completed";
            loadDomainStatusLabelToolStrip.Text = "Ready";
            successDomainsToolStrip.Text = SuccessDomainsCount + " successful";
            failureDomainsToolStrip.Text = FailureDomainsCount + " failed";

            var selectedDisplayOption = displayOptionsComboBox.SelectedItem as NameValueModel;
            if (selectedDisplayOption.Value == "OnlyListDomainsWithData")
            {
                DataSet dataSet = new DataSet();
                dataSet.Tables.Add("DomainsWithResults");
                dataSet.Tables[0].Columns.Add("Domain", typeof(string));
                foreach (var domain in DomainsWithResults)
                {
                    var dataRow = dataSet.Tables[0].NewRow();
                    dataRow["Domain"] = domain;
                    dataSet.Tables[0].Rows.Add(dataRow);
                }
                DisplayOutputInTabsAsync("Domains with results", dataSet);
            }
            else
            {
                if (useSavedTemplateCheckBox.Checked && savedTemplatesComboBox.SelectedIndex > 0 && savedTemplatesComboBox.SelectedValue.HasContent())
                {
                    var selectedQueryTemplate = (savedTemplatesComboBox.SelectedItem as QueryTemplate);
                    if (selectedQueryTemplate == null)
                    {
                        MessageBox.Show("Template is not available");
                        return;
                    }
                    QueryCompleted[selectedQueryTemplate.Value]();
                }
                else
                {
                    if (selectedDisplayOption.Value == "SingleResultSingleTab" || selectedDisplayOption.Value == "MultiResultSingleTab")
                    {
                        if (QueryResultDataSet == null || QueryResultDataSet.Count == 0)
                        {
                            MessageBox.Show("No records found in any of the SSDL domains");
                            return;
                        }
                        DisplayOutputInTabs(QueryResultDataSet);
                    }

                    var selectedExportOption = (exportOptionsComboBox.SelectedItem as NameValueModel);
                    if (selectedExportOption != null && (selectedExportOption.Value == "ExportOneFilePerDomain" || selectedExportOption.Value == "ExportAllDomainsInOneFile"))
                    {
                        VistaFolderBrowserDialog vistaFolderBrowserDialog = new VistaFolderBrowserDialog();
                        vistaFolderBrowserDialog.Description = "Export Query Result - Select folder to export the file";
                        vistaFolderBrowserDialog.UseDescriptionForTitle = true;
                        var saveFileDialogResult = vistaFolderBrowserDialog.ShowDialog(this);
                        if (saveFileDialogResult == DialogResult.OK)
                        {
                            var queryResultsList = QueryResultDataSet.Select(a => a).ToList();
                            queryResultsList.Sort((s1, s2) => s1.Key.CompareTo(s2.Key));

                            ExportDataSetCollectionToExcel(queryResultsList, vistaFolderBrowserDialog.SelectedPath, GlobalConstants.QueryExecutionerQueryResultDirectory, selectedExportOption.Value, exportFileNameInputDialog.Input);
                        }
                    }
                }
            }
        }

        private void QueryExecutionCompletedAsync(string databaseName, DataSet dataSet, string displayOption)
        {
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                if (displayOption == "OnlyListDomainsWithData")
                {
                    if (dataSet.Tables[0].Rows.Count > 0)
                        DomainsWithResults.Add(databaseName);
                }
                else
                {
                    if (displayOption == "SingleResultSingleTab")
                        if (dataSet.Tables[0].Rows.Count > 0)
                            DisplayOutputInTabsAsync(databaseName, dataSet);
                }
            }
        }

        private void DisplayOutputInTabs(ConcurrentDictionary<string, DataSet> dataSetCollection)
        {
            foreach (var item in dataSetCollection)
            {
                TabPage tabPage = new TabPage(item.Key);
                tabPage.SizeChanged += DynamicallyCreatedTabPage_SizeChanged;

                OutputTabPages.Add(tabPage);

                if (item.Value.Tables.Count > 0)
                {
                    var selectedDisplayOption = displayOptionsComboBox.SelectedItem as NameValueModel;
                    if (selectedDisplayOption.Value == "MultiResultSingleTab")
                    {
                        var thisTabControl = new TabControl();
                        tabPage.Controls.Add(thisTabControl);
                        thisTabControl.SizeChanged += DynamicallyCreatedTabControl_SizeChanged;

                        var innerTabPages = new List<TabPage>();

                        foreach (DataTable dataTable in item.Value.Tables)
                        {
                            var innerTabPage = new TabPage(dataTable.TableName);
                            innerTabPages.Add(innerTabPage);

                            DataGridView dataGridView = new DataGridView();
                            dataGridView.DataSource = dataTable;

                            dataGridView.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.ColumnHeader);
                            dataGridView.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCells);

                            innerTabPage.Controls.Add(dataGridView);
                            innerTabPage.AutoScroll = true;
                            innerTabPage.SizeChanged += DynamicallyCreatedTabPage_SizeChanged;
                        }

                        innerTabPages.Sort((s1, s2) => s1.Text.CompareTo(s2.Text));

                        thisTabControl.TabPages.AddRange(innerTabPages.ToArray());
                        tabPage.AutoScroll = true;
                    }
                    else
                    {
                        DataGridView dataGridView = new DataGridView();
                        dataGridView.DataSource = item.Value.Tables[0];
                        tabPage.Controls.Add(dataGridView);
                        dataGridView.Height = tabPage.Height - 50;
                        dataGridView.Width = tabPage.Width - 50;
                        tabPage.AutoScroll = true;
                    }
                }

                tabPage.Height = queryOutputTabControl.Height;
                tabPage.Width = queryOutputTabControl.Width;
            }

            OutputTabPages.Sort((s1, s2) => s1.Text.CompareTo(s2.Text));

            queryOutputTabControl.TabPages.AddRange(OutputTabPages.ToArray());
        }

        private void DynamicallyCreatedTabControl_SizeChanged(object sender, EventArgs e)
        {
            TabControl dynamicTabControl = (sender as TabControl);
            foreach (TabPage tabPage in dynamicTabControl.TabPages)
            {
                tabPage.Height = dynamicTabControl.Height - 20;
                tabPage.Width = dynamicTabControl.Width - 20;
            }
        }

        private void DynamicallyCreatedTabPage_SizeChanged(object sender, EventArgs e)
        {
            TabPage dynamicTabPage = (sender as TabPage);
            if (dynamicTabPage.Controls.Count > 0)
            {
                var innerFirstControl = dynamicTabPage.Controls[0];
                if (innerFirstControl is DataGridView)
                {
                    var dataGridView = innerFirstControl as DataGridView;
                    if (dataGridView != null)
                    {
                        dataGridView.Height = dynamicTabPage.Height - 20;
                        dataGridView.Width = dynamicTabPage.Width - 20;
                    }
                }
                else if (innerFirstControl is TabControl)
                {
                    var tabControl = innerFirstControl as TabControl;
                    if (tabControl != null)
                    {
                        tabControl.Height = dynamicTabPage.Height - 20;
                        tabControl.Width = dynamicTabPage.Width - 20;
                    }
                }
            }
        }

        private void DisplayOutputInTabsAsync(string databaseName, DataSet dataSetWithKey)
        {
            queryOutputTabControl.TabPages.Add(databaseName, databaseName);
            var tabPage = queryOutputTabControl.TabPages[databaseName];
            tabPage.SizeChanged += DynamicallyCreatedTabPage_SizeChanged;

            if (dataSetWithKey.Tables.Count > 0)
            {
                var selectedDisplayOption = displayOptionsComboBox.SelectedItem as NameValueModel;
                if (selectedDisplayOption.Value == "MultiResultSingleTab")
                {
                    var thisTabControl = new TabControl();
                    tabPage.Controls.Add(thisTabControl);

                    foreach (DataTable dataTable in dataSetWithKey.Tables)
                    {
                        thisTabControl.TabPages.Add(dataTable.TableName, dataTable.TableName);

                        var newTabPage = thisTabControl.TabPages[dataTable.TableName];

                        DataGridView dataGridView = new DataGridView();
                        dataGridView.DataSource = dataTable;

                        dataGridView.Height = tabPage.Height - 20;
                        dataGridView.Width = tabPage.Width - 20;

                        dataGridView.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.ColumnHeader);
                        dataGridView.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCells);

                        newTabPage.Controls.Add(dataGridView);
                        newTabPage.AutoScroll = true;
                    }
                    tabPage.AutoScroll = true;
                }
                else
                {
                    DataGridView dataGridView = new DataGridView();
                    dataGridView.DataSource = dataSetWithKey.Tables[0];
                    tabPage.Controls.Add(dataGridView);
                    dataGridView.Height = tabPage.Height - 20;
                    dataGridView.Width = tabPage.Width - 20;
                    tabPage.AutoScroll = true;
                }
            }
        }

        private void PredefinedQueriesCompleted()
        {
            var selectedDisplayOption = displayOptionsComboBox.SelectedItem as NameValueModel;
            if (selectedDisplayOption.Value == "SingleResultSingleTab")
            {
                if (QueryResultDataSet == null || QueryResultDataSet.Count == 0)
                {
                    MessageBox.Show("No records found in any of the SSDL domains");
                    return;
                }
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

        public ConcurrentDictionary<string, DataSet> ExecuteQuery(ConnectionDetails connectionDetails, List<Domain> domains, string query, string displayOption, out string errorMessage)
        {
            var domainDataSets = new ConcurrentDictionary<string, DataSet>();
            errorMessage = "";

            if (connectionDetails.IsMultiTenant)
            {
                var parallelOptions = new ParallelOptions();
                parallelOptions.MaxDegreeOfParallelism = ((int)parallelismDegreeNumericUpDown.Value) == 0 ? 10 : ((int)parallelismDegreeNumericUpDown.Value);
                Parallel.For(0, domains.Count, parallelOptions, (domainIndex) =>
                {
                    try
                    {
                        var resultSet = new DataSet();
                        var domain = domains[domainIndex];
                        var copyConnection = _connectionStringHandler.GetDeepCopy(connectionDetails);
                        copyConnection.Database = domain.DatabaseName;
                        if (displayOption == "OnlyListDomainsWithAffectedRecords")
                        {
                            DAO.ChangeData(query, copyConnection);
                        }
                        else
                        {
                            resultSet = DAO.GetData(query, copyConnection);
                        }

                        if (displayOption == "OnlyListDomainsWithData" || displayOption == "OnlyListDomainsWithAffectedRecords")
                            DomainsWithResults.Add(copyConnection.Database);
                        else
                        {
                            if (resultSet != null && resultSet.Tables != null && resultSet.Tables.Count > 0 && resultSet.Tables[0] != null && resultSet.Tables[0].Rows.Count > 0)
                            {
                                resultSet.Tables[0].TableName = copyConnection.Database;
                                domainDataSets.TryAdd(copyConnection.Database, resultSet.Copy());
                            }
                        }

                        //Send the update to our UI thread
                        synchronizationContext.Post(new SendOrPostCallback(o =>
                        {
                            SuccessDomainsCount++;
                            var percentage = ((double)SuccessDomainsCount / (double)domains.Count) * 100;
                            queryProgressBarToolStrip.Value = (int)percentage;
                        }), null);
                    }
                    catch (Exception ex)
                    {
                        FailureDomainsCount++;
                    }
                });
            }
            else
            {
                var resultSet = new DataSet();

                if (displayOption == "OnlyListDomainsWithAffectedRecords")
                {
                    DAO.ChangeData(query, connectionDetails);
                }
                else
                {
                    resultSet = DAO.GetData(query, connectionDetails);
                }

                if (displayOption == "OnlyListDomainsWithData" || displayOption == "OnlyListDomainsWithAffectedRecords")
                    DomainsWithResults.Add(connectionDetails.Database);
                else
                {
                    if (resultSet == null || resultSet.Tables == null || resultSet.Tables.Count == 0 || resultSet.Tables[0] == null || resultSet.Tables[0].Rows.Count == 0)
                    {
                        errorMessage = "No data found";
                        return null;
                    }
                    resultSet.Tables[0].Namespace = connectionDetails.Database;
                    var newSet = resultSet.Copy();
                    domainDataSets.TryAdd(connectionDetails.Database, resultSet.Copy());
                }

                //Send the update to our UI thread
                synchronizationContext.Post(new SendOrPostCallback(o =>
                {
                    queryProgressBarToolStrip.Value = 100;
                }), null);
            }
            return domainDataSets;
        }

        public ConcurrentDictionary<string, DataSet> ExecuteQueryAsync(ConnectionDetails connectionDetails, List<Domain> domains, string query, string displayOption, out string errorMessage)
        {
            var domainDataSets = new ConcurrentDictionary<string, DataSet>();
            errorMessage = "";

            if (connectionDetails.IsMultiTenant)
            {
                var parallelOptions = new ParallelOptions();
                parallelOptions.MaxDegreeOfParallelism = ((int)parallelismDegreeNumericUpDown.Value) == 0 ? 1 : ((int)parallelismDegreeNumericUpDown.Value);
                Parallel.For(0, domains.Count, parallelOptions, (domainIndex) =>
                {
                    try
                    {
                        var resultSet = new DataSet();
                        var domain = domains[domainIndex];
                        var copyConnection = _connectionStringHandler.GetDeepCopy(connectionDetails);
                        copyConnection.Database = domain.DatabaseName;
                        if (displayOption == "OnlyListDomainsWithAffectedRecords")
                        {
                            DAO.ChangeData(query, copyConnection);
                        }
                        else
                        {
                            resultSet = DAO.GetData(query, copyConnection);
                            if (resultSet != null && resultSet.Tables != null && resultSet.Tables.Count != 0 && resultSet.Tables[0] != null && resultSet.Tables[0].Rows.Count > 0)
                            {
                                resultSet.Tables[0].TableName = copyConnection.Database;
                                domainDataSets.TryAdd(copyConnection.Database, resultSet.Copy());
                            }
                        }

                        if (displayOption == "OnlyListDomainsWithAffectedRecords")
                        {
                            DomainsWithResults.Add(copyConnection.Database);
                        }

                        //Send the update to our UI thread
                        synchronizationContext.Post(new SendOrPostCallback(o =>
                        {
                            if (displayOption != "OnlyListDomainsWithAffectedRecords")
                            {
                                QueryExecutionCompletedAsync(copyConnection.Database, resultSet.Copy(), displayOption);
                            }

                            SuccessDomainsCount++;
                            var percentage = ((double)SuccessDomainsCount / (double)domains.Count) * 100;
                            queryProgressBarToolStrip.Value = (int)percentage;
                        }), null);
                    }
                    catch (Exception ex)
                    {
                        FailureDomainsCount++;
                    }
                });

                //Send the update to our UI thread
                synchronizationContext.Post(new SendOrPostCallback(o =>
                {
                    queryStatusLabelToolStrip.Text = "Completed";
                    loadDomainStatusLabelToolStrip.Text = "Ready";
                    successDomainsToolStrip.Text = SuccessDomainsCount + " successful";
                    failureDomainsToolStrip.Text = FailureDomainsCount + " failed";

                    var selectedDisplayOption = displayOptionsComboBox.SelectedItem as NameValueModel;
                    if (selectedDisplayOption.Value == "OnlyListDomainsWithData")
                    {
                        DataSet dataSet = new DataSet();
                        dataSet.Tables.Add("DomainsWithResults");
                        dataSet.Tables[0].Columns.Add("Domain", typeof(string));
                        foreach (var domain in DomainsWithResults)
                        {
                            var dataRow = dataSet.Tables[0].NewRow();
                            dataRow["Domain"] = domain;
                            dataSet.Tables[0].Rows.Add(dataRow);
                        }
                        DisplayOutputInTabsAsync("Domains with results", dataSet);
                    }
                }), null);
            }
            else
            {
                var resultSet = new DataSet();
                try
                {
                    if (displayOption == "OnlyListDomainsWithAffectedRecords")
                        DAO.ChangeData(query, connectionDetails);
                    else
                    {
                        resultSet = DAO.GetData(query, connectionDetails);
                        domainDataSets.TryAdd(connectionDetails.Database, resultSet.Copy());
                    }
                    SuccessDomainsCount = 1;
                }
                catch (Exception ex)
                {
                    FailureDomainsCount = 1;
                }

                //Send the update to our UI thread
                synchronizationContext.Post(new SendOrPostCallback(o =>
                {
                    queryStatusLabelToolStrip.Text = "Completed";
                    loadDomainStatusLabelToolStrip.Text = "Ready";
                    successDomainsToolStrip.Text = SuccessDomainsCount + " successful";
                    failureDomainsToolStrip.Text = FailureDomainsCount + " failed";
                    QueryExecutionCompletedAsync(connectionDetails.Database, resultSet.Copy(), displayOption);
                }), null);
            }
            return domainDataSets;
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

            EnableUIForDomainList(false);
            loadDomainsProgressBarToolStrip.Value = 0;
            loadDomainStatusLabelToolStrip.Text = "Running";
            queryProgressBarToolStrip.Value = 0;
            queryStatusLabelToolStrip.Text = "Query execution is blocked - till domains are being loaded";

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
            queryRichTextBox.ReadOnly = !value;
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
            domainsCheckListBox.SelectedIndexChanged -= DomainsCheckListBox_SelectedIndexChanged;
            domainsCheckListBox.DataSource = Domains;
            domainsCheckListBox.ValueMember = "Name";
            domainsCheckListBox.DisplayMember = "DisplayName";
            exportDomainsButton.Enabled = true;
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

        public ConnectionDetails GetSelectedConnectionDetails(out string validationMessage)
        {
            validationMessage = "";
            if (connectionStringsComboBox.SelectedValue == null)
            {
                validationMessage = "Select a connection";
                return null;
            }

            var connectionDetail = ConnectionStringHandler.ConnectionStrings.FirstOrDefault(a => a.Name == connectionStringsComboBox.SelectedValue.ToString());
            if (connectionDetail == null)
            {
                validationMessage = "Connection string not available";
                return null;
            }
            return connectionDetail;
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

        private void connectionStringsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            domainsCheckListBox.DataSource = null;
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

        void ExportDataSetCollectionToExcel(List<KeyValuePair<string, DataSet>> dataSetCollection, string usersSaveDirectoryPath, string rootDirectoryName, string exportOption, string exportFileName)
        {
            if (!usersSaveDirectoryPath.ToLower().EndsWith(this.Name.ToLower()))
                usersSaveDirectoryPath += @"\" + this.Name;

            var resolvedDirectoryPath = usersSaveDirectoryPath + @"\" + rootDirectoryName + @"\";
            Directory.CreateDirectory(resolvedDirectoryPath);

            if (exportOption == "ExportOneFilePerDomain")
            {
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
                                ws.Cells[ws.Dimension.Address].AutoFitColumns();
                            }
                            catch (Exception ex)
                            {

                            }
                            pck.Save();
                        }
                    }
                }
            }
            else if (exportOption == "ExportAllDomainsInOneFile")
            {
                var resolvedFilePath = resolvedDirectoryPath + exportFileName + ".xlsx";
                using (ExcelPackage pck = new ExcelPackage(resolvedFilePath))
                {
                    if (pck.Workbook.Worksheets.Count > 0)
                    {
                        for (int i = 0; i < pck.Workbook.Worksheets.Count; i++)
                        {
                            pck.Workbook.Worksheets.Delete(i);
                        }
                    }
                    foreach (var item in dataSetCollection)
                    {
                        try
                        {
                            ExcelWorksheet ws = pck.Workbook.Worksheets.Add(item.Value.Tables[0].TableName);
                            ws.Cells["A1"].LoadFromDataTable(item.Value.Tables[0], true);
                            ws.Cells[ws.Dimension.Address].AutoFitColumns();
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    pck.Save();
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

        private void useSavedTemplateCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            savedTemplatesComboBox.Enabled = useSavedTemplateCheckBox.Checked;
            queryRichTextBox.ReadOnly = useSavedTemplateCheckBox.Checked;
        }

        private void savedTemplatesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (useSavedTemplateCheckBox.Checked && savedTemplatesComboBox.SelectedIndex > 0 && savedTemplatesComboBox.SelectedValue.HasContent())
            {
                var selectedQueryTemplate = (savedTemplatesComboBox.SelectedItem as QueryTemplate);
                if (selectedQueryTemplate == null)
                {
                    MessageBox.Show("Template is not available");
                    return;
                }
                queryRichTextBox.Text = File.ReadAllText(selectedQueryTemplate.QueryTemplateFilePath);
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
            mainPanel.Width = this.Width - 30;
            mainPanel.Height = this.Height - 70;

            if (mainPanel.VerticalScroll.Visible)
                queryOutputTabControl.Width = Width - 55;
            else
                queryOutputTabControl.Width = Width - 40;
        }

        private void filterDomainsTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Back)
            {
                e.Handled = true;
                if (filterDomainsTextBox.Text.HasContent())
                {
                    filterDomainsTextBox.Text = filterDomainsTextBox.Text.Substring(0, filterDomainsTextBox.Text.Length - 1);
                    filterDomainsTextBox.SelectionStart = filterDomainsTextBox.TextLength;
                }
            }
        }

        private void queryOutputTabControl_SizeChanged(object sender, EventArgs e)
        {
            if (queryOutputTabControl.TabPages.Count > 0)
            {
                foreach (TabPage tabPage in queryOutputTabControl.TabPages)
                {
                    tabPage.Height = queryOutputTabControl.Height;
                    tabPage.Width = queryOutputTabControl.Width;
                }
            }
        }

        private void displayOptionsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (displayOptionsComboBox.SelectedItem != null)
            {
                var selectedDisplayOption = (displayOptionsComboBox.SelectedItem as NameValueModel);
                if (selectedDisplayOption == null)
                {
                    MessageBox.Show("Template is not available");
                    return;
                }
                if (selectedDisplayOption.Value == "SingleResultSingleTab" || selectedDisplayOption.Value == "MultiResultSingleTab")
                    exportOptionsComboBox.Enabled = true;
                else
                {
                    exportOptionsComboBox.SelectedIndex = 0;
                    exportOptionsComboBox.Enabled = true;
                }
            }
        }
    }
}