namespace PROJECTO
{
    partial class RealTimeFilter
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RealTimeFilter));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lbStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btSwap = new System.Windows.Forms.ToolStripButton();
            this.pictureFilteredImage = new System.Windows.Forms.PictureBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.txFilter = new System.Windows.Forms.TextBox();
            this.btSelect = new System.Windows.Forms.Button();
            this.panelConfigs = new PROJECTO.ConfigsPanel();
            this.menuStripImage = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.executedFiltersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cloneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cloneOriginalMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openNormalImageFormToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.fitToScreenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.normalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.fourierTransformToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveFileImageDialog = new System.Windows.Forms.SaveFileDialog();
            this.openImageFileDialog = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureFilteredImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.menuStripImage.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.statusStrip1);
            this.splitContainer1.Panel1.Controls.Add(this.toolStrip1);
            this.splitContainer1.Panel1.Controls.Add(this.pictureFilteredImage);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(921, 464);
            this.splitContainer1.SplitterDistance = 603;
            this.splitContainer1.TabIndex = 0;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lbStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 442);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(603, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lbStatus
            // 
            this.lbStatus.Name = "lbStatus";
            this.lbStatus.Size = new System.Drawing.Size(10, 17);
            this.lbStatus.Text = " ";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btSwap});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(603, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btSwap
            // 
            this.btSwap.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btSwap.Image = ((System.Drawing.Image)(resources.GetObject("btSwap.Image")));
            this.btSwap.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btSwap.Name = "btSwap";
            this.btSwap.Size = new System.Drawing.Size(37, 22);
            this.btSwap.Text = "<-->";
            this.btSwap.Click += new System.EventHandler(this.btSwap_Click);
            // 
            // pictureFilteredImage
            // 
            this.pictureFilteredImage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureFilteredImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureFilteredImage.Location = new System.Drawing.Point(0, 0);
            this.pictureFilteredImage.Name = "pictureFilteredImage";
            this.pictureFilteredImage.Size = new System.Drawing.Size(603, 464);
            this.pictureFilteredImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureFilteredImage.TabIndex = 0;
            this.pictureFilteredImage.TabStop = false;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.txFilter);
            this.splitContainer2.Panel1.Controls.Add(this.btSelect);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.panelConfigs);
            this.splitContainer2.Size = new System.Drawing.Size(314, 464);
            this.splitContainer2.SplitterDistance = 25;
            this.splitContainer2.TabIndex = 0;
            // 
            // txFilter
            // 
            this.txFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txFilter.Location = new System.Drawing.Point(28, 0);
            this.txFilter.Name = "txFilter";
            this.txFilter.ReadOnly = true;
            this.txFilter.Size = new System.Drawing.Size(286, 20);
            this.txFilter.TabIndex = 1;
            // 
            // btSelect
            // 
            this.btSelect.Dock = System.Windows.Forms.DockStyle.Left;
            this.btSelect.Location = new System.Drawing.Point(0, 0);
            this.btSelect.Name = "btSelect";
            this.btSelect.Size = new System.Drawing.Size(28, 25);
            this.btSelect.TabIndex = 0;
            this.btSelect.Text = "...";
            this.btSelect.UseVisualStyleBackColor = true;
            this.btSelect.Click += new System.EventHandler(this.btSelect_Click);
            // 
            // panelConfigs
            // 
            this.panelConfigs.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelConfigs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelConfigs.Location = new System.Drawing.Point(0, 0);
            this.panelConfigs.Name = "panelConfigs";
            this.panelConfigs.Size = new System.Drawing.Size(314, 435);
            this.panelConfigs.TabIndex = 0;
            // 
            // menuStripImage
            // 
            this.menuStripImage.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.imageToolStripMenuItem});
            this.menuStripImage.Location = new System.Drawing.Point(0, 0);
            this.menuStripImage.Name = "menuStripImage";
            this.menuStripImage.Size = new System.Drawing.Size(921, 24);
            this.menuStripImage.TabIndex = 5;
            this.menuStripImage.Text = "menuStrip1";
            this.menuStripImage.Visible = false;
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem1,
            this.closeToolStripMenuItem});
            this.fileToolStripMenuItem.MergeAction = System.Windows.Forms.MergeAction.MatchOnly;
            this.fileToolStripMenuItem.MergeIndex = 1;
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.MergeAction = System.Windows.Forms.MergeAction.Replace;
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem1
            // 
            this.saveToolStripMenuItem1.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.saveToolStripMenuItem1.MergeIndex = 2;
            this.saveToolStripMenuItem1.Name = "saveToolStripMenuItem1";
            this.saveToolStripMenuItem1.Size = new System.Drawing.Size(103, 22);
            this.saveToolStripMenuItem1.Text = "Save";
            this.saveToolStripMenuItem1.Click += new System.EventHandler(this.saveToolStripMenuItem1_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.closeToolStripMenuItem.MergeIndex = 3;
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
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
            this.cloneOriginalMenuItem,
            this.openNormalImageFormToolStripMenuItem,
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
            this.cloneToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            this.cloneToolStripMenuItem.Text = "Clone";
            this.cloneToolStripMenuItem.Click += new System.EventHandler(this.cloneToolStripMenuItem_Click);
            // 
            // cloneOriginalMenuItem
            // 
            this.cloneOriginalMenuItem.Name = "cloneOriginalMenuItem";
            this.cloneOriginalMenuItem.Size = new System.Drawing.Size(213, 22);
            this.cloneOriginalMenuItem.Text = "Clone with Original Image";
            this.cloneOriginalMenuItem.Click += new System.EventHandler(this.cloneOriginalMenuItem_Click);
            // 
            // openNormalImageFormToolStripMenuItem
            // 
            this.openNormalImageFormToolStripMenuItem.Name = "openNormalImageFormToolStripMenuItem";
            this.openNormalImageFormToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            this.openNormalImageFormToolStripMenuItem.Text = "Open Normal Image Form";
            this.openNormalImageFormToolStripMenuItem.Click += new System.EventHandler(this.openNormalImageFormToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(210, 6);
            // 
            // fitToScreenToolStripMenuItem
            // 
            this.fitToScreenToolStripMenuItem.Name = "fitToScreenToolStripMenuItem";
            this.fitToScreenToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            this.fitToScreenToolStripMenuItem.Text = "Fit to Screen";
            this.fitToScreenToolStripMenuItem.Click += new System.EventHandler(this.fitToScreenToolStripMenuItem_Click);
            // 
            // normalToolStripMenuItem
            // 
            this.normalToolStripMenuItem.Name = "normalToolStripMenuItem";
            this.normalToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            this.normalToolStripMenuItem.Text = "Normal";
            this.normalToolStripMenuItem.Click += new System.EventHandler(this.normalToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(210, 6);
            // 
            // fourierTransformToolStripMenuItem
            // 
            this.fourierTransformToolStripMenuItem.Name = "fourierTransformToolStripMenuItem";
            this.fourierTransformToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            this.fourierTransformToolStripMenuItem.Text = "Fourier Transform";
            this.fourierTransformToolStripMenuItem.Click += new System.EventHandler(this.fourierTransformToolStripMenuItem_Click);
            // 
            // saveFileImageDialog
            // 
            this.saveFileImageDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.saveFileImageDialog_FileOk);
            // 
            // openImageFileDialog
            // 
            this.openImageFileDialog.Multiselect = true;
            this.openImageFileDialog.Title = "Pick a image";
            this.openImageFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.openImageFileDialog_FileOk);
            // 
            // RealTimeFilter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(921, 464);
            this.Controls.Add(this.menuStripImage);
            this.Controls.Add(this.splitContainer1);
            this.Name = "RealTimeFilter";
            this.Text = "Real Time Filter";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureFilteredImage)).EndInit();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.menuStripImage.ResumeLayout(false);
            this.menuStripImage.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private ConfigsPanel panelConfigs;
        private System.Windows.Forms.PictureBox pictureFilteredImage;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TextBox txFilter;
        private System.Windows.Forms.Button btSelect;
        private System.Windows.Forms.MenuStrip menuStripImage;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem imageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cloneToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openNormalImageFormToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem fitToScreenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem normalToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem fourierTransformToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cloneOriginalMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFileImageDialog;
        private System.Windows.Forms.OpenFileDialog openImageFileDialog;
        private System.Windows.Forms.ToolStripMenuItem executedFiltersToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lbStatus;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btSwap;
    }
}