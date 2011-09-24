// Filters

/// <summary>
/// Holds the implementations of the filters provided and used by the Framework.
/// </summary>
/// <remarks>
/// The filters are categorized in namespaces by their "main-objective".
/// A filter may be a operation that "sharps" an image (ie, enhances its features, enhances edges) 
/// or "blurs" an image (smooths noise); to these "base" processing, is also included the adition of noise to an image,
/// treating the image in the frequency domain and manipulating the image in the spatial domain.
/// 
/// </remarks>
/// <example>
/// Run a filter with an instance of System.Drawing.Image srcImage:
/// 
/// <code>
/// // First you have to create an instance of the filter
/// // * As an example, is used the Framework.Filters.Simple.HighThreshold filter
/// Framework.Filters.Simple.HighThreshold filter = new Framework.Filters.Simple.HighThreshold();
/// 
/// // If you only have a Type instance of the type of the filter, you may do:
/// // * t is an instance of Type, a class that may inherit the FilterCore class
/// Type t = Factory.GetType("Framework.Filters.Simple.HighThreshold");
/// // Or if you have an instance of Filter, you can use the property FilterType to extract the Type instance of the filter.
/// FilterCore filter = (FilterCore)System.Activator.CreateInstance(t); 
/// 
/// // With the instance of the filter, you may get and change the parameters of the filter;
/// // these parameters will need to be supplied to the ApplyFilter method
/// SortedDictionary<string, object> configs = filter.GetDefaultConfigs();
/// 
/// // ... work with configs ... change parameters
/// configs["High-Threshold"] = 75;
/// 
/// // Execute filter
/// Image resultImage = filter.ApplyFilter(srcImage, configs);
/// 
/// // ## The result of the execution is now in resultImage ##
/// 
/// // You may use the Tag property of Image to store information about
/// // the filters' chain of execution 
/// resultImage.Tag = Facilities.CloneTag(srcImage);
/// Facilities.AddFilterExecution(ref ret, filter, configs);
/// 
/// </code>
/// </example>
namespace Framework.Filters
{ }
