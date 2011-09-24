using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Framework.Core;
using Framework.Core.Filters.Base;
using Framework.Core.Filters.Spatial;
using Framework.Core.Filters.Frequency;
using Framework.Core.Metrics;
using System.Drawing;
using System.Drawing.Imaging;

namespace Framework.Core
{
    /// <summary>
    /// Main class for querying the Framework
    /// </summary>
    /// <example>
    /// Examples of using the Factory methods:
    /// <code>
    /// 
    /// // Get all filters within the Framework
    /// Filter[] filters = Factory.GetImplementedFilters();
    /// 
    /// // Prints to Output the filters
    /// foreach(Filter f in filters)
    /// {
    ///     System.Diagnostics.Debug.Print(f.FilterType.FullName);
    /// }
    /// 
    /// </code>
    /// </example>
    /// <remarks>
    /// This class make massive use of reflection, so calling it's functions
    /// should be kept to a minimum.
    /// </remarks>
    public class Factory
    {

        #region Methods

        /// <summary>
        /// Try to get instance of the attribute AttributeType from the memberInfo member.
        /// </summary>
        /// <typeparam name="AttributeType">The type of Attribute to query for</typeparam>
        /// <param name="mInfo">Type MemberInfo supports instances of Type and MethodInfo</param>
        /// <returns>The instance of Attribute that decorates the class. null if the class doesn't have
        /// the attribute</returns>
        internal static AttributeType GetAttributeFromMemberInfo<AttributeType>(MemberInfo mInfo)
            where AttributeType : Attribute
        {
            AttributeType attr = default(AttributeType);
            foreach (object a in mInfo.GetCustomAttributes(false))
            {
                attr = a as AttributeType;
                if (null != attr)
                {
                    return attr;
                }
            }
            return null;
        }

        /// <summary>
        /// Get all implemented filters.
        /// </summary>
        /// <returns>Array of sorted Filter</returns>
        public static Filter[] GetImplementedFilters()
        {
            List<Filter> ret = new List<Filter>();

            Assembly asm = Assembly.GetExecutingAssembly();

            foreach (Type t in asm.GetTypes())
            {
                // All Types that inherit from FilterCore
                if (t.IsSubclassOf(typeof(FilterCore)) && !t.IsAbstract)
                {
                    try
                    {
                        Filter f = new Filter(t);
                        ret.Add(f);
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Failing to get filter from type.", e);
                    }
                }
            }

            ret.Sort();
            return ret.ToArray();
        }

        /// <summary>
        /// Generate Filter instance from a FilterCore implementation.
        /// </summary>
        /// <param name="type">The class of the FilterCore implementation.</param>
        /// <returns>Instance of Filter, encapsulating the filter in type</returns>
        public static Filter GetFilterFromType(Type type)
        {
            if (null != type)
            {
                if (type.IsSubclassOf(typeof(FilterCore)) && !type.IsAbstract)
                {
                    try
                    {
                        // The filter constructor may fail.
                        Filter f = new Filter(type);
                        return f;
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Failing to get filter from type.", e);
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Get metric methods for a filter's class.
        /// </summary>
        /// <param name="filterType"></param>
        /// <returns></returns>
        public static Metric GetMetricsFromFilter(Type filterType)
        {
            Metric ret = null;
            FilterMetricAttribute m = GetAttributeFromMemberInfo<FilterMetricAttribute>(filterType);

            if (null != m)
            {
                ret = m.Metrics;
            }

            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        internal static MetricAttribute GetMetricsFromMethodInfo(MethodInfo method)
        {
            MetricAttribute ret = GetAttributeFromMemberInfo<MetricAttribute>(method);
            return ret;
        }

        /// <summary>
        /// Returns the Type instance from the fullname of the type, if the type is inside the Framework.
        /// </summary>
        /// <param name="fullname">The fullname of the type.</param>
        /// <returns>Instance of Type.</returns>
        public static Type GetType(string fullname)
        {
            return Assembly.GetExecutingAssembly().GetType(fullname);
        }

        /// <summary>
        /// Returns list of installed encoders in string format, ready for use in the OpenFileDialog.Filter property.
        /// </summary>
        /// <param name="includeAllImages">Include option All Images (all extensions found)</param>
        /// <param name="includeAllFiles">Include option All Files (*.*)</param>
        /// <remarks>Uses ImageCodecInfo.GetImageEncoders() function</remarks>
        /// <returns>String in the following format: </returns>
        public static String GetImageEncodersToDialogFilter(bool includeAllImages, bool includeAllFiles)
        {
            // Get an array of available encoders.
            StringBuilder buff = new StringBuilder();
            StringBuilder tmp_buff = null;

            ImageCodecInfo[] myCodecs;
            myCodecs = ImageCodecInfo.GetImageEncoders();

            if (myCodecs.Length > 0)
            {
                tmp_buff = new StringBuilder();

                buff.Append(myCodecs[0].FormatDescription);
                buff.Append(" - ");
                buff.Append(myCodecs[0].CodecName);
                buff.Append(" (");
                buff.Append(myCodecs[0].FilenameExtension.ToLower());
                buff.Append(")|");
                buff.Append(myCodecs[0].FilenameExtension.ToLower());

                tmp_buff.Append(myCodecs[0].FilenameExtension.ToLower());


                for (int i = 1; i < myCodecs.Length; ++i)
                {
                    buff.Append("|");
                    buff.Append(myCodecs[i].FormatDescription);
                    buff.Append(" - ");
                    buff.Append(myCodecs[i].CodecName);
                    buff.Append(" (");
                    buff.Append(myCodecs[i].FilenameExtension.ToLower());
                    buff.Append(")|");
                    buff.Append(myCodecs[i].FilenameExtension.ToLower());

                    tmp_buff.Append(",");
                    tmp_buff.Append(myCodecs[i].FilenameExtension.ToLower());
                }
            }

            if (includeAllImages && null != tmp_buff)
            {
                String all_images = tmp_buff.ToString();
                buff.Append("|All Images (");
                buff.Append(tmp_buff);
                buff.Append(")|");
                buff.Append(tmp_buff);
            }

            if (includeAllFiles)
            {
                buff.Append(myCodecs.Length > 0 ? "|" : "");

                buff.Append("All Files (*.*)|*.*");
            }

            return buff.ToString();
        }

        #endregion

    }
}
