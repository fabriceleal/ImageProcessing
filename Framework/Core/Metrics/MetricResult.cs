using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Framework.Core.Metrics
{
    /// <summary>
    /// Structure to hold the result of measurement.
    /// </summary>
    public class MetricResult
    {

        #region Constructors

        /// <summary>
        /// Structure to hold the result of measurement.
        /// </summary>
        /// <param name="reference">The image used as reference.</param>
        /// <param name="key">The key of the metric.</param>
        /// <param name="value">The value of the measurement.</param>
        public MetricResult(WeakImage reference, string key, double value)
        {
            Reference = reference;
            Key = key;
            Value = value;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the image used as reference.
        /// </summary>
        public WeakImage Reference { get; private set; }

        /// <summary>
        /// Gets the key of the metric.
        /// </summary>
        public string Key { get; private set; }

        /// <summary>
        /// Gets the value of the measurement.
        /// </summary>
        public double Value { get; private set; }

        #endregion
        
    }
}
