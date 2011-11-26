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
    public partial class ErrorForm : Form
    {
        #region Attributes

        private Exception _exception;

        #endregion

        #region Constructors

        private ErrorForm()
        {
            InitializeComponent();
        }

        public ErrorForm(Exception iException)
            : this()
        {
            _exception = iException;

            if (_exception != null)
            {
                Text = _exception.GetType().FullName;

                TbMessage.Text = _exception.Message;
                TbStackTrace.Text = _exception.StackTrace;
                propertyGrid1.SelectedObject = _exception;
            }
        }

        #endregion

    }
}
