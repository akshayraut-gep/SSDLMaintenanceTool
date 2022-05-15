namespace SSDLMaintenanceTool.Forms
{
    partial class GEPCacheClear
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
            this.environmentComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.clearCacheButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.modeComboBox = new System.Windows.Forms.ComboBox();
            this.filterDomainsTextBox = new System.Windows.Forms.TextBox();
            this.domainsCheckListBox = new System.Windows.Forms.CheckedListBox();
            this.loadDomainsButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.singleDomainBPCTextBox = new System.Windows.Forms.TextBox();
            this.domainFilterOptionsComboBox = new System.Windows.Forms.ComboBox();
            this.bottomStatusStrip = new System.Windows.Forms.StatusStrip();
            this.loadDomainsProgressBarToolStrip = new System.Windows.Forms.ToolStripProgressBar();
            this.loadDomainStatusLabelToolStrip = new System.Windows.Forms.ToolStripStatusLabel();
            this.queryProgressBarToolStrip = new System.Windows.Forms.ToolStripProgressBar();
            this.queryStatusLabelToolStrip = new System.Windows.Forms.ToolStripStatusLabel();
            this.successDomainsToolStrip = new System.Windows.Forms.ToolStripStatusLabel();
            this.failureDomainsToolStrip = new System.Windows.Forms.ToolStripStatusLabel();
            this.resultDomainsToolStrip = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.parallelismDegreeNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.asyncCheckBox = new System.Windows.Forms.CheckBox();
            this.cacheKeyNameTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.bottomStatusStrip.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.parallelismDegreeNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // environmentComboBox
            // 
            this.environmentComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.environmentComboBox.FormattingEnabled = true;
            this.environmentComboBox.Location = new System.Drawing.Point(161, 67);
            this.environmentComboBox.Name = "environmentComboBox";
            this.environmentComboBox.Size = new System.Drawing.Size(137, 28);
            this.environmentComboBox.TabIndex = 2;
            this.environmentComboBox.SelectedIndexChanged += new System.EventHandler(this.environmentComboBox_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 70);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Environment";
            // 
            // clearCacheButton
            // 
            this.clearCacheButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.clearCacheButton.Location = new System.Drawing.Point(480, 472);
            this.clearCacheButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.clearCacheButton.Name = "clearCacheButton";
            this.clearCacheButton.Size = new System.Drawing.Size(133, 32);
            this.clearCacheButton.TabIndex = 3;
            this.clearCacheButton.Text = "Clear cache";
            this.clearCacheButton.UseVisualStyleBackColor = true;
            this.clearCacheButton.Click += new System.EventHandler(this.clearCacheButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "Mode";
            // 
            // modeComboBox
            // 
            this.modeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.modeComboBox.FormattingEnabled = true;
            this.modeComboBox.Location = new System.Drawing.Point(161, 24);
            this.modeComboBox.Name = "modeComboBox";
            this.modeComboBox.Size = new System.Drawing.Size(263, 28);
            this.modeComboBox.TabIndex = 1;
            this.modeComboBox.SelectedIndexChanged += new System.EventHandler(this.modeComboBox_SelectedIndexChanged);
            // 
            // filterDomainsTextBox
            // 
            this.filterDomainsTextBox.Location = new System.Drawing.Point(161, 112);
            this.filterDomainsTextBox.Name = "filterDomainsTextBox";
            this.filterDomainsTextBox.Size = new System.Drawing.Size(452, 26);
            this.filterDomainsTextBox.TabIndex = 15;
            this.filterDomainsTextBox.TextChanged += new System.EventHandler(this.filterDomainsTextBox_TextChanged);
            this.filterDomainsTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.filterDomainsTextBox_KeyPress);
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
            this.domainsCheckListBox.Location = new System.Drawing.Point(161, 144);
            this.domainsCheckListBox.Name = "domainsCheckListBox";
            this.domainsCheckListBox.Size = new System.Drawing.Size(452, 165);
            this.domainsCheckListBox.TabIndex = 14;
            // 
            // loadDomainsButton
            // 
            this.loadDomainsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.loadDomainsButton.Location = new System.Drawing.Point(447, 64);
            this.loadDomainsButton.Name = "loadDomainsButton";
            this.loadDomainsButton.Size = new System.Drawing.Size(166, 32);
            this.loadDomainsButton.TabIndex = 16;
            this.loadDomainsButton.Text = "Load all domains";
            this.loadDomainsButton.UseVisualStyleBackColor = true;
            this.loadDomainsButton.Click += new System.EventHandler(this.loadDomainsButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(12, 118);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 20);
            this.label3.TabIndex = 17;
            this.label3.Text = "Filter";
            // 
            // singleDomainBPCTextBox
            // 
            this.singleDomainBPCTextBox.Location = new System.Drawing.Point(430, 24);
            this.singleDomainBPCTextBox.Name = "singleDomainBPCTextBox";
            this.singleDomainBPCTextBox.Size = new System.Drawing.Size(183, 26);
            this.singleDomainBPCTextBox.TabIndex = 18;
            // 
            // domainFilterOptionsComboBox
            // 
            this.domainFilterOptionsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.domainFilterOptionsComboBox.FormattingEnabled = true;
            this.domainFilterOptionsComboBox.Location = new System.Drawing.Point(304, 67);
            this.domainFilterOptionsComboBox.Name = "domainFilterOptionsComboBox";
            this.domainFilterOptionsComboBox.Size = new System.Drawing.Size(137, 28);
            this.domainFilterOptionsComboBox.TabIndex = 30;
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
            this.bottomStatusStrip.Location = new System.Drawing.Point(0, 587);
            this.bottomStatusStrip.Name = "bottomStatusStrip";
            this.bottomStatusStrip.Size = new System.Drawing.Size(939, 32);
            this.bottomStatusStrip.TabIndex = 31;
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
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.parallelismDegreeNumericUpDown);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.asyncCheckBox);
            this.groupBox1.Location = new System.Drawing.Point(259, 405);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(354, 62);
            this.groupBox1.TabIndex = 32;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Run options";
            // 
            // parallelismDegreeNumericUpDown
            // 
            this.parallelismDegreeNumericUpDown.Location = new System.Drawing.Point(157, 32);
            this.parallelismDegreeNumericUpDown.Name = "parallelismDegreeNumericUpDown";
            this.parallelismDegreeNumericUpDown.Size = new System.Drawing.Size(85, 26);
            this.parallelismDegreeNumericUpDown.TabIndex = 21;
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
            // cacheKeyNameTextBox
            // 
            this.cacheKeyNameTextBox.Location = new System.Drawing.Point(161, 324);
            this.cacheKeyNameTextBox.Name = "cacheKeyNameTextBox";
            this.cacheKeyNameTextBox.Size = new System.Drawing.Size(183, 26);
            this.cacheKeyNameTextBox.TabIndex = 33;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(12, 327);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(143, 20);
            this.label5.TabIndex = 34;
            this.label5.Text = "Cache Key name";
            // 
            // GEPCacheClear
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(939, 619);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cacheKeyNameTextBox);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.bottomStatusStrip);
            this.Controls.Add(this.domainFilterOptionsComboBox);
            this.Controls.Add(this.singleDomainBPCTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.loadDomainsButton);
            this.Controls.Add(this.filterDomainsTextBox);
            this.Controls.Add(this.domainsCheckListBox);
            this.Controls.Add(this.modeComboBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.clearCacheButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.environmentComboBox);
            this.Name = "GEPCacheClear";
            this.Text = "GEPCacheClear";
            this.Load += new System.EventHandler(this.GEPCacheClear_Load);
            this.bottomStatusStrip.ResumeLayout(false);
            this.bottomStatusStrip.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.parallelismDegreeNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox environmentComboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button clearCacheButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox modeComboBox;
        private System.Windows.Forms.TextBox filterDomainsTextBox;
        private System.Windows.Forms.CheckedListBox domainsCheckListBox;
        private System.Windows.Forms.Button loadDomainsButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox singleDomainBPCTextBox;
        private System.Windows.Forms.ComboBox domainFilterOptionsComboBox;
        private System.Windows.Forms.StatusStrip bottomStatusStrip;
        private System.Windows.Forms.ToolStripProgressBar loadDomainsProgressBarToolStrip;
        private System.Windows.Forms.ToolStripStatusLabel loadDomainStatusLabelToolStrip;
        private System.Windows.Forms.ToolStripProgressBar queryProgressBarToolStrip;
        private System.Windows.Forms.ToolStripStatusLabel queryStatusLabelToolStrip;
        private System.Windows.Forms.ToolStripStatusLabel successDomainsToolStrip;
        private System.Windows.Forms.ToolStripStatusLabel failureDomainsToolStrip;
        private System.Windows.Forms.ToolStripStatusLabel resultDomainsToolStrip;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown parallelismDegreeNumericUpDown;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox asyncCheckBox;
        private System.Windows.Forms.TextBox cacheKeyNameTextBox;
        private System.Windows.Forms.Label label5;
    }
}