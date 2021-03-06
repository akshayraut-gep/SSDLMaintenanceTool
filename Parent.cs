using SSDLMaintenanceTool.Forms;
using SSDLMaintenanceTool.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SSDLMaintenanceTool
{
    public partial class Parent : Form
    {
        public Parent()
        {
            InitializeComponent();
            new EPPlugExcelHelper();
        }

        #region File Corrupt Validator
        private void OpenFileCorruptValidator()
        {
            var fileCorruptValidator = new FileCorruptValidator();
            fileCorruptValidator.Show();
        }

        private void fileCorruptValidator_Click(object sender, EventArgs e)
        {
            OpenFileCorruptValidator();
        }

        private void openFileCorrutValidatorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileCorruptValidator();
        }
        #endregion

        #region Predefined Query Editor
        private void OpenPredefinedQueryEditor()
        {
            var predefinedQueriesEditor = new PredefinedQueriesEditor();
            predefinedQueriesEditor.Show();
        }

        private void openPredefinedQueryEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenPredefinedQueryEditor();
        }

        private void queryEditorButton_Click(object sender, EventArgs e)
        {
            OpenPredefinedQueryEditor();
        }
        #endregion

        #region Query Executioner
        private void OpenQueryExecutioner()
        {
            var queryExecutionerForm = new QueryExecutioner();
            queryExecutionerForm.Show();
        }

        private void queryExecutorButton_Click(object sender, EventArgs e)
        {
            OpenQueryExecutioner();
        }

        private void openQueryExecutionerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenQueryExecutioner();
        }
        #endregion

        #region GEP Cache Clear
        private void OpenGEPCacheClear()
        {
            var gepCacheClear = new GEPCacheClear();
            gepCacheClear.Show();
        }
        private void cacheClearAtGEPButton_Click(object sender, EventArgs e)
        {
            OpenGEPCacheClear();
        }
        #endregion

        private void gEPCacheClearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenGEPCacheClear();
        }

        private void OpenServiceBus()
        {
            var serviceBus = new ServiceBus();
            serviceBus.Show();
        }

        private void serviceBusButton_Click(object sender, EventArgs e)
        {
            OpenServiceBus();
        }

        private void serviceBusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenServiceBus();
        }
    }
}
