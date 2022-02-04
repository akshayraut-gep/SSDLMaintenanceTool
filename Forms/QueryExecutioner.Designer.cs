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
            this.statusLabelToolStrip = new System.Windows.Forms.ToolStripStatusLabel();
            this.progressBarToolStrip = new System.Windows.Forms.ToolStripProgressBar();
            this.bottomStatusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // executeQueryButton
            // 
            this.executeQueryButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.executeQueryButton.Location = new System.Drawing.Point(465, 523);
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
            // queryOutputTabControl
            // 
            this.queryOutputTabControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.queryOutputTabControl.Location = new System.Drawing.Point(17, 576);
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
            // canExportToExcelCheckBox
            // 
            this.canExportToExcelCheckBox.AutoSize = true;
            this.canExportToExcelCheckBox.Location = new System.Drawing.Point(157, 528);
            this.canExportToExcelCheckBox.Name = "canExportToExcelCheckBox";
            this.canExportToExcelCheckBox.Size = new System.Drawing.Size(141, 24);
            this.canExportToExcelCheckBox.TabIndex = 14;
            this.canExportToExcelCheckBox.Text = "Export to Excel";
            this.canExportToExcelCheckBox.UseVisualStyleBackColor = true;
            // 
            // displayQueryOutputCheckBox
            // 
            this.displayQueryOutputCheckBox.AutoSize = true;
            this.displayQueryOutputCheckBox.Location = new System.Drawing.Point(65, 528);
            this.displayQueryOutputCheckBox.Name = "displayQueryOutputCheckBox";
            this.displayQueryOutputCheckBox.Size = new System.Drawing.Size(86, 24);
            this.displayQueryOutputCheckBox.TabIndex = 15;
            this.displayQueryOutputCheckBox.Text = "Display";
            this.displayQueryOutputCheckBox.UseVisualStyleBackColor = true;
            // 
            // multiTabOutputCheckBox
            // 
            this.multiTabOutputCheckBox.AutoSize = true;
            this.multiTabOutputCheckBox.Location = new System.Drawing.Point(304, 528);
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
            this.progressBarToolStrip,
            this.statusLabelToolStrip});
            this.bottomStatusStrip.Location = new System.Drawing.Point(0, 650);
            this.bottomStatusStrip.Name = "bottomStatusStrip";
            this.bottomStatusStrip.Size = new System.Drawing.Size(610, 32);
            this.bottomStatusStrip.TabIndex = 17;
            this.bottomStatusStrip.Text = "statusStrip1";
            // 
            // statusLabelToolStrip
            // 
            this.statusLabelToolStrip.Name = "statusLabelToolStrip";
            this.statusLabelToolStrip.Size = new System.Drawing.Size(88, 25);
            this.statusLabelToolStrip.Text = "Loading...";
            // 
            // progressBarToolStrip
            // 
            this.progressBarToolStrip.Name = "progressBarToolStrip";
            this.progressBarToolStrip.Size = new System.Drawing.Size(200, 24);
            // 
            // QueryExecutioner
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(610, 682);
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
            this.Controls.Add(this.queryTextBox);
            this.Controls.Add(this.executeQueryButton);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "QueryExecutioner";
            this.Text = "DML Query Executioner";
            this.Load += new System.EventHandler(this.QueryExecutioner_Load);
            this.SizeChanged += new System.EventHandler(this.QueryExecutioner_SizeChanged);
            this.bottomStatusStrip.ResumeLayout(false);
            this.bottomStatusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Button executeQueryButton;
        private System.Windows.Forms.TextBox queryTextBox;
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
        private System.Windows.Forms.ToolStripStatusLabel statusLabelToolStrip;
        private System.Windows.Forms.ToolStripProgressBar progressBarToolStrip;
    }
}