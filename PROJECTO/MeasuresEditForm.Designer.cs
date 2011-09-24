namespace PROJECTO
{
    partial class MeasuresEditForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.gridMetrics = new System.Windows.Forms.DataGridView();
            this.mKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridExecution = new System.Windows.Forms.DataGridView();
            this.eType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eImage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btOk = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.btAdd = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gridMetrics)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridExecution)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(217, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "List of metrics (methods) assigned to the filter";
            // 
            // gridMetrics
            // 
            this.gridMetrics.AllowUserToAddRows = false;
            this.gridMetrics.AllowUserToResizeRows = false;
            this.gridMetrics.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridMetrics.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.mKey});
            this.gridMetrics.Location = new System.Drawing.Point(3, 74);
            this.gridMetrics.Name = "gridMetrics";
            this.gridMetrics.ReadOnly = true;
            this.gridMetrics.RowHeadersWidth = 25;
            this.gridMetrics.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridMetrics.Size = new System.Drawing.Size(241, 210);
            this.gridMetrics.TabIndex = 3;
            this.gridMetrics.SelectionChanged += new System.EventHandler(this.gridMetrics_SelectionChanged);
            // 
            // mKey
            // 
            this.mKey.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.mKey.DataPropertyName = "Item1";
            this.mKey.HeaderText = "Key";
            this.mKey.Name = "mKey";
            this.mKey.ReadOnly = true;
            this.mKey.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // gridExecution
            // 
            this.gridExecution.AllowUserToAddRows = false;
            this.gridExecution.AllowUserToResizeRows = false;
            this.gridExecution.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridExecution.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.eType,
            this.eImage});
            this.gridExecution.Location = new System.Drawing.Point(246, 75);
            this.gridExecution.Name = "gridExecution";
            this.gridExecution.ReadOnly = true;
            this.gridExecution.RowHeadersWidth = 25;
            this.gridExecution.Size = new System.Drawing.Size(352, 210);
            this.gridExecution.TabIndex = 4;
            this.gridExecution.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridExecution_CellDoubleClick);
            // 
            // eType
            // 
            this.eType.DataPropertyName = "Item2";
            this.eType.HeaderText = "Type";
            this.eType.Name = "eType";
            this.eType.ReadOnly = true;
            // 
            // eImage
            // 
            this.eImage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.eImage.DataPropertyName = "Item3";
            this.eImage.HeaderText = "Image";
            this.eImage.Name = "eImage";
            this.eImage.ReadOnly = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(259, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(265, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "List of executions (each metric may have n executions)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(259, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(320, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "A execution may depend only on input-output of the filter, or it may ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(259, 41);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(252, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "operate using a reference and the output of the filter";
            // 
            // btOk
            // 
            this.btOk.Location = new System.Drawing.Point(3, 290);
            this.btOk.Name = "btOk";
            this.btOk.Size = new System.Drawing.Size(75, 23);
            this.btOk.TabIndex = 8;
            this.btOk.Text = "Ok";
            this.btOk.UseVisualStyleBackColor = true;
            this.btOk.Click += new System.EventHandler(this.btOk_Click);
            // 
            // btCancel
            // 
            this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Location = new System.Drawing.Point(523, 290);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 23);
            this.btCancel.TabIndex = 9;
            this.btCancel.Text = "Cancel";
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 318);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(602, 22);
            this.statusStrip1.TabIndex = 10;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // btAdd
            // 
            this.btAdd.Location = new System.Drawing.Point(523, 36);
            this.btAdd.Name = "btAdd";
            this.btAdd.Size = new System.Drawing.Size(75, 23);
            this.btAdd.TabIndex = 11;
            this.btAdd.Text = "Add";
            this.btAdd.UseVisualStyleBackColor = true;
            this.btAdd.Click += new System.EventHandler(this.btAdd_Click);
            // 
            // MeasuresEditForm
            // 
            this.AcceptButton = this.btOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(602, 340);
            this.Controls.Add(this.btAdd);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btOk);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.gridExecution);
            this.Controls.Add(this.gridMetrics);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "MeasuresEditForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Edit Measures Options";
            this.Load += new System.EventHandler(this.MeasuresModeForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridMetrics)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridExecution)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView gridMetrics;
        private System.Windows.Forms.DataGridView gridExecution;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btOk;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.DataGridViewTextBoxColumn mKey;
        private System.Windows.Forms.DataGridViewTextBoxColumn eType;
        private System.Windows.Forms.DataGridViewTextBoxColumn eImage;
        private System.Windows.Forms.Button btAdd;

    }
}