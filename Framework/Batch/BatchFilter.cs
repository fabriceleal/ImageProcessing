using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Framework.Core;
using Framework.Core.Filters.Base;
using Framework.Core.Filters.Frequency;
using Framework.Core.Filters.Spatial;
using Framework.Transforms;
using System.Threading;
using System.Windows.Forms;
using Framework.Core.Metrics;
using System.Data;

namespace Framework.Batch
{
    /// <summary>
    /// A batch of filters, structured as a graph.
    /// </summary>
    /// <remarks>
    /// Provides functionality for executing and collecting statistics from filters assynchronously.
    /// </remarks>
    public class BatchFilter
    {

        #region Attributes

        /// <summary>
        /// Input to the filters.
        /// </summary>
        private List<WeakImage> _inputImages;

        /// <summary>
        /// Graph of filters to execute.
        /// </summary>
        private Graph _graph;

        /// <summary>
        /// Represents the method that handles a FilterExecuted event.
        /// </summary>
        /// <param name="sender">The BatchFilter sender of the event.</param>
        /// <param name="input">The input of the filter.</param>
        /// <param name="output">The output of the filter.</param>
        /// <param name="filter">The filter executed.</param>
        /// <param name="configs">The configurations used by the filter.</param>
        /// <param name="duration">The configurations used by the filter.</param>
        public delegate void FilterExecutedDelegate(BatchFilter sender, WeakImage input, WeakImage output, Filter filter, SortedDictionary<string, object> configs, TimeSpan duration);

        /// <summary>
        /// Occurs after a filter executes.
        /// </summary>
        public event FilterExecutedDelegate FilterExecuted;

        /// <summary>
        /// Represents the method that handles a FilterMeasured event.
        /// </summary>
        /// <param name="sender">The BatchFilter sender of the event.</param>
        /// <param name="input">The input of the filter.</param>
        /// <param name="output">The output of the filter.</param>
        /// <param name="filter">The filter executed.</param>
        /// <param name="configs">The configurations used by the filter.</param>
        /// <param name="measures">The measures of the filter executed.</param>
        /// <param name="duration">The configurations used by the filter.</param>
        public delegate void FilterMeasuredDelegate(BatchFilter sender, WeakImage input, WeakImage output, Filter filter, SortedDictionary<string, object> configs, List<MetricResult> measures, TimeSpan duration);

        /// <summary>
        /// Occurs after a filter is measured.
        /// </summary>
        public event FilterMeasuredDelegate FilterMeasured;

        /// <summary>
        /// Represents the method that handles a BatchFinished event.
        /// </summary>
        /// <param name="sender">The BatchFilter sender of the event.</param>
        public delegate void BatchFinishedDelegate(BatchFilter sender);

        /// <summary>
        /// Occurs after the BatchFilter finishes execution.
        /// </summary>
        public event BatchFinishedDelegate BatchFinished;

        #endregion

        #region Constructors

        /// <summary>
        /// Clean BatchFilter.
        /// </summary>
        public BatchFilter() { }

        ///// <summary>
        ///// BatchFilter from XML file.
        ///// </summary>
        ///// <param name="_filename"></param>
        //public BatchFilter(String _filename)
        //    : this()
        //{
        //    Load(_filename);
        //}

        /// <summary>
        /// BatchFilter from managed structures.
        /// </summary>
        /// <param name="relations">The parent-child relations between the existing nodes.</param>
        /// <param name="images">The inputs for the BatchFilter.</param>
        /// <param name="filters">The filters to execute.</param>
        public BatchFilter(
                List<RowNodeRelation> relations, List<RowImage> images, List<RowBatchFilter> filters)
        {
            _inputImages = new List<WeakImage>();

            foreach (RowImage r in images)
            {
                _inputImages.Add(r.Item1);
            }

            _graph = new Graph(this, relations, filters);
            _graph.FilterExecuted += new Graph.FilterExecutedDelegate(_graph_FilterExecuted);
            _graph.FilterMeasured += new Graph.FilterMeasuredDelegate(_graph_FilterMeasured);
        }

        #endregion

        #region Properties

        public int CountFilters
        {
            get
            {
                if (_graph != null)
                {
                    return _graph.CountFilters * _inputImages.Count;
                }
                return 0;
            }
        }

        #endregion

        #region Execution Management

        /// <summary>
        /// The private thread in which the batch will be executed.
        /// </summary>
        private Thread _t;

        /// <summary>
        /// Starts execution.
        /// </summary>
        /// <remarks>
        /// Execution of the batch occurs in a separate thread, in background.
        /// </remarks>
        /// <returns>true in case of sucess; false if there is some exception.</returns>
        public void Start()
        {
            try
            {
                ThreadStart st = new ThreadStart(BatchExection_Start);
                if (_t != null)
                {
                    Stop();
                }

                _t = new Thread(st);

                _t.IsBackground = true;

                _t.Start();
            }
            catch (Exception e)
            {
                throw new Exception("Error when trying to start BatchFilter.", e);
            }
        }

        /// <summary>
        /// Execution of the batch. Called assynchronously by Start().
        /// </summary>
        private void BatchExection_Start()
        {
            Execute();

            OnBatchFinished(this);
        }

        /// <summary>
        /// Pauses execution.
        /// </summary>
        /// <returns>true in case of sucess; false if there is some exception.</returns>
        public void Pause()
        {
            try
            {
                if (null != _t)
                {
                    _t.Suspend();
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error when trying to pause.", e);
            }
        }

        /// <summary>
        /// Forces the abortion of the execution. Does not raises the BatchFinished event.
        /// </summary>
        /// <returns>true in case of sucess; false if there is some exception.</returns>
        public void Stop()
        {
            try
            {
                if (null != _t)
                {
                    _t.Abort();
                    _t = null;
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error when trying to abort.", e);
            }
        }

        #endregion

        #region Persist

        /// <summary>
        /// Loads the content of a xml file into lists of nodes, filters, images and references.
        /// </summary>
        /// <remarks>
        /// The out parameters will be filled with the contents of the xml file.
        /// </remarks>
        /// <param name="fname">The file name of the xml to load.</param>
        /// <param name="nodes">The list of nodes.</param>
        /// <param name="filters">The list of filters.</param>
        /// <param name="images">The list of images</param>
        /// <param name="references">The list of references.</param>
        public static void Load(string fname,
                out List<RowNode> nodes,
                out List<RowBatchFilter> filters,
                out List<RowImage> images,
                out List<RowImage> references)
        {
            nodes = new List<RowNode>(); filters = new List<RowBatchFilter>();
            images = new List<RowImage>(); references = new List<RowImage>();

            try
            {

                // --

                DataSet ds = new DataSet("DS");
                ds.ReadXml(fname);

                DataTable dtNodes = ds.Tables["Nodes"]; DataTable dtAlgorithms = ds.Tables["Algorithms"];
                DataTable dtImages = ds.Tables["Images"]; DataTable dtReferences = ds.Tables["References"];

                if (null != dtNodes)
                {
                    foreach (DataRow r in dtNodes.Rows)
                    {
                        Guid id, pai; string texto;

                        id = r.Field<Guid>("Id");
                        texto = r.Field<string>("Texto");
                        pai = r.Field<Guid>("Pai");

                        nodes.Add(new RowNode(id, texto, pai));
                    }

                }

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

                            switch (m.Field<MetricExecutionType>("TypeMetric"))
                            {
                                case MetricExecutionType.InputOutput:
                                    measures.Add(new MetricExec(
                                            m.Field<string>("Key"),
                                            mm));
                                    break;
                                case MetricExecutionType.RefOutput:
                                    measures.Add(new MetricExecReference(
                                            m.Field<string>("Key"),
                                            mm,
                                            new WeakImage(Facilities.ToImage(m.Field<string>("Input")))
                                            ));
                                    break;
                            }
                        }

                        Type t = Factory.GetType(r.Field<string>("FilterFullName"));
                        Guid g = r.Field<Guid>("Node");

                        filters.Add(new RowBatchFilter(
                                t.Name, configs,
                                Factory.GetFilterFromType(t),
                                measures, g));

                    }
                }

                if (null != dtImages)
                {
                    foreach (DataRow r in dtImages.Rows)
                    {
                        images.Add(new RowImage(new WeakImage(Facilities.ToImage(r.Field<string>("Image")))));
                    }
                }

                if (null != dtReferences)
                {
                    foreach (DataRow r in dtReferences.Rows)
                    {
                        references.Add(new RowImage(new WeakImage(Facilities.ToImage(r.Field<string>("Image")))));
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error when trying to load.", ex);
            }
        }

        /// <summary>
        /// Saves the lists of managed objects that compose a batch filter into a xml file.
        /// </summary>
        /// <param name="fname">The file name of the xml.</param>
        /// <param name="nodes">The list of nodes.</param>
        /// <param name="filters">The list of filters.</param>
        /// <param name="images">The list of images.</param>
        /// <param name="references">The list of references.</param>
        public static void Save(
                string fname,
                List<RowNode> nodes,
                List<RowBatchFilter> filters,
                List<RowImage> images,
                List<RowImage> references)
        {
            try
            {
                // Save ...
                DataSet ds = new DataSet("DS");
                DataRow dr, dr2;

                // Algorithms
                DataTable dtNodes = new DataTable("Nodes");
                DataTable dtAlgorithms = new DataTable("Algorithms");
                DataTable dtMeasures = new DataTable("Measures");
                DataTable dtConfigs = new DataTable("Configs");

                dtNodes.Columns.AddRange(new DataColumn[]{
                    new DataColumn("Id", typeof(Guid)),
                    new DataColumn("Texto", typeof(string)),
                    new DataColumn("Pai", typeof(Guid))
                });

                dtConfigs.Columns.AddRange(new DataColumn[] { 
                    new DataColumn("Key", typeof(string)),
                    new DataColumn("Value", typeof(object))
                });

                dtMeasures.Columns.AddRange(new DataColumn[] { 
                    new DataColumn("Key", typeof(string)),
                    new DataColumn("Input", typeof(string)),
                    new DataColumn("Output", typeof(string)),
                    new DataColumn("TypeMethod", typeof(string)),
                    new DataColumn("TypeMetric", typeof(Framework.Batch.MetricExecutionType)),
                    new DataColumn("Method", typeof(string))
                });

                dtAlgorithms.Columns.AddRange(new DataColumn[] { 
                    new DataColumn("FilterFullName", typeof(string)),
                    new DataColumn("Configs", typeof(DataTable)),
                    new DataColumn("Measures", typeof(DataTable)),
                    new DataColumn("Node", typeof(Guid))
                });

                if (null != nodes)
                {
                    foreach (RowNode nde in nodes)
                    {
                        dr = dtNodes.NewRow();

                        dr.SetField<Guid>("Id", nde.Item1);
                        dr.SetField<string>("Texto", nde.Item2);
                        dr.SetField<Guid>("Pai", nde.Item3);

                        dtNodes.Rows.Add(dr);
                    }
                }
                ds.Tables.Add(dtNodes);

                if (null != filters)
                {
                    foreach (RowBatchFilter alg in filters)
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
                                if (v.Value as Range.Rangeable != null)
                                {
                                    dr2.SetField<object>("Value", ((Range.Rangeable)v.Value).Value);
                                }
                                else
                                {
                                    dr2.SetField<object>("Value", v.Value);
                                }
                                configs.Rows.Add(dr2);
                            }
                        }
                        // Measures to Persistable
                        DataTable measures = dtMeasures.Clone();
                        if (null != alg.Item4)
                        {
                            foreach (MetricExecBase m in alg.Item4)
                            {
                                if (null != m)
                                {
                                    dr2 = measures.NewRow();
                                    dr2.SetField<string>("Key", m.Key);

                                    if (m.Input != null)
                                        dr2.SetField<string>("Input", Facilities.ToBase64String((Image)m.Input));
                                    if (m.Output != null)
                                        dr2.SetField<string>("Output", Facilities.ToBase64String((Image)m.Output));

                                    if (m.GetType() == typeof(MetricExec))
                                    {
                                        dr2.SetField<Framework.Batch.MetricExecutionType>("TypeMetric", Framework.Batch.MetricExecutionType.InputOutput);
                                    }
                                    else if (m.GetType() == typeof(MetricExecReference))
                                    {
                                        dr2.SetField<Framework.Batch.MetricExecutionType>("TypeMetric", Framework.Batch.MetricExecutionType.RefOutput);
                                    }

                                    dr2.SetField<string>("TypeMethod", m.Method.Method.DeclaringType.FullName);
                                    dr2.SetField<string>("Method", m.Method.Method.Name);
                                    measures.Rows.Add(dr2);
                                }
                            }
                        }

                        dr.SetField<string>("FilterFullName", alg.Item3.FilterType.FullName);
                        dr.SetField<DataTable>("Configs", configs);
                        dr.SetField<Guid>("Node", alg.Item5);
                        dr.SetField<DataTable>("Measures", measures);

                        dtAlgorithms.Rows.Add(dr);
                    }
                }

                ds.Tables.Add(dtAlgorithms);

                // References
                DataTable dtReferences = new DataTable("References");
                dtReferences.Columns.AddRange(new DataColumn[] { new DataColumn("Image", typeof(string)) });
                if (null != references)
                {
                    foreach (RowImage r in references)
                    {
                        dr = dtReferences.NewRow();

                        dr.SetField<string>("Image", Facilities.ToBase64String((Image)r.Item1));

                        dtReferences.Rows.Add(dr);
                    }
                }
                ds.Tables.Add(dtReferences);

                // Images
                DataTable dtImages = new DataTable("Images");
                dtImages.Columns.AddRange(new DataColumn[] { new DataColumn("Image", typeof(string)) });
                if (null != images)
                {
                    foreach (RowImage r in images)
                    {
                        dr = dtImages.NewRow();

                        dr.SetField<string>("Image", Facilities.ToBase64String((Image)r.Item1));

                        dtImages.Rows.Add(dr);
                    }
                }
                ds.Tables.Add(dtImages);

                // Save
                ds.WriteXml(fname, XmlWriteMode.WriteSchema);
            }
            catch (Exception ex)
            {
                throw new Exception("Error when trying to save.", ex);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Executes the internal structure of nodes, using the input images associated with
        /// the instance of BatchFilter.
        /// </summary>
        /// <returns>The output images of the filters.</returns>
        public List<WeakImage> Execute()
        {
            return _graph.Execute(_inputImages);
        }

        /// <summary>
        /// Executes the internal graph.
        /// </summary>
        /// <param name="inputImages">The input images to the graph.</param>
        /// <returns>The output images of the filters.</returns>
        public List<WeakImage> Execute(List<WeakImage> inputImages)
        {
            return _graph.Execute(inputImages);
        }

        /// <summary>
        /// Executed when a filter in a graph finishes execution.
        /// </summary>
        /// <param name="input">The input image of the filter.</param>
        /// <param name="output">The output image of the filter.</param>
        /// <param name="filter">The filter executed.</param>
        /// <param name="configs">The configurations used by the filter.</param>
        private void _graph_FilterExecuted(WeakImage input, WeakImage output, Filter filter, SortedDictionary<string, object> configs, TimeSpan duration)
        {
            OnFilterExecuted(this, input, output, filter, configs, duration);
        }

        /// <summary>
        /// Executed when a filter in a graph finishes measurement.
        /// </summary>
        /// <param name="input">The input image of the filter.</param>
        /// <param name="output">The output image of the filter.</param>
        /// <param name="filter">The filter executed.</param>
        /// <param name="configs">The configurations used by the filter.</param>
        /// <param name="measures">The measures of the filter executed.</param>
        private void _graph_FilterMeasured(WeakImage input, WeakImage output, Filter filter, SortedDictionary<string, object> configs, List<MetricResult> measures, TimeSpan duration)
        {
            OnFilterMeasured(this, input, output, filter, configs, measures, duration);
        }

        /// <summary>
        /// Called to signal the end of a filter execution.
        /// </summary>
        /// <param name="sender">The BatchFilter sender of the event.</param>
        /// <param name="input">The input of the filter.</param>
        /// <param name="output">The output of the filter.</param>
        /// <param name="filter">The filter executed.</param>
        /// <param name="configs">The configurations used by the filter.</param>
        private void OnFilterExecuted(BatchFilter sender, WeakImage input, WeakImage output, Filter filter, SortedDictionary<string, object> configs, TimeSpan duration)
        {
            if (null != FilterExecuted)
            {
                FilterExecuted(sender, input, output, filter, configs, duration);
            }
        }

        /// <summary>
        /// Called to signal the end of a batch execution.
        /// </summary>
        /// <param name="sender"></param>
        private void OnBatchFinished(BatchFilter sender)
        {
            if (null != BatchFinished)
                BatchFinished(this);
        }

        /// <summary>
        /// Called to signal the end of a filter measurement.
        /// </summary>
        /// <param name="sender">The BatchFilter sender of the event.</param>
        /// <param name="input">The input of the filter.</param>
        /// <param name="output">The output of the filter.</param>
        /// <param name="filter">The filter executed.</param>
        /// <param name="configs">The configurations used by the filter.</param>
        /// <param name="measures">The measures of the filter executed.</param>
        private void OnFilterMeasured(BatchFilter sender, WeakImage input, WeakImage output, Filter filter, SortedDictionary<string, object> configs, List<MetricResult> measures, TimeSpan duration)
        {
            if (null != FilterMeasured)
                FilterMeasured(sender, input, output, filter, configs, measures, duration);
        }

        #endregion

    }
}