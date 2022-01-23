using OfficeOpenXml;
using Ookii.Dialogs.WinForms;
using SSDLMaintenanceTool.Constants;
using SSDLMaintenanceTool.Helpers;
using SSDLMaintenanceTool.Implementations;
using SSDLMaintenanceTool.Models;
using System;
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
        private readonly SynchronizationContext synchronizationContext;
        public delegate bool MethodInvokerWithDataSetResult();
        public delegate void QueryCompletion();
        public Dictionary<string, QueryCompletion> QueryCompleted { get; set; }
        public DataSet QueryResultDataSet { get; set; }

        public QueryExecutioner()
        {
            InitializeComponent();
            DAO = new DAO();
            _connectionStringHandler = new ConnectionStringHandler();
            queryExecutionBackgroundWorker.WorkerReportsProgress = true;
            queryExecutionBackgroundWorker.WorkerSupportsCancellation = true;
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

            if (domainsCheckListBox.CheckedItems.Count == 0)
            {
                MessageBox.Show("Select at least one SSDL domain");
                return;
            }
            var checkDomains = ConvertToDomainDatabases();

            EnableUIForQueryExecution(false);

            Task.Run(() =>
            {
                try
                {
                    QueryResultDataSet = ExecuteQuery(connectionDetails, checkDomains, out var errorMessage);

                    //Send the update to our UI thread  
                    synchronizationContext.Post(new SendOrPostCallback(o =>
                    {
                        EnableUIForQueryExecution();
                        if (errorMessage.HasContent())
                        {
                            MessageBox.Show(errorMessage);
                            return;
                        }
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
                            QueryExecutionCompleted();
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

        private void QueryFailed(Exception ex)
        {
            MessageBox.Show(ex.Message);
        }

        private void QueryExecutionCompleted()
        {
            if (QueryResultDataSet == null || QueryResultDataSet.Tables.Count == 0)
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
                ExportDataSetToExcel(QueryResultDataSet, vistaFolderBrowserDialog.SelectedPath, GlobalConstants.QueryExecutionerQueryResultDirectory);
            }
        }
        private void PredefinedQueriesCompleted()
        {
            if (QueryResultDataSet == null || QueryResultDataSet.Tables.Count == 0)
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
                if (checkedDomain != null)
                    checkedDomains.Add(checkedDomain);
            }
            return checkedDomains;
        }

        public DataSet ExecuteQuery(ConnectionDetails connectionDetails, List<Domain> checkedDomains, out string errorMessage)
        {
            var dataSet = new DataSet();
            errorMessage = "";

            if (connectionDetails.IsMultiTenant)
            {
                var copyConnection = _connectionStringHandler.GetDeepCopy(connectionDetails);
                foreach (var checkedDomain in checkedDomains)
                {
                    copyConnection.Database = checkedDomain.DatabaseName;
                    var resultSet = DAO.GetData(queryTextBox.Text, copyConnection);
                    if (resultSet == null || resultSet.Tables == null || resultSet.Tables.Count == 0 || resultSet.Tables[0] == null || resultSet.Tables[0].Rows.Count == 0)
                    {
                        errorMessage = "No data found";
                        return null;
                    }
                    resultSet.Tables[0].TableName = copyConnection.Database;
                    dataSet.Tables.Add(resultSet.Tables[0].Copy());
                }
            }
            else
            {
                var resultSet = DAO.GetData(queryTextBox.Text, connectionDetails);
                if (resultSet == null || resultSet.Tables == null || resultSet.Tables.Count == 0 || resultSet.Tables[0] == null || resultSet.Tables[0].Rows.Count == 0)
                {
                    errorMessage = "No data found";
                    return null;
                }
            }
            return dataSet;
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
            Domains = new List<Domain>();
            foreach (DataRow row in DomainsTableSet.Tables[0].Rows)
            {
                Domain domain = new Domain();
                domain.Name = row["Name"].ToString();
                domain.DisplayName = row["Name"].ToString();
                domain.DatabaseName = row["Name"].ToString();
                Domains.Add(domain);
            }
        }

        private void PopulateDomainsCheckListBox()
        {
            this.domainsCheckListBox.DataSource = Domains;
            this.domainsCheckListBox.ValueMember = "Name";
            this.domainsCheckListBox.DisplayMember = "Name";
            this.domainsCheckListBox.SelectedIndex = -1;
            this.domainsCheckListBox.Text = "Select domain";
            this.exportDomainsButton.Enabled = true;
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

        void ExportPublishPredefinedQueries(DataSet dataSet, string usersSaveDirectoryPath, string rootDirectoryName)
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

                        //var totalColumns = worksheet.Columns.Count();
                        //for (int i = 1; i <= totalColumns - 2; i++)
                        //{
                        //    worksheet.Columns[i].AutoFit(4.64, 50.0);
                        //}

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

        private void exportQueryResultButton_Click(object sender, EventArgs e)
        {

        }

        private void queryExecutionBackgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
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
    }
}
