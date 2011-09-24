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
    public partial class ReplicateForm : Form
    {

        #region Attributes

        private decimal _selection;

        #endregion

        #region Constructors

        public ReplicateForm()
        {
            InitializeComponent();

            DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        #endregion

        #region Properties
        
        public decimal Selection
        {
            get
            {
                return _selection;
            }
        }

        #endregion

        #region Methods

        private void Ok()
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
            _selection = slideNumber.Value;
            Close();
        }

        private void Cancel()
        {
            Close();
        }

        #endregion

        #region Events

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            label2.Text = slideNumber.Value == 1 ? " time" : " times";
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            Cancel();
        }

        private void btOk_Click(object sender, EventArgs e)
        {
            Ok();
        }

        #endregion

    }
}
