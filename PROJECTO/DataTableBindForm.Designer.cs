namespace PROJECTO
{
    partial class DataTableBindForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DataTableBindForm));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btOpen = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.layoutGrid = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.gridDatatable = new System.Windows.Forms.DataGridView();
            this.openResultsFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.GridProperties = new System.Windows.Forms.PropertyGrid();
            this.saveResultsDialog = new System.Windows.Forms.SaveFileDialog();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridDatatable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 369);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(728, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btOpen,
            this.toolStripSeparator2,
            this.layoutGrid,
            this.toolStripSeparator3});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(728, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btOpen
            // 
            this.btOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btOpen.Image = ((System.Drawing.Image)(resources.GetObject("btOpen.Image")));
            this.btOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btOpen.Name = "btOpen";
            this.btOpen.Size = new System.Drawing.Size(40, 22);
            this.btOpen.Text = "Open";
            this.btOpen.Click += new System.EventHandler(this.btOpen_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // layoutGrid
            // 
            this.layoutGrid.CheckOnClick = true;
            this.layoutGrid.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.layoutGrid.Image = ((System.Drawing.Image)(resources.GetObject("layoutGrid.Image")));
            this.layoutGrid.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.layoutGrid.Name = "layoutGrid";
            this.layoutGrid.Size = new System.Drawing.Size(78, 22);
            this.layoutGrid.Text = "Grid Settings";
            this.layoutGrid.Click += new System.EventHandler(this.layoutGrid_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // gridDatatable
            // 
            this.gridDatatable.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridDatatable.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.gridDatatable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridDatatable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridDatatable.Location = new System.Drawing.Point(0, 0);
            this.gridDatatable.Name = "gridDatatable";
            this.gridDatatable.Size = new System.Drawing.Size(728, 344);
            this.gridDatatable.TabIndex = 2;
            this.gridDatatable.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridDatatable_CellDoubleClick);
            // 
            // openResultsFileDialog
            // 
            this.openResultsFileDialog.Filter = "XML files|*.xml";
            this.openResultsFileDialog.Title = "Open File";
            this.openResultsFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.openResultsFileDialog_FileOk);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 25);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.gridDatatable);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.GridProperties);
            this.splitContainer1.Panel2Collapsed = true;
            this.splitContainer1.Size = new System.Drawing.Size(728, 344);
            this.splitContainer1.SplitterDistance = 500;
            this.splitContainer1.TabIndex = 3;
            // 
            // GridProperties
            // 
            this.GridProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GridProperties.Location = new System.Drawing.Point(0, 0);
            this.GridProperties.Name = "GridProperties";
            this.GridProperties.SelectedObject = this.gridDatatable;
            this.GridProperties.Size = new System.Drawing.Size(96, 100);
            this.GridProperties.TabIndex = 0;
            // 
            // saveResultsDialog
            // 
            this.saveResultsDialog.Filter = "XML files|*.xml";
            this.saveResultsDialog.Title = "Save to File";
            // 
            // DataTableBindForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(728, 391);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.statusStrip1);
            this.Name = "DataTableBindForm";
            this.Text = "Datatable Viewer";
            this.Load += new System.EventHandler(this.DataTableBindForm_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridDatatable)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btOpen;
        private System.Windows.Forms.DataGridView gridDatatable;
        private System.Windows.Forms.OpenFileDialog openResultsFileDialog;
        private System.Windows.Forms.ToolStripButton layoutGrid;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.PropertyGrid GridProperties;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.SaveFileDialog saveResultsDialog;
    }
}