using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Core.Filters.Base
{
    /// <summary>
    /// Simple structure to package a filter's data.
    /// </summary>
    /// <remarks>
    /// Cannot be instanciated from outside the framework; it is only usable by
    /// using the Factory.GetImplementedFilters() or the Factory.GetFilterFromType() functions.
    /// </remarks>
    public class Filter : IComparable
    {

        #region Attributes

        /// <summary>
        /// The type of the actual implementation of FilterCore
        /// </summary>
        private Type _filterType;

        /// <summary>
        /// A hashtable with all the (key, value) pairs in the FilterAttribute associated 
        /// to the actual implementation of FilterCore.
        /// </summary>
        private Hashtable _attributes;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor of the Filter structure. The passed type should inherit from the FilterCore class
        /// and should be decorated with the attribute FilterAttribute.
        /// </summary>
        /// <param name="filterType">The type of the implementation.</param>
        /// <exception cref="NullReferenceException">If filterType is null.</exception>
        /// <exception cref="ArgumentException">If filterType doesn't have the attribute FilterAttribute associated.</exception>
        /// <exception cref="Exception">If the attribute FilterAttribute associated has the Show property set to false.</exception>
        internal Filter(Type filterType)
        {
            // Throw exception if parameter is null !!!
            if (null == filterType)
                throw new NullReferenceException();

            _filterType = filterType;

            FilterAttribute attributes = Factory.GetAttributeFromMemberInfo<FilterAttribute>(filterType);

            // Throw exception if attributes is null !!!
            if (null == attributes)
                throw new ArgumentException("Type " + filterType.FullName + " doesn't have the attribute FilterAttribute associated.");

            // Throw exception if the attribute set the class as 'invisible'
            if (!attributes.Show)
                throw new Exception("Not possible to create instance for type " + filterType.FullName);

            _attributes = new Hashtable();
            _attributes.Add("Name", attributes.Name);
            _attributes.Add("ShortName", attributes.ShortName);
            _attributes.Add("Description", attributes.Description);
            _attributes.Add("Categories", attributes.Categories);

        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the type of the filter implemented.
        /// </summary>
        public Type FilterType { get { return _filterType; } }

        /// <summary>
        /// Gets the attributes associated with the filter.
        /// </summary>
        public Hashtable Attributes { get { return _attributes; } }

        #endregion

        #region IComparable

        /// <summary>
        /// Compares two Filter using the ShortName attribute.
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        int System.IComparable.CompareTo(object o)
        {
            Filter wrk = o as Filter;
            if (null != o)
            {
                string this_name = (string)this.Attributes["ShortName"];
                string wrk_name = (string)wrk.Attributes["ShortName"];

                this_name = this_name ?? string.Empty;
                wrk_name = wrk_name ?? string.Empty;

                return this_name.CompareTo(wrk_name);

            }
            return int.MinValue;
        }

        #endregion
    }
}
