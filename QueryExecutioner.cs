using SSDLMaintenanceTool.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Windows.Forms;

namespace SSDLMaintenanceTool
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
            var section = ConfigurationManager.GetSection("DevSmartConfiguration") as ConnectionString;
            var value = section.Server;
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
            
        }
    }
}
