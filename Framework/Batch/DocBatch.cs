// Batch

/// <summary>
/// Holds the implementation of a automatizable and persistable mechanism of running filters,
/// configuring images, references, filters and their parameters. A filter may feed another filters.
/// </summary>
/// <remarks>
/// </remarks>
/// <example>
/// Generate and run a batch.
/// This example assumes you are running from a System.Windows.Forms.Form and you want
/// to display notifications from the BatchFilter in the Form:
/// 
/// <code>
/// // Type aliases for better readibility
/// using FilterMeasuredDelegate = Framework.Batch.BatchFilter.FilterMeasuredDelegate;
/// using FilterExecutedDelegate = Framework.Batch.BatchFilter.FilterExecutedDelegate;
/// using BatchFinishedDelegate = Framework.Batch.BatchFilter.BatchFinishedDelegate;
/// 
/// // Locks, to avoid nasty run-conditions when executing notification logic
/// object filterExec_lock = new object();
/// object filterMeas_lock = new object();
/// 
/// private void Run()
/// {
/// 
///     bool notifyOnlyExecution = true;
///     List<RowNodeRelation> nodes = new List<RowNodeRelation>();
///     List<RowBatchFilter> filters = new List<RowBatchFilter>();
///     List<RowImage> srcs = new List<RowImage>();
/// 
///     // Setup nodes and filters 
///     nodes.Add(new RowNodeRelation(Guid.Empty, Guid.NewGuid())); // root node, with no filters
///     nodes.Add(new RowNodeRelation(nodes[0].Item2, Guid.NewGuid()));
///     nodes.Add(new RowNodeRelation(nodes[1].Item2, Guid.NewGuid()));
/// 
///     Filter laplacian = Factory.GetFilterFromType(Factory.GetType("Framework.Filters.EdgeDetection.SndDerivate.Laplacian"));
///     Filter bipolar = Factory.GetFilterFromType(Factory.GetType("Framework.Filters.Simple.BipolarThreshold"));
///     SortedDictionary<string, object> laplacianConfigs1 = new SortedDictionary<string, object>();
///     SortedDictionary<string, object> laplacianConfigs2 = new SortedDictionary<string, object>();
///     SortedDictionary<string, object> bipolarConfigs = new SortedDictionary<string, object>();
///     
///     laplacianConfigs1.Add("Threshold", 5);
///     laplacianConfigs1.Add("Alpha", 0.2);
///     laplacianConfigs2.Add("Threshold", 5);
///     laplacianConfigs2.Add("Alpha", 0.5);
///     bipolarConfigs.Add("Threshold", 10);
/// 
///     filters.Add(new RowBatchFilter(laplacian.FilterType.FullName, laplacianConfigs1, laplacian, null, nodes[1].Item1 )); // Apply Laplacian
///     filters.Add(new RowBatchFilter(laplacian.FilterType.FullName, laplacianConfigs2, laplacian, null, nodes[1].Item1 )); // Apply Laplacian
///     filters.Add(new RowBatchFilter(bipolar.FilterType.FullName, bipolarConfigs, bipolar, null, nodes[2].Item1 )); // Apply BipolarThreshold
/// 
///     // Setup metrics
///     // * References will need to be supplied to each metric individually
///     List<MetricExecBase> metrics = new List<MetricExecBase>();
///     // ## Setup reference here ##
///     WeakImage reference = null; 
///     
///     // Add methods
///     foreach(KeyValuePair<string, Metric.MetricDelegate> it in Factory.GetMetricsFromFilter(laplacian.FilterType).GetListMetrics())
///     {
///         // As an edge detection method, metrics are evaluated with a reference ...
///         metrics.Add(new MetricExecReference(it.Key, it.Value, reference));
///     }
/// 
///     filters[0].Item4 = filters[1].Item4 = filters[2].Item4 = metrics;
/// 
///     // Setup images (sources)
///     // ## Add images here ##
///     // srcs.Add(new RowImage(new WeakImage( ... )))
/// 
///     // Create batch
///     BatchFilter batch = new BatchFilter(nodes , srcs, filters);
/// 
///     // Add handlers
///     // * you need to add these handlers so you can be notified with
///     // * information about the execution of each filter
/// 
///     if (notifyOnlyExecution)
///     {
///         // Add handler to the batch.FilterExecuted event
///         batch.FilterExecuted += new FilterExecutedDelegate(batch_FilterExecuted);
///     }
///     else
///     {
///         // Add handler to the batch.FilterMeasured event
///         batch.FilterMeasured += new FilterMeasuredDelegate(batch_FilterMeasured);
///     }
/// 
///     // Add handler to the batch.BatchFinished event
///     batch.BatchFinished += new BatchFinishedDelegate(batch_BatchFinished);
/// 
///     // Run
///     // * you may pause at any time by calling batch.Pause() 
///     // * or abort by calling batch.Stop()
///     batch.Start();
///     
///     // ... batch execution is in a separate thread ...
/// }
/// 
/// // Because the events are raised in another thread, you need to use these handlers
/// // only as wrappers to an invocation of another method so you can access the controls
/// // in a System.Windows.Forms.Form
/// 
/// // Handlers
/// 
/// private void batch_FilterExecuted(BatchFilter sender, WeakImage input, WeakImage output, Filter filter, SortedDictionary<string, object> configs, TimeSpan duration)
/// {
///     FilterExecutedDelegate inv = new FilterExecutedDelegate(FilterExecuted);
///     this.Invoke(inv, sender, input, output, filter, configs, duration);
/// }
/// 
/// private void batch_FilterMeasured(BatchFilter sender, WeakImage input, WeakImage output, Filter filter, SortedDictionary<string, object> configs, List<MetricResult> measures, TimeSpan duration)
/// {
///     FilterMeasuredDelegate inv = new FilterMeasuredDelegate(FilterMeasured);
///     this.Invoke(inv, sender, input, output, filter, configs, measures, duration);
/// }
/// 
/// private void batch_BatchFinished(BatchFilter sender)
/// {
///     BatchFinishedDelegate inv = new BatchFinishedDelegate(BatchFinished);
///     this.Invoke(inv, sender);
/// }
/// 
/// // Actual methods
/// 
/// private void FilterExecuted(BatchFilter sender, WeakImage input, WeakImage output, Filter filter, SortedDictionary<string, object> configs, TimeSpan duration)
/// {
///     Monitor.TryEnter(filterExec_lock, -1);
///     
///     // ... Add notification logic here ...
///     
///     Monitor.Exit(filterExec_lock);
/// }
/// 
/// private void FilterMeasured(BatchFilter sender, WeakImage input, WeakImage output, Filter filter, SortedDictionary<string, object> configs, List<MetricResult> measures, TimeSpan duration)
/// {
///     Monitor.TryEnter(filterMeas_lock, -1);
///     
///     // ... Add notification logic here ...
///     
///     Monitor.Exit(filterMeas_lock);
/// }
/// 
/// private void BatchFinished(BatchFilter sender)
/// {
///  
///     // ... Add notification logic here ...
///     
/// }
/// 
/// </code>
/// </example>
namespace Framework.Batch
{ }
