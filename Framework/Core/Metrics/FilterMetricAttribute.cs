using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Core.Metrics
{
    /// <summary>
    /// Defines a metric profile to associate to a FilterCore class. A class may be associated to only one metric profiles.
    /// </summary>
    /// <remarks>
    /// This attribute should be inherited to create a "metric profile", associable to a filter.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public abstract class FilterMetricAttribute : Attribute
    {

        #region Attributes

        /// <summary>
        /// The object that holds the metric functions.
        /// </summary>
        private Metric _metrics;

        #endregion

        #region Constructors

        /// <summary>
        /// Provides a inheritable attribute class to define a metric profile to associate to a filter.
        /// </summary>
        /// <param name="metrics">The object that holds the metric functions.</param>
        internal FilterMetricAttribute(Metric metrics)
        {
            _metrics = metrics;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the object that holds the metric functions.
        /// </summary>
        public Metric Metrics
        {
            get
            {
                return _metrics;
            }
        }

        #endregion

    }
}
