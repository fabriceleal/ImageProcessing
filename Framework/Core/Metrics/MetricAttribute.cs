using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Core.Metrics
{
    /// <summary>
    /// Attribute for defining additional information for a metric implementation.
    /// </summary>
    /// <remarks>Every method intended to be used as a metric 
    /// should have this attribute.</remarks>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class MetricAttribute : Attribute
    {
        /// <summary>
        /// The name of the method as viewed by the outside world. May be unique in a Metric implementation.
        /// </summary>
        private string _name;

        /// <summary>
        /// The description of the method.
        /// </summary>
        private string _description;

        /// <summary>
        /// Attribute for defining additional information for a metric implementation.
        /// </summary>
        /// <param name="name">The name of the method as viewed by the outside world. 
        /// May be unique in a Metric implementation.</param>
        /// <param name="description">The description of the method.</param>
        public MetricAttribute(string name, string description)
        {
            _name = name;
            _description = description;
        }

        /// <summary>
        /// Gets the name of the method as viewed by the outside world. May be unique in a Metric implementation.
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
        }

        /// <summary>
        /// Gets the description of the method.
        /// </summary>
        public string Description
        {
            get
            {
                return _description;
            }
        }

    }

}
