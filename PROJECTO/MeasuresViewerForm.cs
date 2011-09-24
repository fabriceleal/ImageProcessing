using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Framework.Core.Metrics;
using Framework.Core;

namespace PROJECTO
{
    public partial class MeasuresViewerForm : Form
    {
        #region Attributes

        private List<MetricResult> _list;

        #endregion

        #region Constructors

        public MeasuresViewerForm(List<MetricResult> list)
        {
            InitializeComponent();

            _list = list;
        }

        #endregion

        #region Events

        private void MeasuresViewerForm_Load(object sender, EventArgs e)
        {
            try
            {
                GridMeasures.DataSource = _list;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        private void GridMeasures_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {

                if (e.RowIndex < 0 || e.ColumnIndex < 0)
                    return;

                ImageForm frm;

                if (GridMeasures.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.GetType().Equals(typeof(Image)) ||
                    GridMeasures.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.GetType().Equals(typeof(Bitmap))
                    )
                {
                    frm = new ImageForm((Image)GridMeasures.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                    frm.MdiParent = MdiParent;
                    frm.Show();
                }
                else if (GridMeasures.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.GetType().Equals(typeof(WeakImage)))
                {
                    frm = new ImageForm(((WeakImage)GridMeasures.Rows[e.RowIndex].Cells[e.ColumnIndex].Value).Image);
                    frm.MdiParent = MdiParent;
                    frm.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        #endregion
    }
}
