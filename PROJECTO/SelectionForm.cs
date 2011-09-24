using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PROJECTO
{
    public partial class SelectionForm<T> : Form
        where T : class
    {

        #region Attributes

        private Array _options;
        private T[] _selection;

        #endregion

        #region Constructors

        public SelectionForm(Array options, bool multiSelect)
        {
            InitializeComponent();

            try
            {
                _options = options;

                BindingList<Tuple<T>> ls_options = new BindingList<Tuple<T>>();
                foreach (object o in _options)
                {
                    ls_options.Add(new Tuple<T>(o as T));
                }
                dataGridView1.AutoGenerateColumns = false;
                dataGridView1.DataSource = ls_options;
                dataGridView1.MultiSelect = multiSelect;

                DialogResult = DialogResult.Cancel;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        #endregion

        #region Properties

        public T[] Selection
        {
            get
            {
                return _selection;
            }
        }

        #endregion

        #region Methods

        private void SubmitSelection()
        {
            try
            {
                List<T> options = new List<T>();
                foreach (DataGridViewRow r in dataGridView1.SelectedRows)
                {
                    options.Add(r.Cells[0].Value as T);
                }
                _selection = options.ToArray();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        private void Cancel()
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

        private void Ok()
        {
            try
            {
                SubmitSelection();
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        #endregion

        #region Events

        private void btOk_Click(object sender, EventArgs e)
        {
            try
            {
                Ok();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            try
            {
                Cancel();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                Ok();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                switch (e.KeyCode)
                {
                    case Keys.Enter:
                        Ok();
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        private void SelectionForm_Load(object sender, EventArgs e)
        {

        }

        #endregion

    }
}
