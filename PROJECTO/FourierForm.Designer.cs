namespace PROJECTO
{
    partial class FourierForm
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
            this.statusStripFourier = new System.Windows.Forms.StatusStrip();
            this.lbMode = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStripFourier = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fourierToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.magnitudePlotToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.phasePlotToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.backToSpatialDomainToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.filtersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pictureBoxFourier = new System.Windows.Forms.PictureBox();
            this.saveFileFourierDialog = new System.Windows.Forms.SaveFileDialog();
            this.statusStripFourier.SuspendLayout();
            this.menuStripFourier.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxFourier)).BeginInit();
            this.SuspendLayout();
            // 
            // statusStripFourier
            // 
            this.statusStripFourier.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lbMode});
            this.statusStripFourier.Location = new System.Drawing.Point(0, 385);
            this.statusStripFourier.Name = "statusStripFourier";
            this.statusStripFourier.Size = new System.Drawing.Size(676, 22);
            this.statusStripFourier.TabIndex = 0;
            this.statusStripFourier.Text = "statusStrip1";
            // 
            // lbMode
            // 
            this.lbMode.Name = "lbMode";
            this.lbMode.Size = new System.Drawing.Size(41, 17);
            this.lbMode.Text = "MODE";
            // 
            // menuStripFourier
            // 
            this.menuStripFourier.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.fourierToolStripMenuItem,
            this.filtersToolStripMenuItem});
            this.menuStripFourier.Location = new System.Drawing.Point(0, 0);
            this.menuStripFourier.Name = "menuStripFourier";
            this.menuStripFourier.Size = new System.Drawing.Size(676, 24);
            this.menuStripFourier.TabIndex = 1;
            this.menuStripFourier.Text = "menuStrip1";
            this.menuStripFourier.Visible = false;
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToolStripMenuItem,
            this.closeToolStripMenuItem});
            this.fileToolStripMenuItem.MergeAction = System.Windows.Forms.MergeAction.MatchOnly;
            this.fileToolStripMenuItem.MergeIndex = 1;
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.saveToolStripMenuItem.MergeIndex = 1;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.closeToolStripMenuItem.MergeIndex = 2;
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // fourierToolStripMenuItem
            // 
            this.fourierToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.magnitudePlotToolStripMenuItem,
            this.phasePlotToolStripMenuItem,
            this.toolStripMenuItem1,
            this.backToSpatialDomainToolStripMenuItem});
            this.fourierToolStripMenuItem.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.fourierToolStripMenuItem.MergeIndex = 2;
            this.fourierToolStripMenuItem.Name = "fourierToolStripMenuItem";
            this.fourierToolStripMenuItem.Size = new System.Drawing.Size(56, 20);
            this.fourierToolStripMenuItem.Text = "Fourier";
            // 
            // magnitudePlotToolStripMenuItem
            // 
            this.magnitudePlotToolStripMenuItem.Name = "magnitudePlotToolStripMenuItem";
            this.magnitudePlotToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.magnitudePlotToolStripMenuItem.Text = "Magnitude Plot";
            this.magnitudePlotToolStripMenuItem.Click += new System.EventHandler(this.magnitudePlotToolStripMenuItem_Click);
            // 
            // phasePlotToolStripMenuItem
            // 
            this.phasePlotToolStripMenuItem.Name = "phasePlotToolStripMenuItem";
            this.phasePlotToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.phasePlotToolStripMenuItem.Text = "Phase Plot";
            this.phasePlotToolStripMenuItem.Click += new System.EventHandler(this.phasePlotToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(193, 6);
            // 
            // backToSpatialDomainToolStripMenuItem
            // 
            this.backToSpatialDomainToolStripMenuItem.Name = "backToSpatialDomainToolStripMenuItem";
            this.backToSpatialDomainToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.backToSpatialDomainToolStripMenuItem.Text = "Back to Spatial Domain";
            this.backToSpatialDomainToolStripMenuItem.Click += new System.EventHandler(this.backToSpatialDomainToolStripMenuItem_Click);
            // 
            // filtersToolStripMenuItem
            // 
            this.filtersToolStripMenuItem.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.filtersToolStripMenuItem.MergeIndex = 3;
            this.filtersToolStripMenuItem.Name = "filtersToolStripMenuItem";
            this.filtersToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
            this.filtersToolStripMenuItem.Text = "Filters";
            // 
            // pictureBoxFourier
            // 
            this.pictureBoxFourier.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxFourier.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxFourier.Name = "pictureBoxFourier";
            this.pictureBoxFourier.Size = new System.Drawing.Size(676, 385);
            this.pictureBoxFourier.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxFourier.TabIndex = 2;
            this.pictureBoxFourier.TabStop = false;
            // 
            // saveFileFourierDialog
            // 
            this.saveFileFourierDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.saveFileFourierDialog_FileOk);
            // 
            // FourierForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(676, 407);
            this.Controls.Add(this.pictureBoxFourier);
            this.Controls.Add(this.menuStripFourier);
            this.Controls.Add(this.statusStripFourier);
            this.MainMenuStrip = this.menuStripFourier;
            this.Name = "FourierForm";
            this.Text = "Fourier Transform";
            this.Load += new System.EventHandler(this.FourierForm_Load);
            this.statusStripFourier.ResumeLayout(false);
            this.statusStripFourier.PerformLayout();
            this.menuStripFourier.ResumeLayout(false);
            this.menuStripFourier.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxFourier)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStripFourier;
        private System.Windows.Forms.MenuStrip menuStripFourier;
        private System.Windows.Forms.ToolStripMenuItem fourierToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem backToSpatialDomainToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem filtersToolStripMenuItem;
        private System.Windows.Forms.PictureBox pictureBoxFourier;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem magnitudePlotToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem phasePlotToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripStatusLabel lbMode;
        private System.Windows.Forms.SaveFileDialog saveFileFourierDialog;

    }
}