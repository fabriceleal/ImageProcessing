using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Framework.Core;
using Framework.Core.Filters.Base;
using Framework.Core.Filters.Frequency;
using Framework.Core.Filters.Spatial;
using Framework.Filters;
using Framework.Transforms;

namespace PROJECTO
{
    public partial class ImageForm : Form
    {
        #region Attributes

        private string _fileSrc;
        private PictureBoxSizeMode _mode;

        #endregion

        #region Constructors

        /// <summary>
        /// Empty
        /// </summary>
        private ImageForm()
        {
            InitializeComponent();

            Mode = PictureBoxSizeMode.CenterImage;
        }

        public ImageForm(string iFileSrc)
            : this()
        {
            // Load image from file; store file name
            _fileSrc = iFileSrc;
            Text = _fileSrc;

            Reload();
        }

        public ImageForm(ImageForm iFormImageSrc)
            : this()
        {

            // Inherit settings also ...
            Mode = iFormImageSrc.Mode;

            // Copy the image, pixel-by-pixel ...
            SetImage((Image)iFormImageSrc.pictureBoxImage.Image.Clone());
        }

        public ImageForm(Image iImageSrc)
            : this()
        {
            SetImage(iImageSrc);
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
                    saveFileImageDialog.Filter = father.ImageDialogFilter;
                }
                base.MdiParent = value;
            }
        }

        public PictureBoxSizeMode Mode
        {
            get { return _mode; }
            set
            {
                _mode = value;
                pictureBoxImage.SizeMode = value;
                lbMode.Text = value.ToString();
            }
        }

        #endregion

        #region Methods

        private void Reload()
        {
            try
            {
                if (string.IsNullOrEmpty(_fileSrc))
                {
                    SetImage(null);
                }
                else
                {
                    SetImage(Image.FromFile(_fileSrc));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        private void SetImage(Image img)
        {
            try
            {
                pictureBoxImage.Image = img;
                if (null == img)
                {
                    lbDims.Text = "- x -";
                }
                else
                {
                    lbDims.Text = img.Width.ToString() + " x " + img.Height.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        private void ExecuteFilter(Type t, Image img)
        {
            try
            {
                // Initialize instance
                FilterCore filter = (FilterCore)System.Activator.CreateInstance(t);

                // Setup instance (if necessary ...)
                SortedDictionary<string, object> configs = filter.GetDefaultConfigs();
                if (null != configs)
                {
                    ConfigurationsForm conf = new ConfigurationsForm(ref configs);
                    if (conf.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                        return;
                }

                // Execute filter, get new image ...
                Image ret = filter.ApplyFilter(img, configs);

                ret.Tag = Facilities.CloneTag(img);
                Facilities.AddFilterExecution(ref ret, filter, configs);

                ImageForm ret_form = new ImageForm(ret);
                ret_form.MdiParent = MdiParent;
                ret_form.Mode = Mode;
                ret_form.Show();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, e.GetType().FullName);
            }
        }

        private ToolStripItem GetItemByText(ToolStripDropDownItem menu, string text)
        {
            if (null == menu)
            {
                return null;
            }

            foreach (ToolStripItem item in menu.DropDownItems)
            {
                if (item.Text == text)
                {
                    return item;
                }
            }

            return null;
        }

        private void LoadAllFilters()
        {
            try
            {
                Filter[] filters = Factory.GetImplementedFilters();

                foreach (Filter f in filters)
                {
                    string[] categories = (string[])f.Attributes["Categories"];

                    ToolStripDropDownItem mnu = MnuFilters;

                    if (categories != null)
                    {
                        // For each filter, build the category-tree menu
                        foreach (string cat in categories)
                        {
                            ToolStripDropDownItem mnu2 = (ToolStripDropDownItem)GetItemByText(mnu, cat);
                            if (mnu2 == null)
                            {
                                mnu2 = (ToolStripDropDownItem)mnu.DropDownItems.Add(cat);
                            }

                            mnu = mnu2;
                        }
                    }

                    // And add a button for setup and calling the filter against the selected image
                    // Put the f in the Tag property of the button ...
                    ToolStripItem button = mnu.DropDownItems.Add(f.Attributes["ShortName"].ToString());
                    button.Tag = f;
                    button.Click += new EventHandler(FILTER_ToolStripMenuItem_Click);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        #endregion

        #region Events

        #region Menu Image

        private void cloneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ImageForm img = new ImageForm(this);
                img.MdiParent = MdiParent;
                img.Show();
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
                Reload();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
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

        private void saveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                // Save to disk ...
                saveFileImageDialog.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        private void saveFileImageDialog_FileOk(object sender, CancelEventArgs e)
        {
            try
            {
                SaveFileDialog wrkSender = sender as SaveFileDialog;

                if (wrkSender != null)
                {
                    pictureBoxImage.Image.Save(wrkSender.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        private void fourierTransformToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                FourierTransform t = new FourierTransform();

                FourierForm frm = new FourierForm(
                    t.ApplyTransform(
                        Facilities.To8bppGreyScale(
                            Facilities.ToBitmap(pictureBoxImage.Image))));
                frm.MdiParent = MdiParent;
                frm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        private void openEnhancedImageFormToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                RealTimeFilter imForm = new RealTimeFilter(pictureBoxImage.Image);
                imForm.MdiParent = MdiParent;
                imForm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        #region Display Mode

        private void normalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Mode = PictureBoxSizeMode.CenterImage;
        }

        private void fitToScreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Mode = PictureBoxSizeMode.Zoom;
        }

        #endregion

        #endregion

        #region Menu Filters

        private void FILTER_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // Handlers assigned by code ...
                if (sender == null)
                    return;

                ToolStripItem wrkSender = (ToolStripItem)sender;

                if (wrkSender.Tag == null)
                    return;

                Filter filter = (Filter)wrkSender.Tag;

                System.Diagnostics.Debug.Print(
                        "Hello from " + filter.Attributes["Name"].ToString() + ":" + Environment.NewLine +
                        filter.Attributes["Description"].ToString());


                ExecuteFilter(filter.FilterType, pictureBoxImage.Image);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        #endregion

        #region Form

        private void FormImage_Load(object sender, EventArgs e)
        {
            try
            {
                LoadAllFilters();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        #endregion

        #region Menu Executed

        private void executedFiltersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ExecutedFiltersForm exec = new ExecutedFiltersForm(pictureBoxImage.Image);
                exec.MdiParent = MdiParent;
                exec.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        #endregion

        #endregion

    }
}
