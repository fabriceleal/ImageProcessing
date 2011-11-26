using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Framework.Core;
using Framework.Core.Filters.Frequency;
using Framework.Filters;
using Framework.Transforms;

namespace PROJECTO
{
    public partial class FourierForm : Form
    {
        #region Attributes

        ComplexImage _complex;
        ComplexImage.ComplexImageBitmapType _mode;

        #endregion

        #region Constructors

        public FourierForm()
        {
            InitializeComponent();
        }

        public FourierForm(ComplexImage complexImage)
            : this()
        {
            _complex = complexImage;
        }

        #endregion

        #region Properties

        public new Form MdiParent
        {
            // Keyword new to hide property MdiParent of Form (base)
            get
            {
                return base.MdiParent;
            }
            set
            {

                MainForm father = value as MainForm;
                if (null != father)
                {
                    saveFileFourierDialog.Filter = father.ImageDialogFilter;
                }
                base.MdiParent = value;
            }
        }

        public ComplexImage.ComplexImageBitmapType Mode
        {
            get { return _mode; }
            set
            {
                _mode = value;
                RefreshImage();
                lbMode.Text = value.ToString();
            }
        }

        #endregion

        #region Methods

        private void RefreshImage()
        {
            try
            {
                if (null != _complex)
                    pictureBoxFourier.Image = _complex.ToBitmap(Mode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        #endregion

        #region Events

        private void FourierForm_Load(object sender, EventArgs e)
        {
            Mode = ComplexImage.ComplexImageBitmapType.Magnitude;
        }

        private void backToSpatialDomainToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                FourierTransform t = new FourierTransform();

                ImageForm frmImg = new ImageForm(Facilities.ToImage(Facilities.ToBitmap(t.ApplyReverseTransform(_complex))));
                frmImg.MdiParent = this.MdiParent;
                frmImg.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        private void phasePlotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Mode = ComplexImage.ComplexImageBitmapType.Phase;
        }

        private void magnitudePlotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Mode = ComplexImage.ComplexImageBitmapType.Magnitude;
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // Save to disk ...
                saveFileFourierDialog.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        private void saveFileFourierDialog_FileOk(object sender, CancelEventArgs e)
        {
            try
            {
                SaveFileDialog wrkSender = sender as SaveFileDialog;

                if (wrkSender != null)
                {
                    pictureBoxFourier.Image.Save(wrkSender.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        private void logSlide_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                RefreshImage();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        #endregion

    }
}
