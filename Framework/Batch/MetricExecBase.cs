using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Framework.Core;
using Framework.Core.Metrics;

namespace Framework.Batch
{
    /// <summary>
    /// Base class for implementing a metric associable to a Node.
    /// </summary>
    /// <remarks>
    /// When calculating the metrics, the input and output image's of the filters are always
    /// given by calling respectively the SetInput() and the SetOutput() methods of this class.
    /// To setup a fixed "image", independent of the input / output of the filter, you may overload 
    /// the constructor and override the SetInput() method, leaving the method body clean.
    /// </remarks>
    public abstract class MetricExecBase
    {

        #region Attributes

        /// <summary>
        /// The key of the metric.
        /// </summary>
        protected string key;

        /// <summary>
        /// The metric function.
        /// </summary>
        protected Metric.MetricDelegate method;

        /// <summary>
        /// The input of the filter which is being measured; is set in by using the SetInput() method.
        /// </summary>
        protected WeakImage input;

        /// <summary>
        /// The output of the filter which is being measured; is set in by using the SetOutput() method.
        /// </summary>
        protected WeakImage output;

        #endregion

        #region Constructors

        /// <summary>
        /// Base class for implementing a metric associable to a Node.
        /// </summary>
        /// <param name="key">The key of the metric.</param>
        /// <param name="method">The metric function.</param>
        public MetricExecBase(string key, Metric.MetricDelegate method)
        {
            this.key = key;
            this.method = method;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the key of the metric.
        /// </summary>
        public string Key
        {
            get
            {
                return key;
            }
        }

        /// <summary>
        /// Gets the metric function.
        /// </summary>
        public Metric.MetricDelegate Method
        {
            get
            {
                return method;
            }
        }

        /// <summary>
        /// Gets the "input-image", used by the metric function.
        /// </summary>
        public virtual WeakImage Input
        {
            get
            {
                return input;
            }
        }

        /// <summary>
        /// Gets the "output-image", used by the metric function.
        /// </summary>
        public virtual WeakImage Output
        {
            get
            {
                return output;
            }
        }

        #endregion

        #region SetInput

        /// <summary>
        /// Sets the "input-image", used by the metric function.
        /// </summary>
        /// <param name="img"></param>
        public virtual void SetInput(WeakImage img)
        {
            input = img;
        }

        #endregion

        #region SetOutput

        /// <summary>
        /// Sets the "output-image", used by the metric function.
        /// </summary>
        /// <param name="img"></param>
        public virtual void SetOutput(WeakImage img)
        {
            output = img;
        }

        #endregion

        #region Exec

        /// <summary>
        /// Calculates and returns the value of the metric function associated.
        /// </summary>
        /// <returns></returns>
        public double Calculate()
        {
            return method((Image)input, (Image)output);
        }

        #endregion

    }
}
