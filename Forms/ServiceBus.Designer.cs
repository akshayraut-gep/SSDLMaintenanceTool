namespace SSDLMaintenanceTool.Forms
{
    partial class ServiceBus
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
            this.bpcTextBox = new System.Windows.Forms.TextBox();
            this.bpcLabel = new System.Windows.Forms.Label();
            this.logsDataGridView = new System.Windows.Forms.DataGridView();
            this.getMessagesButton = new System.Windows.Forms.Button();
            this.connectionStringsComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.loadDomainsButton = new System.Windows.Forms.Button();
            this.domainsCheckListBox = new System.Windows.Forms.CheckedListBox();
            this.filterDomainsTextBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.logsDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // bpcTextBox
            // 
            this.bpcTextBox.Location = new System.Drawing.Point(120, 51);
            this.bpcTextBox.Name = "bpcTextBox";
            this.bpcTextBox.Size = new System.Drawing.Size(249, 26);
            this.bpcTextBox.TabIndex = 0;
            // 
            // bpcLabel
            // 
            this.bpcLabel.AutoSize = true;
            this.bpcLabel.Location = new System.Drawing.Point(35, 54);
            this.bpcLabel.Name = "bpcLabel";
            this.bpcLabel.Size = new System.Drawing.Size(41, 20);
            this.bpcLabel.TabIndex = 1;
            this.bpcLabel.Text = "BPC";
            // 
            // logsDataGridView
            // 
            this.logsDataGridView.AllowUserToAddRows = false;
            this.logsDataGridView.AllowUserToDeleteRows = false;
            this.logsDataGridView.AllowUserToOrderColumns = true;
            this.logsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.logsDataGridView.Location = new System.Drawing.Point(54, 384);
            this.logsDataGridView.Name = "logsDataGridView";
            this.logsDataGridView.ReadOnly = true;
            this.logsDataGridView.RowHeadersWidth = 62;
            this.logsDataGridView.RowTemplate.Height = 28;
            this.logsDataGridView.Size = new System.Drawing.Size(760, 440);
            this.logsDataGridView.TabIndex = 2;
            // 
            // getMessagesButton
            // 
            this.getMessagesButton.Location = new System.Drawing.Point(392, 41);
            this.getMessagesButton.Name = "getMessagesButton";
            this.getMessagesButton.Size = new System.Drawing.Size(187, 46);
            this.getMessagesButton.TabIndex = 3;
            this.getMessagesButton.Text = "Get messages";
            this.getMessagesButton.UseVisualStyleBackColor = true;
            this.getMessagesButton.Click += new System.EventHandler(this.getMessagesButton_Click);
            // 
            // connectionStringsComboBox
            // 
            this.connectionStringsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.connectionStringsComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.connectionStringsComboBox.FormattingEnabled = true;
            this.connectionStringsComboBox.Items.AddRange(new object[] {
            "Dev",
            "QC",
            "UAT",
            "Production"});
            this.connectionStringsComboBox.Location = new System.Drawing.Point(131, 118);
            this.connectionStringsComboBox.Name = "connectionStringsComboBox";
            this.connectionStringsComboBox.Size = new System.Drawing.Size(390, 28);
            this.connectionStringsComboBox.TabIndex = 10;
            this.connectionStringsComboBox.SelectedIndexChanged += new System.EventHandler(this.connectionStringsComboBox_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(16, 172);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 20);
            this.label1.TabIndex = 9;
            this.label1.Text = "Domain";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(16, 121);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(109, 20);
            this.label3.TabIndex = 11;
            this.label3.Text = "Environment";
            // 
            // loadDomainsButton
            // 
            this.loadDomainsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.loadDomainsButton.Location = new System.Drawing.Point(527, 115);
            this.loadDomainsButton.Name = "loadDomainsButton";
            this.loadDomainsButton.Size = new System.Drawing.Size(183, 32);
            this.loadDomainsButton.TabIndex = 12;
            this.loadDomainsButton.Text = "Load all domains";
            this.loadDomainsButton.UseVisualStyleBackColor = true;
            this.loadDomainsButton.Click += new System.EventHandler(this.loadDomainsButton_Click);
            // 
            // domainsCheckListBox
            // 
            this.domainsCheckListBox.CheckOnClick = true;
            this.domainsCheckListBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.domainsCheckListBox.FormattingEnabled = true;
            this.domainsCheckListBox.Items.AddRange(new object[] {
            "A",
            "B",
            "C"});
            this.domainsCheckListBox.Location = new System.Drawing.Point(131, 201);
            this.domainsCheckListBox.Name = "domainsCheckListBox";
            this.domainsCheckListBox.Size = new System.Drawing.Size(579, 165);
            this.domainsCheckListBox.TabIndex = 13;
            // 
            // filterDomainsTextBox
            // 
            this.filterDomainsTextBox.Location = new System.Drawing.Point(131, 169);
            this.filterDomainsTextBox.Name = "filterDomainsTextBox";
            this.filterDomainsTextBox.Size = new System.Drawing.Size(579, 26);
            this.filterDomainsTextBox.TabIndex = 14;
            this.filterDomainsTextBox.TextChanged += new System.EventHandler(this.filterDomainsTextBox_TextChanged);
            this.filterDomainsTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.filterDomainsTextBox_KeyPress);
            // 
            // ServiceBus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(983, 675);
            this.Controls.Add(this.filterDomainsTextBox);
            this.Controls.Add(this.domainsCheckListBox);
            this.Controls.Add(this.loadDomainsButton);
            this.Controls.Add(this.connectionStringsComboBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.getMessagesButton);
            this.Controls.Add(this.logsDataGridView);
            this.Controls.Add(this.bpcLabel);
            this.Controls.Add(this.bpcTextBox);
            this.Name = "ServiceBus";
            this.Text = "ServiceBus";
            this.Load += new System.EventHandler(this.ServiceBus_Load);
            ((System.ComponentModel.ISupportInitialize)(this.logsDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox bpcTextBox;
        private System.Windows.Forms.Label bpcLabel;
        private System.Windows.Forms.DataGridView logsDataGridView;
        private System.Windows.Forms.Button getMessagesButton;
        private System.Windows.Forms.ComboBox connectionStringsComboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button loadDomainsButton;
        private System.Windows.Forms.CheckedListBox domainsCheckListBox;
        private System.Windows.Forms.TextBox filterDomainsTextBox;
    }
}