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

namespace PROJECTO
{
    public partial class AlgorithmsForm : Form
    {

        #region Attributes

        private const string SEPARATOR = "; ";

        private Filter[] _listFilters;
        private List<CheckBox> _checkBoxes;
        private Filter[] _selection;

        #endregion

        #region Constructors

        public AlgorithmsForm(bool multiselect)
        {
            InitializeComponent();
            // --
            DialogResult = DialogResult.Cancel;
            GridAlgorithms.MultiSelect = multiselect;
        }

        #endregion

        #region Properties

        public Filter[] Selection
        {
            get
            {
                return _selection;
            }
        }

        #endregion

        #region Methods

        private Filter[] Get_Selection()
        {
            List<Filter> ret = new List<Filter>();
            try
            {


                foreach (DataGridViewRow r in GridAlgorithms.SelectedRows)
                {
                    ret.Add(((Tuple<string, string, Filter>)r.DataBoundItem).Item3);
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
            return ret.ToArray();
        }


        // Create algorithms' filters
        public void CreateCategories()
        {
            try
            {
                List<string> parsed = new List<string>();
                string key;
                CheckBox cb;

                _checkBoxes = new List<CheckBox>();

                foreach (Filter f in _listFilters)
                {
                    key = string.Join(SEPARATOR, (string[])f.Attributes["Categories"]);
                    if (!parsed.Contains(key))
                    {
                        cb = new CheckBox();
                        cb.Text = key;
                        cb.Checked = true;
                        cb.Width = 200;

                        cb.CheckedChanged += new EventHandler(cb_CheckedChanged);

                        _checkBoxes.Add(cb);
                        panelCategories.Controls.Add(cb);
                        parsed.Add(key);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        private void Cancel()
        {
            Close();
        }

        private void Ok()
        {
            try
            {

                _selection = Get_Selection();

                DialogResult = DialogResult.OK;

                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        // List all algorithms
        public void FillAlgorihtms()
        {
            try
            {
                List<string> keys = new List<string>();
                // get selected check boxes
                foreach (CheckBox cb in _checkBoxes)
                {
                    if (cb.Checked)
                        keys.Add(cb.Text);
                }


                IEnumerable<Filter> q = _listFilters.Where(
                        f => keys.Contains(string.Join(SEPARATOR, (string[])f.Attributes["Categories"])));

                List<Tuple<string, string, Filter>> dataSource = new List<Tuple<string, string, Filter>>();

                dataSource = q.Select(
                        f => new Tuple<string, string, Filter>(
                                f.Attributes["ShortName"].ToString(),
                                string.Join(SEPARATOR, (string[])f.Attributes["Categories"]),
                                f)).ToList();

                GridAlgorithms.DataSource = dataSource;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        #endregion

        #region Events

        private void cb_CheckedChanged(object sender, EventArgs e)
        {
            FillAlgorihtms();
        }

        private void AlgorithmsForm_Load(object sender, EventArgs e)
        {
            try
            {
                GridAlgorithms.AutoGenerateColumns = false;

                _listFilters = Factory.GetImplementedFilters();

                // Create a checkbox for each Algorithm category
                CreateCategories();

                // Fill list of algorithms ... sort by category
                FillAlgorihtms();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Cancel();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Ok();
        }

        private void gridAlgorithms_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Ok();
            }
        }

        private void invertSelection_Click(object sender, EventArgs e)
        {
            foreach (CheckBox cb in _checkBoxes)
            {
                cb.Checked = !cb.Checked;
            }
        }
        #endregion


    }
}
