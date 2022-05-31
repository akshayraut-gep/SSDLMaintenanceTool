using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using SSDLMaintenanceTool.Helpers;
using SSDLMaintenanceTool.Implementations;
using SSDLMaintenanceTool.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SSDLMaintenanceTool.Forms
{
    public partial class ServiceBus : Form
    {
        public DAO dao;
        public ConnectionStringHandler _configDBConnectionStringHandler;
        public ConnectionStringHandler _microDBConnectionStringHandler;
        private readonly SynchronizationContext synchronizationContext;
        public List<Domain> Domains { get; set; }
        public List<Domain> BackupDomains { get; set; }
        public bool IsAllDomainsSelected { get; set; }
        public DataSet DomainsTableSet { get; set; }
        Dictionary<string, string> keyValuePairs;
        public List<ServiceBusLog> ServiceBusLog { get; set; }
        public ConnectionDetails microDBConnection { get; private set; }

        public ServiceBus()
        {
            InitializeComponent();
            dao = new DAO();
            _configDBConnectionStringHandler = new ConnectionStringHandler(@"Assets\service-bus-connection-strings.json");
            _microDBConnectionStringHandler = new ConnectionStringHandler(@"Assets\connection-strings.json");
            synchronizationContext = SynchronizationContext.Current; //context from UI thread
        }

        private void getMessagesButton_Click(object sender, EventArgs e)
        {
            var connectionDetails = GetSelectedConnectionDetails(out var valiationMessage);
            if (valiationMessage.HasContent())
            {
                MessageBox.Show(valiationMessage);
                return;
            }
            if (domainsCheckListBox.CheckedItems.Count == 0)
            {
                MessageBox.Show("Please select a Buyer Partner");
                return;
            }

            var selectedDomain = (domainsCheckListBox.CheckedItems[0] as Domain);

            var copyConnection = _configDBConnectionStringHandler.GetDeepCopy(connectionDetails);
            copyConnection.Database = "master";

            LoadSSDLDomains(copyConnection, selectedDomain.Name);

            if (DomainsTableSet != null && DomainsTableSet.Tables.Count > 0 && DomainsTableSet.Tables[0].Rows.Count > 0)
            {
                microDBConnection = _configDBConnectionStringHandler.GetDeepCopy(connectionDetails);
                microDBConnection.Database = DomainsTableSet.Tables[0].Rows[0][0].ToString();
                microDBConnection.IsMultiTenant = true;

                if (microDBConnection != null)
                {
                    LoadResourceKeyValues(microDBConnection);

                    Task.Run(async () =>
                    {
                        await ProcessQueueResponse(selectedDomain);
                    });
                }
            }
        }

        private void connectionStringsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            domainsCheckListBox.DataSource = null;
            IsAllDomainsSelected = false;
        }

        private void ServiceBus_Load(object sender, EventArgs e)
        {
            connectionStringsComboBox.DataSource = _configDBConnectionStringHandler.ConnectionStrings;
            connectionStringsComboBox.ValueMember = "Name";
            connectionStringsComboBox.DisplayMember = "DisplayName";
            connectionStringsComboBox.SelectedIndex = -1;
            connectionStringsComboBox.Text = "Select a connection";
            domainsCheckListBox.SelectionMode = SelectionMode.One;
        }

        private void LoadResourceKeyValues(ConnectionDetails connectionDetails)
        {
            keyValuePairs = new Dictionary<string, string>();
            var dataSet = dao.GetData("SELECT [ConfigKey], [ConfigValue] FROM [SSDL].[SPEND_RESOURCES]", connectionDetails);

            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    var configKey = row["ConfigKey"].ToString();
                    if (!keyValuePairs.ContainsKey(configKey))
                    {
                        keyValuePairs.Add(configKey, row["ConfigValue"].ToString());
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

            var connectionDetail = _configDBConnectionStringHandler.ConnectionStrings.FirstOrDefault(a => a.Name == connectionStringsComboBox.SelectedValue.ToString());
            if (connectionDetail == null)
            {
                validationMessage = "Connection string not available";
                return null;
            }
            return connectionDetail;
        }

        private void loadDomainsButton_Click(object sender, EventArgs e)
        {
            var connectionDetails = GetSelectedConnectionDetails(out var valiationMessage);
            if (valiationMessage.HasContent())
            {
                MessageBox.Show(valiationMessage);
                return;
            }

            var copyConnection = _configDBConnectionStringHandler.GetDeepCopy(connectionDetails);

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

            //Send the update to our UI thread
            Task.Run(() =>
            {
                try
                {
                    LoadSSDLDomains(copyConnection);
                    ConvertToDomainModel();

                    synchronizationContext.Post(new SendOrPostCallback(o =>
                    {
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
                    }), null);
                }
            });
        }

        private void LoadSSDLDomains(ConnectionDetails copyConnection)
        {
            var query = "SELECT PARTNERCODE, PROJECT_NAME, IS_SUBSCRIPTION_EXISTS FROM SPEND_PROJECT_MST";

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
                    domain.Name = row["PROJECT_NAME"].ToString();
                    domain.IsSubscriptionExists = Convert.ToBoolean(row["IS_SUBSCRIPTION_EXISTS"]);
                    domain.BuyerPartnerCode = row["PARTNERCODE"].ToString();
                    domain.DisplayName = domain.Name + " " + domain.BuyerPartnerCode;
                    BackupDomains.Add(domain);
                }
            }
            MapDomainNewCopy(BackupDomains);
        }

        private void LoadingDomainsFailed(Exception ex)
        {
            MessageBox.Show(ex.Message);
        }

        private void PopulateDomainsCheckListBox()
        {
            domainsCheckListBox.SelectedIndexChanged -= DomainsCheckListBox_SelectedIndexChanged;
            domainsCheckListBox.DataSource = Domains;
            domainsCheckListBox.ValueMember = "BuyerPartnerCode";
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
            MapDomainNewCopy(filteredDomains);
            PopulateDomainsCheckListBox();
        }

        private void MapDomainNewCopy(List<Domain> filteredDomains)
        {
            Domains = filteredDomains.Select(a => new Domain()
            {
                Name = a.Name,
                DatabaseName = a.DatabaseName,
                DisplayName = a.DisplayName,
                IsChecked = a.IsChecked,
                BuyerPartnerCode = a.BuyerPartnerCode,
                IsSubscriptionExists = a.IsSubscriptionExists
            }).ToList();
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

        public async Task ProcessQueueResponse(Domain bpc)
        {
            try
            {
                if (bpc.IsSubscriptionExists)
                {
                    //Process Queue
                    await ReceiveMessagesFromSubscriptionAsync(bpc);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public async Task ReceiveMessagesFromSubscriptionAsync(Domain bpc)
        {
            try
            {

                //string connectionString = Constants.connectionString_ServiceBus;
                //string topicName = Constants.topicName_ServiceBus;

                string connectionString = keyValuePairs["spend-ss-servicebusconnectionstring"];
                string topicName = keyValuePairs["spend-ss-servicebustopicname"];
                //string topicName = "adb-topics-poc123";

                //topicName = "adb-topics-poc789";
                //subscriptionName = 914;            

                //var adminClient = new ServiceBusAdministrationClient(connectionString);

                //await using (ServiceBusClient client = new ServiceBusClient(connectionString))
                ServiceBusClient client = new ServiceBusClient(connectionString);

                // create the options to use for configuring the processor
                var options = new ServiceBusSessionProcessorOptions
                {
                    // By default after the message handler returns, the processor will complete the message
                    // If I want more fine-grained control over settlement, I can set this to false.
                    AutoCompleteMessages = false,

                    // I can also allow for processing multiple sessions
                    MaxConcurrentSessions = 1,

                    // By default or when AutoCompleteMessages is set to true, the processor will complete the message after executing the message handler
                    // Set AutoCompleteMessages to false to [settle messages](https://docs.microsoft.com/en-us/azure/service-bus-messaging/message-transfers-locks-settlement#peeklock) on your own.
                    // In both cases, if the message handler throws an exception without settling the message, the processor will abandon the message.
                    MaxConcurrentCallsPerSession = 1,

                    // Processing can be optionally limited to a subset of session Ids.
                    SessionIds = { bpc.BuyerPartnerCode.ToString() }
                };

                // create a processor that we can use to process the messages
                ServiceBusSessionProcessor _processor = client.CreateSessionProcessor(topicName, bpc.BuyerPartnerCode.ToString(), options);

                ServiceBusLog = new List<ServiceBusLog>();
                // add handler to process messages
                _processor.ProcessMessageAsync += MessageHandler;

                // add handler to process any errors
                _processor.ProcessErrorAsync += ErrorHandler;

                //if (!bpc.IsSubscriptionLock)
                //{

                // start processing 
                await _processor.StartProcessingAsync();

                TimeSpan stopprocessing = new TimeSpan(0, 0, 14, 55);

                await Task.Delay(stopprocessing);

                await _processor.StopProcessingAsync();

                if (!_processor.IsProcessing)
                {
                    TimeSpan closeprocess = new TimeSpan(0, 0, 5, 0);
                    await Task.Delay(closeprocess);
                    await _processor.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private List<Domain> ConvertToDomainDatabases()
        {
            List<Domain> checkedDomains = new List<Domain>();
            foreach (var checkedDomainItem in domainsCheckListBox.CheckedItems)
            {
                var checkedDomain = checkedDomainItem as Domain;
                if (checkedDomain != null && checkedDomain.Name != "SelectAll")
                {
                    checkedDomains.Add(checkedDomain);
                }
            }
            return checkedDomains;
        }

        async Task MessageHandler(ProcessSessionMessageEventArgs args)
        {
            try
            {
                string body = args.Message.Body.ToString();

                var objDataBricksResponse = JsonConvert.DeserializeObject<DataBricksJobResponse>(body);
                var userExecutionContext = objDataBricksResponse.return_object.UserContext;

                var serviceBusLog = new ServiceBusLog();
                serviceBusLog.MessageId = args.Message.MessageId;
                serviceBusLog.CorrelationId = args.Message.CorrelationId;
                serviceBusLog.DeliveryCount = args.Message.DeliveryCount;
                serviceBusLog.EnqueuedSequenceNumber = args.Message.EnqueuedSequenceNumber;
                serviceBusLog.EnqueuedTime = args.Message.EnqueuedTime;
                serviceBusLog.ExpiresAt = args.Message.ExpiresAt;
                serviceBusLog.LockedUntil = args.Message.LockedUntil;
                serviceBusLog.ScheduledEnqueueTime = args.Message.ScheduledEnqueueTime;
                serviceBusLog.SequenceNumber = args.Message.SequenceNumber;
                serviceBusLog.SessionId = args.Message.SessionId;

                ServiceBusLog.Add(serviceBusLog);

                //Send the update to our UI thread
                synchronizationContext.Post(new SendOrPostCallback(o =>
                {
                    logsDataGridView.DataSource = ServiceBusLog;
                    logsDataGridView.Refresh();
                }), null);

                var utcNow = DateTime.UtcNow;
                var timeDiff = utcNow - args.Message.EnqueuedTime.UtcDateTime;

                if (microDBConnection != null && (timeDiff.TotalHours > 1))
                {
                    var resultSet = dao.GetData($"select 1 from SSDL.DATABRICKS_RESPONSE where JobID = {objDataBricksResponse.job_id} and ActivityID = {objDataBricksResponse.activity_id} and SubmissionID = {objDataBricksResponse.submissionid} and EventId = {objDataBricksResponse.event_id} and Status = '{objDataBricksResponse.status}'", microDBConnection);

                    if (resultSet != null && resultSet.Tables.Count > 0 && resultSet.Tables[0].Rows.Count > 0)
                    {
                        // complete the message. messages is deleted from the queue. 
                        if (args.Message.LockedUntil > DateTime.UtcNow)
                        {
                            await args.CompleteMessageAsync(args.Message);
                        }
                    }
                    else
                    {
                        await args.AbandonMessageAsync(args.Message);
                    }
                }
                else
                {
                    await args.AbandonMessageAsync(args.Message);
                }

                await Task.Delay(5000);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        Task ErrorHandler(ProcessErrorEventArgs args)
        {
            //SpendLoggerHelper.LogError(args.Exception.ToString(), string.Format("Service Queue error for {0}, {1}", objDataBricksResponse.JobId, objAutomationJobEntities.TransactionId), objAutomationJobEntities.PartnerCode, objAutomationJobEntities.ContactCode, "Automation");

            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }

        private void LoadSSDLDomains(ConnectionDetails copyConnection, string domainNameFilter)
        {
            var selectClause = "Name";
            string filterCondition;
            var query = "";

            if (domainNameFilter.HasContent())
            {
                filterCondition = $"Name LIKE '{domainNameFilter}[_]SSDL'";
                query = $"SELECT {selectClause} FROM sys.databases WHERE {filterCondition}";
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

    }
}
