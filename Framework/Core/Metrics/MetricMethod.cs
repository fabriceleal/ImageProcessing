using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Framework.Core.Metrics
{
    /// <summary>
    /// Simple structure to package a filter's data.
    /// </summary>
    /// <remarks>
    /// Cannot be instanciated from outside the framework.
    /// </remarks>
    public class MetricMethod
    {
        #region Attributes

        /// <summary>
        /// Metric function.
        /// </summary>
        private Metric.MetricDelegate _method;

        /// <summary>
        /// A hashtable with all the (key, value) pairs in the MetricAttribute associated 
        /// to the actual implementation of a metric.
        /// </summary>
        private Hashtable _attributes;

        #endregion

        #region Constructors

        /// <summary>
        /// Simple structure to package a filter's data.
        /// </summary>
        /// <param name="method">Metric function.</param>
        internal MetricMethod(Metric.MetricDelegate method)
        {
            MetricAttribute mm = Factory.GetMetricsFromMethodInfo(method.Method);
            if (mm == null)
                throw new NotSupportedException(string.Format("Method {0} is not decorated with MetricAttribute ", method.Method.Name));

            _method = method;

            _attributes = new Hashtable();
            _attributes.Add("Name", mm.Name);
            _attributes.Add("Description", mm.Description);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the Metric function.
        /// </summary>
        public Metric.MetricDelegate Method
        {
            get
            {
                return _method;
            }
        }

        /// <summary>
        /// Gets the hashtable with all the (key, value) pairs in the MetricAttribute associated 
        /// to the actual implementation of a metric.
        /// </summary>
        public Hashtable Attributes
        {
            get
            {
                return _attributes;
            }
        }

        #endregion
    }
}
