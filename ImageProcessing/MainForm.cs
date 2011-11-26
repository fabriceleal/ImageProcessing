using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Framework;
using Framework.Core;
using Framework.Filters;

namespace PROJECTO
{
    public partial class MainForm : Form
    {

        #region Attributes

        public String ImageDialogFilter { get; private set; }

        #endregion

        #region Constructors

        public MainForm()
        {
            try
            {
                InitializeComponent();
                ImageDialogFilter = Factory.GetImageEncodersToDialogFilter(true, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        #endregion

        #region Events

        #region Menu File

        private void openToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            try
            {
                openImageFileDialog.Filter = ImageDialogFilter;
                openImageFileDialog.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            try
            {
                OpenFileDialog wrkSender = sender as OpenFileDialog;

                if (wrkSender != null)
                {
                    foreach (String fn in wrkSender.FileNames)
                    {
                        try
                        {
                            ImageForm img = new ImageForm(fn);
                            img.MdiParent = this;
                            img.Show();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, ex.GetType().FullName);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void closeAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // Close all MDI Children ...
                foreach (Form toClose in this.MdiChildren)
                {
                    toClose.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        private void minimizeAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // minimize MDI Children ...
                foreach (Form toClose in this.MdiChildren)
                {
                    toClose.WindowState = FormWindowState.Minimized;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        private void maximizeAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // maximize all MDI Children ...
                foreach (Form toClose in this.MdiChildren)
                {
                    toClose.WindowState = FormWindowState.Maximized;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        private void restoreAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // Restore all MDI Children ...
                foreach (Form toClose in this.MdiChildren)
                {
                    toClose.WindowState = FormWindowState.Normal;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        #endregion

        #region Form

        private void ParentForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (e.CloseReason == CloseReason.UserClosing && this.MdiChildren.Length > 0)
                {
                    e.Cancel = (MessageBox.Show("Close app?", "Question", MessageBoxButtons.YesNo) != System.Windows.Forms.DialogResult.Yes);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
                e.Cancel = false;
            }
        }

        private void FormParent_Load(object sender, EventArgs e)
        {

        }

        private void previousWindItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (null != MdiChildren && MdiChildren.Length > 0)
                {
                    int length = MdiChildren.Length;
                    int i = Array.IndexOf<Form>(MdiChildren, ActiveMdiChild);
                    i += length; // to avoid negative indexes ...
                    i = (int)Math.Abs(i - 1);
                    MdiChildren[i % length].Select();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        private void nextWindItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (null != MdiChildren && MdiChildren.Length > 0)
                {
                    int length = MdiChildren.Length;
                    int i = Array.IndexOf<Form>(MdiChildren, ActiveMdiChild);
                    i = (int)Math.Abs(i + 1);
                    MdiChildren[i % length].Select();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        #endregion

        #region Run

        private void btWizardTree_Click(object sender, EventArgs e)
        {
            try
            {
                TreeBatch tree = new TreeBatch();
                tree.MdiParent = this;
                tree.Show();
                tree.WindowState = FormWindowState.Maximized;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        private void btDynamicFilters_Click(object sender, EventArgs e)
        {
            try
            {
                RealTimeFilter enhan = new RealTimeFilter();
                enhan.MdiParent = this;
                enhan.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }

        }

        private void btDetails_Click(object sender, EventArgs e)
        {
            try
            {
                OpenDialog.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        private void OpenDialog_FileOk(object sender, CancelEventArgs e)
        {
            string[] fnames = ((OpenFileDialog)sender).FileNames;
            try
            {
                DataTableBindForm.FromFileNames(this, fnames);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().FullName);
            }
        }

        #endregion

        #endregion

    }
}
