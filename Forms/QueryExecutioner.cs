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
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SSDLMaintenanceTool.Forms
{
    public partial class QueryExecutioner : Form
    {
        public List<EnvironmentSetting> Environments { get; set; }
        public DataSet DomainsTableSet { get; set; }
        public DAO dao;
        public ConnectionStringHandler _connectionStringHandler;
        private DataTable combinedDomainResultDataTable;

        public List<Domain> Domains { get; set; }
        public List<Domain> BackupDomains { get; set; }
        private readonly SynchronizationContext synchronizationContext;
        public delegate bool MethodInvokerWithDataSetResult();
        public delegate void QueryCompletion(string exportingFileName = null);
        public Dictionary<string, QueryCompletion> QueryCompleted { get; set; }
        public ConcurrentDictionary<string, DataSet> QueryResultDataSet { get; set; }
        public bool IsAllDomainsSelected { get; set; }
        public int SuccessDomainsCount { get; set; }
        public int FailureDomainsCount { get; set; }
        public List<TabPage> OutputTabPages { get; set; }
        public ConcurrentDictionary<string, string> DomainsWithResults { get; set; }
        public List<string> DangerousScripts { get; set; }
        public List<NameValueModel> ExportOptions { get; set; }
        public SortedList<int, QueryExecutionLog> QueryExecutionLogList { get; set; }

        public QueryExecutioner()
        {
            InitializeComponent();
            dao = new DAO();
            _connectionStringHandler = new ConnectionStringHandler(@"Assets\connection-strings.json");
            synchronizationContext = SynchronizationContext.Current; //context from UI thread
        }

        private void QueryExecutioner_Load(object sender, EventArgs e)
        {
            DangerousScripts = new List<string> { "insert", "update", "delete", "lock", "transaction", "exec", "drop", "alter", "truncate" };

            connectionStringsComboBox.DataSource = _connectionStringHandler.ConnectionStrings;
            connectionStringsComboBox.ValueMember = "Name";
            connectionStringsComboBox.DisplayMember = "DisplayName";
            connectionStringsComboBox.SelectedIndex = -1;
            connectionStringsComboBox.Text = "Select a connection";
            exportDomainsButton.Enabled = false;
            queryRichTextBox.ScrollBars = RichTextBoxScrollBars.Both;

            List<QueryTemplate> queryTemplates = new List<QueryTemplate>();
            queryTemplates.Add(new QueryTemplate { Name = "Select a template", Value = "" });
            queryTemplates.Add(new QueryTemplate
            {
                Name = "<blank>",
                Value = "",
                QueryTemplateFilePath = Path.Combine(Environment.CurrentDirectory.Replace(@"bin\Debug", ""), @"QueryTemplates\Publish-Predefined-Migration-Part-01.sql"),
                QueryCompletionCallback = new QueryCompletionCallback()
                {
                    Assembly = "SSDLMaintenanceTool",
                    ClassName = "Forms.QueryExecutioner",
                    Method = ""
                }
            });
            queryTemplates.Add(new QueryTemplate
            {
                Name = "<blank>",
                Value = "",
                QueryTemplateFilePath = Path.Combine(Environment.CurrentDirectory.Replace(@"bin\Debug", ""), @"QueryTemplates\main-table-master-columns-mismatch-detection.sql")
            });

            savedTemplatesComboBox.Enabled = false;
            savedTemplatesComboBox.DataSource = queryTemplates;
            savedTemplatesComboBox.DisplayMember = "Name";
            savedTemplatesComboBox.ValueMember = "Value";
            savedTemplatesComboBox.SelectedIndex = 0;

            displayOptionsComboBox.Items.Add(new NameValueModel { Name = "No Display", Value = "NoDisplay" });
            displayOptionsComboBox.Items.Add(new NameValueModel { Name = "Multi result per domain", Value = "Display" });
            displayOptionsComboBox.Items.Add(new NameValueModel { Name = "Only list domains with affected records", Value = "OnlyListDomainsWithAffectedRecords" });
            displayOptionsComboBox.Items.Add(new NameValueModel { Name = "Only list domains with data", Value = "OnlyListDomainsWithData" });
            displayOptionsComboBox.Items.Add(new NameValueModel { Name = "Display first result of multi domains in one tab", Value = "DisplayFirstResultMultiDomainsInSingleTab" });
            displayOptionsComboBox.DisplayMember = "Name";
            displayOptionsComboBox.ValueMember = "Value";
            displayOptionsComboBox.SelectedIndex = 0;

            ExportOptions = new List<NameValueModel>();
            ExportOptions.Add(new NameValueModel { Name = "No Export", Value = "NoExport" });
            ExportOptions.Add(new NameValueModel { Name = "Export one file per domain", Value = "ExportOneFilePerDomain" });
            ExportOptions.Add(new NameValueModel { Name = "Export first result per domain in one file", Value = "ExportFirstSheetPerDomainsInOneFile" });
            ExportOptions.Add(new NameValueModel { Name = "Export first result per domain in single sheet", Value = "ExportFirstResultPerDomainsInSingleSheet" });
            exportOptionsComboBox.DataSource = ExportOptions;
            exportOptionsComboBox.DisplayMember = "Name";
            exportOptionsComboBox.ValueMember = "Value";
            exportOptionsComboBox.SelectedIndex = 0;

            domainFilterOptionsComboBox.Items.Add(new NameValueModel { Name = "SSDL Micro DBs", Value = "SSDLMicroDBs" });
            domainFilterOptionsComboBox.Items.Add(new NameValueModel { Name = "SSDL Buyer DBs", Value = "SSDLBuyerDBs" });
            domainFilterOptionsComboBox.Items.Add(new NameValueModel { Name = "All Buyer DBs", Value = "AllBuyerDBs" });
            domainFilterOptionsComboBox.Items.Add(new NameValueModel { Name = "All DBs", Value = "AllDBs" });
            domainFilterOptionsComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            domainFilterOptionsComboBox.DisplayMember = "Name";
            domainFilterOptionsComboBox.ValueMember = "Value";
            domainFilterOptionsComboBox.SelectedIndex = 0;

            QueryCompleted = new Dictionary<string, QueryCompletion>();

            foreach (var queryTemplate in queryTemplates)
            {
                if (queryTemplate.Value.HasContent())
                {
                    if (queryTemplate.QueryCompletionCallback != null)
                    {
                        Assembly assembly = Assembly.Load(queryTemplate.QueryCompletionCallback.Assembly);
                        Type t = assembly.GetType(queryTemplate.QueryCompletionCallback.Assembly + "." + queryTemplate.QueryCompletionCallback.ClassName);
                        var method = t.GetMethod(queryTemplate.QueryCompletionCallback.Method);
                        if (method != null)
                        {
                            var function = (QueryCompletion)Delegate.CreateDelegate(typeof(QueryCompletion), this, method);
                            QueryCompleted.Add(queryTemplate.Value, function);
                        }
                    }
                    else
                        QueryCompleted.Add(queryTemplate.Value, QueryExecutionCompleted);
                }
            }

            ConvertToDomainModel();
            PopulateDomainsCheckListBox();
            queryOutputTabControl.Height = 450;

            queryProgressBarToolStrip.Minimum = 0;
            queryProgressBarToolStrip.Maximum = 100;
            successDomainsToolStrip.Text = "";
            failureDomainsToolStrip.Text = "";

            loadDomainsProgressBarToolStrip.Minimum = 0;
            loadDomainsProgressBarToolStrip.Maximum = 100;

            DomainsWithResults = new ConcurrentDictionary<string, string>();
            mainPanel.AutoScroll = true;

            mainPanel.Width = this.Width - 30;
            mainPanel.Height = this.Height - 70;

            if (mainPanel.VerticalScroll.Visible)
                queryOutputTabControl.Width = Width - 55;
            else
                queryOutputTabControl.Width = Width - 40;

            parallelismDegreeNumericUpDown.Minimum = 0;
            parallelismDegreeNumericUpDown.Maximum = 20;
            parallelismDegreeNumericUpDown.Value = 0;
        }

        private void executeQueryButton_Click(object sender, EventArgs e)
        {
            var connectionDetails = GetSelectedConnectionDetails(out var valiationMessage);
            if (valiationMessage.HasContent())
            {
                MessageBox.Show(valiationMessage);
                return;
            }

            if (!queryRichTextBox.Text.HasContent())
            {
                MessageBox.Show("Query Text is mandatory");
                return;
            }

            if (domainFilterOptionsComboBox.SelectedItem != null)
            {
                var selectedDomainFilter = (domainFilterOptionsComboBox.SelectedItem as NameValueModel).Value;
                if (selectedDomainFilter != "SSDLMicroDBs")
                {
                    InputDialog promptForDangerous = new InputDialog();
                    promptForDangerous.WindowTitle = "Be careful with script!";
                    promptForDangerous.MainInstruction = "You're about to execute script(s) on database that might be non-SSDL. Please enter password";
                    var dialogResult = promptForDangerous.ShowDialog(this);
                    if (dialogResult != DialogResult.OK)
                    {
                        return;
                    }
                    if (promptForDangerous.Input != "IUnderstand")
                    {

                        return;
                    }
                }
            }

            if (DangerousScripts.Any(a => queryRichTextBox.Text.ToLower().Contains(a.ToLower())))
            {
                InputDialog promptForDangerous = new InputDialog();
                promptForDangerous.WindowTitle = "Be careful with script";
                promptForDangerous.MainInstruction = "You're about to execute script with Data Modification. Please enter password.";
                var dialogResult = promptForDangerous.ShowDialog(this);
                if (dialogResult != DialogResult.OK)
                {
                    return;
                }
                if (promptForDangerous.Input != "AmICorrect?")
                {
                    return;
                }
            }

            List<Domain> domains = new List<Domain>();

            if (connectionDetails.IsMultiTenant)
            {
                if (domainsCheckListBox.CheckedItems.Count == 0)
                {
                    MessageBox.Show("Select at least one domain.");
                    return;
                }
                if (domainsCheckListBox.CheckedItems.Count > 1)
                {
                    InputDialog promptForDangerous = new InputDialog();
                    promptForDangerous.WindowTitle = "Be careful with script!";
                    promptForDangerous.MainInstruction = $"You're about to execute script(s) on more than one domains. Please type {domainsCheckListBox.CheckedItems.Count} below and press OK.";
                    var dialogResult = promptForDangerous.ShowDialog(this);
                    if (dialogResult != DialogResult.OK)
                    {
                        return;
                    }
                    if (promptForDangerous.Input != domainsCheckListBox.CheckedItems.Count.ToString())
                    {
                        return;
                    }
                }
                domains = ConvertToDomainDatabases();
            }

            var exportingFileName = "";
            var selectedExportOption = (exportOptionsComboBox.SelectedItem as NameValueModel);
            if (selectedExportOption != null && (selectedExportOption.Value == "ExportFirstSheetPerDomainsInOneFile" || selectedExportOption.Value == "ExportFirstResultPerDomainsInSingleSheet"))
            {
                InputDialog exportFileNameInputDialog = new InputDialog();
                exportFileNameInputDialog.WindowTitle = "Save as XLSX";
                exportFileNameInputDialog.MainInstruction = "Enter name of file being exported";
                var dialogResult = exportFileNameInputDialog.ShowDialog(this);
                if (dialogResult != DialogResult.OK)
                {
                    return;
                }
                exportingFileName = exportFileNameInputDialog.Input;
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
            DomainsWithResults = new ConcurrentDictionary<string, string>();
            successDomainsToolStrip.Text = "";
            failureDomainsToolStrip.Text = "";

            var selectedDisplayOption = displayOptionsComboBox.SelectedItem as NameValueModel;

            StartExecution(connectionDetails, domains, queryRichTextBox.Text, asyncCheckBox.Checked, selectedDisplayOption.Value, selectedExportOption.Value, exportingFileName);
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

        private void StartExecution(ConnectionDetails connectionDetails, List<Domain> domains, string query, bool isAsync, string displayOption, string exportOption, string exportingFileName)
        {
            Task.Run(() =>
            {
                try
                {
                    if (isAsync)
                    {
                        StartQueryExecutionAsync(connectionDetails, domains, query, displayOption, exportOption, exportingFileName);
                    }
                    else
                    {
                        StartQueryExecution(connectionDetails, domains, query, displayOption, exportOption, exportingFileName);
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

        private void StartQueryExecutionAsync(ConnectionDetails connectionDetails, List<Domain> domains, string query, string displayOption, string exportOption, string exportingFileName)
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

                if (exportOption == "ExportOneFilePerDomain" || exportOption == "ExportFirstSheetPerDomainsInOneFile")
                {
                    var queryResultsList = QueryResultDataSet.Select(a => a).ToList();
                    queryResultsList.Sort((s1, s2) => s1.Key.CompareTo(s2.Key));

                    VistaFolderBrowserDialog vistaFolderBrowserDialog = new VistaFolderBrowserDialog();
                    vistaFolderBrowserDialog.Description = "Export Query Result - Select folder to export the file";
                    vistaFolderBrowserDialog.UseDescriptionForTitle = true;
                    var saveFileDialogResult = vistaFolderBrowserDialog.ShowDialog(this);
                    if (saveFileDialogResult == DialogResult.OK)
                    {
                        ExportDataSetCollectionToExcel(queryResultsList, vistaFolderBrowserDialog.SelectedPath, GlobalConstants.QueryExecutionerQueryResultDirectory, exportOption, exportingFileName);
                    }
                }
            }), null);
        }

        private void StartQueryExecution(ConnectionDetails connectionDetails, List<Domain> domains, string query, string displayOption, string exportOption, string exportingFileName)
        {
            QueryResultDataSet = ExecuteQuery(connectionDetails, domains, query, displayOption, exportOption, out var errorMessage);

            //Send the update to our UI thread
            synchronizationContext.Post(new SendOrPostCallback(o =>
            {
                if (errorMessage.HasContent())
                {
                    MessageBox.Show(errorMessage);
                    return;
                }
                QueryExecutionCompleted(exportingFileName);
            }), null);
        }

        public ConcurrentDictionary<string, DataSet> ExecuteQuery(ConnectionDetails connectionDetails, List<Domain> domains, string query, string displayOption, string exportOption, out string errorMessage)
        {
            var domainDataSets = new ConcurrentDictionary<string, DataSet>();
            errorMessage = "";
            QueryExecutionLogList = new SortedList<int, QueryExecutionLog>(new DescendingComparer<int>());

            if (connectionDetails.IsMultiTenant)
            {
                var parallelOptions = new ParallelOptions();
                parallelOptions.MaxDegreeOfParallelism = ((int)parallelismDegreeNumericUpDown.Value) == 0 ? 10 : ((int)parallelismDegreeNumericUpDown.Value);
                Parallel.For(0, domains.Count, parallelOptions, (domainIndex) =>
                {
                    var domain = domains[domainIndex];
                    var queryExecutionLog = new QueryExecutionLog();
                    queryExecutionLog.DatabaseName = domain.DatabaseName;
                    try
                    {
                        var resultSet = new DataSet();
                        var rowsAffected = 0;
                        var copyConnection = _connectionStringHandler.GetDeepCopy(connectionDetails);
                        copyConnection.Database = domain.DatabaseName;

                        if (displayOption == "OnlyListDomainsWithAffectedRecords")
                        {
                            rowsAffected = dao.ChangeData(query, copyConnection);
                            queryExecutionLog.RowsAffected = rowsAffected;
                        }
                        else
                        {
                            resultSet = dao.GetData(query, copyConnection);
                        }

                        var hasData = false;
                        foreach (DataTable dataTable in resultSet.Tables)
                        {
                            if (dataTable != null && dataTable.Rows.Count > 0)
                            {
                                hasData = true;
                                break;
                            }
                        }

                        if (displayOption == "OnlyListDomainsWithData" && hasData)
                        {
                            DomainsWithResults.TryAdd(copyConnection.Database, "");
                        }
                        else if (displayOption == "OnlyListDomainsWithAffectedRecords" && rowsAffected > 0)
                        {
                            DomainsWithResults.TryAdd(copyConnection.Database, rowsAffected.ToString());
                        }
                        else
                        {
                            if (hasData)
                            {
                                if (displayOption == "Display" || displayOption == "DisplayFirstResultMultiDomainsInSingleTab" || exportOption == "ExportFirstResultPerDomainsInSingleSheet" || exportOption == "ExportOneFilePerDomain"
                                || exportOption == "ExportFirstSheetPerDomainsInOneFile")
                                {
                                    domainDataSets.TryAdd(copyConnection.Database, resultSet.Copy());
                                }
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
                        //Send the update to our UI thread
                        synchronizationContext.Post(new SendOrPostCallback(o =>
                        {
                            FailureDomainsCount++;
                            queryExecutionLog.ErrorMessage = ex.Message;
                        }), null);
                    }
                    QueryExecutionLogList.Add(domainIndex, queryExecutionLog);
                });
            }
            else
            {
                var resultSet = new DataSet();
                var rowsAffected = 0;
                var queryExecutionLog = new QueryExecutionLog();
                try
                {
                    if (displayOption == "OnlyListDomainsWithAffectedRecords")
                    {
                        rowsAffected = dao.ChangeData(query, connectionDetails);
                        queryExecutionLog.RowsAffected = rowsAffected;
                    }
                    else
                    {
                        resultSet = dao.GetData(query, connectionDetails);
                    }

                    var hasData = false;
                    foreach (DataTable dataTable in resultSet.Tables)
                    {
                        if (dataTable != null && dataTable.Rows.Count > 0)
                        {
                            hasData = true;
                            break;
                        }
                    }

                    if (displayOption == "OnlyListDomainsWithData" && hasData)
                    {
                        DomainsWithResults.TryAdd(connectionDetails.Database, "");
                    }
                    else if (displayOption == "OnlyListDomainsWithAffectedRecords" && rowsAffected > 0)
                    {
                        DomainsWithResults.TryAdd(connectionDetails.Database, rowsAffected.ToString());
                    }
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
                catch (Exception ex)
                {
                    //Send the update to our UI thread
                    synchronizationContext.Post(new SendOrPostCallback(o =>
                    {
                        FailureDomainsCount++;
                        queryExecutionLog.ErrorMessage = ex.Message;
                    }), null);
                }
                QueryExecutionLogList.Add(0, queryExecutionLog);
            }
            return domainDataSets;
        }

        public ConcurrentDictionary<string, DataSet> ExecuteQueryAsync(ConnectionDetails connectionDetails, List<Domain> domains, string query, string displayOption, out string errorMessage)
        {
            var domainDataSets = new ConcurrentDictionary<string, DataSet>();
            var combinedDomainResultDataTable = new DataTable();
            errorMessage = "";

            if (connectionDetails.IsMultiTenant)
            {
                var parallelOptions = new ParallelOptions();
                parallelOptions.MaxDegreeOfParallelism = ((int)parallelismDegreeNumericUpDown.Value) == 0 ? 2 : ((int)parallelismDegreeNumericUpDown.Value);
                Parallel.For(0, domains.Count, parallelOptions, (domainIndex) =>
                {
                    long rowsAffected = -1;
                    try
                    {
                        var resultSet = new DataSet();
                        var domain = domains[domainIndex];
                        var copyConnection = _connectionStringHandler.GetDeepCopy(connectionDetails);
                        copyConnection.Database = domain.DatabaseName;
                        if (displayOption == "OnlyListDomainsWithAffectedRecords")
                        {
                            rowsAffected = dao.ChangeData(query, copyConnection);
                        }
                        else
                        {
                            resultSet = dao.GetData(query, copyConnection);
                            var hasData = false;
                            foreach (DataTable dataTable in resultSet.Tables)
                            {
                                if (dataTable != null && dataTable.Rows.Count > 0)
                                {
                                    hasData = true;
                                    break;
                                }
                            }
                            if (hasData)
                            {
                                if (
                                displayOption == "Display" || displayOption == "DisplayFirstResultMultiDomainsInSingleTab")
                                {
                                    domainDataSets.TryAdd(copyConnection.Database, resultSet.Copy());
                                }
                            }
                        }

                        //Send the update to our UI thread
                        synchronizationContext.Post(new SendOrPostCallback(o =>
                        {
                            if (displayOption != "OnlyListDomainsWithAffectedRecords")
                            {
                                QueryExecutionCompletedAsync(copyConnection.Database, resultSet.Copy(), displayOption, rowsAffected);
                            }
                            SuccessDomainsCount++;
                            var percentage = ((double)SuccessDomainsCount / (double)domains.Count) * 100;
                            queryProgressBarToolStrip.Value = (int)percentage;
                        }), null);
                    }
                    catch (Exception ex)
                    {
                        FailureDomainsCount++;
                        //Send the update to our UI thread
                        synchronizationContext.Post(new SendOrPostCallback(o =>
                        {
                            failureDomainsToolStrip.Text = FailureDomainsCount + " failed";
                        }), null);
                    }
                });

                //Send the update to our UI thread
                synchronizationContext.Post(new SendOrPostCallback(o =>
                {
                    queryStatusLabelToolStrip.Text = "Completed";
                    loadDomainStatusLabelToolStrip.Text = "Ready";
                    successDomainsToolStrip.Text = SuccessDomainsCount + " successful";

                    if (FailureDomainsCount > 0)
                    {
                        MessageBox.Show("The query has failed on some databases. Please check the logs.");
                    }

                    var selectedDisplayOption = displayOptionsComboBox.SelectedItem as NameValueModel;
                    if (selectedDisplayOption.Value == "OnlyListDomainsWithData" || selectedDisplayOption.Value == "OnlyListDomainsWithAffectedRecords")
                    {
                        DataSet dataSet = new DataSet();
                        dataSet.Tables.Add("DomainsWithResults");
                        dataSet.Tables[0].Columns.Add("Domain", typeof(string));
                        dataSet.Tables[0].Columns.Add("Data", typeof(string));
                        if (DomainsWithResults.Count == 0)
                        {
                            MessageBox.Show("No results");
                            return;
                        }
                        //DomainsWithResults.Sort((a, b) => a.CompareTo(b));
                        foreach (var domain in DomainsWithResults)
                        {
                            var dataRow = dataSet.Tables[0].NewRow();
                            dataRow["Domain"] = domain.Key;
                            dataRow["Data"] = domain.Value;
                            dataSet.Tables[0].Rows.Add(dataRow);
                        }
                        DisplayOutputInTabsAsync("Domains with results", dataSet);
                    }
                }), null);
            }
            else
            {
                long rowsAffected = -1;
                var resultSet = new DataSet();
                try
                {
                    if (displayOption == "OnlyListDomainsWithAffectedRecords")
                        dao.ChangeData(query, connectionDetails);
                    else
                    {
                        resultSet = dao.GetData(query, connectionDetails);
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

                    if (FailureDomainsCount > 0)
                    {
                        MessageBox.Show("The query has failed on some databases. Please check the logs.");
                    }
                    QueryExecutionCompletedAsync(connectionDetails.Database, resultSet.Copy(), displayOption, rowsAffected);
                }), null);
            }
            return domainDataSets;
        }

        private void QueryFailed(Exception ex)
        {
            queryStatusLabelToolStrip.Text = "Failed";
            loadDomainStatusLabelToolStrip.Text = "Ready";
            MessageBox.Show(ex.Message);
        }

        private void QueryExecutionCompleted(string exportingFileName = null)
        {
            queryStatusLabelToolStrip.Text = "Completed";
            loadDomainStatusLabelToolStrip.Text = "Ready";
            successDomainsToolStrip.Text = SuccessDomainsCount + " successful";
            failureDomainsToolStrip.Text = FailureDomainsCount + " failed";

            var selectedDisplayOption = (displayOptionsComboBox.SelectedItem as NameValueModel).Value;
            var selectedExportOption = (exportOptionsComboBox.SelectedItem as NameValueModel).Value;

            if (selectedDisplayOption == "DisplayFirstResultMultiDomainsInSingleTab" || selectedExportOption == "ExportFirstResultPerDomainsInSingleSheet")
            {
                if (QueryResultDataSet.Count > 0)
                {
                    this.combinedDomainResultDataTable = QueryResultDataSet.ElementAt(0).Value.Tables[0].Clone();
                    combinedDomainResultDataTable.Columns.Add("Domain").DataType = typeof(string);
                    foreach (var item in QueryResultDataSet)
                    {
                        item.Value.Tables[0].Columns.Add("Domain").DataType = typeof(string);
                        foreach (DataRow row in item.Value.Tables[0].Rows)
                        {
                            row["Domain"] = item.Key;
                        }
                        combinedDomainResultDataTable.Merge(item.Value.Tables[0], true, MissingSchemaAction.Ignore);
                    }
                }
            }

            if (selectedDisplayOption == "OnlyListDomainsWithData" || selectedDisplayOption == "OnlyListDomainsWithAffectedRecords")
            {
                DataSet dataSetForDisplay = new DataSet();
                dataSetForDisplay.Tables.Add("DomainsWithResults");
                dataSetForDisplay.Tables[0].Columns.Add("Domain", typeof(string));
                dataSetForDisplay.Tables[0].Columns.Add("Data", typeof(string));
                if (DomainsWithResults.Count == 0)
                {
                    MessageBox.Show("No records found in any of the SSDL domains");
                }
                if (DomainsWithResults.Count > 0)
                {
                    //DomainsWithResults.Sort((a, b) => a.CompareTo(b));
                    resultDomainsToolStrip.Text = DomainsWithResults.Count + " has matching data";

                    foreach (var domain in DomainsWithResults)
                    {
                        var dataRow = dataSetForDisplay.Tables[0].NewRow();
                        dataRow["Domain"] = domain.Key;
                        dataRow["Data"] = domain.Value;
                        dataSetForDisplay.Tables[0].Rows.Add(dataRow);
                    }
                    DisplayOutputInTabsAsync("Domains with results", dataSetForDisplay);

                    if (selectedExportOption.HasContent() && selectedExportOption.ToLower() != "NoExport".ToLower())
                    {
                        VistaFolderBrowserDialog vistaFolderBrowserDialog = new VistaFolderBrowserDialog();
                        vistaFolderBrowserDialog.Description = "Export Query Result - Select folder to export the file";
                        vistaFolderBrowserDialog.UseDescriptionForTitle = true;
                        var saveFileDialogResult = vistaFolderBrowserDialog.ShowDialog(this);
                        if (saveFileDialogResult == DialogResult.OK)
                        {
                            if (selectedExportOption == "ExportOneFilePerDomain" || selectedExportOption == "ExportFirstSheetPerDomainsInOneFile")
                            {
                                DataSet dataSetForExport = new DataSet();
                                foreach (var domain in DomainsWithResults)
                                {
                                    var dataTableForExport = new DataTable();
                                    dataTableForExport.Columns.Add("Data");
                                    dataTableForExport.TableName = domain.Key;

                                    var dataRow = dataTableForExport.NewRow();
                                    dataRow["Data"] = domain.Value;
                                    dataTableForExport.Rows.Add(dataRow);
                                    dataSetForExport.Tables.Add(dataTableForExport);
                                }
                                ExportDataSetToExcel(dataSetForExport, vistaFolderBrowserDialog.SelectedPath, GlobalConstants.QueryExecutionerQueryResultDirectory, selectedExportOption, exportingFileName.HasContent() ? exportingFileName : "DomainsWithResults");
                            }
                            else if (selectedExportOption == "ExportFirstResultPerDomainsInSingleSheet")
                            {
                                ExportDataTableToExcel(dataSetForDisplay.Tables[0], vistaFolderBrowserDialog.SelectedPath, GlobalConstants.QueryExecutionerQueryResultDirectory, exportingFileName.HasContent() ? exportingFileName : "DomainsWithResults");
                            }
                        }
                    }
                }
            }
            else
            {
                if (useSavedTemplateCheckBox.Checked && savedTemplatesComboBox.SelectedIndex > 0 && savedTemplatesComboBox.SelectedValue.HasContent())
                {
                    var selectedQueryTemplate = (savedTemplatesComboBox.SelectedItem as QueryTemplate);
                    if (selectedQueryTemplate == null)
                    {
                        MessageBox.Show("Template is not available");
                    }
                    if (selectedQueryTemplate != null)
                    {
                        QueryCompleted[selectedQueryTemplate.Value]();
                    }
                }
                else
                {
                    if (selectedDisplayOption == "Display" || selectedDisplayOption == "DisplayFirstResultMultiDomainsInSingleTab")
                    {
                        if (QueryResultDataSet == null || QueryResultDataSet.Count == 0)
                        {
                            MessageBox.Show("No records found in any of the SSDL domains");
                        }
                        if (QueryResultDataSet != null && QueryResultDataSet.Count > 0)
                        {
                            DisplayOutputInTabs(QueryResultDataSet, selectedDisplayOption, selectedExportOption);
                        }
                    }

                    if (QueryResultDataSet != null && QueryResultDataSet.Count > 0)
                    {
                        if (selectedExportOption == "ExportOneFilePerDomain" || selectedExportOption == "ExportFirstSheetPerDomainsInOneFile" || selectedExportOption == "ExportFirstResultPerDomainsInSingleSheet")
                        {
                            VistaFolderBrowserDialog vistaFolderBrowserDialog = new VistaFolderBrowserDialog();
                            vistaFolderBrowserDialog.Description = "Export Query Result - Select folder to export the file";
                            vistaFolderBrowserDialog.UseDescriptionForTitle = true;
                            var saveFileDialogResult = vistaFolderBrowserDialog.ShowDialog(this);
                            if (saveFileDialogResult == DialogResult.OK)
                            {
                                if (selectedExportOption == "ExportOneFilePerDomain" || selectedExportOption == "ExportFirstSheetPerDomainsInOneFile")
                                {
                                    var queryResultsList = QueryResultDataSet.Select(a => a).ToList();
                                    queryResultsList.Sort((s1, s2) => s1.Key.CompareTo(s2.Key));

                                    ExportDataSetCollectionToExcel(queryResultsList, vistaFolderBrowserDialog.SelectedPath, GlobalConstants.QueryExecutionerQueryResultDirectory, selectedExportOption, exportingFileName);
                                }
                                else if (selectedExportOption == "ExportFirstResultPerDomainsInSingleSheet")
                                {
                                    ExportDataTableToExcel(combinedDomainResultDataTable, vistaFolderBrowserDialog.SelectedPath, GlobalConstants.QueryExecutionerQueryResultDirectory, exportingFileName);
                                }
                            }
                        }
                    }
                }
            }

            if (QueryExecutionLogList != null && QueryExecutionLogList.Count > 0)
            {
                logsDataGridView.DataSource = QueryExecutionLogList.Select(a => a.Value).ToList();
                logsDataGridView.Refresh();
            }
        }

        private void QueryExecutionCompletedAsync(string databaseName, DataSet dataSet, string displayOption, long rowsAffected)
        {
            if (displayOption == "OnlyListDomainsWithAffectedRecords")
            {
                DomainsWithResults.TryAdd(databaseName, rowsAffected.ToString());
            }
            else if (dataSet != null && dataSet.Tables.Count > 0)
            {
                if (displayOption == "OnlyListDomainsWithData" || displayOption == "OnlyListDomainsWithAffectedRecords")
                {
                    var hasData = false;
                    foreach (DataTable dataTable in dataSet.Tables)
                    {
                        if (dataTable != null && dataTable.Rows.Count > 0)
                        {
                            hasData = true;
                            break;
                        }
                    }
                    if (hasData)
                        DomainsWithResults.TryAdd(databaseName, rowsAffected.ToString());
                }
                else
                {
                    if (displayOption == "Display" || displayOption == "DisplayFirstResultMultiDomainsInSingleTab")
                        DisplayOutputInTabsAsync(databaseName, dataSet);
                }
            }
        }

        private void DisplayOutputInTabs(ConcurrentDictionary<string, DataSet> dataSetCollection, string selectedDisplayOption, string selectedExportOption)
        {
            if (selectedDisplayOption == "Display")
            {
                foreach (var item in dataSetCollection)
                {
                    TabPage tabPage = new TabPage(item.Key);
                    tabPage.SizeChanged += DynamicallyCreatedTabPage_SizeChanged;

                    OutputTabPages.Add(tabPage);

                    if (item.Value.Tables.Count > 0)
                    {
                        var hasData = false;
                        foreach (DataTable dataTable in item.Value.Tables)
                        {
                            if (dataTable != null && dataTable.Rows.Count > 0)
                            {
                                hasData = true;
                                break;
                            }
                        }
                        if (hasData)
                        {
                            if (selectedDisplayOption == "Display")
                            {
                                int totalRecords = 0;
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

                                    //dataGridView.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.ColumnHeader);
                                    //dataGridView.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCells);

                                    innerTabPage.Controls.Add(dataGridView);
                                    innerTabPage.AutoScroll = true;
                                    innerTabPage.SizeChanged += DynamicallyCreatedTabPage_SizeChanged;
                                    StatusStrip statusStrip = new StatusStrip();
                                    ToolStripStatusLabel rowCountToolStripStatusLabel = new ToolStripStatusLabel();
                                    rowCountToolStripStatusLabel.Text = dataTable.TableName + " rows: " + dataTable.Rows.Count;
                                    statusStrip.Items.Add(rowCountToolStripStatusLabel);
                                    innerTabPage.Controls.Add(statusStrip);
                                    totalRecords += dataTable.Rows.Count;
                                }

                                innerTabPages.Sort((s1, s2) => s1.Text.CompareTo(s2.Text));

                                thisTabControl.TabPages.AddRange(innerTabPages.ToArray());
                                tabPage.AutoScroll = true;
                                StatusStrip domainStatusStrip = new StatusStrip();
                                ToolStripStatusLabel domainRowCountToolStripStatusLabel = new ToolStripStatusLabel();
                                domainRowCountToolStripStatusLabel.Text = item.Key + " rows: " + totalRecords;
                                domainStatusStrip.Items.Add(domainRowCountToolStripStatusLabel);
                                tabPage.Controls.Add(domainStatusStrip);
                            }
                            //else if (selectedDisplayOption == "SingleResultPerDomain")
                            //{
                            //    DataGridView dataGridView = new DataGridView();
                            //    var dataTable = item.Value.Tables[0];
                            //    dataGridView.DataSource = dataTable;
                            //    tabPage.Controls.Add(dataGridView);
                            //    dataGridView.Height = tabPage.Height - 50;
                            //    dataGridView.Width = tabPage.Width - 50;
                            //    tabPage.AutoScroll = true;
                            //    StatusStrip domainStatusStrip = new StatusStrip();
                            //    ToolStripStatusLabel domainRowCountToolStripStatusLabel = new ToolStripStatusLabel();
                            //    domainRowCountToolStripStatusLabel.Text = item.Key + " rows: " + dataTable.Rows.Count;
                            //    domainStatusStrip.Items.Add(domainRowCountToolStripStatusLabel);
                            //    tabPage.Controls.Add(domainStatusStrip);
                            //}
                        }
                    }

                    tabPage.Height = queryOutputTabControl.Height;
                    tabPage.Width = queryOutputTabControl.Width;
                }
            }
            else if (selectedDisplayOption == "DisplayFirstResultMultiDomainsInSingleTab" || selectedExportOption == "ExportFirstResultPerDomainsInSingleSheet")
            {
                TabPage tabPage = new TabPage("Combined result from domains");
                tabPage.SizeChanged += DynamicallyCreatedTabPage_SizeChanged;

                OutputTabPages.Add(tabPage);

                DataGridView dataGridView = new DataGridView();
                dataGridView.DataSource = combinedDomainResultDataTable;
                tabPage.Controls.Add(dataGridView);
                dataGridView.Height = tabPage.Height - 50;
                dataGridView.Width = tabPage.Width - 50;
                tabPage.AutoScroll = true;
                StatusStrip domainStatusStrip = new StatusStrip();
                ToolStripStatusLabel domainRowCountToolStripStatusLabel = new ToolStripStatusLabel();
                domainRowCountToolStripStatusLabel.Text = "Combined result" + " rows: " + combinedDomainResultDataTable.Rows.Count;
                domainStatusStrip.Items.Add(domainRowCountToolStripStatusLabel);
                tabPage.Controls.Add(domainStatusStrip);
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
                var hasData = false;
                foreach (DataTable dataTable in dataSetWithKey.Tables)
                {
                    if (dataTable != null && dataTable.Rows.Count > 0)
                    {
                        hasData = true;
                        break;
                    }
                }
                if (hasData)
                {
                    var selectedDisplayOption = displayOptionsComboBox.SelectedItem as NameValueModel;
                    if (selectedDisplayOption.Value == "Display")
                    {
                        int totalRecords = 0;
                        var innerTabControl = new TabControl();
                        tabPage.Controls.Add(innerTabControl);
                        innerTabControl.SizeChanged += DynamicallyCreatedTabControl_SizeChanged;

                        foreach (DataTable dataTable in dataSetWithKey.Tables)
                        {
                            innerTabControl.TabPages.Add(dataTable.TableName, dataTable.TableName);

                            var innerTabPage = innerTabControl.TabPages[dataTable.TableName];

                            DataGridView dataGridView = new DataGridView();
                            dataGridView.DataSource = dataTable;

                            //dataGridView.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.ColumnHeader);
                            //dataGridView.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCells);

                            innerTabPage.Controls.Add(dataGridView);
                            innerTabPage.AutoScroll = true;
                            innerTabPage.SizeChanged += DynamicallyCreatedTabPage_SizeChanged;
                            StatusStrip statusStrip = new StatusStrip();
                            ToolStripStatusLabel rowCountToolStripStatusLabel = new ToolStripStatusLabel();
                            rowCountToolStripStatusLabel.Text = dataTable.TableName + " rows: " + dataTable.Rows.Count;
                            statusStrip.Items.Add(rowCountToolStripStatusLabel);
                            innerTabPage.Controls.Add(statusStrip);
                            totalRecords += dataTable.Rows.Count;
                        }
                        tabPage.AutoScroll = true;
                        StatusStrip domainStatusStrip = new StatusStrip();
                        ToolStripStatusLabel domainRowCountToolStripStatusLabel = new ToolStripStatusLabel();
                        domainRowCountToolStripStatusLabel.Text = databaseName + " rows: " + totalRecords;
                        domainStatusStrip.Items.Add(domainRowCountToolStripStatusLabel);
                        tabPage.Controls.Add(domainStatusStrip);
                    }
                    else if (selectedDisplayOption.Value == "OnlyListDomainsWithData" || selectedDisplayOption.Value == "OnlyListDomainsWithAffectedRecords")
                    {
                        DataGridView dataGridView = new DataGridView();
                        var dataTable = dataSetWithKey.Tables[0];
                        dataGridView.DataSource = dataSetWithKey.Tables[0];
                        tabPage.Controls.Add(dataGridView);
                        dataGridView.Height = tabPage.Height - 20;
                        dataGridView.Width = tabPage.Width - 20;
                        tabPage.AutoScroll = true;
                        StatusStrip domainStatusStrip = new StatusStrip();
                        ToolStripStatusLabel domainRowCountToolStripStatusLabel = new ToolStripStatusLabel();
                        domainRowCountToolStripStatusLabel.Text = databaseName + " rows: " + dataTable.Rows.Count;
                        domainStatusStrip.Items.Add(domainRowCountToolStripStatusLabel);
                        tabPage.Controls.Add(domainStatusStrip);
                    }
                    else if (selectedDisplayOption.Value == "DisplayFirstResultMultiDomainsInSingleTab")
                    {

                    }
                }
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
            queryStatusLabelToolStrip.Text = "Query execution is blocked - till domains are being loaded";

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
            displayOptionsComboBox.Enabled = value;
            exportOptionsComboBox.Enabled = value;
        }

        private void EnableUIForQueryExecution(bool value = true)
        {
            EnableUIForDomainList(value);
            queryRichTextBox.ReadOnly = !value;
        }

        private void LoadSSDLDomains(ConnectionDetails copyConnection, string domainNameFilter = "")
        {
            var selectClause = "Name";
            string filterCondition;
            var query = "";

            if (!domainNameFilter.HasContent() || domainNameFilter == "SSDLMicroDBs")
            {
                filterCondition = $"Name LIKE '%[_]SSDL'";
                query = $"SELECT {selectClause} FROM sys.databases WHERE {filterCondition}";
            }
            else
            {
                if (domainNameFilter == "SSDLBuyerDBs")
                {
                    query = @"With CTE AS
                            (
                                select name from sys.databases where name like '%[_]SSDL'
                            )
                            select a.name
                            from sys.databases a
                            join CTE b on a.name like(replace(b.name, '_SSDL', '') +'%') and a.name not like '%[_]%'
                            ";
                }
                else if (domainNameFilter == "AllBuyerDBs")
                {
                    filterCondition = $"Name NOT LIKE '%[_]%'";
                    query = $"SELECT {selectClause} FROM sys.databases WHERE {filterCondition}";
                }
            }

            var resultSet = dao.GetData(query, copyConnection);
            if (resultSet == null || resultSet.Tables == null || resultSet.Tables.Count == 0 || resultSet.Tables[0] == null || resultSet.Tables[0].Rows.Count == 0)
            {
                domainsCheckListBox.DataSource = null;
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

            var connectionDetail = _connectionStringHandler.ConnectionStrings.FirstOrDefault(a => a.Name == connectionStringsComboBox.SelectedValue.ToString());
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
            IsAllDomainsSelected = false;
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
            if (!fileName.HasContent())
                fileName = "data";

            if (!usersSaveDirectoryPath.ToLower().EndsWith(this.Name.ToLower()))
                usersSaveDirectoryPath += @"\" + this.Name;

            var resolvedDirectoryPath = usersSaveDirectoryPath + @"\" + rootDirectoryName + @"\";
            Directory.CreateDirectory(resolvedDirectoryPath);

            var resolvedFilePath = resolvedDirectoryPath + (fileName ?? dataTable.TableName) + ".xlsx";

            using (ExcelPackage pck = new ExcelPackage(resolvedFilePath))
            {
                try
                {
                    if (pck.Workbook.Worksheets.Count > 0)
                    {
                        var existingWorksheetsCount = pck.Workbook.Worksheets.Count;
                        for (int i = 0; i < existingWorksheetsCount; existingWorksheetsCount--)
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
            var successDialogResult = MessageBox.Show("File has been exported.\nClick on Yes to open the file location.", "Success", MessageBoxButtons.YesNo);
            if (successDialogResult == DialogResult.Yes)
                Process.Start("explorer.exe", resolvedDirectoryPath);
        }

        void ExportDataSetToExcel(DataSet dataSet, string usersSaveDirectoryPath, string rootDirectoryName, string exportOption, string exportingFileName)
        {
            if (!usersSaveDirectoryPath.ToLower().EndsWith(this.Name.ToLower()))
                usersSaveDirectoryPath += @"\" + this.Name;

            var resolvedDirectoryPath = usersSaveDirectoryPath + @"\" + rootDirectoryName + @"\";
            Directory.CreateDirectory(resolvedDirectoryPath);

            if (exportOption == "ExportOneFilePerDomain")
            {
                foreach (DataTable dataTable in dataSet.Tables)
                {
                    var resolvedFilePath = resolvedDirectoryPath + dataTable.TableName + ".xlsx";

                    using (ExcelPackage pck = new ExcelPackage(resolvedFilePath))
                    {
                        try
                        {
                            if (pck.Workbook.Worksheets.Count > 0)
                            {
                                var existingWorksheetsCount = pck.Workbook.Worksheets.Count;
                                for (int i = 0; i < existingWorksheetsCount; existingWorksheetsCount--)
                                {
                                    pck.Workbook.Worksheets.Delete(i);
                                }
                            }
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
            else if (exportOption == "ExportFirstSheetPerDomainsInOneFile")
            {
                var resolvedFilePath = resolvedDirectoryPath + exportingFileName + ".xlsx";
                using (ExcelPackage pck = new ExcelPackage(resolvedFilePath))
                {
                    if (pck.Workbook.Worksheets.Count > 0)
                    {
                        var existingWorksheetsCount = pck.Workbook.Worksheets.Count;
                        for (int i = 0; i < existingWorksheetsCount; existingWorksheetsCount--)
                        {
                            pck.Workbook.Worksheets.Delete(i);
                        }
                    }
                    foreach (DataTable item in dataSet.Tables)
                    {
                        try
                        {
                            ExcelWorksheet ws = pck.Workbook.Worksheets.Add(item.TableName);
                            ws.Cells["A1"].LoadFromDataTable(item, true);
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

        void ExportDataSetCollectionToExcel(List<KeyValuePair<string, DataSet>> dataSetCollection, string usersSaveDirectoryPath, string rootDirectoryName, string exportOption, string exportFileName = null)
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
                            var existingWorksheetsCount = pck.Workbook.Worksheets.Count;
                            for (int i = 0; i < existingWorksheetsCount; existingWorksheetsCount--)
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
            else if (exportOption == "ExportFirstSheetPerDomainsInOneFile")
            {
                var resolvedFilePath = resolvedDirectoryPath + exportFileName + ".xlsx";
                using (ExcelPackage pck = new ExcelPackage(resolvedFilePath))
                {
                    if (pck.Workbook.Worksheets.Count > 0)
                    {
                        var existingWorksheetsCount = pck.Workbook.Worksheets.Count;
                        for (int i = 0; i < existingWorksheetsCount; existingWorksheetsCount--)
                        {
                            pck.Workbook.Worksheets.Delete(i);
                        }
                    }
                    foreach (var item in dataSetCollection)
                    {
                        try
                        {
                            ExcelWorksheet ws = pck.Workbook.Worksheets.Add(item.Key);
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
                    if (filterDomainsTextBox.SelectedText.HasContent())
                    {
                        filterDomainsTextBox.Text = filterDomainsTextBox.Text.Replace(filterDomainsTextBox.SelectedText, "");
                    }
                    else
                    {
                        filterDomainsTextBox.Text = filterDomainsTextBox.Text.Substring(0, filterDomainsTextBox.Text.Length - 1);
                    }
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
                    MessageBox.Show("Display option is not available");
                    return;
                }
                //if (selectedDisplayOption.Value == "OnlyListDomainsWithAffectedRecords")
                //{
                //    exportOptionsComboBox.SelectedItem = ExportOptions.Find(a => a.Value == "ExportAsIs");
                //    exportOptionsComboBox.Enabled = false;
                //}
                //else
                //{
                //    exportOptionsComboBox.Enabled = true;
                //}
            }
        }
    }
}