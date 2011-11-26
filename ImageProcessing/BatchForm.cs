using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Framework.Core.Filters.Base;
using Framework.Batch;
using Framework.Core.Metrics;
using Framework.Core;

using RowBatchAlgorithm = System.Tuple<
        string,
        System.Collections.Generic.SortedDictionary<string, object>,
        Framework.Core.Filters.Base.Filter,
        System.Collections.Generic.List<Framework.Batch.MetricExecBase>>;
using RowImage = System.Tuple<System.Drawing.Image>;
using RowResults = System.Tuple<
        string,
        System.Drawing.Image,
        System.Drawing.Image,
        System.Collections.Generic.List<Framework.Core.Metrics.MetricResult>,
        Framework.Core.Filters.Base.Filter,
        System.Collections.Generic.SortedDictionary<string, object>>;
using FilterMeasuredDelegate = Framework.Batch.BatchFilter.FilterMeasuredDelegate;
using FilterExecutedDelegate = Framework.Batch.BatchFilter.FilterExecutedDelegate;
using BatchFinishedDelegate = Framework.Batch.BatchFilter.BatchFinishedDelegate;
using System.Collections;

namespace PROJECTO
{

    public partial class BatchForm : Form
    {

        #region Attributes

        private BindingList<RowBatchAlgorithm> _algorithms;
        private BindingList<RowImage> _images;
        private BindingList<RowImage> _references;
        private BindingList<RowResults> _results;
        private BatchFilter bt;

        #endregion

        #region Constructors

        public BatchForm()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        public string DictionaryToString(SortedDictionary<string, object> dictionary)
        {
            if (dictionary == null)
                return "";

            StringBuilder buffer = new StringBuilder();
            buffer.Append("{");

            if (dictionary == null || dictionary.Count == 0)
                buffer.Append(" ");
            else
            {
                buffer.Append(string.Join(", ", dictionary.Select(e => string.Format("{0} = {1}", e.Key, e.Value)).ToArray()));
            }

            buffer.Append("}");
            return buffer.ToString();
        }

        private Image[] ReferencesToArray()
        {
            List<Image> ls_img = new List<Image>();

            foreach (RowImage r in _references)
            {
                ls_img.Add(r.Item1);
            }

            return ls_img.ToArray();
        }

        public DataTable RowResultsToDatatable(IList<RowResults> results)
        {
            DataTable dt = new DataTable("Details");

            dt.Columns.AddRange(new DataColumn[] { 
                    new DataColumn("Algorithm", typeof(string)),
                    new DataColumn("Configs", typeof(string)),
                    new DataColumn("ImageSrc", typeof(Image)),
                    new DataColumn("ImageDest", typeof(Image)),
                    new DataColumn("ImageRef", typeof(Image)),
                    new DataColumn("Measure", typeof(string)),
                    new DataColumn("Value", typeof(double))
                });

            //List<String> colsMeasures = new List<string>();
            //foreach (RowResults r in results)
            //{
            //    foreach (String key in r.Item6.Keys)
            //    {
            //        if (!colsMeasures.Contains(key))
            //            colsMeasures.Add(key);
            //    }
            //}
            //foreach (string key in colsMeasures)
            //{
            //    dt.Columns.Add(new DataColumn(key, typeof(double)));
            //}

            DataRow dr;

            foreach (RowResults r in results)
            {
                for (int i = 0; i < Math.Max(r.Item4.Count, 1); ++i)
                {
                    dr = dt.NewRow();

                    dr.SetField<string>("Algorithm", r.Item5.FilterType.FullName);
                    dr.SetField<string>("Configs", DictionaryToString(r.Item6));
                    dr.SetField<Image>("ImageSrc", r.Item2);
                    dr.SetField<Image>("ImageDest", r.Item3);

                    if (i < r.Item4.Count)
                    {
                        dr.SetField<Image>("ImageRef", r.Item4[i].Reference);
                        dr.SetField<string>("Measure", r.Item4[i].Key);
                        dr.SetField<double>("Value", r.Item4[i].Value);
                    }

                    dt.Rows.Add(dr);
                }
            }

            // Open new form ...

            return dt;
        }

        private DataTable RowResultsToDatatable2(IList<RowResults> results)
        {
            DataTable dt = new DataTable("Details");

            dt.Columns.AddRange(new DataColumn[] { 
                    new DataColumn("Algorithm", typeof(string)),
                    new DataColumn("Configs", typeof(string)),
                    new DataColumn("ImageSrc", typeof(Image)),
                    new DataColumn("ImageDest", typeof(Image))
                });

            List<String> colsMeasures = new List<string>(); string k;

            foreach (RowResults r in results)
            {
                foreach (MetricResult mr in r.Item4)
                {
                    k = mr.Key;
                    if (!colsMeasures.Contains(k))
                        colsMeasures.Add(k);
                }
            }

            foreach (string key in colsMeasures)
            {
                dt.Columns.Add(new DataColumn(key, typeof(double)));
            }

            DataRow dr;

            foreach (RowResults r in results)
            {
                dr = dt.NewRow();

                dr.SetField<string>("Algorithm", r.Item5.FilterType.FullName);
                dr.SetField<string>("Configs", DictionaryToString(r.Item6));
                dr.SetField<Image>("ImageSrc", r.Item2);
                dr.SetField<Image>("ImageDest", r.Item3);

                for (int i = 0; i < r.Item4.Count; ++i)
                {
                    if (dt.Columns.Contains(r.Item4[i].Key))
                    {
                        dr.SetField<double>(r.Item4[i].Key, r.Item4[i].Value);
                    }
                }

                dt.Rows.Add(dr);
            }

            // Open new form ...

            return dt;
        }

        private void Swap_Rows(DataGridViewRow row0, DataGridViewRow row1)
        {
            throw new NotImplementedException();

            if (null != row0)
            {
                if (null != row1)
                {
                    RowBatchAlgorithm value_row0 = _algorithms[row0.Index];
                    RowBatchAlgorithm value_row1 = _algorithms[row1.Index];

                    string tmp_item1 = value_row1.Item1;
                    SortedDictionary<string, object> tmp_item2 = value_row1.Item2;
                    BatchFilterFactory.NodeFilterInputType tmp_item3 = value_row1.Item3;
                    Filter tmp_item4 = value_row1.Item4;
                    List<MetricExecBase> tmp_item5 = value_row1.Item5;

                    value_row0 = new RowBatchAlgorithm(tmp_item1, tmp_item2, tmp_item3, tmp_item4, tmp_item5);
                    value_row1 = new RowBatchAlgorithm(value_row0.Item1, value_row0.Item2, value_row0.Item3, value_row0.Item4, value_row0.Item5);

                }
            }
        }

        private void OpenHashConfig_CurrentRow()
        {
            if (null != GridAlgorithms.CurrentRow)
            {
                RowBatchAlgorithm row = (RowBatchAlgorithm)GridAlgorithms.CurrentRow.DataBoundItem;

                if (null != row)
                {
                    SortedDictionary<string, object> hash = row.Item2;
                    ConfigurationsForm conf = new ConfigurationsForm(ref hash);
                    conf.ShowDialog(this);
                }
            }
        }

        private void OpenAlgorithm_CurrentRow()
        {
            if (null != GridAlgorithms.SelectedRows && GridAlgorithms.SelectedRows.Count > 0)
            {
                AlgorithmsForm frm = new AlgorithmsForm(false);
                frm.ShowDialog(this);
                if (frm.DialogResult == DialogResult.OK)
                {
                    Filter[] selection = frm.Selection;

                    if (null != selection)
                    {
                        BindingList<RowBatchAlgorithm> datasource = (BindingList<RowBatchAlgorithm>)GridAlgorithms.DataSource;
                        RowBatchAlgorithm binded;

                        foreach (DataGridViewRow r in GridAlgorithms.SelectedRows)
                        {
                            binded = (RowBatchAlgorithm)r.DataBoundItem;
                            datasource[r.Index] = new RowBatchAlgorithm(
                                selection[0].Attributes["ShortName"].ToString(),
                                binded.Item2, binded.Item3, selection[0], binded.Item5);
                        }
                    }
                }
            }
        }

        private void OpenMeasurement_CurrentRow()
        {
            if (null != GridAlgorithms.CurrentRow)
            {
                RowBatchAlgorithm row = (RowBatchAlgorithm)GridAlgorithms.CurrentRow.DataBoundItem;
                if (null != row)
                {

                    MeasuresEditForm sel = new MeasuresEditForm(row.Item4, ReferencesToArray(), row.Item5);
                    sel.ShowDialog(this);
                    if (sel.DialogResult == System.Windows.Forms.DialogResult.OK)
                    {
                        List<MetricExecBase> ex = sel.Execs;

                        BindingList<RowBatchAlgorithm> datasource = (BindingList<RowBatchAlgorithm>)GridAlgorithms.DataSource;

                        datasource[GridAlgorithms.CurrentRow.Index] = new RowBatchAlgorithm(
                                row.Item1, row.Item2, row.Item3, row.Item4, ex);

                    }
                    sel.Dispose();

                }
            }
        }

        private void OpenSelectionInputType_CurrentRow()
        {
            if (null != GridAlgorithms.CurrentRow)
            {
                RowBatchAlgorithm row = (RowBatchAlgorithm)GridAlgorithms.CurrentRow.DataBoundItem;
                if (null != row)
                {
                    BatchFilterFactory.NodeFilterInputType value = row.Item3;

                    IEnumerable<BatchFilterFactory.NodeFilterInputType> values =
                            (IEnumerable<BatchFilterFactory.NodeFilterInputType>)
                            Enum.GetValues(typeof(BatchFilterFactory.NodeFilterInputType));

                    SelectionForm<string> sel = new SelectionForm<string>(
                            values.Select(s => s.ToString()).ToArray(),
                            false);

                    sel.ShowDialog(this);
                    if (sel.DialogResult == System.Windows.Forms.DialogResult.OK)
                    {
                        if (null != sel.Selection && sel.Selection.Length > 0)
                        {
                            value = (BatchFilterFactory.NodeFilterInputType)Enum.Parse(typeof(BatchFilterFactory.NodeFilterInputType), sel.Selection[0]);

                            BindingList<RowBatchAlgorithm> datasource = (BindingList<RowBatchAlgorithm>)GridAlgorithms.DataSource;

                            datasource[GridAlgorithms.CurrentRow.Index] = new RowBatchAlgorithm(
                                    row.Item1, row.Item2, value, row.Item4, row.Item5);

                        }
                    }
                    sel.Dispose();
                }
            }
        }

        public void LoadFile(string fname)
        {
            DataSet ds = new DataSet("DS");
            ds.ReadXml(fname);

            DataTable dtAlgorithms = ds.Tables["Algorithms"];
            DataTable dtImages = ds.Tables["Images"];
            DataTable dtReferences = ds.Tables["References"];

            if (null != dtAlgorithms)
            {
                foreach (DataRow r in dtAlgorithms.Rows)
                {
                    SortedDictionary<string, object> configs = new SortedDictionary<string, object>();
                    foreach (DataRow m in r.Field<DataTable>("Configs").Rows)
                    {
                        configs.Add(m.Field<string>("Key"), m.Field<object>("Value"));
                    }

                    List<MetricExecBase> measures = new List<MetricExecBase>();
                    foreach (DataRow m in r.Field<DataTable>("Measures").Rows)
                    {
                        Metric.MetricDelegate mm;
                        Type tm = Factory.GetType(m.Field<string>("TypeMethod"));

                        mm = (Metric.MetricDelegate)Delegate.CreateDelegate(
                                typeof(Metric.MetricDelegate),
                                tm.GetMethod(m.Field<string>("Method")));
                        //--

                        switch (m.Field<MeasuresEditForm.ExecutionType>("TypeMetric"))
                        {
                            case MeasuresEditForm.ExecutionType.InputOutput:
                                measures.Add(new MetricExec(
                                        m.Field<string>("Key"),
                                        mm));
                                break;
                            case MeasuresEditForm.ExecutionType.RefOutput:
                                measures.Add(new MetricExecReference(
                                        m.Field<string>("Key"),
                                        mm,
                                        Facilities.FromBase64(m.Field<string>("Input"))));
                                break;
                        }
                    }

                    Type t = Factory.GetType(r.Field<string>("FilterFullName"));

                    _algorithms.Add(new RowBatchAlgorithm(
                            t.Name, configs,
                            r.Field<BatchFilterFactory.NodeFilterInputType>("Input"),
                            Factory.GetFilter_For_Type(t),
                            measures));

                }
            }

            if (null != dtImages)
            {
                foreach (DataRow r in dtImages.Rows)
                {
                    _images.Add(new RowImage(Facilities.FromBase64(r.Field<string>("Image"))));
                }
            }
            if (null != dtReferences)
            {
                foreach (DataRow r in dtReferences.Rows)
                {
                    _references.Add(new RowImage(Facilities.FromBase64(r.Field<string>("Image"))));
                }
            }

        }

        public void SaveFile(string fname)
        {
            DataSet ds = new DataSet("DS");
            DataRow dr, dr2;

            // Algorithms
            DataTable dtAlgorithms = new DataTable("Algorithms");
            DataTable dtMeasures = new DataTable("Measures");
            DataTable dtConfigs = new DataTable("Configs");

            dtConfigs.Columns.AddRange(new DataColumn[] { 
                    new DataColumn("Key", typeof(string)),
                    new DataColumn("Value", typeof(object))
                });

            dtMeasures.Columns.AddRange(new DataColumn[] { 
                    new DataColumn("Key", typeof(string)),
                    new DataColumn("Input", typeof(string)),
                    new DataColumn("Output", typeof(string)),
                    new DataColumn("TypeMethod", typeof(string)),
                    new DataColumn("TypeMetric", typeof(MeasuresEditForm.ExecutionType)),
                    new DataColumn("Method", typeof(string))
                });

            dtAlgorithms.Columns.AddRange(new DataColumn[] { 
                    new DataColumn("FilterFullName", typeof(string)),
                    new DataColumn("Configs", typeof(DataTable)),
                    new DataColumn("Input", typeof(BatchFilterFactory.NodeFilterInputType)), 
                    new DataColumn("Measures", typeof(DataTable))
                });

            if (null != _algorithms)
            {
                foreach (RowBatchAlgorithm alg in _algorithms)
                {
                    dr = dtAlgorithms.NewRow();

                    // Configs to Persistable
                    DataTable configs = dtConfigs.Clone();

                    if (null != alg.Item2)
                    {
                        foreach (KeyValuePair<string, object> v in alg.Item2)
                        {
                            dr2 = configs.NewRow();

                            dr2.SetField<string>("Key", v.Key);
                            dr2.SetField<object>("Value", v.Value);

                            configs.Rows.Add(dr2);
                        }
                    }
                    // Measures to Persistable
                    DataTable measures = dtMeasures.Clone();
                    if (null != alg.Item5)
                    {
                        foreach (MetricExecBase m in alg.Item5)
                        {
                            dr2 = measures.NewRow();
                            dr2.SetField<string>("Key", m.Key);
                            dr2.SetField<string>("Input", Facilities.ToBase64(m.Input));
                            dr2.SetField<string>("Output", Facilities.ToBase64(m.Output));

                            if (m.GetType() == typeof(MetricExec))
                            {
                                dr2.SetField<MeasuresEditForm.ExecutionType>("TypeMetric", MeasuresEditForm.ExecutionType.InputOutput);
                            }
                            else if (m.GetType() == typeof(MetricExecReference))
                            {
                                dr2.SetField<MeasuresEditForm.ExecutionType>("TypeMetric", MeasuresEditForm.ExecutionType.RefOutput);
                            }

                            dr2.SetField<string>("TypeMethod", m.Method.Method.DeclaringType.FullName);
                            dr2.SetField<string>("Method", m.Method.Method.Name);
                            measures.Rows.Add(dr2);
                        }
                    }

                    dr.SetField<string>("FilterFullName", alg.Item4.FilterType.FullName);
                    dr.SetField<DataTable>("Configs", configs);
                    dr.SetField<BatchFilterFactory.NodeFilterInputType>("Input", alg.Item3);
                    dr.SetField<DataTable>("Measures", measures);

                    dtAlgorithms.Rows.Add(dr);
                }
            }

            ds.Tables.Add(dtAlgorithms);

            // References
            DataTable dtReferences = new DataTable("References");
            dtReferences.Columns.AddRange(new DataColumn[] { new DataColumn("Image", typeof(string)) });
            if (null != _references)
            {
                foreach (RowImage r in _references)
                {
                    dr = dtReferences.NewRow();

                    dr.SetField<string>("Image", Facilities.ToBase64(r.Item1));

                    dtReferences.Rows.Add(dr);
                }
            }
            ds.Tables.Add(dtReferences);

            // Images
            DataTable dtImages = new DataTable("Images");
            dtImages.Columns.AddRange(new DataColumn[] { new DataColumn("Image", typeof(string)) });
            if (null != _images)
            {
                foreach (RowImage r in _images)
                {
                    dr = dtImages.NewRow();

                    dr.SetField<string>("Image", Facilities.ToBase64(r.Item1));

                    dtImages.Rows.Add(dr);
                }
            }
            ds.Tables.Add(dtImages);

            // Save
            ds.WriteXml(fname, XmlWriteMode.WriteSchema);
        }

        #endregion

        #region Events

        #region ToolStrip Base

        private void MnuNew_Click(object sender, EventArgs e)
        {
            // New, Clean
            BatchForm frm = new BatchForm();
            frm.MdiParent = MdiParent;
            frm.Show();
        }

        private void MnuOpen_Click(object sender, EventArgs e)
        {
            // Load from XML
            OpenDialog.ShowDialog(this);
        }

        private void MnuSave_Click(object sender, EventArgs e)
        {
            // Save to XML (all serialized)
            saveFileDialog1.ShowDialog(this);
        }

        private void MnuExport_Click(object sender, EventArgs e)
        {
            // Export to powershell script ...

        }

        private void btClone_Click(object sender, EventArgs e)
        {

        }

        private void SaveDialog_FileOk(object sender, CancelEventArgs e)
        {
            DataTable dt = RowResultsToDatatable(_results);
            dt.WriteXml(SaveResultsDialog.FileName, true);
        }

        private void OpenDialog_FileOk(object sender, CancelEventArgs e)
        {
            LoadFile(OpenDialog.FileName);
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            SaveFile(saveFileDialog1.FileName);
        }

        #endregion

        #region bt Handlers

        private void bt_FilterExecuted(BatchFilter sender, Image input, Image output, Filter filter, SortedDictionary<string, object> configs)
        {
            FilterExecutedDelegate inv = new FilterExecutedDelegate(bt_FilterExecuted_threadsafe_);
            this.Invoke(inv, sender, input, output, filter, configs);
        }

        private void bt_FilterExecuted_threadsafe_(BatchFilter sender, Image input, Image output, Filter filter, SortedDictionary<string, object> configs)
        {
            // Add row to GridResults ...
            _results.Add(new RowResults(filter.Attributes["ShortName"].ToString(), input, output, null, filter, configs));

            GridResults.Refresh();
        }

        private void bt_FilterMeasured(BatchFilter sender, Image input, Image output, Filter filter, SortedDictionary<string, object> configs, List<MetricResult> measures)
        {
            FilterMeasuredDelegate inv = new FilterMeasuredDelegate(bt_FilterMeasured_threadsafe_);
            this.Invoke(inv, sender, input, output, filter, configs, measures);
        }

        private void bt_FilterMeasured_threadsafe_(BatchFilter sender, Image input, Image output, Filter filter, SortedDictionary<string, object> configs, List<MetricResult> measures)
        {
            // Add row to GridResults ...
            _results.Add(new RowResults(filter.Attributes["ShortName"].ToString(), input, output, measures, filter, configs));

            GridResults.Refresh();
        }

        private void bt_BatchFinished(BatchFilter sender)
        {
            BatchFinishedDelegate inv = new BatchFinishedDelegate(bt_BatchFinished_threadsafe_);
            this.Invoke(inv, sender);
        }

        private void bt_BatchFinished_threadsafe_(BatchFilter sender)
        {
            MessageBox.Show(this, "Finished!");
        }

        #endregion

        #region ToolStrip bt

        private void MnuStart_Click(object sender, EventArgs e)
        {
            // Start batch
            //bt = new BatchFilter(_images.ToList(), _algorithms.ToList());

            bt.FilterMeasured += new FilterMeasuredDelegate(bt_FilterMeasured);
            //bt.FilterExecuted += new FilterExecutedDelegate(bt_FilterExecuted);
            bt.BatchFinished += new BatchFinishedDelegate(bt_BatchFinished);
            bt.Start();
        }

        private void MnuPause_Click(object sender, EventArgs e)
        {
            if (null != bt)
                // Pause batch
                bt.Pause();
        }

        private void MnuStop_Click(object sender, EventArgs e)
        {
            if (null != bt)
                // Stop batch
                bt.Stop();
        }

        #endregion

        #region Form

        private void BatchForm_Load(object sender, EventArgs e)
        {
            GridAlgorithms.AutoGenerateColumns = false;
            GridImages.AutoGenerateColumns = false;
            GridReferences.AutoGenerateColumns = false;
            GridResults.AutoGenerateColumns = false;

            _algorithms = new BindingList<RowBatchAlgorithm>();
            _images = new BindingList<RowImage>();
            _results = new BindingList<RowResults>();
            _references = new BindingList<RowImage>();

            //Array values = Enum.GetValues(typeof(BatchFilterFactory.NodeFilterInputType));

            GridAlgorithms.DataSource = _algorithms;
            GridReferences.DataSource = _references;
            GridImages.DataSource = _images;
            GridResults.DataSource = _results;
        }

        #endregion

        #region Algorithm

        private void btAddAlgorithms_Click(object sender, EventArgs e)
        {
            AlgorithmsForm frm = new AlgorithmsForm(true);
            frm.ShowDialog(this);
            if (frm.DialogResult == DialogResult.OK)
            {
                Filter[] selection = frm.Selection;

                if (null != selection)
                {
                    foreach (Filter f in selection)
                    {
                        _algorithms.Add(new RowBatchAlgorithm(
                                f.Attributes["ShortName"].ToString(),
                                ((FilterCore)Activator.CreateInstance(f.FilterType)).GetDefaultConfigs(),
                                BatchFilterFactory.NodeFilterInputType.OriginalImages,
                                f,
                                null));
                    }
                }
            }
            GridAlgorithms.Refresh();
            frm.Dispose();
        }

        private void btConfigureHash_Click(object sender, EventArgs e)
        {
            OpenHashConfig_CurrentRow();
        }

        private void btMoveBack_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = GridAlgorithms.CurrentRow;
            if (null != row && row.Index > 0)
            {
                // Swap row with row - 1
                Swap_Rows(GridAlgorithms.Rows[row.Index - 1], row);

                GridAlgorithms.Refresh();
            }
        }

        private void btMoveFront_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = GridAlgorithms.CurrentRow;
            if (null != row && row.Index < GridAlgorithms.RowCount - 1)
            {
                // Swap row with row + 1
                Swap_Rows(row, GridAlgorithms.Rows[row.Index + 1]);

                GridAlgorithms.Refresh();
            }
        }

        private void GridAlgorithms_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int c_index = Configs.Index;
            switch (e.ColumnIndex)
            {
                case 0: // Algorithms
                    OpenAlgorithm_CurrentRow();

                    break;
                case 1: // Configs
                    OpenHashConfig_CurrentRow();

                    break;
                case 2: //  InputType
                    OpenSelectionInputType_CurrentRow();

                    break;
                case 3:
                    OpenMeasurement_CurrentRow();

                    break;
                default:
                    break;
            }

        }

        private void btEliminar_Click(object sender, EventArgs e)
        {

        }

        private void btReplicate_Click(object sender, EventArgs e)
        {
            if (null != GridAlgorithms.CurrentRow)
            {
                ReplicateForm repl = new ReplicateForm();
                repl.ShowDialog(this);
                if (repl.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    decimal r = repl.Selection;
                    RowBatchAlgorithm template = (RowBatchAlgorithm)GridAlgorithms.CurrentRow.DataBoundItem;
                    int i = GridAlgorithms.CurrentRow.Index + 1;
                    SortedDictionary<string, object> sorted_dict_copy;

                    while (r-- > 0)
                    {
                        sorted_dict_copy = new SortedDictionary<string, object>(template.Item2);

                        _algorithms.Insert(i, new RowBatchAlgorithm(
                                template.Item1,
                                sorted_dict_copy,
                                template.Item3,
                                template.Item4,
                                template.Item5));
                    }
                }
                repl.Dispose();
            }
        }

        #endregion

        #region References

        private void SelectReferences_FileOk(object sender, CancelEventArgs e)
        {
            BindingList<RowImage> datasource = (BindingList<RowImage>)GridReferences.DataSource;

            // Insert images' filename; 
            // TODO: do not repeat files!
            foreach (string fname in SelectReferences.FileNames)
            {
                if (null == datasource.FirstOrDefault(r => 1 == 0))
                {
                    datasource.Add(new RowImage(Image.FromFile(fname)));
                }
            }
            GridReferences.Refresh();
        }

        private void btFromWindowRefs_Click(object sender, EventArgs e)
        {

        }

        private void btOpenFileRefs_Click(object sender, EventArgs e)
        {
            // Select image file ...
            SelectReferences.ShowDialog(this);
        }

        private void GridReferences_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            RowImage tuple = (RowImage)GridReferences.Rows[e.RowIndex].DataBoundItem;
            ImageForm frm;

            if (GridReferences.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.GetType().Equals(typeof(Image)) ||
                GridReferences.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.GetType().Equals(typeof(Bitmap))
                )
            {
                frm = new ImageForm((Image)GridReferences.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                frm.MdiParent = MdiParent;
                frm.Show();
            }
        }

        #endregion

        #region Images

        private void btOpenFile_Click(object sender, EventArgs e)
        {
            // Select image file ...
            SelectImages.ShowDialog(this);
        }

        private void SelectImages_FileOk(object sender, CancelEventArgs e)
        {
            BindingList<RowImage> datasource = (BindingList<RowImage>)GridImages.DataSource;
            // Insert images' filename; 
            // TODO: do not repeat files!
            foreach (string fname in SelectImages.FileNames)
            {
                if (null == datasource.FirstOrDefault(r => 1 == 0))
                {
                    datasource.Add(new RowImage(Image.FromFile(fname)));
                }
            }
            GridImages.Refresh();
        }

        private void GridImages_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            RowImage tuple = (RowImage)GridImages.Rows[e.RowIndex].DataBoundItem;
            ImageForm frm;

            if (GridImages.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.GetType().Equals(typeof(Image)) ||
                GridImages.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.GetType().Equals(typeof(Bitmap))
                )
            {
                frm = new ImageForm((Image)GridImages.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                frm.MdiParent = MdiParent;
                frm.Show();
            }
        }

        private void btFromProgram_Click(object sender, EventArgs e)
        {
            // Select from open window ...

        }

        #endregion

        #region Results

        private void GridResults_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            RowResults tuple = (RowResults)GridResults.Rows[e.RowIndex].DataBoundItem;
            ImageForm frm;

            if (GridResults.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.GetType().Equals(typeof(Image)) ||
                GridResults.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.GetType().Equals(typeof(Bitmap))
                )
            {
                frm = new ImageForm((Image)GridResults.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                frm.MdiParent = MdiParent;
                frm.Show();
            }
            else
            {
                switch (e.ColumnIndex)
                {
                    case 3:

                        MeasuresViewerForm m = new MeasuresViewerForm(tuple.Item4);
                        m.MdiParent = MdiParent;
                        m.Show();

                        break;
                }
            }
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            SaveResultsDialog.ShowDialog(this);
        }

        private void btDetailed_Click(object sender, EventArgs e)
        {
            DataTable dt = RowResultsToDatatable(_results);

            DataTableBindForm frm = new DataTableBindForm(dt);
            frm.MdiParent = MdiParent;
            frm.Show();
        }

        private void btDetailed2_Click(object sender, EventArgs e)
        {
            DataTable dt = RowResultsToDatatable2(_results);

            DataTableBindForm frm = new DataTableBindForm(dt);
            frm.MdiParent = MdiParent;
            frm.Show();
            
        }

        #endregion

        #endregion

    }
}
