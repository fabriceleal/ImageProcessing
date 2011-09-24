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
    /// Node implementation that can execute a Filter.
    /// </summary>
    public class NodeFilter : Node
    {

        #region Attributes

        /// <summary>
        /// The filter to execute.
        /// </summary>
        protected Filter filter;

        /// <summary>
        /// The configurations of the filter to execute.
        /// </summary>
        protected SortedDictionary<string, object> configs;

        /// <summary>
        /// The measures to calculate.
        /// </summary>
        protected List<MetricExecBase> measures;

        #endregion

        #region Constructors

        /// <summary>
        /// Node that executes a certain filter.
        /// </summary>
        /// <param name="graphFilter">The graph that holds this node.</param>
        /// <param name="filter">The filter to execute.</param>
        /// <param name="configs">The configurations of the filter to execute.</param>
        /// <param name="measures">The measures to calculate.</param>
        public NodeFilter(
                    Graph graphFilter, Node parent, Filter filter,
                    SortedDictionary<string, object> configs,
                    List<MetricExecBase> measures)
            : base(graphFilter, parent)
        {
            this.filter = filter;
            this.configs = configs;
            this.measures = measures;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The number of filters executed in this node.
        /// </summary>
        public override int CountFilters
        {
            get
            {
                int countChilds = 0;

                if (Children != null)
                {
                    foreach (Node child in Children)
                    {
                        countChilds += child.CountFilters;
                    }
                }

                return 1 + countChilds;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Implementation of the execution of the node.
        /// </summary>
        /// <param name="inputs">The input of the node.</param>
        /// <returns>The output of the node.</returns>
        protected override List<WeakImage> Execute_Impl(List<WeakImage> inputs)
        {
            List<WeakImage> outputs = new List<WeakImage>();

            if (null == filter)
            {
                outputs = inputs;
            }
            else
            {
                FilterCore flt = (FilterCore)Activator.CreateInstance(filter.FilterType);
                Image o; WeakImage wo;
                foreach (WeakImage i in inputs)
                {
                    DateTime start = DateTime.Now;

                    o = flt.ApplyFilter((Image)i, configs);

                    DateTime end = DateTime.Now;

                    // _duration is always initialized to TimeSpan.Zero
                    // if the node has a parent, _duration should be initialized 
                    // with the duration of the parent
                    if (Parent != null)
                        _duration = Parent.Duration;

                    _duration += (end - start);

                    o.Tag = Facilities.CloneTag((Image)i);
                    Facilities.AddFilterExecution(ref o, flt, configs);

                    wo = new WeakImage(o);
                    outputs.Add(wo);

                    OnFilterExecuted(i, wo, filter, configs, _duration);


                    // Calculate Metrics ** Here **
                    // Raise even without measures
                    List<MetricResult> results = new List<MetricResult>();
                    if (null != measures)
                    {
                        //SortedDictionary<string, double> results = new SortedDictionary<string, double>();
                        foreach (MetricExecBase m in measures)
                        {
                            m.SetInput(i);
                            m.SetOutput(wo);
                            results.Add(new MetricResult(m.Input, m.Key, m.Calculate()));
                        }
                    }

                    OnFilterMeasured(i, wo, filter, configs, results, _duration);
                }
            }

            return outputs;
        }

        #endregion

    }
}
