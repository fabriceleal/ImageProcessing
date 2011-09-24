using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Framework.Core.Filters.Base;
using Framework.Range;

namespace PROJECTO
{
    public partial class ConfigsPanel : UserControl
    {
        #region Attributes

        private List<ParamConfigPanel> _params = new List<ParamConfigPanel>();

        public delegate void DelegateConfigsChanged(object sender, EventArgs e);
        public event DelegateConfigsChanged ConfigsChanged;

        #endregion

        #region Constructor

        public ConfigsPanel()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        public void SetFilter(Filter filter)
        {
            if (null == filter)
                return;

            _params.Clear();
            flowLayoutPanel1.Controls.Clear();

            // ---------------------------------------------

            FilterCore f = (FilterCore)System.Activator.CreateInstance(filter.FilterType);
            SortedDictionary<string, object> dict = f.GetDefaultConfigs();

            if (dict != null)
            {
                foreach (String k in dict.Keys)
                {
                    double defaultValue;
                    double minValue;
                    double maxValue;
                    double step;
                    Rangeable val = dict[k] as Rangeable;

                    if (null != val)
                    {
                        defaultValue = val.Value;
                        minValue = val.Min;
                        maxValue = val.Max;
                        step = val.Step;
                    }
                    else
                    {
                        defaultValue = double.Parse(dict[k].ToString());
                        minValue = double.MinValue;
                        maxValue = double.MaxValue;
                        step = 0.000001;
                    }

                    AddParam(k, defaultValue, minValue, maxValue, step);
                }
            }
        }

        public void AddParam(String key, double defaultValue, double minValue, double maxValue, double step)
        {
            ParamConfigPanel param = new ParamConfigPanel(key, defaultValue, minValue, maxValue, step);
            param.Dock = DockStyle.Top;

            param.ValueChanged += new ParamConfigPanel.DelegateValueChanged(param_ValueChanged);

            _params.Add(param);
            flowLayoutPanel1.Controls.Add(param);
        }

        //public void AddParam(String key, double defaultValue)
        //{
        //    AddParam(key, defaultValue, double.MinValue, double.MaxValue, double.Epsilon);
        //}

        public SortedDictionary<string, object> GetConfigs()
        {
            SortedDictionary<string, object> ret = new SortedDictionary<string, object>();

            foreach (ParamConfigPanel pn in _params)
            {
                ret.Add(pn.Key, pn.Value);
            }

            return ret;
        }

        #endregion

        #region Events

        public void param_ValueChanged(object sender, EventArgs e)
        {
            if (ConfigsChanged != null)
            {
                ConfigsChanged(this, e);
            }
        }

        #endregion

    }
}
