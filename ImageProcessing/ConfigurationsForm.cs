using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Collections;

namespace PROJECTO
{
    public partial class ConfigurationsForm : Form
    {

        #region Attributes

        private SortedDictionary<string, object> _hashtable;

        #endregion

        #region Constructors

        public ConfigurationsForm(ref SortedDictionary<string, object> hashtable)
        {
            InitializeComponent();
            try
            {
                _hashtable = hashtable;
                BindToGrid();

                // set default ...
                DialogResult = DialogResult.Cancel;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        #endregion

        #region Methods

        private void Submit()
        {
            try
            {
                PersistToHashtable();
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        private void BindToGrid()
        {
            try
            {
                gridHashtable.Rows.Clear();
                if (null != _hashtable)
                {
                    foreach (string key in _hashtable.Keys)
                    {
                        gridHashtable.Rows.Add(new object[] { key, _hashtable[key] });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        private void PersistToHashtable()
        {
            try
            {
                if (null != _hashtable)
                {
                    foreach (DataGridViewRow r in gridHashtable.Rows)
                    {
                        if (!string.IsNullOrEmpty(r.Cells[0].Value.ToString()))
                        {
                            _hashtable[r.Cells[0].Value.ToString()] = r.Cells[1].Value;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        #endregion

        #region Events

        private void btnOk_Click(object sender, EventArgs e)
        {
            Submit();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void gridHashtable_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !gridHashtable.IsCurrentCellInEditMode)
                Submit();
        }

        #endregion

    }
}
