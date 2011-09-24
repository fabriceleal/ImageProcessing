using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Framework.Core.Filters.Base;
using Framework.Core.Metrics;
using Framework.Core;

namespace Framework.Batch
{
    /// <summary>
    /// Provides a representation of a graph-like structure for representing
    /// the execution of interdependent filters.
    /// </summary>
    public class Graph
    {

        #region Attributes

        /// <summary>
        /// The root node of the Graph.
        /// </summary>
        private Node _root;

        /// <summary>
        /// The parent BatchFilter that holds this graph.
        /// </summary>
        private BatchFilter _batchFilter;

        /// <summary>
        /// Represents the method that handles a FilterExecuted event.
        /// </summary>
        /// <param name="input">The input of the filter.</param>
        /// <param name="output">The output of the filter.</param>
        /// <param name="filter">The filter executed.</param>
        /// <param name="configs">The configurations used by the filter.</param>
        /// <param name="duration">The duration of the execution of the filter.</param>
        public delegate void FilterExecutedDelegate(WeakImage input, WeakImage output, Filter filter, SortedDictionary<string, object> configs, TimeSpan duration);

        /// <summary>
        /// Occurs after a filter executes.
        /// </summary>
        public event FilterExecutedDelegate FilterExecuted;

        /// <summary>
        /// Represents the method that handles a FilterMeasured event.
        /// </summary>
        /// <param name="input">The input of the filter.</param>
        /// <param name="output">The output of the filter.</param>
        /// <param name="filter">The filter executed.</param>
        /// <param name="configs">The configurations used by the filter.</param>
        /// <param name="measures">The measures of the filter executed.</param>
        /// <param name="duration">The duration of the execution of the filter.</param>
        public delegate void FilterMeasuredDelegate(WeakImage reference, WeakImage ouput, Filter filter, SortedDictionary<string, object> configs, List<MetricResult> measures, TimeSpan duration);

        /// <summary>
        /// Occurs after a filter is measured.
        /// </summary>
        public event FilterMeasuredDelegate FilterMeasured;

        #endregion

        #region Constructors

        /// <summary>
        /// Provides a representation of a graph-like structure for representing
        /// the execution of interdependent filters.
        /// </summary>
        /// <param name="batchFilter">The parent BatchFilter that holds this graph.</param>
        /// <param name="relations">List of relations between nodes.</param>
        /// <param name="filters">List of filters.</param>
        public Graph(
                BatchFilter batchFilter,
                List<RowNodeRelation> relations,
                List<RowBatchFilter> filters)
        {
            _batchFilter = batchFilter;

            // Create root node, dependent of the input images associated with the BatchFilter ...
            _root = new Node(this, null);

            MakeGraph(Guid.Empty, _root, relations, filters);
        }

        /// <summary>
        /// Builds the graph of nodes from a list of node relations and a list of filters.
        /// After building the currentNode, it will build the countChilds of this node, using the 
        /// relations list as a reference.
        /// </summary>
        /// <param name="currentGuid">The Guid of the node begin created.</param>
        /// <param name="currentNode">The node being created.</param>
        /// <param name="relations">List of relations between nodes.</param>
        /// <param name="filters">List of filters.</param>
        private void MakeGraph(
                Guid currentGuid, Node currentNode,
                List<RowNodeRelation> relations,
                List<RowBatchFilter> filters)
        {
            SortedDictionary<string, object> configs;
            Filter filter; Node filter_f;
            List<MetricExecBase> metrics;

            IEnumerable<RowBatchFilter> sons =
                    from a in filters
                    join r in relations on a.Item5 equals r.Item2
                    where r.Item1 == currentGuid
                    select a;
            // --

            foreach (RowBatchFilter r in sons)
            {
                configs = r.Item2; filter = r.Item3; metrics = r.Item4;

                filter_f = (Node)new NodeFilter(this, currentNode, filter, configs, metrics);
                filter_f.FilterExecuted += new Node.FilterExecutedDelegate(filter_f_FilterExecuted);
                filter_f.FilterMeasured += new Node.FilterMeasuredDelegate(filter_f_FilterMeasured);

                currentNode.Children.Add(filter_f);

                MakeGraph(r.Item5, filter_f, relations, filters);
            }
        }

        #endregion

        #region Properties

        public int CountFilters
        {
            get
            {
                if (_root != null)
                {
                    return _root.CountFilters;
                }
                return 0;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Executes the graph.
        /// </summary>
        /// <param name="images">The input images to the graph.</param>
        /// <returns>The output images of the filters.</returns>
        public List<WeakImage> Execute(List<WeakImage> images)
        {
            return _root.Execute(images);
        }

        /// <summary>
        /// Executed when a filter in a graph finishes execution.
        /// </summary>
        /// <param name="input">The input of the filter.</param>
        /// <param name="output">The output of the filter.</param>
        /// <param name="filter">The filter executed.</param>
        /// <param name="configs">The configurations used by the filter.</param>
        /// <param name="duration">The duration of the execution of the filter.</param>
        protected void filter_f_FilterExecuted(WeakImage input, WeakImage output, Filter filter, SortedDictionary<string, object> configs, TimeSpan duration)
        {
            OnFilterExecuted(input, output, filter, configs, duration);
        }

        /// <summary>
        /// Called to signal the end of a filter execution.
        /// </summary>
        /// <param name="input">The input of the filter.</param>
        /// <param name="output">The output of the filter.</param>
        /// <param name="filter">The filter executed.</param>
        /// <param name="configs">The configurations used by the filter.</param>
        /// <param name="duration">The duration of the execution of the filter.</param>
        protected void OnFilterExecuted(WeakImage input, WeakImage output, Filter filter, SortedDictionary<string, object> configs, TimeSpan duration)
        {
            if (null != FilterExecuted)
                FilterExecuted(input, output, filter, configs, duration);
        }

        /// <summary>
        /// Executed when a filter in a graph finishes measurement.
        /// </summary>
        /// <param name="input">The input of the filter.</param>
        /// <param name="output">The output of the filter.</param>
        /// <param name="filter">The filter executed.</param>
        /// <param name="configs">The configurations used by the filter.</param>
        /// <param name="results">The measures of the filter executed.</param>
        /// <param name="duration">The duration of the execution of the filter.</param>
        protected void filter_f_FilterMeasured(WeakImage input, WeakImage output, Filter filter, SortedDictionary<string, object> configs, List<MetricResult> results, TimeSpan duration)
        {
            OnFilterMeasured(input, output, filter, configs, results, duration);
        }

        /// <summary>
        /// Called to signal the end of a filter measurement.
        /// </summary>
        /// <param name="input">The input of the filter.</param>
        /// <param name="output">The output of the filter.</param>
        /// <param name="filter">The filter executed.</param>
        /// <param name="configs">The configurations used by the filter.</param>
        /// <param name="results">The measures of the filter executed.</param>
        /// <param name="duration">The duration of the execution of the filter.</param>
        protected void OnFilterMeasured(WeakImage input, WeakImage output, Filter filter, SortedDictionary<string, object> configs, List<MetricResult> results, TimeSpan duration)
        {
            if (null != FilterMeasured)
                FilterMeasured(input, output, filter, configs, results, duration);
        }

        #endregion

    }
}
