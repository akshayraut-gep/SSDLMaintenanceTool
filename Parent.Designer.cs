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
            this.button1 = new System.Windows.Forms.Button();
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
            this.menuStrip1.Size = new System.Drawing.Size(800, 36);
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
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.button1.Image = global::SSDLMaintenanceTool.Properties.Resources.sql_configure_icon;
            this.button1.Location = new System.Drawing.Point(82, 72);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(219, 260);
            this.button1.TabIndex = 1;
            this.button1.Text = "Predefined Query Editor";
            this.button1.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Parent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.button1);
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
        private System.Windows.Forms.Button button1;
    }
}

