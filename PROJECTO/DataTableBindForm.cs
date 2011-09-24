using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Framework.Core;

namespace PROJECTO
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// Call inside try-catch, as it throws a lot of exceptions :P
    /// </remarks>
    public partial class DataTableBindForm : Form
    {
        #region Attributes

        private DataTable _dtData;

        #endregion

        #region Constructors

        public DataTableBindForm()
        {
            InitializeComponent();
        }

        public DataTableBindForm(string filename)
            : this()
        {
            try
            {
                _dtData = OpenFile(filename);
            }
            catch (Exception ex)
            {
                throw new Exception("Error when trying to open file {0}.", ex);
            }
        }

        public DataTableBindForm(DataTable dtData)
            : this()
        {
            _dtData = dtData;
        }

        #endregion

        #region Methods

        public static void FromFileNames(Form mdiParent, string[] filenames)
        {
            foreach (string fname in filenames)
            {
                try
                {
                    DataTableBindForm viewer = new DataTableBindForm(fname);
                    viewer.MdiParent = mdiParent;
                    viewer.Show();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error trying to load files.", ex);
                }
            }
        }

        private void Bind()
        {
            if (null != _dtData)
            {

                gridDatatable.DataSource = null;
                gridDatatable.DataSource = _dtData;
            }
        }

        public static DataTable OpenFile(string filename)
        {
            DataSet dsTest = new DataSet();
            DataTable _dtData;
            dsTest.ReadXml(filename);

            if (dsTest.Tables[0].TableName == "Details" || dsTest.Tables[0].TableName == "Details2")
            {
                // Try to convert to a new DataTable;
                // columns with names started with Image* will be converted from
                // base-64 strings to images

                _dtData = new DataTable(dsTest.Tables[0].TableName);
                List<DataColumn> cols = new List<DataColumn>();
                DataRow r;

                foreach (DataColumn col in dsTest.Tables[0].Columns)
                {
                    if (col.ColumnName.StartsWith("Image"))
                    {
                        cols.Add(new DataColumn(col.ColumnName, typeof(Image)));
                    }
                    else
                    {
                        cols.Add(new DataColumn(col.ColumnName, col.DataType));
                    }
                }
                _dtData.Columns.AddRange(cols.ToArray());

                foreach (DataRow dr in dsTest.Tables[0].Rows)
                {
                    r = _dtData.NewRow();

                    foreach (DataColumn col in cols)
                    {
                        if (col.ColumnName.StartsWith("Image"))
                        {
                            r[col.ColumnName] = Facilities.ToImage(dr.Field<string>(col.ColumnName));
                        }
                        else
                        {
                            r[col.ColumnName] = dr[col.ColumnName];
                        }
                    }

                    _dtData.Rows.Add(r);
                }
            }
            else
            {
                throw new Exception("File in unknown format.");
            }
            return _dtData;
        }

        #endregion

        #region Events

        private void DataTableBindForm_Load(object sender, EventArgs e)
        {
            try
            {
                GridProperties.SelectedObject = gridDatatable;

                Bind();

                foreach (DataGridViewColumn col in gridDatatable.Columns)
                {
                    if (col as DataGridViewImageColumn != null)
                    {
                        ((DataGridViewImageColumn)col).ImageLayout = DataGridViewImageCellLayout.Zoom;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        private void btOpen_Click(object sender, EventArgs e)
        {
            try
            {
                openResultsFileDialog.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            try
            {
                saveResultsDialog.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        private void layoutGrid_Click(object sender, EventArgs e)
        {
            splitContainer1.Panel2Collapsed = !layoutGrid.Checked;
        }

        private void gridDatatable_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0 || e.ColumnIndex < 0)
                    return;

                ImageForm frm;

                if (gridDatatable.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.GetType().Equals(typeof(Image)) ||
                    gridDatatable.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.GetType().Equals(typeof(Bitmap)))
                {
                    frm = new ImageForm((Image)gridDatatable.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                    frm.MdiParent = MdiParent;
                    frm.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        private void openResultsFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            try
            {
                string fname = ((OpenFileDialog)sender).FileName;

                _dtData = OpenFile(fname);

                Bind();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().FullName);
            }
        }

        #endregion

    }
}
