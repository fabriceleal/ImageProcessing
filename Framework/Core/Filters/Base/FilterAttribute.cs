using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Core.Filters.Base
{
    /// <summary>
    /// Attribute for defining additional information for a filter implementation.
    /// </summary>
    /// <remarks>
    /// Every implementation of a filter intended to be "visible" from the outside world
    /// though the functions Factory.GetImplementedFilters() or the Factory.GetFilterFromType() functions 
    /// should have this attribute.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class FilterAttribute : Attribute
    {

        #region Attributes

        /// <summary>
        /// Short name of the filter.
        /// </summary>
        private string _shortName;
        /// <summary>
        /// Name of the filter.
        /// </summary>
        private string _name;
        /// <summary>
        /// Description of the filter.
        /// </summary>
        private string _description;
        /// <summary>
        /// Categories (from bigger category .. to smaller category) for classifying a filter.
        /// </summary>
        private string[] _categories;
        /// <summary>
        /// Show the filter to the outside world.
        /// </summary>
        private bool _show;

        #endregion

        #region Constructors

        /// <summary>
        /// Attribute for defining additional information for a filter implementation.
        /// </summary>
        /// <param name="shortName">Short name of the filter.</param>
        /// <param name="name">Name of the filter.</param>
        /// <param name="description">Description of the filter.</param>
        /// <param name="categories">Categories (from bigger category .. to smaller category) for classifying a filter.</param>
        /// <param name="show">Show the filter to the outside world. A false value will not return the filter in the Factory.GetImplementedFilters() or the Factory.GetFilterFromType() functions.</param>
        public FilterAttribute(
                string shortName, string name,
                string description, string[] categories,
                bool show = true)
        {

            _shortName = shortName;
            _name = name;
            _description = description;
            _categories = categories;
            _show = show;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the short name of the filter.
        /// </summary>
        public string ShortName { get { return _shortName; } }

        /// <summary>
        /// Gets the name of the filter
        /// </summary>
        public string Name { get { return _name; } }

        /// <summary>
        /// Gets the description of the filter
        /// </summary>
        public string Description { get { return _description; } }

        /// <summary>
        /// Gets the categories (from bigger category .. to smaller category) for classifying a filter.
        /// </summary>
        public string[] Categories { get { return _categories; } }

        /// <summary>
        /// Gets the visibility of the filter's implementation to the outside world.
        /// </summary>
        /// <remarks>True to make the implementation visible to the outside world; false otherwise.
        /// A false value will not return the filter in the Factory.GetImplementedFilters() or the Factory.GetFilterFromType() functions.
        /// </remarks>
        public bool Show { get { return _show; } }

        /// <summary>
        /// Empty FilterAttribute.
        /// </summary>
        public FilterAttribute Empty
        {
            get
            {
                return new FilterAttribute("", "", "", null, false);
            }
        }

        #endregion

    }
}
