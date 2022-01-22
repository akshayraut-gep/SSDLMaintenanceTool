namespace SSDLMaintenanceTool.Forms
{
    partial class CredentialsPrompt
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.userNameTxBx = new System.Windows.Forms.TextBox();
            this.passwordTxBx = new System.Windows.Forms.TextBox();
            this.submitButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label1.Location = new System.Drawing.Point(22, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "User name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label2.Location = new System.Drawing.Point(22, 173);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 25);
            this.label2.TabIndex = 1;
            this.label2.Text = "Password";
            // 
            // userNameTxBx
            // 
            this.userNameTxBx.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.userNameTxBx.Location = new System.Drawing.Point(152, 59);
            this.userNameTxBx.Name = "userNameTxBx";
            this.userNameTxBx.Size = new System.Drawing.Size(227, 30);
            this.userNameTxBx.TabIndex = 2;
            // 
            // passwordTxBx
            // 
            this.passwordTxBx.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.passwordTxBx.Location = new System.Drawing.Point(152, 170);
            this.passwordTxBx.Name = "passwordTxBx";
            this.passwordTxBx.Size = new System.Drawing.Size(227, 30);
            this.passwordTxBx.TabIndex = 3;
            // 
            // submitButton
            // 
            this.submitButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.submitButton.Location = new System.Drawing.Point(152, 252);
            this.submitButton.Name = "submitButton";
            this.submitButton.Size = new System.Drawing.Size(99, 40);
            this.submitButton.TabIndex = 4;
            this.submitButton.Text = "Submit";
            this.submitButton.UseVisualStyleBackColor = true;
            this.submitButton.Click += new System.EventHandler(this.submitButton_Click);
            // 
            // CredentialsPrompt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(430, 359);
            this.Controls.Add(this.submitButton);
            this.Controls.Add(this.passwordTxBx);
            this.Controls.Add(this.userNameTxBx);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "CredentialsPrompt";
            this.Text = "CredentialsPrompt";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox userNameTxBx;
        private System.Windows.Forms.TextBox passwordTxBx;
        private System.Windows.Forms.Button submitButton;
    }
}