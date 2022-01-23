using Microsoft.WindowsAPICodePack.Dialogs;
using OfficeOpenXml;
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
            this.connectionStringsComboBox.Text = "Select connection";
            this.connectionStringsComboBox.SelectedIndexChanged += connectionStringsComboBox_SelectedIndexChanged;
            this.exportDomainsButton.Enabled = false;
        }

        private void executeQueryButton_Click(object sender, EventArgs e)
        {
            if (queryTextBox.Text == null || queryTextBox.Text.Trim() == "")
            {
                MessageBox.Show("Query Text is mandatory");
                return;
            }

            var connectionDetails = GetSelectedConnectionDetails(out var valiationMessage);
            if (valiationMessage.HasContent())
            {
                MessageBox.Show(valiationMessage);
                return;
            }

            var checkDomains = ConvertToDomainDatabases();

            //MethodInvoker methodInvoker = delegate
            //{
            //    QueryResultDataSet = ExecuteQuery(connectionDetails, checkDomains);
            //};
            //methodInvoker.BeginInvoke(Done, null);

            EnableUIForQueryExecution();

            Task.Run(() =>
            {
                try
                {
                    QueryResultDataSet = ExecuteQuery(connectionDetails, checkDomains);

                    //Send the update to our UI thread  
                    synchronizationContext.Post(new SendOrPostCallback(o =>
                    {
                        EnableUIForQueryExecution(true);
                        QueryCompleted();
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
                        EnableUIForQueryExecution(true);
                    }), null);
                }
            });
        }

        private void QueryFailed(Exception ex)
        {
            MessageBox.Show(ex.Message);
        }

        private void QueryCompleted()
        {
            if (QueryResultDataSet == null || QueryResultDataSet.Tables.Count == 0)
            {
                MessageBox.Show("No records found in any of the SSDL domains");
                return;
            }
            CommonOpenFileDialog fileSaveDirectoryDialog = new CommonOpenFileDialog("Export Query Result - Select folder to export the file");
            fileSaveDirectoryDialog.IsFolderPicker = true;
            var saveFileDialogResult = fileSaveDirectoryDialog.ShowDialog(this.Handle);
            if (saveFileDialogResult == CommonFileDialogResult.Ok)
            {
                ExportDataSetToExcel(QueryResultDataSet, fileSaveDirectoryDialog.FileName, GlobalConstants.QueryExecutionerQueryResultDirectory);
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

        public DataSet ExecuteQuery(ConnectionDetails connectionDetails, List<Domain> checkedDomains)
        {
            var dataSet = new DataSet();

            if (connectionDetails.IsMultiTenant)
            {
                var copyConnection = _connectionStringHandler.GetDeepCopy(connectionDetails);
                foreach (var checkedDomain in checkedDomains)
                {
                    copyConnection.Database = checkedDomain.DatabaseName;
                    var resultSet = DAO.GetData(queryTextBox.Text, copyConnection);
                    if (resultSet == null || resultSet.Tables == null || resultSet.Tables.Count == 0 || resultSet.Tables[0] == null || resultSet.Tables[0].Rows.Count == 0)
                    {
                        MessageBox.Show("No data found");
                        //return;
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
                    MessageBox.Show("No data found");
                    //return;
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

            EnableUIForDomainList();

            //Send the update to our UI thread
            Task.Run(() =>
            {
                try
                {
                    LoadSSDLDomains(copyConnection);
                    ConvertToDomainModel();
                    synchronizationContext.Post(new SendOrPostCallback(o =>
                    {
                        EnableUIForDomainList(true);
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
                        EnableUIForDomainList(true);
                    }), null);
                }
            });
        }

        private void LoadingDomainsFailed(Exception ex)
        {
            MessageBox.Show(ex.Message);
        }

        private void EnableUIForDomainList(bool value = false)
        {
            connectionStringsComboBox.Enabled = value;
            loadDomainsButton.Enabled = value;
            domainsCheckListBox.SelectionMode = value ? SelectionMode.One : SelectionMode.None;
            executeQueryButton.Enabled = value;
            exportQueryResultButton.Enabled = value;
        }

        private void EnableUIForQueryExecution(bool value = false)
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
            if (connectionStringsComboBox.SelectedValue != null)
            {
                var matchingQueryType = ConnectionStringHandler.ConnectionStrings.FirstOrDefault(a => a.Name == connectionStringsComboBox.SelectedValue.ToString());
            }
        }

        private void exportDomainsButton_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog fileSaveDirectoryDialog = new CommonOpenFileDialog("Export Domains - Select folder to export the file(s)");
            fileSaveDirectoryDialog.IsFolderPicker = true;
            var saveFileDialogResult = fileSaveDirectoryDialog.ShowDialog(this.Handle);
            if (saveFileDialogResult == CommonFileDialogResult.Ok)
            {
                var connectionDetails = GetSelectedConnectionDetails(out var validationMessage);

                ExportDataTableToExcel(DomainsTableSet.Tables[0], fileSaveDirectoryDialog.FileName, GlobalConstants.QueryExecutionerDomainExportDirectory, connectionDetails.Environment + "-" + connectionDetails.Region + "-" + connectionDetails.Instance);
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
    }
}
