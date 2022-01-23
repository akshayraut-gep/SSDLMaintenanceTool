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
            this.exportQueryResultButton = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.exportDomainsButton = new System.Windows.Forms.Button();
            this.queryExecutionBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // executeQueryButton
            // 
            this.executeQueryButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.executeQueryButton.Location = new System.Drawing.Point(344, 490);
            this.executeQueryButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.executeQueryButton.Name = "executeQueryButton";
            this.executeQueryButton.Size = new System.Drawing.Size(144, 55);
            this.executeQueryButton.TabIndex = 0;
            this.executeQueryButton.Text = "Execute";
            this.executeQueryButton.UseVisualStyleBackColor = true;
            this.executeQueryButton.Click += new System.EventHandler(this.executeQueryButton_Click);
            // 
            // queryTextBox
            // 
            this.queryTextBox.Location = new System.Drawing.Point(17, 323);
            this.queryTextBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.queryTextBox.Multiline = true;
            this.queryTextBox.Name = "queryTextBox";
            this.queryTextBox.Size = new System.Drawing.Size(716, 163);
            this.queryTextBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label1.Location = new System.Drawing.Point(12, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 29);
            this.label1.TabIndex = 3;
            this.label1.Text = "Domain";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label2.Location = new System.Drawing.Point(12, 277);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 29);
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
            this.domainsCheckListBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.domainsCheckListBox.FormattingEnabled = true;
            this.domainsCheckListBox.Items.AddRange(new object[] {
            "A",
            "B",
            "C"});
            this.domainsCheckListBox.Location = new System.Drawing.Point(169, 104);
            this.domainsCheckListBox.Name = "domainsCheckListBox";
            this.domainsCheckListBox.Size = new System.Drawing.Size(370, 166);
            this.domainsCheckListBox.TabIndex = 5;
            this.domainsCheckListBox.SelectedIndexChanged += new System.EventHandler(this.DomainsCheckListBox_SelectedIndexChanged);
            // 
            // loadDomainsButton
            // 
            this.loadDomainsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.loadDomainsButton.Location = new System.Drawing.Point(169, 61);
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
            this.connectionStringsComboBox.FormattingEnabled = true;
            this.connectionStringsComboBox.Items.AddRange(new object[] {
            "Dev",
            "QC",
            "UAT",
            "Production"});
            this.connectionStringsComboBox.Location = new System.Drawing.Point(169, 12);
            this.connectionStringsComboBox.Name = "connectionStringsComboBox";
            this.connectionStringsComboBox.Size = new System.Drawing.Size(370, 28);
            this.connectionStringsComboBox.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label3.Location = new System.Drawing.Point(12, 11);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(147, 29);
            this.label3.TabIndex = 8;
            this.label3.Text = "Environment";
            // 
            // exportQueryResultButton
            // 
            this.exportQueryResultButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.exportQueryResultButton.Location = new System.Drawing.Point(494, 490);
            this.exportQueryResultButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.exportQueryResultButton.Name = "exportQueryResultButton";
            this.exportQueryResultButton.Size = new System.Drawing.Size(239, 55);
            this.exportQueryResultButton.TabIndex = 9;
            this.exportQueryResultButton.Text = "Export to Excel";
            this.exportQueryResultButton.UseVisualStyleBackColor = true;
            this.exportQueryResultButton.Click += new System.EventHandler(this.exportQueryResultButton_Click);
            // 
            // exportDomainsButton
            // 
            this.exportDomainsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.exportDomainsButton.Location = new System.Drawing.Point(358, 61);
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
            // QueryExecutioner
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(768, 682);
            this.Controls.Add(this.exportDomainsButton);
            this.Controls.Add(this.exportQueryResultButton);
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
        private System.Windows.Forms.Button exportQueryResultButton;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button exportDomainsButton;
        private System.ComponentModel.BackgroundWorker queryExecutionBackgroundWorker;
    }
}