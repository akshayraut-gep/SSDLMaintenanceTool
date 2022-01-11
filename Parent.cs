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
        }

        private void openFileCorrutValidatorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fileCorruptValidator = new FileCorruptValidator();
            fileCorruptValidator.ShowDialog(this);
        }

        private void openPredefinedQueryEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fileCorruptValidator = new PredefinedQueriesEditor();
            fileCorruptValidator.ShowDialog(this);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var fileCorruptValidator = new PredefinedQueriesEditor();
            fileCorruptValidator.ShowDialog(this);
        }
    }
}
