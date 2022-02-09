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
            this.canExportToExcelCheckBox = new System.Windows.Forms.CheckBox();
            this.displayQueryOutputCheckBox = new System.Windows.Forms.CheckBox();
            this.multiTabOutputCheckBox = new System.Windows.Forms.CheckBox();
            this.bottomStatusStrip = new System.Windows.Forms.StatusStrip();
            this.queryProgressBarToolStrip = new System.Windows.Forms.ToolStripProgressBar();
            this.queryStatusLabelToolStrip = new System.Windows.Forms.ToolStripStatusLabel();
            this.successDomainsToolStrip = new System.Windows.Forms.ToolStripStatusLabel();
            this.failureDomainsToolStrip = new System.Windows.Forms.ToolStripStatusLabel();
            this.asyncCheckBox = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.parallelismDegreeNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.queryRichTextBox = new System.Windows.Forms.RichTextBox();
            this.loadDomainsProgressBarToolStrip = new System.Windows.Forms.ToolStripProgressBar();
            this.loadDomainStatusLabelToolStrip = new System.Windows.Forms.ToolStripStatusLabel();
            this.bottomStatusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.parallelismDegreeNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // executeQueryButton
            // 
            this.executeQueryButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.executeQueryButton.Location = new System.Drawing.Point(632, 555);
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
            // queryOutputTabControl
            // 
            this.queryOutputTabControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.queryOutputTabControl.Location = new System.Drawing.Point(16, 623);
            this.queryOutputTabControl.Name = "queryOutputTabControl";
            this.queryOutputTabControl.SelectedIndex = 0;
            this.queryOutputTabControl.Size = new System.Drawing.Size(200, 100);
            this.queryOutputTabControl.TabIndex = 4;
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
            this.domainsCheckListBox.Size = new System.Drawing.Size(647, 165);
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
            this.connectionStringsComboBox.Size = new System.Drawing.Size(647, 28);
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
            this.exportDomainsButton.Location = new System.Drawing.Point(584, 60);
            this.exportDomainsButton.Name = "exportDomainsButton";
            this.exportDomainsButton.Size = new System.Drawing.Size(181, 32);
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
            this.savedTemplatesComboBox.Location = new System.Drawing.Point(270, 321);
            this.savedTemplatesComboBox.Name = "savedTemplatesComboBox";
            this.savedTemplatesComboBox.Size = new System.Drawing.Size(493, 28);
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
            this.filterDomainsTextBox.Size = new System.Drawing.Size(646, 26);
            this.filterDomainsTextBox.TabIndex = 13;
            this.filterDomainsTextBox.TextChanged += new System.EventHandler(this.filterDomainsTextBox_TextChanged);
            this.filterDomainsTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.filterDomainsTextBox_KeyPress);
            // 
            // canExportToExcelCheckBox
            // 
            this.canExportToExcelCheckBox.AutoSize = true;
            this.canExportToExcelCheckBox.Location = new System.Drawing.Point(377, 526);
            this.canExportToExcelCheckBox.Name = "canExportToExcelCheckBox";
            this.canExportToExcelCheckBox.Size = new System.Drawing.Size(141, 24);
            this.canExportToExcelCheckBox.TabIndex = 14;
            this.canExportToExcelCheckBox.Text = "Export to Excel";
            this.canExportToExcelCheckBox.UseVisualStyleBackColor = true;
            // 
            // displayQueryOutputCheckBox
            // 
            this.displayQueryOutputCheckBox.AutoSize = true;
            this.displayQueryOutputCheckBox.Location = new System.Drawing.Point(285, 526);
            this.displayQueryOutputCheckBox.Name = "displayQueryOutputCheckBox";
            this.displayQueryOutputCheckBox.Size = new System.Drawing.Size(86, 24);
            this.displayQueryOutputCheckBox.TabIndex = 15;
            this.displayQueryOutputCheckBox.Text = "Display";
            this.displayQueryOutputCheckBox.UseVisualStyleBackColor = true;
            // 
            // multiTabOutputCheckBox
            // 
            this.multiTabOutputCheckBox.AutoSize = true;
            this.multiTabOutputCheckBox.Location = new System.Drawing.Point(524, 526);
            this.multiTabOutputCheckBox.Name = "multiTabOutputCheckBox";
            this.multiTabOutputCheckBox.Size = new System.Drawing.Size(155, 24);
            this.multiTabOutputCheckBox.TabIndex = 16;
            this.multiTabOutputCheckBox.Text = "Multi tabs display";
            this.multiTabOutputCheckBox.UseVisualStyleBackColor = true;
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
            this.failureDomainsToolStrip});
            this.bottomStatusStrip.Location = new System.Drawing.Point(0, 749);
            this.bottomStatusStrip.Name = "bottomStatusStrip";
            this.bottomStatusStrip.Size = new System.Drawing.Size(777, 32);
            this.bottomStatusStrip.TabIndex = 17;
            this.bottomStatusStrip.Text = "statusStrip1";
            // 
            // queryProgressBarToolStrip
            // 
            this.queryProgressBarToolStrip.Name = "queryProgressBarToolStrip";
            this.queryProgressBarToolStrip.Size = new System.Drawing.Size(200, 24);
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
            // asyncCheckBox
            // 
            this.asyncCheckBox.AutoSize = true;
            this.asyncCheckBox.Location = new System.Drawing.Point(685, 526);
            this.asyncCheckBox.Name = "asyncCheckBox";
            this.asyncCheckBox.Size = new System.Drawing.Size(78, 24);
            this.asyncCheckBox.TabIndex = 18;
            this.asyncCheckBox.Text = "Async";
            this.asyncCheckBox.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 527);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(141, 20);
            this.label4.TabIndex = 20;
            this.label4.Text = "Parallelism Degree";
            // 
            // parallelismDegreeNumericUpDown
            // 
            this.parallelismDegreeNumericUpDown.Location = new System.Drawing.Point(159, 525);
            this.parallelismDegreeNumericUpDown.Name = "parallelismDegreeNumericUpDown";
            this.parallelismDegreeNumericUpDown.Size = new System.Drawing.Size(120, 26);
            this.parallelismDegreeNumericUpDown.TabIndex = 21;
            // 
            // queryRichTextBox
            // 
            this.queryRichTextBox.Location = new System.Drawing.Point(16, 353);
            this.queryRichTextBox.Name = "queryRichTextBox";
            this.queryRichTextBox.Size = new System.Drawing.Size(747, 166);
            this.queryRichTextBox.TabIndex = 22;
            this.queryRichTextBox.Text = "";
            // 
            // loadDomainsProgressBarToolStrip
            // 
            this.loadDomainsProgressBarToolStrip.Name = "loadDomainsProgressBarToolStrip";
            this.loadDomainsProgressBarToolStrip.Size = new System.Drawing.Size(200, 24);
            // 
            // loadDomainStatusLabelToolStrip
            // 
            this.loadDomainStatusLabelToolStrip.Name = "loadDomainStatusLabelToolStrip";
            this.loadDomainStatusLabelToolStrip.Size = new System.Drawing.Size(60, 25);
            this.loadDomainStatusLabelToolStrip.Text = "Ready";
            // 
            // QueryExecutioner
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(777, 781);
            this.Controls.Add(this.queryRichTextBox);
            this.Controls.Add(this.parallelismDegreeNumericUpDown);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.asyncCheckBox);
            this.Controls.Add(this.bottomStatusStrip);
            this.Controls.Add(this.multiTabOutputCheckBox);
            this.Controls.Add(this.displayQueryOutputCheckBox);
            this.Controls.Add(this.canExportToExcelCheckBox);
            this.Controls.Add(this.filterDomainsTextBox);
            this.Controls.Add(this.useSavedTemplateCheckBox);
            this.Controls.Add(this.savedTemplatesComboBox);
            this.Controls.Add(this.exportDomainsButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.connectionStringsComboBox);
            this.Controls.Add(this.loadDomainsButton);
            this.Controls.Add(this.domainsCheckListBox);
            this.Controls.Add(this.queryOutputTabControl);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.executeQueryButton);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "QueryExecutioner";
            this.Text = "DML Query Executioner";
            this.Load += new System.EventHandler(this.QueryExecutioner_Load);
            this.SizeChanged += new System.EventHandler(this.QueryExecutioner_SizeChanged);
            this.bottomStatusStrip.ResumeLayout(false);
            this.bottomStatusStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.parallelismDegreeNumericUpDown)).EndInit();
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
        private System.Windows.Forms.CheckBox canExportToExcelCheckBox;
        private System.Windows.Forms.CheckBox displayQueryOutputCheckBox;
        private System.Windows.Forms.CheckBox multiTabOutputCheckBox;
        private System.Windows.Forms.StatusStrip bottomStatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel queryStatusLabelToolStrip;
        private System.Windows.Forms.ToolStripProgressBar queryProgressBarToolStrip;
        private System.Windows.Forms.CheckBox asyncCheckBox;
        private System.Windows.Forms.ToolStripStatusLabel successDomainsToolStrip;
        private System.Windows.Forms.ToolStripStatusLabel failureDomainsToolStrip;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown parallelismDegreeNumericUpDown;
        private System.Windows.Forms.RichTextBox queryRichTextBox;
        private System.Windows.Forms.ToolStripProgressBar loadDomainsProgressBarToolStrip;
        private System.Windows.Forms.ToolStripStatusLabel loadDomainStatusLabelToolStrip;
    }
}