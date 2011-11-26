namespace PROJECTO
{
    partial class MeasuresViewerForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be _disposed; otherwise, false.</param>
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
            this.GridMeasures = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.GridMeasures)).BeginInit();
            this.SuspendLayout();
            // 
            // GridMeasures
            // 
            this.GridMeasures.AllowUserToAddRows = false;
            this.GridMeasures.AllowUserToDeleteRows = false;
            this.GridMeasures.AllowUserToResizeRows = false;
            this.GridMeasures.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GridMeasures.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3});
            this.GridMeasures.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GridMeasures.Location = new System.Drawing.Point(0, 0);
            this.GridMeasures.Name = "GridMeasures";
            this.GridMeasures.ReadOnly = true;
            this.GridMeasures.RowHeadersWidth = 23;
            this.GridMeasures.Size = new System.Drawing.Size(449, 291);
            this.GridMeasures.TabIndex = 0;
            this.GridMeasures.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.GridMeasures_CellDoubleClick);
            // 
            // Column1
            // 
            this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column1.DataPropertyName = "Reference";
            this.Column1.HeaderText = "Reference";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // Column2
            // 
            this.Column2.DataPropertyName = "Key";
            this.Column2.HeaderText = "Key";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // Column3
            // 
            this.Column3.DataPropertyName = "Value";
            this.Column3.HeaderText = "Value";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            // 
            // MeasuresViewerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(449, 291);
            this.Controls.Add(this.GridMeasures);
            this.Name = "MeasuresViewerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MeasuresViewer";
            this.Load += new System.EventHandler(this.MeasuresViewerForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.GridMeasures)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView GridMeasures;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
    }
}