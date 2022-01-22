using SSDLMaintenanceTool.Helpers;
using SSDLMaintenanceTool.Implementations;
using SSDLMaintenanceTool.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Windows.Forms;

namespace SSDLMaintenanceTool.Forms
{
    public partial class QueryExecutioner : Form
    {
        public List<EnvironmentSetting> Environments { get; set; }
        public QueryExecutioner()
        {
            InitializeComponent();
        }

        private void QueryExecutioner_Load(object sender, EventArgs e)
        {
            this.connectionStringsComboBox.DataSource = ConnectionStringHandler.ConnectionStrings;
            this.connectionStringsComboBox.ValueMember = "Name";
            this.connectionStringsComboBox.DisplayMember = "DisplayName";
            this.connectionStringsComboBox.SelectedIndex = -1;
            this.connectionStringsComboBox.Text = "Select connection";
            this.connectionStringsComboBox.SelectedIndexChanged += connectionStringsComboBox_SelectedIndexChanged;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (tbQuery.Text == null || tbQuery.Text.Trim() == "")
                MessageBox.Show("Query Text is mandatory");

            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LoadSSDLDomains();
        }

        private void LoadSSDLDomains()
        {
            if (connectionStringsComboBox.SelectedValue == null)
                MessageBox.Show("Select any Envrionment Micro DB connection");

            var connectionString = ConnectionStringHandler.ConnectionStrings.FirstOrDefault(a => a.Name == connectionStringsComboBox.SelectedValue.ToString());
            if (connectionString == null)
                MessageBox.Show("Connection string not available");

            var copyConnection = ConnectionStringHandler.GetDeepCopy(connectionString);
            copyConnection.Database = "master";

            if (copyConnection.IsInputCredentialsRequired)
            {
                var credentialsPrompt = new CredentialsPrompt();
                credentialsPrompt.ShowDialog(this);
                copyConnection.UserName = credentialsPrompt.UserName;
                copyConnection.Password = credentialsPrompt.Password;
            }

            var resultSet = new DAO().GetData("SELECT Name FROM sys.databases WHERE Name LIKE '%[_]SSDL'", copyConnection);
            if (resultSet == null || resultSet.Tables == null || resultSet.Tables.Count == 0 || resultSet.Tables[0] == null || resultSet.Tables[0].Rows.Count == 0)
                MessageBox.Show("No SSDL domains found");

            this.domainsCheckListBox.SelectedIndexChanged -= DomainsCheckListBox_SelectedIndexChanged;
            this.domainsCheckListBox.DataSource = resultSet.Tables[0];
            this.domainsCheckListBox.ValueMember = "Name";
            this.domainsCheckListBox.DisplayMember = "Name";
            this.domainsCheckListBox.SelectedIndex = -1;
            this.domainsCheckListBox.Text = "Select domain";
            this.domainsCheckListBox.SelectedIndexChanged += DomainsCheckListBox_SelectedIndexChanged;


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
    }
}
