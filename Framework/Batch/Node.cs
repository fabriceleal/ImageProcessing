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
    /// Simple node to include in a Graph of the Framework.Batch.
    /// </summary>
    public class Node
    {

        #region Attributes

        /// <summary>
        /// Duration of the node execution.
        /// </summary>
        protected TimeSpan _duration;

        /// <summary>
        /// The parent node of this node.
        /// </summary>
        private Node _parent;

        /// <summary>
        /// The nodes that take as input the output of this node.
        /// </summary>
        private List<Node> _children;

        /// <summary>
        /// The Graph that holds this node.
        /// </summary>
        private Graph _graphFilter;

        /// <summary>
        /// Represents the method that handles a FilterExecuted event.
        /// </summary>
        /// <param name="input">The input of the filter.</param>
        /// <param name="ouput">The output of the filter.</param>
        /// <param name="filter">The filter executed.</param>
        /// <param name="configs">The configurations used by the filter.</param>
        /// <param name="duration">The duration of the execution of the filter.</param>
        public delegate void FilterExecutedDelegate(WeakImage input, WeakImage ouput, Filter filter, SortedDictionary<string, object> configs, TimeSpan duration);

        /// <summary>
        /// Occurs after a filter executes.
        /// </summary>
        public event FilterExecutedDelegate FilterExecuted;

        /// <summary>
        /// Represents the method that handles a FilterMeasured event.
        /// </summary>
        /// <param name="input">The input of the filter.</param>
        /// <param name="ouput">The output of the filter.</param>
        /// <param name="filter">The filter executed.</param>
        /// <param name="configs">The configurations used by the filter.</param>
        /// <param name="measures">The measures of the filter executed.</param>
        /// <param name="duration">The duration of the execution of the filter.</param>
        public delegate void FilterMeasuredDelegate(WeakImage input, WeakImage ouput, Filter filter, SortedDictionary<string, object> configs, List<MetricResult> measures, TimeSpan duration);

        /// <summary>
        /// Occurs after a filter measured.
        /// </summary>
        public event FilterMeasuredDelegate FilterMeasured;

        #endregion

        #region Constructors

        /// <summary>
        /// Base class for implementing a Node to include into a Graph.
        /// </summary>
        /// <param name="graphFilter">The graph that holds this node.</param>
        /// <param name="parent">The parent node of this node. Set to null if this is the root node.</param>
        public Node(Graph graphFilter, Node parent)
        {
            _children = new List<Node>();
            _graphFilter = graphFilter;
            _parent = parent;
            _duration = TimeSpan.Zero;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the child nodes of this node; the nodes that take as input the output of this node.
        /// </summary>
        public List<Node> Children
        {
            get
            {
                return _children;
            }
            set
            {
                _children = value;
            }
        }

        /// <summary>
        /// The parent node of this node. Set to null if this is the root node.
        /// </summary>
        public Node Parent
        {
            get
            {
                return _parent;
            }
        }

        /// <summary>
        /// The duration of the execution of the node.
        /// </summary>
        public TimeSpan Duration
        {
            get
            {
                return _duration;
            }
        }

        /// <summary>
        /// The number of filters executed in this node.
        /// </summary>
        public virtual int CountFilters
        {
            get
            {
                int countFilters = 0;
                if (Children != null)
                {
                    foreach (Node child in Children)
                    {
                        countFilters += child.CountFilters;
                    }
                }
                return countFilters;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns the chain of nodes executed until reaching this node.
        /// </summary>
        /// <returns></returns>
        public List<Node> GetChain()
        {
            List<Node> ret = new List<Node>();

            if (_parent != null)
            {
                // Add first chain of parent ...
                ret.AddRange(_parent.GetChain());
                // ... then add parent itself
                ret.Add(_parent);
            }

            return ret;
        }

        ///// <summary>
        ///// Returns the sum of the times taken by the parents of this node.
        ///// </summary>
        ///// <returns></returns>
        //public TimeSpan GetParentTimes()
        //{
        //    TimeSpan ret = _duration;

        //    if (_parent != null)
        //    {
        //        ret += _parent.GetParentTimes();
        //    }

        //    return ret;
        //}

        /// <summary>
        /// Executes the node; passes the output to the children, executing them recursively.
        /// </summary>
        /// <param name="inputs">The input of the node.</param>
        /// <returns>The outputs of every node executed.</returns>
        public List<WeakImage> Execute(List<WeakImage> inputs)
        {
            List<WeakImage> _toChildren = Execute_Impl(inputs);

            List<WeakImage> ret = new List<WeakImage>();
            foreach (Node child in _children)
            {
                ret.AddRange(child.Execute(_toChildren).ToArray());
            }

            return ret;
        }

        /// <summary>
        /// Implementation of the execution of the node.
        /// </summary>
        /// <param name="inputs">The input of the node.</param>
        /// <returns>The output of the node.</returns>
        protected virtual List<WeakImage> Execute_Impl(List<WeakImage> inputs)
        {
            // clean body 
            return inputs;
        }

        /// <summary>
        /// Called to signal the end of a filter execution.
        /// </summary>
        /// <param name="input">The input of the filter.</param>
        /// <param name="ouput">The output of the filter.</param>
        /// <param name="filter">The filter executed.</param>
        /// <param name="configs">The configurations used by the filter.</param>
        protected void OnFilterExecuted(WeakImage input, WeakImage output, Filter filter, SortedDictionary<string, object> configs, TimeSpan duration)
        {
            if (null != FilterExecuted)
                FilterExecuted(input, output, filter, configs, duration);
        }

        /// <summary>
        /// Called to signal the end of a filter measurement.
        /// </summary>
        /// <param name="input">The input of the filter.</param>
        /// <param name="ouput">The output of the filter.</param>
        /// <param name="filter">The filter executed.</param>
        /// <param name="configs">The configurations used by the filter.</param>
        /// <param name="measures">The measures of the filter executed.</param>
        protected void OnFilterMeasured(WeakImage input, WeakImage output, Filter filter, SortedDictionary<string, object> configs, List<MetricResult> measures, TimeSpan duration)
        {
            if (null != FilterMeasured)
                FilterMeasured(input, output, filter, configs, measures, duration);
        }

        #endregion

    }
}
