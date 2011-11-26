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
using Framework.Core.Metrics;
using Framework.Batch;

namespace PROJECTO
{
    public partial class MeasuresEditForm : Form
    {
        #region Attributes

        private Filter _filter;
        private WeakImage[] _references;
        private List<MetricExecBase> _originalMetrics;
        private BindingList<RowMetric> _metrics;
        private BindingList<RowMetricExecution> _executions;
        private List<MetricExecBase> _execs;
        private RowMetric _selected;

        #endregion

        #region Constructors

        public MeasuresEditForm(Filter filter, WeakImage[] references, List<MetricExecBase> metrics)
        {
            InitializeComponent();

            try
            {
                _filter = filter;
                _references = references;
                _originalMetrics = metrics;

                DialogResult = System.Windows.Forms.DialogResult.Cancel;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        #endregion

        #region Properties

        public List<MetricExecBase> Execs
        {
            get
            {
                return _execs;
            }
        }

        #endregion

        #region Methods

        private void Ok()
        {
            try
            {

                // to isEnd save ...
                SaveCurrent();

                // Transform to instance of ....
                _execs = new List<MetricExecBase>();
                string key;

                foreach (RowMetricExecution r in _executions)
                {
                    key = _metrics.Where(f => object.Equals(f.Item2, r.Item1)).Select(f => f.Item1).FirstOrDefault();

                    switch (r.Item2)
                    {
                        case Framework.Batch.MetricExecutionType.InputOutput:
                            _execs.Add(new MetricExec(key, r.Item1));
                            break;
                        case Framework.Batch.MetricExecutionType.RefOutput:
                            _execs.Add(new MetricExecReference(key, r.Item1, r.Item3));
                            break;
                    }
                }

                DialogResult = System.Windows.Forms.DialogResult.OK;
                Close();
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

        private void SaveCurrent()
        {
            try
            {
                if (gridMetrics.DataSource == null)
                    return;

                DataGridViewSelectedRowCollection rows = gridMetrics.SelectedRows;

                // update filter ...
                if (null == rows || rows.Count > 1)
                {
                    // no filter
                    gridExecution.DataSource = null;
                }
                else
                {
                    // pastSelected is the old selection
                    RowMetric pastSelected = _selected;
                    // currentSelected is the new selection
                    RowMetric currentSelected = (RowMetric)(rows.Count == 0 ? null : rows[0].DataBoundItem);
                    BindingList<RowMetricExecution> datasource = (BindingList<RowMetricExecution>)gridExecution.DataSource;

                    if (datasource != null && pastSelected != null)
                    {

                        if (datasource.Count > 0)
                        {
                            // "save" changes ...
                            // remove from original all from currentSelected, re-insert ...
                            IEnumerable<RowMetricExecution> tmp1 = _executions.Where(
                                    r => !object.Equals(r.Item1, pastSelected.Item2));

                            tmp1 = tmp1.Concat(datasource);

                            _executions = new BindingList<RowMetricExecution>(tmp1.ToList());
                        }
                        else
                        {
                            // remove from original all from currentSelected
                            IEnumerable<RowMetricExecution> tmp1 = _executions.Where(
                                    r => !object.Equals(r.Item1, pastSelected.Item2));

                            _executions = new BindingList<RowMetricExecution>(tmp1.ToList());
                        }
                    }

                    // filter
                    if (null != currentSelected)
                    {
                        Metric.MetricDelegate method = (Metric.MetricDelegate)currentSelected.Item2;

                        IEnumerable<RowMetricExecution> tmp2 = _executions.Where(r => object.Equals(r.Item1, method));
                        gridExecution.DataSource = new BindingList<RowMetricExecution>(tmp2.ToList());
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

        private void MeasuresModeForm_Load(object sender, EventArgs e)
        {
            try
            {
                Metric metric = Factory.GetMetricsFromFilter(_filter.FilterType);
                SortedDictionary<string, Metric.MetricDelegate> lsMetrics = metric.GetListMetrics();

                gridMetrics.AutoGenerateColumns = false;
                gridExecution.AutoGenerateColumns = false;

                _metrics = new BindingList<RowMetric>();
                _executions = new BindingList<RowMetricExecution>();

                foreach (string key in lsMetrics.Keys)
                {
                    _metrics.Add(new RowMetric(key, lsMetrics[key]));
                }

                if (null != _originalMetrics)
                {
                    foreach (MetricExecBase m in _originalMetrics.OfType<MetricExec>())
                    {
                        _executions.Add(new RowMetricExecution(m.Method, Framework.Batch.MetricExecutionType.InputOutput, null));
                    }

                    foreach (MetricExecReference m in _originalMetrics.OfType<MetricExecReference>())
                    {
                        _executions.Add(new RowMetricExecution(m.Method, Framework.Batch.MetricExecutionType.RefOutput, m.Input));
                    }
                }

                gridExecution.DataSource = null;
                gridMetrics.DataSource = _metrics;
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

        private void gridMetrics_SelectionChanged(object sender, EventArgs e)
        {
            try
            {

                SaveCurrent();

                DataGridViewSelectedRowCollection rows = gridMetrics.SelectedRows;
                _selected = (RowMetric)(rows.Count == 0 ? null : rows[0].DataBoundItem);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        private void gridExecution_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex < 0 || e.RowIndex < 0)
                    return;

                switch (e.ColumnIndex)
                {
                    case 0:
                        IEnumerable<MetricExecutionType> values = (IEnumerable<MetricExecutionType>)Enum.GetValues(typeof(MetricExecutionType));

                        SelectionForm<string> sel = new SelectionForm<string>(
                                 values.Select(s => s.ToString()).ToArray(),
                                 false);

                        sel.ShowDialog(this);
                        if (sel.DialogResult == System.Windows.Forms.DialogResult.OK)
                        {
                            RowMetricExecution r = (RowMetricExecution)gridExecution.Rows[e.RowIndex].DataBoundItem;

                            BindingList<RowMetricExecution> datasource = (BindingList<RowMetricExecution>)gridExecution.DataSource;

                            datasource[e.RowIndex] = new RowMetricExecution(
                                    r.Item1,
                                    (MetricExecutionType)Enum.Parse(typeof(MetricExecutionType), sel.Selection[0]),
                                    r.Item3);

                        }
                        sel.Dispose();

                        break;
                    case 1:
                        RowMetricExecution r2 = (RowMetricExecution)gridExecution.Rows[e.RowIndex].DataBoundItem;

                        if (r2.Item2 != MetricExecutionType.RefOutput)
                        {
                            MessageBox.Show("Set to ExecutionType.InputOutput first!!!");
                            return;
                        }

                        SelectionForm<Image> sel2 = new SelectionForm<Image>(_references, false);
                        sel2.ShowDialog(this);
                        if (sel2.DialogResult == System.Windows.Forms.DialogResult.OK)
                        {
                            BindingList<RowMetricExecution> datasource = (BindingList<RowMetricExecution>)gridExecution.DataSource;

                            datasource[e.RowIndex] = new RowMetricExecution(r2.Item1, r2.Item2, new WeakImage(sel2.Selection[0]));
                        }
                        sel2.Dispose();

                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        private void btAdd_Click(object sender, EventArgs e)
        {
            try
            {
                DataGridViewSelectedRowCollection rows = gridMetrics.SelectedRows;

                // update filter ...
                if (null == rows || rows.Count < 1)
                {
                    return;
                }
                else
                {
                    RowMetric selected = (RowMetric)rows[0].DataBoundItem;
                    BindingList<RowMetricExecution> datasource = (BindingList<RowMetricExecution>)gridExecution.DataSource;
                    datasource.Add(new RowMetricExecution(selected.Item2, MetricExecutionType.InputOutput, null));
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
