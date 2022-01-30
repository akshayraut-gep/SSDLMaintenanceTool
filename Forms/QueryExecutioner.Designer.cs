namespace SSDLMaintenanceTool.Forms
{
    partial class QueryExecutioner
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
            this.executeQueryButton = new System.Windows.Forms.Button();
            this.queryTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.domainsCheckListBox = new System.Windows.Forms.CheckedListBox();
            this.loadDomainsButton = new System.Windows.Forms.Button();
            this.connectionStringsComboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.exportDomainsButton = new System.Windows.Forms.Button();
            this.queryExecutionBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.savedTemplatesComboBox = new System.Windows.Forms.ComboBox();
            this.useSavedTemplateCheckBox = new System.Windows.Forms.CheckBox();
            this.filterDomainsTextBox = new System.Windows.Forms.TextBox();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // executeQueryButton
            // 
            this.executeQueryButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.executeQueryButton.Location = new System.Drawing.Point(392, 523);
            this.executeQueryButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.executeQueryButton.Name = "executeQueryButton";
            this.executeQueryButton.Size = new System.Drawing.Size(133, 32);
            this.executeQueryButton.TabIndex = 0;
            this.executeQueryButton.Text = "Execute";
            this.executeQueryButton.UseVisualStyleBackColor = true;
            this.executeQueryButton.Click += new System.EventHandler(this.executeQueryButton_Click);
            // 
            // queryTextBox
            // 
            this.queryTextBox.Location = new System.Drawing.Point(16, 356);
            this.queryTextBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.queryTextBox.Multiline = true;
            this.queryTextBox.Name = "queryTextBox";
            this.queryTextBox.Size = new System.Drawing.Size(582, 163);
            this.queryTextBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.label1.Location = new System.Drawing.Point(12, 66);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "Domain";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.label2.Location = new System.Drawing.Point(12, 324);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "Query";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.tabControl1.Location = new System.Drawing.Point(17, 550);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(200, 100);
            this.tabControl1.TabIndex = 4;
            // 
            // tabPage1
            // 
            this.tabPage1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.tabPage1.Location = new System.Drawing.Point(4, 38);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(192, 58);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
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
            this.domainsCheckListBox.Location = new System.Drawing.Point(116, 141);
            this.domainsCheckListBox.Name = "domainsCheckListBox";
            this.domainsCheckListBox.Size = new System.Drawing.Size(482, 165);
            this.domainsCheckListBox.TabIndex = 5;
            this.domainsCheckListBox.SelectedIndexChanged += new System.EventHandler(this.DomainsCheckListBox_SelectedIndexChanged);
            // 
            // loadDomainsButton
            // 
            this.loadDomainsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.loadDomainsButton.Location = new System.Drawing.Point(116, 60);
            this.loadDomainsButton.Name = "loadDomainsButton";
            this.loadDomainsButton.Size = new System.Drawing.Size(183, 32);
            this.loadDomainsButton.TabIndex = 6;
            this.loadDomainsButton.Text = "Load all domains";
            this.loadDomainsButton.UseVisualStyleBackColor = true;
            this.loadDomainsButton.Click += new System.EventHandler(this.loadDomainsButton_Click);
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
            this.connectionStringsComboBox.Location = new System.Drawing.Point(116, 12);
            this.connectionStringsComboBox.Name = "connectionStringsComboBox";
            this.connectionStringsComboBox.Size = new System.Drawing.Size(482, 28);
            this.connectionStringsComboBox.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.label3.Location = new System.Drawing.Point(12, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 20);
            this.label3.TabIndex = 8;
            this.label3.Text = "Environment";
            // 
            // exportDomainsButton
            // 
            this.exportDomainsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.exportDomainsButton.Location = new System.Drawing.Point(417, 60);
            this.exportDomainsButton.Name = "exportDomainsButton";
            this.exportDomainsButton.Size = new System.Drawing.Size(181, 32);
            this.exportDomainsButton.TabIndex = 10;
            this.exportDomainsButton.Text = "Export domains";
            this.exportDomainsButton.UseVisualStyleBackColor = true;
            this.exportDomainsButton.Click += new System.EventHandler(this.exportDomainsButton_Click);
            // 
            // queryExecutionBackgroundWorker
            // 
            this.queryExecutionBackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.queryExecutionBackgroundWorker_DoWork);
            // 
            // savedTemplatesComboBox
            // 
            this.savedTemplatesComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.savedTemplatesComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.savedTemplatesComboBox.FormattingEnabled = true;
            this.savedTemplatesComboBox.Items.AddRange(new object[] {
            "Dev",
            "QC",
            "UAT",
            "Production"});
            this.savedTemplatesComboBox.Location = new System.Drawing.Point(256, 321);
            this.savedTemplatesComboBox.Name = "savedTemplatesComboBox";
            this.savedTemplatesComboBox.Size = new System.Drawing.Size(342, 28);
            this.savedTemplatesComboBox.TabIndex = 11;
            this.savedTemplatesComboBox.SelectedIndexChanged += new System.EventHandler(this.savedTemplatesComboBox_SelectedIndexChanged);
            // 
            // useSavedTemplateCheckBox
            // 
            this.useSavedTemplateCheckBox.AutoSize = true;
            this.useSavedTemplateCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.useSavedTemplateCheckBox.Location = new System.Drawing.Point(116, 323);
            this.useSavedTemplateCheckBox.Name = "useSavedTemplateCheckBox";
            this.useSavedTemplateCheckBox.Size = new System.Drawing.Size(134, 24);
            this.useSavedTemplateCheckBox.TabIndex = 12;
            this.useSavedTemplateCheckBox.Text = "Use Template";
            this.useSavedTemplateCheckBox.UseVisualStyleBackColor = true;
            this.useSavedTemplateCheckBox.CheckedChanged += new System.EventHandler(this.useSavedTemplateCheckBox_CheckedChanged);
            // 
            // filterDomainsTextBox
            // 
            this.filterDomainsTextBox.Location = new System.Drawing.Point(117, 109);
            this.filterDomainsTextBox.Name = "filterDomainsTextBox";
            this.filterDomainsTextBox.Size = new System.Drawing.Size(481, 26);
            this.filterDomainsTextBox.TabIndex = 13;
            this.filterDomainsTextBox.TextChanged += new System.EventHandler(this.filterDomainsTextBox_TextChanged);
            // 
            // QueryExecutioner
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(610, 682);
            this.Controls.Add(this.filterDomainsTextBox);
            this.Controls.Add(this.useSavedTemplateCheckBox);
            this.Controls.Add(this.savedTemplatesComboBox);
            this.Controls.Add(this.exportDomainsButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.connectionStringsComboBox);
            this.Controls.Add(this.loadDomainsButton);
            this.Controls.Add(this.domainsCheckListBox);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.queryTextBox);
            this.Controls.Add(this.executeQueryButton);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "QueryExecutioner";
            this.Text = "DML Query Executioner";
            this.Load += new System.EventHandler(this.QueryExecutioner_Load);
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Button executeQueryButton;
        private System.Windows.Forms.TextBox queryTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;

        #endregion
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.CheckedListBox domainsCheckListBox;
        private System.Windows.Forms.Button loadDomainsButton;
        private System.Windows.Forms.ComboBox connectionStringsComboBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button exportDomainsButton;
        private System.ComponentModel.BackgroundWorker queryExecutionBackgroundWorker;
        private System.Windows.Forms.ComboBox savedTemplatesComboBox;
        private System.Windows.Forms.CheckBox useSavedTemplateCheckBox;
        private System.Windows.Forms.TextBox filterDomainsTextBox;
    }
}