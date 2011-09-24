namespace PROJECTO
{
    partial class ImageForm
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
            this.menuStripImage = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reloadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.executedFiltersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cloneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openEnhancedImageFormToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.fitToScreenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.normalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.fourierTransformToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MnuFilters = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStripImage = new System.Windows.Forms.StatusStrip();
            this.lbMode = new System.Windows.Forms.ToolStripStatusLabel();
            this.lbDims = new System.Windows.Forms.ToolStripStatusLabel();
            this.pictureBoxImage = new System.Windows.Forms.PictureBox();
            this.saveFileImageDialog = new System.Windows.Forms.SaveFileDialog();
            this.menuStripImage.SuspendLayout();
            this.statusStripImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxImage)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStripImage
            // 
            this.menuStripImage.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.imageToolStripMenuItem,
            this.MnuFilters});
            this.menuStripImage.Location = new System.Drawing.Point(0, 0);
            this.menuStripImage.Name = "menuStripImage";
            this.menuStripImage.Size = new System.Drawing.Size(752, 24);
            this.menuStripImage.TabIndex = 4;
            this.menuStripImage.Text = "menuStrip1";
            this.menuStripImage.Visible = false;
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.reloadToolStripMenuItem,
            this.saveToolStripMenuItem1,
            this.closeToolStripMenuItem});
            this.fileToolStripMenuItem.MergeAction = System.Windows.Forms.MergeAction.MatchOnly;
            this.fileToolStripMenuItem.MergeIndex = 1;
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // reloadToolStripMenuItem
            // 
            this.reloadToolStripMenuItem.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.reloadToolStripMenuItem.MergeIndex = 1;
            this.reloadToolStripMenuItem.Name = "reloadToolStripMenuItem";
            this.reloadToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.reloadToolStripMenuItem.Text = "Reload";
            this.reloadToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem1
            // 
            this.saveToolStripMenuItem1.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.saveToolStripMenuItem1.MergeIndex = 2;
            this.saveToolStripMenuItem1.Name = "saveToolStripMenuItem1";
            this.saveToolStripMenuItem1.Size = new System.Drawing.Size(110, 22);
            this.saveToolStripMenuItem1.Text = "Save";
            this.saveToolStripMenuItem1.Click += new System.EventHandler(this.saveToolStripMenuItem1_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.closeToolStripMenuItem.MergeIndex = 3;
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.executedFiltersToolStripMenuItem});
            this.viewToolStripMenuItem.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.viewToolStripMenuItem.MergeIndex = 2;
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // executedFiltersToolStripMenuItem
            // 
            this.executedFiltersToolStripMenuItem.Name = "executedFiltersToolStripMenuItem";
            this.executedFiltersToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.executedFiltersToolStripMenuItem.Text = "Executed Filters";
            this.executedFiltersToolStripMenuItem.Click += new System.EventHandler(this.executedFiltersToolStripMenuItem_Click);
            // 
            // imageToolStripMenuItem
            // 
            this.imageToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cloneToolStripMenuItem,
            this.openEnhancedImageFormToolStripMenuItem,
            this.toolStripMenuItem1,
            this.fitToScreenToolStripMenuItem,
            this.normalToolStripMenuItem,
            this.toolStripMenuItem2,
            this.fourierTransformToolStripMenuItem});
            this.imageToolStripMenuItem.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.imageToolStripMenuItem.MergeIndex = 3;
            this.imageToolStripMenuItem.Name = "imageToolStripMenuItem";
            this.imageToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.imageToolStripMenuItem.Text = "Image";
            // 
            // cloneToolStripMenuItem
            // 
            this.cloneToolStripMenuItem.Name = "cloneToolStripMenuItem";
            this.cloneToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.cloneToolStripMenuItem.Text = "Clone";
            this.cloneToolStripMenuItem.Click += new System.EventHandler(this.cloneToolStripMenuItem_Click);
            // 
            // openEnhancedImageFormToolStripMenuItem
            // 
            this.openEnhancedImageFormToolStripMenuItem.Name = "openEnhancedImageFormToolStripMenuItem";
            this.openEnhancedImageFormToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.openEnhancedImageFormToolStripMenuItem.Text = "Open Real Time Filter";
            this.openEnhancedImageFormToolStripMenuItem.Click += new System.EventHandler(this.openEnhancedImageFormToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(184, 6);
            // 
            // fitToScreenToolStripMenuItem
            // 
            this.fitToScreenToolStripMenuItem.Name = "fitToScreenToolStripMenuItem";
            this.fitToScreenToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.fitToScreenToolStripMenuItem.Text = "Fit to Screen";
            this.fitToScreenToolStripMenuItem.Click += new System.EventHandler(this.fitToScreenToolStripMenuItem_Click);
            // 
            // normalToolStripMenuItem
            // 
            this.normalToolStripMenuItem.Name = "normalToolStripMenuItem";
            this.normalToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.normalToolStripMenuItem.Text = "Normal";
            this.normalToolStripMenuItem.Click += new System.EventHandler(this.normalToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(184, 6);
            // 
            // fourierTransformToolStripMenuItem
            // 
            this.fourierTransformToolStripMenuItem.Name = "fourierTransformToolStripMenuItem";
            this.fourierTransformToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.fourierTransformToolStripMenuItem.Text = "Fourier Transform";
            this.fourierTransformToolStripMenuItem.Click += new System.EventHandler(this.fourierTransformToolStripMenuItem_Click);
            // 
            // MnuFilters
            // 
            this.MnuFilters.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.MnuFilters.MergeIndex = 4;
            this.MnuFilters.Name = "MnuFilters";
            this.MnuFilters.Size = new System.Drawing.Size(50, 20);
            this.MnuFilters.Text = "Filters";
            // 
            // statusStripImage
            // 
            this.statusStripImage.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lbMode,
            this.lbDims});
            this.statusStripImage.Location = new System.Drawing.Point(0, 399);
            this.statusStripImage.Name = "statusStripImage";
            this.statusStripImage.Size = new System.Drawing.Size(752, 24);
            this.statusStripImage.TabIndex = 5;
            this.statusStripImage.Text = "statusStrip1";
            // 
            // lbMode
            // 
            this.lbMode.Name = "lbMode";
            this.lbMode.Size = new System.Drawing.Size(41, 19);
            this.lbMode.Text = "MODE";
            // 
            // lbDims
            // 
            this.lbDims.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.lbDims.Name = "lbDims";
            this.lbDims.Size = new System.Drawing.Size(110, 19);
            this.lbDims.Text = "(WIDTH * HEIGHT)";
            // 
            // pictureBoxImage
            // 
            this.pictureBoxImage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBoxImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxImage.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxImage.Name = "pictureBoxImage";
            this.pictureBoxImage.Size = new System.Drawing.Size(752, 399);
            this.pictureBoxImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBoxImage.TabIndex = 6;
            this.pictureBoxImage.TabStop = false;
            // 
            // saveFileImageDialog
            // 
            this.saveFileImageDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.saveFileImageDialog_FileOk);
            // 
            // ImageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(752, 423);
            this.Controls.Add(this.pictureBoxImage);
            this.Controls.Add(this.statusStripImage);
            this.Controls.Add(this.menuStripImage);
            this.MainMenuStrip = this.menuStripImage;
            this.Name = "ImageForm";
            this.Text = "Image";
            this.Load += new System.EventHandler(this.FormImage_Load);
            this.menuStripImage.ResumeLayout(false);
            this.menuStripImage.PerformLayout();
            this.statusStripImage.ResumeLayout(false);
            this.statusStripImage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStripImage;
        private System.Windows.Forms.StatusStrip statusStripImage;
        private System.Windows.Forms.PictureBox pictureBoxImage;
        private System.Windows.Forms.ToolStripMenuItem imageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cloneToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem MnuFilters;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reloadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFileImageDialog;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem fitToScreenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem normalToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem fourierTransformToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel lbMode;
        private System.Windows.Forms.ToolStripStatusLabel lbDims;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem executedFiltersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openEnhancedImageFormToolStripMenuItem;
    }
}