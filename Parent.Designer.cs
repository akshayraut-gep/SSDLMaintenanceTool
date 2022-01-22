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
            this.fileCorruptValidator = new System.Windows.Forms.Button();
            this.queryExecutorButton = new System.Windows.Forms.Button();
            this.queryEditorButton = new System.Windows.Forms.Button();
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
            this.menuStrip1.Size = new System.Drawing.Size(846, 36);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // menuToolStripMenuItem
            // 
            this.menuToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openQueryExecutionerToolStripMenuItem,
            this.openPredefinedQueryEditorToolStripMenuItem,
            this.openFileCorrutValidatorToolStripMenuItem});
            this.menuToolStripMenuItem.Name = "menuToolStripMenuItem";
            this.menuToolStripMenuItem.Size = new System.Drawing.Size(73, 30);
            this.menuToolStripMenuItem.Text = "Menu";
            // 
            // openQueryExecutionerToolStripMenuItem
            // 
            this.openQueryExecutionerToolStripMenuItem.Name = "openQueryExecutionerToolStripMenuItem";
            this.openQueryExecutionerToolStripMenuItem.Size = new System.Drawing.Size(360, 34);
            this.openQueryExecutionerToolStripMenuItem.Text = "Open QueryExecutioner";
            this.openQueryExecutionerToolStripMenuItem.Click += new System.EventHandler(this.openQueryExecutionerToolStripMenuItem_Click);
            // 
            // openPredefinedQueryEditorToolStripMenuItem
            // 
            this.openPredefinedQueryEditorToolStripMenuItem.Name = "openPredefinedQueryEditorToolStripMenuItem";
            this.openPredefinedQueryEditorToolStripMenuItem.Size = new System.Drawing.Size(360, 34);
            this.openPredefinedQueryEditorToolStripMenuItem.Text = "Open Pre-defined Query Editor";
            this.openPredefinedQueryEditorToolStripMenuItem.Click += new System.EventHandler(this.openPredefinedQueryEditorToolStripMenuItem_Click);
            // 
            // openFileCorrutValidatorToolStripMenuItem
            // 
            this.openFileCorrutValidatorToolStripMenuItem.Name = "openFileCorrutValidatorToolStripMenuItem";
            this.openFileCorrutValidatorToolStripMenuItem.Size = new System.Drawing.Size(360, 34);
            this.openFileCorrutValidatorToolStripMenuItem.Text = "Open File Corrut Validator";
            this.openFileCorrutValidatorToolStripMenuItem.Click += new System.EventHandler(this.openFileCorrutValidatorToolStripMenuItem_Click);
            // 
            // fileCorruptValidator
            // 
            this.fileCorruptValidator.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.fileCorruptValidator.Image = global::SSDLMaintenanceTool.Properties.Resources.corrupt_file;
            this.fileCorruptValidator.Location = new System.Drawing.Point(82, 356);
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
            this.queryExecutorButton.Image = global::SSDLMaintenanceTool.Properties.Resources.sql_configure_icon;
            this.queryExecutorButton.Location = new System.Drawing.Point(410, 72);
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
            this.queryEditorButton.Location = new System.Drawing.Point(82, 72);
            this.queryEditorButton.Name = "queryEditorButton";
            this.queryEditorButton.Size = new System.Drawing.Size(219, 260);
            this.queryEditorButton.TabIndex = 1;
            this.queryEditorButton.Text = "Predefined Query Editor";
            this.queryEditorButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.queryEditorButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.queryEditorButton.UseVisualStyleBackColor = true;
            this.queryEditorButton.Click += new System.EventHandler(this.queryEditorButton_Click);
            // 
            // Parent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(846, 659);
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
    }
}

