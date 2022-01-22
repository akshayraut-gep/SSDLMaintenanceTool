namespace SSDLMaintenanceTool.Forms
{
    partial class PredefinedQueriesEditor
    {
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox cbQueryTypes;
        private System.Windows.Forms.DataGridView dgvQueries;
        private System.Windows.Forms.TextBox tbQueries;
        private System.Windows.Forms.Button btnGenerateSQL;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbJobID;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbStepName;
        private System.Windows.Forms.CheckBox cbExistingStep;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nudSequence;

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
            this.button1 = new System.Windows.Forms.Button();
            this.cbQueryTypes = new System.Windows.Forms.ComboBox();
            this.dgvQueries = new System.Windows.Forms.DataGridView();
            this.tbQueries = new System.Windows.Forms.TextBox();
            this.btnGenerateSQL = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tbJobID = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbStepName = new System.Windows.Forms.TextBox();
            this.cbExistingStep = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.nudSequence = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.dgvQueries)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSequence)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.button1.Location = new System.Drawing.Point(596, 5);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(181, 40);
            this.button1.TabIndex = 0;
            this.button1.Text = "Add Query";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // cbQueryTypes
            // 
            this.cbQueryTypes.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.cbQueryTypes.FormattingEnabled = true;
            this.cbQueryTypes.Location = new System.Drawing.Point(156, 6);
            this.cbQueryTypes.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cbQueryTypes.Name = "cbQueryTypes";
            this.cbQueryTypes.Size = new System.Drawing.Size(369, 40);
            this.cbQueryTypes.TabIndex = 1;
            // 
            // dgvQueries
            // 
            this.dgvQueries.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvQueries.Location = new System.Drawing.Point(18, 50);
            this.dgvQueries.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dgvQueries.Name = "dgvQueries";
            this.dgvQueries.RowHeadersWidth = 62;
            this.dgvQueries.Size = new System.Drawing.Size(759, 234);
            this.dgvQueries.TabIndex = 2;
            this.dgvQueries.Text = "dataGridView1";
            // 
            // tbQueries
            // 
            this.tbQueries.Location = new System.Drawing.Point(18, 444);
            this.tbQueries.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbQueries.Multiline = true;
            this.tbQueries.Name = "tbQueries";
            this.tbQueries.Size = new System.Drawing.Size(759, 146);
            this.tbQueries.TabIndex = 3;
            // 
            // btnGenerateSQL
            // 
            this.btnGenerateSQL.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btnGenerateSQL.Location = new System.Drawing.Point(548, 389);
            this.btnGenerateSQL.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnGenerateSQL.Name = "btnGenerateSQL";
            this.btnGenerateSQL.Size = new System.Drawing.Size(229, 41);
            this.btnGenerateSQL.TabIndex = 0;
            this.btnGenerateSQL.Text = "Generate SQL query";
            this.btnGenerateSQL.UseVisualStyleBackColor = true;
            this.btnGenerateSQL.Click += new System.EventHandler(this.btnGenerateSQL_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(138, 32);
            this.label2.TabIndex = 4;
            this.label2.Text = "Query Type";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.label3.Location = new System.Drawing.Point(12, 305);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 32);
            this.label3.TabIndex = 4;
            this.label3.Text = "Job ID";
            // 
            // tbJobID
            // 
            this.tbJobID.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.tbJobID.Location = new System.Drawing.Point(147, 305);
            this.tbJobID.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbJobID.Name = "tbJobID";
            this.tbJobID.Size = new System.Drawing.Size(229, 35);
            this.tbJobID.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.label4.Location = new System.Drawing.Point(12, 348);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(129, 32);
            this.label4.TabIndex = 4;
            this.label4.Text = "Step name";
            // 
            // tbStepName
            // 
            this.tbStepName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.tbStepName.Location = new System.Drawing.Point(147, 348);
            this.tbStepName.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbStepName.Name = "tbStepName";
            this.tbStepName.Size = new System.Drawing.Size(229, 35);
            this.tbStepName.TabIndex = 5;
            // 
            // cbExistingStep
            // 
            this.cbExistingStep.AutoSize = true;
            this.cbExistingStep.Location = new System.Drawing.Point(391, 356);
            this.cbExistingStep.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cbExistingStep.Name = "cbExistingStep";
            this.cbExistingStep.Size = new System.Drawing.Size(145, 24);
            this.cbExistingStep.TabIndex = 6;
            this.cbExistingStep.Text = "Existing Step ? ";
            this.cbExistingStep.UseVisualStyleBackColor = true;
            this.cbExistingStep.CheckedChanged += new System.EventHandler(this.cbExistingStep_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.label1.Location = new System.Drawing.Point(12, 393);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 32);
            this.label1.TabIndex = 4;
            this.label1.Text = "Sequence";
            // 
            // nudSequence
            // 
            this.nudSequence.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.nudSequence.Location = new System.Drawing.Point(147, 394);
            this.nudSequence.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.nudSequence.Name = "nudSequence";
            this.nudSequence.Size = new System.Drawing.Size(229, 35);
            this.nudSequence.TabIndex = 7;
            // 
            // PredefinedQueriesEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(850, 625);
            this.Controls.Add(this.nudSequence);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbExistingStep);
            this.Controls.Add(this.tbStepName);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tbJobID);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnGenerateSQL);
            this.Controls.Add(this.tbQueries);
            this.Controls.Add(this.dgvQueries);
            this.Controls.Add(this.cbQueryTypes);
            this.Controls.Add(this.button1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "PredefinedQueriesEditor";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.PredefinedQueriesEditor_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvQueries)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSequence)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }
}