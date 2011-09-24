using System;
using System.Collections;
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
    public partial class ExecutedFiltersForm : Form
    {
        #region Attributes

        private Image _img;

        #endregion

        #region Constructors

        public ExecutedFiltersForm()
        {
            InitializeComponent();
        }

        public ExecutedFiltersForm(Image img)
            : this()
        {
            _img = img;
        }

        #endregion

        #region Form

        private void ExecutedFiltersForm_Load(object sender, EventArgs e)
        {
            try
            {
                if (null != _img)
                {
                    ArrayList data = Facilities.GetBucket<ArrayList>(ref _img, Facilities.EXECUTED_FILTERS);
                    if (null != data)
                    {
                        foreach (object o in data)
                        {
                            gridExecutedFilters.Rows.Add(new object[] { o });
                        }
                    }
                    // --
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        private void gridExecutedFilters_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                switch (e.KeyCode)
                {
                    case Keys.Escape:
                        Close();
                        break;
                    // ....
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
