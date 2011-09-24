namespace PROJECTO
{
    partial class ExecutedFiltersForm
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
            this.gridExecutedFilters = new System.Windows.Forms.DataGridView();
            this.ExecutedFilter = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.gridExecutedFilters)).BeginInit();
            this.SuspendLayout();
            // 
            // gridExecutedFilters
            // 
            this.gridExecutedFilters.AllowUserToAddRows = false;
            this.gridExecutedFilters.AllowUserToDeleteRows = false;
            this.gridExecutedFilters.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridExecutedFilters.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ExecutedFilter});
            this.gridExecutedFilters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridExecutedFilters.Location = new System.Drawing.Point(0, 0);
            this.gridExecutedFilters.Name = "gridExecutedFilters";
            this.gridExecutedFilters.ReadOnly = true;
            this.gridExecutedFilters.RowHeadersVisible = false;
            this.gridExecutedFilters.Size = new System.Drawing.Size(306, 303);
            this.gridExecutedFilters.TabIndex = 1;
            this.gridExecutedFilters.KeyDown += new System.Windows.Forms.KeyEventHandler(this.gridExecutedFilters_KeyDown);
            // 
            // ExecutedFilter
            // 
            this.ExecutedFilter.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ExecutedFilter.HeaderText = "";
            this.ExecutedFilter.Name = "ExecutedFilter";
            this.ExecutedFilter.ReadOnly = true;
            // 
            // ExecutedFiltersForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(306, 303);
            this.Controls.Add(this.gridExecutedFilters);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.KeyPreview = true;
            this.Name = "ExecutedFiltersForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Executed Filters";
            this.Load += new System.EventHandler(this.ExecutedFiltersForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridExecutedFilters)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView gridExecutedFilters;
        private System.Windows.Forms.DataGridViewTextBoxColumn ExecutedFilter;
    }
}