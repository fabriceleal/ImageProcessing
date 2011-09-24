using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PROJECTO
{
    public partial class ParamConfigPanel : UserControl
    {

        #region Attributes

        private ulong _timer = 0;
        private const int THRESHOLD = 8;
        private double _normFactor = 100;

        public delegate void DelegateValueChanged(object sender, EventArgs e);
        public event DelegateValueChanged ValueChanged;

        private bool _ready = false;

        #endregion

        #region Constructors

        public ParamConfigPanel()
        {
            InitializeComponent();

            timer1.Enabled = true;
            timer1.Stop();
        }

        public ParamConfigPanel(String key, double defaultValue, double minValue, double maxValue, double step)
            : this()
        {
            label1.Text = key;

            // calculate normFactor
            _normFactor = Math.Ceiling(1 / step);

            trackBar1.SmallChange = (int)(step * _normFactor);
            trackBar1.Minimum = (int)(minValue * _normFactor);
            trackBar1.Maximum = (int)(maxValue * _normFactor);
            trackBar1.Value = (int)(defaultValue * _normFactor);

            label2.Text = (trackBar1.Value / _normFactor).ToString();

            _ready = true;
        }

        #endregion

        #region Methods

        public string Key
        {
            get
            {
                return label1.Text;
            }
        }

        public double Value
        {
            get
            {
                // apply normFactor
                return (trackBar1.Value / _normFactor);
            }
        }

        #endregion

        #region Events

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            if (_ready)
            {
                _timer = 0;

                timer1.Enabled = true;
                timer1.Start();
            }

            label2.Text = (trackBar1.Value / _normFactor).ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            trackBar1.Value = (int)Math.Max(trackBar1.Minimum, trackBar1.Value - trackBar1.SmallChange);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            trackBar1.Value = (int)Math.Min(trackBar1.Maximum, trackBar1.Value + trackBar1.SmallChange);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (_ready)
            {
                ++_timer;
                if (_timer > THRESHOLD)
                {
                    if (ValueChanged != null)
                    {
                        ValueChanged(this, e);
                    }
                    timer1.Stop();
                }
            }
        }

        #endregion

    }
}
