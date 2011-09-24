using System;
using System.Drawing;
using System.Globalization;
using System.Collections.Generic;
using Framework.Core.Filters.Base;
using Framework.Core.Metrics;
using System.Text;
using System.IO;
using System.Reflection;
using System.Collections;
using System.Xml;
using System.Xml.Schema;
using Framework.Core;

namespace Framework.Batch
{

    /// <summary>
    /// Defines a relationship parent-child between two RowNode.
    /// </summary>
    public class RowNodeRelation
    {
        /// <summary>
        /// The Guid of the parent node.
        /// </summary>
        public Guid Item1 { get; set; }

        /// <summary>
        /// The Guid of the child node.
        /// </summary>
        public Guid Item2 { get; set; }

        /// <summary>
        /// Defines a relationship parent-child between two nodes.
        /// </summary>
        /// <param name="Item1">The Guid of the parent node.</param>
        /// <param name="Item2">The Guid of the child node.</param>
        public RowNodeRelation(Guid Item1, Guid Item2)
        {            
            this.Item1 = Item1;
            this.Item2 = Item2;
        }
    }

    /// <summary>
    /// Defines a execution node in the batch; a execution node may include several RowBatchFilter, 
    /// and their output may be passed through another RowNode.
    /// </summary>
    public class RowNode
    {
        /// <summary>
        /// Guid of the RowNode. Guid.Empty is the Guid of the root RowNode.
        /// </summary>
        public Guid Item1 { get; set; }

        /// <summary>
        /// Description of the RowNode.
        /// </summary>
        public string Item2 { get; set; }

        /// <summary>
        /// Guid of the parent RowNode. Guid.Empty is the Guid of the root RowNode.
        /// </summary>
        public Guid Item3 { get; set; }

        /// <summary>
        /// Defines a execution node in the batch; a execution node may include several RowBatchFilter, 
        /// and their output may be passed through another RowNode.
        /// </summary>
        /// <param name="Item1">Guid of this RowNode.</param>
        /// <param name="Item2">Description of this RowNode. Guid.Empty is the Guid of the root RowNode.</param>
        /// <param name="Item3">Guid of the parent RowNode. Guid.Empty is the Guid of the root RowNode.</param>
        public RowNode(Guid Item1, string Item2, Guid Item3)
        {
            this.Item1 = Item1;
            this.Item2 = Item2;
            this.Item3 = Item3;
        }
    }

    /// <summary>
    /// Defines the execution of a filter in the batch.
    /// </summary>
    public class RowBatchFilter
    {
        /// <summary>
        /// The name of the filter (for display only).
        /// </summary>
        public string Item1 { get; set; }

        /// <summary>
        /// The configurations of the filter.
        /// </summary>
        public SortedDictionary<string, object> Item2 { get; set; }

        /// <summary>
        /// The filter to execute.
        /// </summary>
        public Filter Item3 { get; set; }

        /// <summary>
        /// The metric's setup for the filter.
        /// </summary>
        public List<MetricExecBase> Item4 { get; set; }

        /// <summary>
        /// The Guid of the RowNode that has this RowBatchFilter.
        /// </summary>
        public Guid Item5 { get; set; }

        /// <summary>
        /// Defines the execution of a filter in the batch.
        /// </summary>
        /// <param name="Item1">The name of the filter (for display only).</param>
        /// <param name="Item2">The configurations of the filter.</param>
        /// <param name="Item3">The filter to execute.</param>
        /// <param name="Item4">The metric's setup for the filter.</param>
        /// <param name="Item5">The Guid of the RowNode that has this RowBatchFilter.</param>
        public RowBatchFilter(
                string Item1, SortedDictionary<string, object> Item2, Filter Item3,
                List<MetricExecBase> Item4, Guid Item5)
        {
            this.Item1 = Item1;
            this.Item2 = Item2;
            this.Item3 = Item3;
            this.Item4 = Item4;
            this.Item5 = Item5;
        }
    }

    /// <summary>
    /// Defines an image inside the batch.
    /// </summary>
    /// <remarks>Images will be persisted using the base64 encoded binary representation of the Image.</remarks>
    public class RowImage
    {
        /// <summary>
        /// The image. Images will be persisted using the base64 encoded binary representation of the Image.
        /// </summary>
        public WeakImage Item1 { get; set; }

        /// <summary>
        /// Defines an image inside the batch.
        /// </summary>
        /// <param name="Item1">The image. Images will be persisted using the base64 encoded binary representation of the Image.</param>
        public RowImage(WeakImage Item1)
        {
            this.Item1 = Item1;
        }
    }


    /// <summary>
    /// Defines a calculated metric.
    /// </summary>
    public class RowMetric
    {
        /// <summary>
        /// The key of the metric in the filter's metric collection
        /// </summary>
        public string Item1 { get; set; }

        /// <summary>
        /// The metric function.
        /// </summary>
        public Metric.MetricDelegate Item2 { get; set; }

        /// <summary>
        /// Defines a metric calculated.
        /// </summary>
        /// <param name="Item1">The key of the metric in the filter's metric collection.</param>
        /// <param name="Item2">The metric function.</param>
        public RowMetric(string Item1, Metric.MetricDelegate Item2)
        {
            this.Item1 = Item1;
            this.Item2 = Item2;
        }
    }

    /// <summary>
    /// Types of execution metrics, by inputs to the metric functions.
    /// </summary>
    public enum MetricExecutionType
    {
        /// <summary>
        /// Uses the input and the output of the filter's execution
        /// </summary>
        InputOutput,
        /// <summary>
        /// Uses a fixed reference and the output of the filter's execution.
        /// </summary>
        RefOutput//,
        ///// <summary>
        ///// NOT IMPLEMENTED.
        ///// </summary>
        //NullOutput
    }

    /// <summary>
    /// Defines a metric execution for a filter RowResults.
    /// </summary>
    public class RowMetricExecution
    {
        /// <summary>
        /// The metric function.
        /// </summary>
        public Metric.MetricDelegate Item1 { get; set; }

        /// <summary>
        /// The metric execution type.
        /// </summary>
        public MetricExecutionType Item2 { get; set; }

        /// <summary>
        /// A "reference" image, if needed.
        /// </summary>
        /// <see cref="MetricExecutionType"/>
        public WeakImage Item3 { get; set; }

        /// <summary>
        /// Defines a metric execution for a filter RowResults.
        /// </summary>
        /// <param name="Item1">The metric function.</param>
        /// <param name="Item2">The metric function.</param>
        /// <param name="Item3">A "reference" image, if needed.</param>
        public RowMetricExecution(
                Metric.MetricDelegate Item1,
                MetricExecutionType Item2, WeakImage Item3)
        {
            this.Item1 = Item1;
            this.Item2 = Item2;
            this.Item3 = Item3;
        }
    }

    /// <summary>
    /// Results of the execution of a filter.
    /// </summary>
    public class RowResults
    {
        /// <summary>
        /// The name of the filter (for display only).
        /// </summary>
        public string Item1 { get; set; }

        /// <summary>
        /// The input image used by the filter.
        /// </summary>
        public WeakImage Item2 { get; set; }

        /// <summary>
        /// The output of the filter.
        /// </summary>
        public WeakImage Item3 { get; set; }

        /// <summary>
        /// The list of calculated metrics.
        /// </summary>
        public List<MetricResult> Item4 { get; set; }

        /// <summary>
        /// The filter used.
        /// </summary>
        public Filter Item5 { get; set; }

        /// <summary>
        /// The filter's configurations.
        /// </summary>
        public SortedDictionary<string, object> Item6 { get; set; }

        /// <summary>
        /// The filter's execution duration.
        /// </summary>
        public TimeSpan Item7 { get; set; }

        /// <summary>
        /// Results of the execution of a filter.
        /// </summary>
        /// <param name="Item1">The name of the filter (for display only).</param>
        /// <param name="Item2">The input image used by the filter.</param>
        /// <param name="Item3">The output of the filter.</param>
        /// <param name="Item4">The list of calculated metrics.</param>
        /// <param name="Item5">The filter used.</param>
        /// <param name="Item6">The filter's configurations.</param>
        public RowResults(string Item1, WeakImage Item2, WeakImage Item3,
                List<MetricResult> Item4, Filter Item5,
                SortedDictionary<string, object> Item6, TimeSpan Item7)
        {
            this.Item1 = Item1;
            this.Item2 = Item2;
            this.Item3 = Item3;
            this.Item4 = Item4;
            this.Item5 = Item5;
            this.Item6 = Item6;
            this.Item7 = Item7;
        }

        /// <summary>
        /// Get the schema used to validate the exported xml of results obtained with ExportToXml()
        /// </summary>
        /// <returns></returns>
        public static XmlSchema GetXmlSchema()
        {
            try
            {
                Assembly _asm = Assembly.GetExecutingAssembly();
                StreamReader _xmlReader = new StreamReader(_asm.GetManifestResourceStream("Framework.Batch.outputSchema.xsd"));

                XmlSchema schema = XmlSchema.Read(_xmlReader, _ValidateXml);

                return schema;
            }
            catch (Exception e)
            {
                throw new Exception("Error when trying to obtain xml schema for validating the xml exportation of List<RowResults>.", e);
            }
        }

        /// <summary>
        /// Exports results to a .xml file. The xml document's schema can be obtained with GetXmlSchema().
        /// </summary>
        /// <param name="_filename">The name of the file to save.</param>
        /// <param name="results">The list of RowResults to persist.</param>
        public static void ExportToXml(string filename, List<RowResults> results)
        {
            try
            {
                // Force the . as decimal separator
                NumberFormatInfo dblFormat = new NumberFormatInfo();
                dblFormat.NumberDecimalSeparator = ".";

                // Create XmlDocument ...
                XmlDocument doc = new XmlDocument();

                // Add namespace
                XmlNamespaceManager nm = new XmlNamespaceManager(doc.NameTable);

                const string PREFIX = "n";
                const string URL = @"http://tempuri.org/outputSchema.xsd";

                nm.AddNamespace(PREFIX, URL);

                // Add schema
                doc.Schemas.Add(GetXmlSchema());

                // create doc declaration, ...
                //doc.CreateXmlDeclaration("1.0", "utf-8", "true");

                // Create xml document
                XmlNode ndeExecution = doc.CreateElement(PREFIX, "execution", URL);
                XmlNode ndeFilter, ndeParameters, ndeChain, ndeMetrics, ndeParameterInstance, ndeMetricInstance;
                XmlNode ndeKey, ndeValue;

                foreach (RowResults rw in results)
                {
                    if (null != rw.Item4 && rw.Item4.Count > 0)
                    {
                        // For each filter with metrics, insert a new node
                        ndeFilter = doc.CreateElement(PREFIX, "filter", URL);

                        // Filter's name
                        ndeFilter.AppendChild(doc.CreateElement(PREFIX, "name", URL)).InnerText = rw.Item5.FilterType.FullName;

                        // Filter's parameters
                        ndeParameters = doc.CreateElement(PREFIX, "parameters", URL);
                        if (null != rw.Item6 && rw.Item6.Count > 0)
                        {
                            foreach (string key in rw.Item6.Keys)
                            {
                                ndeParameterInstance = doc.CreateElement(PREFIX, "parameter", URL);

                                ndeKey = doc.CreateElement(PREFIX, "key", URL);
                                ndeKey.InnerText = key;
                                ndeValue = doc.CreateElement(PREFIX, "value", URL);
                                ndeValue.InnerText = (double.Parse(rw.Item6[key].ToString())).ToString(dblFormat);


                                ndeParameterInstance.AppendChild(ndeKey);
                                ndeParameterInstance.AppendChild(ndeValue);

                                ndeParameters.AppendChild(ndeParameterInstance);
                            }
                        }
                        ndeFilter.AppendChild(ndeParameters);

                        // Filter's chain
                        ndeChain = doc.CreateElement(PREFIX, "filterchain", URL);

                        // Try to get history bucket
                        Image theImage = (Image)rw.Item2;
                        if (theImage != null)
                        {
                            ArrayList bucket = Facilities.GetBucket<ArrayList>(
                                    ref theImage, Facilities.EXECUTED_FILTERS);

                            if (bucket != null)
                            {
                                foreach (object o in bucket)
                                {
                                    ndeChain.AppendChild(doc.CreateElement(PREFIX, "value", URL)).InnerText = o.ToString();
                                }
                            }
                        }

                        ndeFilter.AppendChild(ndeChain);

                        // Filter's metrics
                        ndeMetrics = doc.CreateElement(PREFIX, "metrics", URL);
                        foreach (MetricResult metric in rw.Item4)
                        {
                            ndeMetricInstance = doc.CreateElement(PREFIX, "metric", URL);

                            ndeKey = doc.CreateElement(PREFIX, "name", URL);
                            ndeKey.InnerText = metric.Key;
                            ndeValue = doc.CreateElement(PREFIX, "result", URL);
                            ndeValue.InnerText = metric.Value.ToString(dblFormat);

                            ndeMetricInstance.AppendChild(ndeKey);
                            ndeMetricInstance.AppendChild(ndeValue);

                            ndeMetrics.AppendChild(ndeMetricInstance);
                        }
                        ndeFilter.AppendChild(ndeMetrics);

                        // Filter's duration
                        ndeFilter.AppendChild(doc.CreateElement(PREFIX, "durationticks", URL)).InnerText = rw.Item7.Ticks.ToString();

                        // Append filter to root node
                        ndeExecution.AppendChild(ndeFilter);
                    }
                }

                // Append root to xml
                doc.AppendChild(ndeExecution);

                // Validate with xsd ...
                // Compile xsd embebbed in assembly ...
                // - On error will throw exception
                doc.Validate(_ValidateXml);

                // Save XmlDocument
                doc.Save(filename);
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("Error when trying to export to xml '{0}'.", filename), e);
            }
        }

        /// <summary>
        /// Auxiliary function to perform the invalidation logic when validating a xml file against a schema.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void _ValidateXml(object sender, ValidationEventArgs e)
        {
            throw new Exception("Error validating xml document.", e.Exception);
        }

        /// <summary>
        /// Exports results to a .csv file.
        /// </summary>
        /// <param name="_filename">The name of the file to save.</param>
        /// <param name="results">The list of RowResults to persist.</param>
        public static void ExportToCsv(string filename, List<RowResults> results)
        {
            try
            {
                // Force the . as decimal separator
                NumberFormatInfo dblFormat = new NumberFormatInfo();
                dblFormat.NumberDecimalSeparator = ".";

                StringBuilder buff = new StringBuilder();
                Dictionary<int, string> metrics_cols = new Dictionary<int, string>();

                // Columns headers
                // ***************************************************************
                buff.Append("filter,parameters,durationticks,filter-chain");
                int colidx = 0;

                // Gets all distinct metrics executed ...
                // Update metrics_cols and columns headers for each one.
                foreach (RowResults rw in results)
                {
                    if (null != rw.Item4)
                    {
                        foreach (MetricResult rw_res in rw.Item4)
                        {
                            if (!metrics_cols.ContainsValue(rw_res.Key))
                            {
                                metrics_cols.Add(colidx, rw_res.Key);
                                ++colidx;
                                // Add metric to columns; change commas by _ just in case!
                                buff.Append(",").Append(rw_res.Key.Replace(",", "_"));
                            }
                        }
                    }
                }

                buff.AppendLine();

                // ***************************************************************
                // Add rows ...
                string sFilter = "", sParameters = "", sChain = "";
                int idxMetric;
                foreach (RowResults rw in results)
                {
                    if (null != rw.Item4 && rw.Item4.Count > 0)
                    {

                        // Get filter, parameters

                        Facilities.FilterExecToString(rw.Item5.FilterType, rw.Item6, out sFilter, out sParameters);

                        // Get chain of nodes ... convert to filter-list form
                        // ...
                        sChain = "";
                        // Try to get history bucket
                        Image theImage = (Image)rw.Item2;
                        if (theImage != null)
                        {
                            ArrayList bucket = Facilities.GetBucket<ArrayList>(
                                    ref theImage, Facilities.EXECUTED_FILTERS);

                            if (bucket != null)
                            {
                                foreach (object o in bucket)
                                {
                                    sChain += (string.IsNullOrEmpty(sChain) ? "" : " ; ") + o.ToString();
                                }
                            }
                        }

                        // filter
                        buff.Append(sFilter);
                        // parameters
                        buff.Append(",").Append(sParameters);
                        // duration
                        buff.Append(",").Append(rw.Item7.Ticks.ToString());
                        // chain
                        buff.Append(",").Append(sChain);

                        Dictionary<string, double> dMetrics = new Dictionary<string, double>();

                        foreach (MetricResult rw_res in rw.Item4)
                        {
                            dMetrics.Add(rw_res.Key, rw_res.Value);
                        }

                        for (int i = 0; i < colidx; ++i)
                        {
                            buff.Append(",");
                            if (dMetrics.ContainsKey(metrics_cols[i]))
                            {
                                buff.Append(dMetrics[metrics_cols[i]].ToString(dblFormat));
                            }
                        }

                        // --
                        buff.AppendLine();

                    }
                }

                File.WriteAllText(filename, buff.ToString());
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("Error when trying to export to csv '{0}'.", filename), e);
            }
        }
    }
}
