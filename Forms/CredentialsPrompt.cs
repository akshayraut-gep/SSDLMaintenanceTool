using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SSDLMaintenanceTool.Forms
{
    public partial class CredentialsPrompt : Form
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public CredentialsPrompt()
        {
            InitializeComponent();
        }

        private void submitButton_Click(object sender, EventArgs e)
        {
            this.UserName = this.userNameTextBox.Text;
            this.Password = this.passwordTextBox.Text;
            this.Close();
        }

        private void userNameTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsLetter(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void passwordTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsLetter(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
        }
    }
}
