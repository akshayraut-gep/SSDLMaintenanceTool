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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.queryOutputTabControl = new System.Windows.Forms.TabControl();
            this.domainsCheckListBox = new System.Windows.Forms.CheckedListBox();
            this.loadDomainsButton = new System.Windows.Forms.Button();
            this.connectionStringsComboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.exportDomainsButton = new System.Windows.Forms.Button();
            this.savedTemplatesComboBox = new System.Windows.Forms.ComboBox();
            this.useSavedTemplateCheckBox = new System.Windows.Forms.CheckBox();
            this.filterDomainsTextBox = new System.Windows.Forms.TextBox();
            this.bottomStatusStrip = new System.Windows.Forms.StatusStrip();
            this.loadDomainsProgressBarToolStrip = new System.Windows.Forms.ToolStripProgressBar();
            this.loadDomainStatusLabelToolStrip = new System.Windows.Forms.ToolStripStatusLabel();
            this.queryProgressBarToolStrip = new System.Windows.Forms.ToolStripProgressBar();
            this.queryStatusLabelToolStrip = new System.Windows.Forms.ToolStripStatusLabel();
            this.successDomainsToolStrip = new System.Windows.Forms.ToolStripStatusLabel();
            this.failureDomainsToolStrip = new System.Windows.Forms.ToolStripStatusLabel();
            this.resultDomainsToolStrip = new System.Windows.Forms.ToolStripStatusLabel();
            this.asyncCheckBox = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.parallelismDegreeNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.queryRichTextBox = new System.Windows.Forms.RichTextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.exportOptionsComboBox = new System.Windows.Forms.ComboBox();
            this.displayOptionsComboBox = new System.Windows.Forms.ComboBox();
            this.mainPanel = new System.Windows.Forms.Panel();
            this.domainFilterOptionsComboBox = new System.Windows.Forms.ComboBox();
            this.logsPanel = new System.Windows.Forms.Panel();
            this.logsDataGridView = new System.Windows.Forms.DataGridView();
            this.bottomStatusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.parallelismDegreeNumericUpDown)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.mainPanel.SuspendLayout();
            this.logsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logsDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // executeQueryButton
            // 
            this.executeQueryButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.executeQueryButton.Location = new System.Drawing.Point(564, 588);
            this.executeQueryButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.executeQueryButton.Name = "executeQueryButton";
            this.executeQueryButton.Size = new System.Drawing.Size(133, 32);
            this.executeQueryButton.TabIndex = 0;
            this.executeQueryButton.Text = "Execute";
            this.executeQueryButton.UseVisualStyleBackColor = true;
            this.executeQueryButton.Click += new System.EventHandler(this.executeQueryButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(3, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "Domain";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(3, 320);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "Query";
            // 
            // queryOutputTabControl
            // 
            this.queryOutputTabControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.queryOutputTabControl.Location = new System.Drawing.Point(7, 642);
            this.queryOutputTabControl.Name = "queryOutputTabControl";
            this.queryOutputTabControl.SelectedIndex = 0;
            this.queryOutputTabControl.Size = new System.Drawing.Size(715, 226);
            this.queryOutputTabControl.TabIndex = 4;
            this.queryOutputTabControl.SizeChanged += new System.EventHandler(this.queryOutputTabControl_SizeChanged);
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
            this.domainsCheckListBox.Location = new System.Drawing.Point(118, 137);
            this.domainsCheckListBox.Name = "domainsCheckListBox";
            this.domainsCheckListBox.Size = new System.Drawing.Size(579, 165);
            this.domainsCheckListBox.TabIndex = 5;
            this.domainsCheckListBox.SelectedIndexChanged += new System.EventHandler(this.DomainsCheckListBox_SelectedIndexChanged);
            // 
            // loadDomainsButton
            // 
            this.loadDomainsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.loadDomainsButton.Location = new System.Drawing.Point(338, 56);
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
            this.connectionStringsComboBox.Location = new System.Drawing.Point(118, 8);
            this.connectionStringsComboBox.Name = "connectionStringsComboBox";
            this.connectionStringsComboBox.Size = new System.Drawing.Size(579, 28);
            this.connectionStringsComboBox.TabIndex = 7;
            this.connectionStringsComboBox.SelectedIndexChanged += new System.EventHandler(this.connectionStringsComboBox_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(3, 11);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(109, 20);
            this.label3.TabIndex = 8;
            this.label3.Text = "Environment";
            // 
            // exportDomainsButton
            // 
            this.exportDomainsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.exportDomainsButton.Location = new System.Drawing.Point(527, 56);
            this.exportDomainsButton.Name = "exportDomainsButton";
            this.exportDomainsButton.Size = new System.Drawing.Size(170, 32);
            this.exportDomainsButton.TabIndex = 10;
            this.exportDomainsButton.Text = "Export domains";
            this.exportDomainsButton.UseVisualStyleBackColor = true;
            this.exportDomainsButton.Click += new System.EventHandler(this.exportDomainsButton_Click);
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
            this.savedTemplatesComboBox.Location = new System.Drawing.Point(261, 317);
            this.savedTemplatesComboBox.Name = "savedTemplatesComboBox";
            this.savedTemplatesComboBox.Size = new System.Drawing.Size(436, 28);
            this.savedTemplatesComboBox.TabIndex = 11;
            this.savedTemplatesComboBox.SelectedIndexChanged += new System.EventHandler(this.savedTemplatesComboBox_SelectedIndexChanged);
            // 
            // useSavedTemplateCheckBox
            // 
            this.useSavedTemplateCheckBox.AutoSize = true;
            this.useSavedTemplateCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.useSavedTemplateCheckBox.Location = new System.Drawing.Point(107, 319);
            this.useSavedTemplateCheckBox.Name = "useSavedTemplateCheckBox";
            this.useSavedTemplateCheckBox.Size = new System.Drawing.Size(134, 24);
            this.useSavedTemplateCheckBox.TabIndex = 12;
            this.useSavedTemplateCheckBox.Text = "Use Template";
            this.useSavedTemplateCheckBox.UseVisualStyleBackColor = true;
            this.useSavedTemplateCheckBox.CheckedChanged += new System.EventHandler(this.useSavedTemplateCheckBox_CheckedChanged);
            // 
            // filterDomainsTextBox
            // 
            this.filterDomainsTextBox.Location = new System.Drawing.Point(118, 105);
            this.filterDomainsTextBox.Name = "filterDomainsTextBox";
            this.filterDomainsTextBox.Size = new System.Drawing.Size(579, 26);
            this.filterDomainsTextBox.TabIndex = 13;
            this.filterDomainsTextBox.TextChanged += new System.EventHandler(this.filterDomainsTextBox_TextChanged);
            this.filterDomainsTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.filterDomainsTextBox_KeyPress);
            // 
            // bottomStatusStrip
            // 
            this.bottomStatusStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.bottomStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadDomainsProgressBarToolStrip,
            this.loadDomainStatusLabelToolStrip,
            this.queryProgressBarToolStrip,
            this.queryStatusLabelToolStrip,
            this.successDomainsToolStrip,
            this.failureDomainsToolStrip,
            this.resultDomainsToolStrip});
            this.bottomStatusStrip.Location = new System.Drawing.Point(0, 886);
            this.bottomStatusStrip.Name = "bottomStatusStrip";
            this.bottomStatusStrip.Size = new System.Drawing.Size(1629, 32);
            this.bottomStatusStrip.TabIndex = 17;
            this.bottomStatusStrip.Text = "statusStrip1";
            // 
            // loadDomainsProgressBarToolStrip
            // 
            this.loadDomainsProgressBarToolStrip.Name = "loadDomainsProgressBarToolStrip";
            this.loadDomainsProgressBarToolStrip.Size = new System.Drawing.Size(150, 24);
            // 
            // loadDomainStatusLabelToolStrip
            // 
            this.loadDomainStatusLabelToolStrip.Name = "loadDomainStatusLabelToolStrip";
            this.loadDomainStatusLabelToolStrip.Size = new System.Drawing.Size(60, 25);
            this.loadDomainStatusLabelToolStrip.Text = "Ready";
            // 
            // queryProgressBarToolStrip
            // 
            this.queryProgressBarToolStrip.Name = "queryProgressBarToolStrip";
            this.queryProgressBarToolStrip.Size = new System.Drawing.Size(150, 24);
            // 
            // queryStatusLabelToolStrip
            // 
            this.queryStatusLabelToolStrip.Name = "queryStatusLabelToolStrip";
            this.queryStatusLabelToolStrip.Size = new System.Drawing.Size(60, 25);
            this.queryStatusLabelToolStrip.Text = "Ready";
            // 
            // successDomainsToolStrip
            // 
            this.successDomainsToolStrip.Name = "successDomainsToolStrip";
            this.successDomainsToolStrip.Size = new System.Drawing.Size(147, 25);
            this.successDomainsToolStrip.Text = "Success domains";
            // 
            // failureDomainsToolStrip
            // 
            this.failureDomainsToolStrip.Name = "failureDomainsToolStrip";
            this.failureDomainsToolStrip.Size = new System.Drawing.Size(131, 25);
            this.failureDomainsToolStrip.Text = "Failed domains";
            // 
            // resultDomainsToolStrip
            // 
            this.resultDomainsToolStrip.Name = "resultDomainsToolStrip";
            this.resultDomainsToolStrip.Size = new System.Drawing.Size(133, 25);
            this.resultDomainsToolStrip.Text = "Result domains";
            // 
            // asyncCheckBox
            // 
            this.asyncCheckBox.AutoSize = true;
            this.asyncCheckBox.Location = new System.Drawing.Point(260, 34);
            this.asyncCheckBox.Name = "asyncCheckBox";
            this.asyncCheckBox.Size = new System.Drawing.Size(78, 24);
            this.asyncCheckBox.TabIndex = 18;
            this.asyncCheckBox.Text = "Async";
            this.asyncCheckBox.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 34);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(141, 20);
            this.label4.TabIndex = 20;
            this.label4.Text = "Parallelism Degree";
            // 
            // parallelismDegreeNumericUpDown
            // 
            this.parallelismDegreeNumericUpDown.Location = new System.Drawing.Point(157, 32);
            this.parallelismDegreeNumericUpDown.Name = "parallelismDegreeNumericUpDown";
            this.parallelismDegreeNumericUpDown.Size = new System.Drawing.Size(85, 26);
            this.parallelismDegreeNumericUpDown.TabIndex = 21;
            // 
            // queryRichTextBox
            // 
            this.queryRichTextBox.Location = new System.Drawing.Point(7, 349);
            this.queryRichTextBox.Name = "queryRichTextBox";
            this.queryRichTextBox.Size = new System.Drawing.Size(690, 166);
            this.queryRichTextBox.TabIndex = 22;
            this.queryRichTextBox.Text = "";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.parallelismDegreeNumericUpDown);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.asyncCheckBox);
            this.groupBox1.Location = new System.Drawing.Point(343, 521);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(354, 62);
            this.groupBox1.TabIndex = 26;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Run options";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.exportOptionsComboBox);
            this.groupBox2.Controls.Add(this.displayOptionsComboBox);
            this.groupBox2.Location = new System.Drawing.Point(7, 521);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(330, 99);
            this.groupBox2.TabIndex = 27;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Output options";
            // 
            // exportOptionsComboBox
            // 
            this.exportOptionsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.exportOptionsComboBox.FormattingEnabled = true;
            this.exportOptionsComboBox.Location = new System.Drawing.Point(6, 65);
            this.exportOptionsComboBox.Name = "exportOptionsComboBox";
            this.exportOptionsComboBox.Size = new System.Drawing.Size(317, 28);
            this.exportOptionsComboBox.TabIndex = 29;
            // 
            // displayOptionsComboBox
            // 
            this.displayOptionsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.displayOptionsComboBox.FormattingEnabled = true;
            this.displayOptionsComboBox.Location = new System.Drawing.Point(6, 25);
            this.displayOptionsComboBox.Name = "displayOptionsComboBox";
            this.displayOptionsComboBox.Size = new System.Drawing.Size(317, 28);
            this.displayOptionsComboBox.TabIndex = 28;
            this.displayOptionsComboBox.SelectedIndexChanged += new System.EventHandler(this.displayOptionsComboBox_SelectedIndexChanged);
            // 
            // mainPanel
            // 
            this.mainPanel.Controls.Add(this.logsPanel);
            this.mainPanel.Controls.Add(this.domainFilterOptionsComboBox);
            this.mainPanel.Controls.Add(this.connectionStringsComboBox);
            this.mainPanel.Controls.Add(this.groupBox2);
            this.mainPanel.Controls.Add(this.executeQueryButton);
            this.mainPanel.Controls.Add(this.groupBox1);
            this.mainPanel.Controls.Add(this.label1);
            this.mainPanel.Controls.Add(this.queryRichTextBox);
            this.mainPanel.Controls.Add(this.label2);
            this.mainPanel.Controls.Add(this.queryOutputTabControl);
            this.mainPanel.Controls.Add(this.filterDomainsTextBox);
            this.mainPanel.Controls.Add(this.domainsCheckListBox);
            this.mainPanel.Controls.Add(this.useSavedTemplateCheckBox);
            this.mainPanel.Controls.Add(this.loadDomainsButton);
            this.mainPanel.Controls.Add(this.savedTemplatesComboBox);
            this.mainPanel.Controls.Add(this.label3);
            this.mainPanel.Controls.Add(this.exportDomainsButton);
            this.mainPanel.Location = new System.Drawing.Point(12, 12);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(1605, 871);
            this.mainPanel.TabIndex = 28;
            // 
            // domainFilterOptionsComboBox
            // 
            this.domainFilterOptionsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.domainFilterOptionsComboBox.FormattingEnabled = true;
            this.domainFilterOptionsComboBox.Location = new System.Drawing.Point(118, 59);
            this.domainFilterOptionsComboBox.Name = "domainFilterOptionsComboBox";
            this.domainFilterOptionsComboBox.Size = new System.Drawing.Size(212, 28);
            this.domainFilterOptionsComboBox.TabIndex = 29;
            // 
            // logsPanel
            // 
            this.logsPanel.Controls.Add(this.logsDataGridView);
            this.logsPanel.Location = new System.Drawing.Point(720, 8);
            this.logsPanel.Name = "logsPanel";
            this.logsPanel.Size = new System.Drawing.Size(866, 620);
            this.logsPanel.TabIndex = 29;
            // 
            // logsDataGridView
            // 
            this.logsDataGridView.AllowUserToAddRows = false;
            this.logsDataGridView.AllowUserToDeleteRows = false;
            this.logsDataGridView.AllowUserToOrderColumns = true;
            this.logsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.logsDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logsDataGridView.Location = new System.Drawing.Point(0, 0);
            this.logsDataGridView.Name = "logsDataGridView";
            this.logsDataGridView.ReadOnly = true;
            this.logsDataGridView.RowHeadersWidth = 62;
            this.logsDataGridView.RowTemplate.Height = 28;
            this.logsDataGridView.Size = new System.Drawing.Size(866, 620);
            this.logsDataGridView.TabIndex = 0;
            // 
            // QueryExecutioner
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1629, 918);
            this.Controls.Add(this.bottomStatusStrip);
            this.Controls.Add(this.mainPanel);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "QueryExecutioner";
            this.Text = "DML Query Executioner";
            this.Load += new System.EventHandler(this.QueryExecutioner_Load);
            this.SizeChanged += new System.EventHandler(this.QueryExecutioner_SizeChanged);
            this.bottomStatusStrip.ResumeLayout(false);
            this.bottomStatusStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.parallelismDegreeNumericUpDown)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.mainPanel.ResumeLayout(false);
            this.mainPanel.PerformLayout();
            this.logsPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.logsDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Button executeQueryButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;

        #endregion
        private System.Windows.Forms.TabControl queryOutputTabControl;
        private System.Windows.Forms.CheckedListBox domainsCheckListBox;
        private System.Windows.Forms.Button loadDomainsButton;
        private System.Windows.Forms.ComboBox connectionStringsComboBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button exportDomainsButton;
        private System.Windows.Forms.ComboBox savedTemplatesComboBox;
        private System.Windows.Forms.CheckBox useSavedTemplateCheckBox;
        private System.Windows.Forms.TextBox filterDomainsTextBox;
        private System.Windows.Forms.StatusStrip bottomStatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel queryStatusLabelToolStrip;
        private System.Windows.Forms.ToolStripProgressBar queryProgressBarToolStrip;
        private System.Windows.Forms.CheckBox asyncCheckBox;
        private System.Windows.Forms.ToolStripStatusLabel failureDomainsToolStrip;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown parallelismDegreeNumericUpDown;
        private System.Windows.Forms.RichTextBox queryRichTextBox;
        private System.Windows.Forms.ToolStripProgressBar loadDomainsProgressBarToolStrip;
        private System.Windows.Forms.ToolStripStatusLabel loadDomainStatusLabelToolStrip;
        private System.Windows.Forms.ToolStripStatusLabel successDomainsToolStrip;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.ComboBox displayOptionsComboBox;
        private System.Windows.Forms.ComboBox exportOptionsComboBox;
        private System.Windows.Forms.ToolStripStatusLabel resultDomainsToolStrip;
        private System.Windows.Forms.ComboBox domainFilterOptionsComboBox;
        private System.Windows.Forms.Panel logsPanel;
        private System.Windows.Forms.DataGridView logsDataGridView;
    }
}