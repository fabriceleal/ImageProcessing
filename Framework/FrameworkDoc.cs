// This file holds the documentation for the main namespaces in the Framework
// Documentation for namespaces that "categorize" filters / namespaces
// * SHOULD * be included within the folder (namespace) that holds the files (classes)
// that compose the namespace

/// <summary>
/// Framework for image enhancement and edge detection.
/// </summary>
/// <remarks>
/// 
/// </remarks>
/// <example>
/// Using the Framework in the .NET environment.
/// <code>
/// // Add the Framework assembly (.dll) to the .NET project as a reference 
/// 
/// Framework.Core.Filters.Base.Filter[] listFilters;
/// Framework.Core.Filters.Base.FilterCore filterInst;
/// SortedDictionary<string, object> configs;
/// 
/// // Load all implemented filters
/// listFilters = Framework.Core.Factory.GetImplementedFilters();
/// 
/// // Iterate and display filters
/// foreach (Framework.Core.Filters.Base.Filter f in listFilters)
/// {
/// 	System.Diagnostics.Debug.Print(f.FilterType.FullName);
/// }
/// 
/// // To create an instance of a filter from a Filter structure.    
/// //filterInst = System.Activator.CreateInstance( listFilters[3].FilterType );
/// // To create an instance of a filter with the class name
/// filterInst = new Framework.Filters.Smoothing.Mean.ArithmeticMean();
/// 
/// // Get the configs of the filter
/// configs = filterInst.GetDefaultConfigs();
/// 
/// // Execute filter
/// Image img = Image.FromFile("test.bmp");
/// Image ret;
/// ret = filterInst.ApplyFilter(img, configs);
/// </code>
/// 
/// Using the Framework in the MATLAB environment.
/// <code>
/// 
/// % Full path to the Framework assembly (.dll); if is installed in the GAC, only the 
/// % name of the assembly
/// asm = 'Framework.dll';
/// 
/// % Load assembly, execute filter, show before and after image.
/// try
///    % Load assembly into MATLAB environment
///     asmInfo = NET.addAssembly(asm);
///     
///     % Load all implemented filters
///     filters = Framework.Core.Factory.GetImplementedFilters();
///     
///     % Iterate and display filters
///     for i = (0:filters.Length - 1),
///        disp( sprintf('%s (%s)', char( filters.Get(i).FilterType.FullName ), char( filters.Get(i).Attributes.Item('ShortName') ) ) );     
///     end
///     
///     % To create an instance of a filter from a Filter structure.    
///     %v_filter = System.Activator.CreateInstance( filters.Get(3).FilterType );
///     % To create an instance of a filter with the class name
///     v_filter = Framework.Filters.Smoothing.Mean.ArithmeticMean()    
///     
///     % Get the configs of the filter
///     a_configs = v_filter.GetDefaultConfigs();
///     
///     % To change the value of a setting in the dictionary
///     a_configs.Remove('window size');
///     a_configs.Add('window size', 5);
///     
///     % Load an image, convert to greyscales
///     img1 = imread('pout.tif');
///     
///     % Matlab uint8 2-dim array to .NET byte 2-dim array
///     netArr = NET.convertArray(img1, 'System.Byte', size(img1, 1), size(img1, 2));
/// 
///     % Execute filter
///     img2 = v_filter.ApplyFilter( netArr , a_configs );
/// 
///     % .NET byte 2-dim array to Matlab uint8 2-dim array
///     img2 = uint8(img2);
///     
///     % Show images ...    
///     figure(1), imshow(img1), title('Before');
///     figure(2), imshow(img2), title('After');
///     
/// catch e
///     e.message
///     if(isa(e, 'NET.NetException'))
///         e.ExceptionObject
///     end
/// end
/// </code>
/// </example>
namespace Framework
{ }
