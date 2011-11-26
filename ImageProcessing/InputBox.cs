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
    public partial class InputBox : Form
    {

        #region Constructors

        private InputBox()
        {
            InitializeComponent();

            DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        #endregion

        #region Methods

        public static string AskValue(IWin32Window owner, string caption = "InputBox", string description = "Insert a new value.", string value = "")
        {
            InputBox input = new InputBox();
            string ret = "";
            
            try
            {
                input.Text = caption;

                input.lbDescription.Text = description;
                input.txValue.Text = value;

                input.ShowDialog(owner);
                if (input.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    ret = input.txValue.Text;
                }
                input.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }

            return ret;
        }

        #endregion

        #region Events

        private void btOk_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion

    }
}
