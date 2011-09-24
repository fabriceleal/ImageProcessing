using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Framework.Core.Filters.Base;
using Framework.Transforms;
using Framework.Core;

namespace PROJECTO
{
    public partial class RealTimeFilter : Form
    {

        #region Attributes

        private bool _isOriginal;
        private Filter _filter;
        private Image _image;
        private Image _imageFiltered;

        private object _mutex = new object();

        #endregion

        #region Constructors

        public RealTimeFilter()
        {
            InitializeComponent();

            panelConfigs.ConfigsChanged += new ConfigsPanel.DelegateConfigsChanged(configsPanel1_ConfigsChanged);
        }

        public RealTimeFilter(Image image)
            : this()
        {
            _image = image;
            SetImage(true);
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
                try
                {
                    MainForm father = value as MainForm;
                    if (null != father)
                    {
                        openImageFileDialog.Filter = father.ImageDialogFilter;
                        saveFileImageDialog.Filter = father.ImageDialogFilter;
                    }
                    base.MdiParent = value;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.GetType().FullName, ex.Message);
                }
            }
        }

        #endregion

        #region Methods

        private void SetImage(bool original)
        {
            if (_image == null)
            {
                // Force isOriginal to true
                _isOriginal = true;

                lbStatus.Text = "";

                pictureFilteredImage.Image = null;
            }
            else
            {
                if (_filter == null)
                {
                    //Force isOriginal to true
                    _isOriginal = true;
                }
                else
                {
                    _isOriginal = original;
                }

                lbStatus.Text = _isOriginal ? "ORIGINAL" : "FILTERED";

                pictureFilteredImage.Image = _isOriginal ? _image : _imageFiltered;
            }
        }

        private void ExecuteFilter()
        {

            if (_filter == null || _image == null)
                return;

            try
            {
                if (System.Threading.Monitor.TryEnter(_mutex, 0))
                {
                    Cursor = Cursors.WaitCursor;

                    try
                    {
                        FilterCore f = (FilterCore)Activator.CreateInstance(_filter.FilterType);

                        SortedDictionary<string, object> conf = panelConfigs.GetConfigs();
                        _imageFiltered = f.ApplyFilter(_image, conf);
                        SetImage(false);

                        String sParams = "";
                        if (conf != null)
                        {
                            if (conf.Count > 0)
                            {
                                IEnumerator<KeyValuePair<string, object>> k = conf.GetEnumerator();

                                k.MoveNext();
                                sParams = k.Current.Key + "=" + k.Current.Value.ToString();

                                while (k.MoveNext())
                                {
                                    sParams += ", " + k.Current.Key + "=" + k.Current.Value.ToString();
                                }

                            }
                        }
                        this.Text = _filter.FilterType.FullName + " {" + sParams + "}";

                        Image img = pictureFilteredImage.Image;
                        img.Tag = Facilities.CloneTag(_image);
                        Facilities.AddFilterExecution(ref img, f, conf);

                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message, e.GetType().FullName);
                    }

                    Cursor = Cursors.Default;

                    System.Threading.Thread.Sleep(500);
                    System.Threading.Monitor.Exit(_mutex);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        #endregion

        #region Events

        private void configsPanel1_ConfigsChanged(object sender, EventArgs e)
        {
            try
            {
                ExecuteFilter();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        private void btSelect_Click(object sender, EventArgs e)
        {
            try
            {
                AlgorithmsForm sel = new AlgorithmsForm(false);
                sel.ShowDialog();
                if (sel.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    Filter[] filter = sel.Selection;

                    _filter = filter[0];
                    panelConfigs.SetFilter(_filter);
                    txFilter.Text = _filter.FilterType.FullName;

                    ExecuteFilter();

                }
                sel.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        #region Menus

        private void cloneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                RealTimeFilter enh = new RealTimeFilter(pictureFilteredImage.Image);
                enh.pictureFilteredImage.SizeMode = pictureFilteredImage.SizeMode;
                enh.MdiParent = MdiParent;
                enh.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        private void fitToScreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureFilteredImage.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private void normalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureFilteredImage.SizeMode = PictureBoxSizeMode.CenterImage;
        }

        private void fourierTransformToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                FourierTransform t = new FourierTransform();

                FourierForm frm = new FourierForm(
                    t.ApplyTransform(
                        Facilities.To8bppGreyScale(
                            Facilities.ToBitmap(pictureFilteredImage.Image))));
                frm.MdiParent = MdiParent;
                frm.Show();
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
                saveFileImageDialog.ShowDialog();
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

        private void cloneOriginalMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                RealTimeFilter enh = new RealTimeFilter(_image);
                enh.pictureFilteredImage.SizeMode = pictureFilteredImage.SizeMode;
                enh.MdiParent = MdiParent;
                enh.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        private void openNormalImageFormToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ImageForm imForm = new ImageForm(pictureFilteredImage.Image);
                imForm.MdiParent = MdiParent;
                imForm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                openImageFileDialog.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        private void openImageFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            try
            {
                string filename = ((OpenFileDialog)sender).FileName;

                _image = Image.FromFile(filename);
                SetImage(true);

                ExecuteFilter();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().FullName);
            }
        }

        private void saveFileImageDialog_FileOk(object sender, CancelEventArgs e)
        {
            try
            {
                string filename = ((SaveFileDialog)sender).FileName;

                pictureFilteredImage.Image.Save(filename, pictureFilteredImage.Image.RawFormat);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().FullName);
            }
        }

        private void executedFiltersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ExecutedFiltersForm exec = new ExecutedFiltersForm(pictureFilteredImage.Image);
                exec.MdiParent = MdiParent;
                exec.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        #endregion

        private void btSwap_Click(object sender, EventArgs e)
        {
            try
            {
                SetImage(!_isOriginal);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        #endregion

    }
}
