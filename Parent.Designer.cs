namespace SSDLMaintenanceTool
{
    partial class Parent
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menuToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openQueryExecutionerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openPredefinedQueryEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileCorrutValidatorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cacheClearAtGEPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileCorruptValidator = new System.Windows.Forms.Button();
            this.queryExecutorButton = new System.Windows.Forms.Button();
            this.queryEditorButton = new System.Windows.Forms.Button();
            this.cacheClearAtGEPButton = new System.Windows.Forms.Button();
            this.serviceBusButton = new System.Windows.Forms.Button();
            this.serviceBusToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(983, 33);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // menuToolStripMenuItem
            // 
            this.menuToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openQueryExecutionerToolStripMenuItem,
            this.openPredefinedQueryEditorToolStripMenuItem,
            this.openFileCorrutValidatorToolStripMenuItem,
            this.cacheClearAtGEPToolStripMenuItem,
            this.serviceBusToolStripMenuItem});
            this.menuToolStripMenuItem.Name = "menuToolStripMenuItem";
            this.menuToolStripMenuItem.Size = new System.Drawing.Size(73, 29);
            this.menuToolStripMenuItem.Text = "Menu";
            // 
            // openQueryExecutionerToolStripMenuItem
            // 
            this.openQueryExecutionerToolStripMenuItem.Name = "openQueryExecutionerToolStripMenuItem";
            this.openQueryExecutionerToolStripMenuItem.Size = new System.Drawing.Size(311, 34);
            this.openQueryExecutionerToolStripMenuItem.Text = "QueryExecutioner";
            this.openQueryExecutionerToolStripMenuItem.Click += new System.EventHandler(this.openQueryExecutionerToolStripMenuItem_Click);
            // 
            // openPredefinedQueryEditorToolStripMenuItem
            // 
            this.openPredefinedQueryEditorToolStripMenuItem.Name = "openPredefinedQueryEditorToolStripMenuItem";
            this.openPredefinedQueryEditorToolStripMenuItem.Size = new System.Drawing.Size(311, 34);
            this.openPredefinedQueryEditorToolStripMenuItem.Text = "Pre-defined Query Editor";
            this.openPredefinedQueryEditorToolStripMenuItem.Click += new System.EventHandler(this.openPredefinedQueryEditorToolStripMenuItem_Click);
            // 
            // openFileCorrutValidatorToolStripMenuItem
            // 
            this.openFileCorrutValidatorToolStripMenuItem.Name = "openFileCorrutValidatorToolStripMenuItem";
            this.openFileCorrutValidatorToolStripMenuItem.Size = new System.Drawing.Size(311, 34);
            this.openFileCorrutValidatorToolStripMenuItem.Text = "File Corrut Validator";
            this.openFileCorrutValidatorToolStripMenuItem.Click += new System.EventHandler(this.openFileCorrutValidatorToolStripMenuItem_Click);
            // 
            // cacheClearAtGEPToolStripMenuItem
            // 
            this.cacheClearAtGEPToolStripMenuItem.Name = "cacheClearAtGEPToolStripMenuItem";
            this.cacheClearAtGEPToolStripMenuItem.Size = new System.Drawing.Size(311, 34);
            this.cacheClearAtGEPToolStripMenuItem.Text = "GEP Cache Clear";
            this.cacheClearAtGEPToolStripMenuItem.Click += new System.EventHandler(this.gEPCacheClearToolStripMenuItem_Click);
            // 
            // fileCorruptValidator
            // 
            this.fileCorruptValidator.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.fileCorruptValidator.Image = global::SSDLMaintenanceTool.Properties.Resources.corrupt_file;
            this.fileCorruptValidator.Location = new System.Drawing.Point(74, 334);
            this.fileCorruptValidator.Name = "fileCorruptValidator";
            this.fileCorruptValidator.Size = new System.Drawing.Size(219, 260);
            this.fileCorruptValidator.TabIndex = 3;
            this.fileCorruptValidator.Text = "File Corrupt Validator";
            this.fileCorruptValidator.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.fileCorruptValidator.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.fileCorruptValidator.UseVisualStyleBackColor = true;
            this.fileCorruptValidator.Click += new System.EventHandler(this.fileCorruptValidator_Click);
            // 
            // queryExecutorButton
            // 
            this.queryExecutorButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.queryExecutorButton.Image = global::SSDLMaintenanceTool.Properties.Resources.query_analysis;
            this.queryExecutorButton.Location = new System.Drawing.Point(402, 50);
            this.queryExecutorButton.Name = "queryExecutorButton";
            this.queryExecutorButton.Size = new System.Drawing.Size(219, 260);
            this.queryExecutorButton.TabIndex = 2;
            this.queryExecutorButton.Text = "DML Query Executioner";
            this.queryExecutorButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.queryExecutorButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.queryExecutorButton.UseVisualStyleBackColor = true;
            this.queryExecutorButton.Click += new System.EventHandler(this.queryExecutorButton_Click);
            // 
            // queryEditorButton
            // 
            this.queryEditorButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.queryEditorButton.Image = global::SSDLMaintenanceTool.Properties.Resources.sql_configure_icon;
            this.queryEditorButton.Location = new System.Drawing.Point(74, 50);
            this.queryEditorButton.Name = "queryEditorButton";
            this.queryEditorButton.Size = new System.Drawing.Size(219, 260);
            this.queryEditorButton.TabIndex = 1;
            this.queryEditorButton.Text = "Predefined Query Editor";
            this.queryEditorButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.queryEditorButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.queryEditorButton.UseVisualStyleBackColor = true;
            this.queryEditorButton.Click += new System.EventHandler(this.queryEditorButton_Click);
            // 
            // cacheClearAtGEPButton
            // 
            this.cacheClearAtGEPButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.cacheClearAtGEPButton.Image = global::SSDLMaintenanceTool.Properties.Resources.cache_clear_new;
            this.cacheClearAtGEPButton.Location = new System.Drawing.Point(402, 334);
            this.cacheClearAtGEPButton.Name = "cacheClearAtGEPButton";
            this.cacheClearAtGEPButton.Size = new System.Drawing.Size(219, 260);
            this.cacheClearAtGEPButton.TabIndex = 4;
            this.cacheClearAtGEPButton.Text = "GEP Cache Clear";
            this.cacheClearAtGEPButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.cacheClearAtGEPButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.cacheClearAtGEPButton.UseVisualStyleBackColor = true;
            this.cacheClearAtGEPButton.Click += new System.EventHandler(this.cacheClearAtGEPButton_Click);
            // 
            // serviceBusButton
            // 
            this.serviceBusButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.serviceBusButton.Location = new System.Drawing.Point(700, 50);
            this.serviceBusButton.Name = "serviceBusButton";
            this.serviceBusButton.Size = new System.Drawing.Size(219, 260);
            this.serviceBusButton.TabIndex = 5;
            this.serviceBusButton.Text = "Service Bus";
            this.serviceBusButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.serviceBusButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.serviceBusButton.UseVisualStyleBackColor = true;
            this.serviceBusButton.Click += new System.EventHandler(this.serviceBusButton_Click);
            // 
            // serviceBusToolStripMenuItem
            // 
            this.serviceBusToolStripMenuItem.Name = "serviceBusToolStripMenuItem";
            this.serviceBusToolStripMenuItem.Size = new System.Drawing.Size(311, 34);
            this.serviceBusToolStripMenuItem.Text = "Service Bus";
            this.serviceBusToolStripMenuItem.Click += new System.EventHandler(this.serviceBusToolStripMenuItem_Click);
            // 
            // Parent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(983, 640);
            this.Controls.Add(this.serviceBusButton);
            this.Controls.Add(this.cacheClearAtGEPButton);
            this.Controls.Add(this.fileCorruptValidator);
            this.Controls.Add(this.queryExecutorButton);
            this.Controls.Add(this.queryEditorButton);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Parent";
            this.Text = "Home";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openQueryExecutionerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openPredefinedQueryEditorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openFileCorrutValidatorToolStripMenuItem;
        private System.Windows.Forms.Button queryEditorButton;
        private System.Windows.Forms.Button queryExecutorButton;
        private System.Windows.Forms.Button fileCorruptValidator;
        private System.Windows.Forms.Button cacheClearAtGEPButton;
        private System.Windows.Forms.ToolStripMenuItem cacheClearAtGEPToolStripMenuItem;
        private System.Windows.Forms.Button serviceBusButton;
        private System.Windows.Forms.ToolStripMenuItem serviceBusToolStripMenuItem;
    }
}

