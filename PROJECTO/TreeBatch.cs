using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

using FilterMeasuredDelegate = Framework.Batch.BatchFilter.FilterMeasuredDelegate;
using FilterExecutedDelegate = Framework.Batch.BatchFilter.FilterExecutedDelegate;
using BatchFinishedDelegate = Framework.Batch.BatchFilter.BatchFinishedDelegate;
using Framework.Core.Filters.Base;
using Framework.Batch;
using Framework.Core.Metrics;
using Framework.Core;

namespace PROJECTO
{
    public partial class TreeBatch : Form
    {

        #region Attributes

        private static object _transferArea;

        private BatchFilter _bt;

        private int _totFiltros;
        private int _executedFiltros;

        #region DataSources

        private BindingList<RowNode> _nodes;

        private BindingList<RowBatchFilter> _filters;

        private BindingList<RowImage> _references;
        private BindingList<RowImage> _images;
        private BindingList<RowResults> _results;

        #endregion

        #endregion

        #region Constructors

        public TreeBatch()
        {
            InitializeComponent();
            // --
            try
            {

                _nodes = new BindingList<RowNode>();
                _filters = new BindingList<RowBatchFilter>();
                _references = new BindingList<RowImage>();
                _images = new BindingList<RowImage>();
                _results = new BindingList<RowResults>();
                //--
                gridAlgorithms.AutoGenerateColumns = false;
                gridImages.AutoGenerateColumns = false;
                gridReferences.AutoGenerateColumns = false;
                gridResults.AutoGenerateColumns = false;
                //--
                gridReferences.DataSource = _references;
                gridImages.DataSource = _images;
                gridResults.DataSource = _results;
                //--
                txDir.Text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "PROJECTO", DateTime.Now.ToString("Exec_yyyyMMddHHmmssfff"));
                autoSaveFolderDialog.SelectedPath = txDir.Text;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        #endregion

        #region Properties

        public RowNode SelectedRowNode
        {
            get
            {
                try
                {
                    if (null != mainTree.SelectedNode && null != mainTree.SelectedNode.Tag)
                    {
                        Guid guid = (Guid)mainTree.SelectedNode.Tag;

                        return _nodes.Where(r => r.Item1 == guid).FirstOrDefault();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.GetType().FullName, ex.Message);
                }

                return null;
            }
            set
            {
                if (null == value)
                    return;

                try
                {
                    mainTree.SelectedNode = GetNodeByGuid(mainTree.Nodes["root"], value.Item1);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.GetType().FullName, ex.Message);
                }

            }
        }

        private TreeNode GetNodeByGuid(TreeNode root, Guid guid)
        {
            try
            {
                if (null != root)
                {
                    if (null != root.Tag)
                    {
                        if ((Guid)root.Tag == guid)
                        {
                            return root;
                        }
                    }

                    if (null != root.Nodes && root.Nodes.Count > 0)
                    {
                        TreeNode tmp = null;
                        foreach (TreeNode child in root.Nodes)
                        {
                            tmp = GetNodeByGuid(child, guid);
                            if (null != tmp)
                                return tmp;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }

            return null;
        }

        public new Form MdiParent
        {
            // Keyword new to hide property MdiParent of Form (base)
            get
            {
                return base.MdiParent;
            }
            set
            {
                try
                {
                    MainForm father = value as MainForm;
                    if (null != father)
                    {
                        selectReferences.Filter = father.ImageDialogFilter;
                        selectImages.Filter = father.ImageDialogFilter;
                    }
                    base.MdiParent = value;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.GetType().FullName, ex.Message);
                }

            }
        }

        #endregion

        #region Methods

        #region Nodes
        // User interaction with the TreeView

        public RowNode GenerateNewNode(Guid pai, out Guid guid, out string text)
        {
            guid = Guid.NewGuid();
            text = "<Execution>";

            try
            {
                return new RowNode(guid, text, pai);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }

            return null;
        }

        public TreeNode AddNode(ref TreeNode parent)
        {
            try
            {
                if (null != parent)
                {
                    Guid g; string s;

                    RowNode toAdd = GenerateNewNode((Guid)parent.Tag, out g, out s);
                    _nodes.Add(toAdd);

                    TreeNode node = parent.Nodes.Add(s);
                    node.ContextMenuStrip = treeContextMenu;
                    node.Tag = g;

                    return node;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }

            return null;
        }

        public void ChangeText(ref TreeNode node, string text)
        {
            try
            {
                node.Text = text;

                Guid g = (Guid)node.Tag;
                Guid pai = (Guid)node.Parent.Tag;

                _nodes[_nodes.IndexOf(_nodes.Where(r => r.Item1 == g).First())] = new RowNode(g, text, pai);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        public void RemoveNode(ref TreeNode node)
        {
            try
            {
                // Erase all nodes descendent from this one

                TreeNode child;
                while (node.Nodes.Count > 0)
                {
                    child = node.Nodes[0];

                    RemoveNode(ref child);
                }

                Guid node_tag = (Guid)node.Tag;

                // Clean nodes
                _nodes.Where(n => n.Item1 == node_tag).ToList().ForEach(delegate(RowNode r) { _nodes.Remove(r); });

                // Clean algorithms
                _filters.Where(a => a.Item5 == node_tag).ToList().ForEach(delegate(RowBatchFilter a) { _filters.Remove(a); });


                // Remove
                node.Remove();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        #endregion

        #region Algorithms
        // User interaction with the AlgorithmsGrid

        private void OpenHashConfigCurrentRow()
        {
            try
            {
                if (null != gridAlgorithms.CurrentRow)
                {
                    RowBatchFilter row = (RowBatchFilter)gridAlgorithms.CurrentRow.DataBoundItem;

                    if (null != row)
                    {
                        SortedDictionary<string, object> hash = row.Item2;
                        ConfigurationsForm conf = new ConfigurationsForm(ref hash);
                        conf.ShowDialog(this);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        private void OpenAlgorithmCurrentRow()
        {
            try
            {
                if (null != gridAlgorithms.SelectedRows && gridAlgorithms.SelectedRows.Count > 0)
                {
                    AlgorithmsForm frm = new AlgorithmsForm(false);
                    frm.ShowDialog(this);
                    if (frm.DialogResult == DialogResult.OK)
                    {
                        Filter[] selection = frm.Selection;

                        if (null != selection)
                        {
                            BindingList<RowBatchFilter> datasource = (BindingList<RowBatchFilter>)gridAlgorithms.DataSource;
                            RowBatchFilter binded;

                            foreach (DataGridViewRow r in gridAlgorithms.SelectedRows)
                            {
                                binded = (RowBatchFilter)r.DataBoundItem;
                                datasource[r.Index] = new RowBatchFilter(
                                        selection[0].Attributes["ShortName"].ToString(),
                                        ((FilterCore)Activator.CreateInstance(selection[0].FilterType)).GetDefaultConfigs(),
                                        selection[0], binded.Item4, binded.Item5);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        private void OpenMeasurementCurrentRow()
        {
            try
            {
                if (null != gridAlgorithms.CurrentRow)
                {
                    RowBatchFilter row = (RowBatchFilter)gridAlgorithms.CurrentRow.DataBoundItem;
                    if (null != row)
                    {

                        MeasuresEditForm sel = new MeasuresEditForm(row.Item3, ReferencesToArray(), row.Item4);
                        sel.ShowDialog(this);
                        if (sel.DialogResult == System.Windows.Forms.DialogResult.OK)
                        {
                            List<MetricExecBase> ex = sel.Execs;

                            BindingList<RowBatchFilter> datasource = (BindingList<RowBatchFilter>)gridAlgorithms.DataSource;

                            datasource[gridAlgorithms.CurrentRow.Index] = new RowBatchFilter(
                                    row.Item1, row.Item2, row.Item3, ex, row.Item5);

                        }
                        sel.Dispose();

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        #endregion

        #region Base

        public void CopyInto(TreeNode source, TreeNode destiny)
        {
            try
            {

                // Do not let copy paste into itself.
                if (object.Equals(source, destiny))
                {
                    MessageBox.Show("Destination cant be Source.");
                    return;
                }

                // Do not let copy paste if destiny is descent of source
                if (GetNodeByGuid(source, (Guid)destiny.Tag) != null)
                {
                    MessageBox.Show("Destination cant be contained inside Source.");
                    return;
                }

                TreeNode currentRoot = AddNode(ref destiny);
                Guid currentGuid = (Guid)currentRoot.Tag;
                Guid sourceGuid = (Guid)source.Tag;

                // Start walking through source, copying algorithms ...
                _filters.Where(a => a.Item5 == sourceGuid).Select(
                        r => new RowBatchFilter(
                            r.Item1,
                            r.Item2 == null ? null : new SortedDictionary<string, object>(r.Item2),
                            r.Item3,
                            r.Item4 == null ? null : new List<MetricExecBase>(r.Item4),
                            currentGuid)).ToList().ForEach(
                            delegate(RowBatchFilter r)
                            {
                                _filters.Add(r);
                            });

                if (null != source.Nodes && source.Nodes.Count > 0)
                {
                    foreach (TreeNode child in source.Nodes)
                    {
                        CopyInto(child, currentRoot);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        public List<RowNodeRelation> TreeViewToRowNodeRelation()
        {
            List<RowNodeRelation> ret = new List<RowNodeRelation>();

            try
            {

                TreeNode root = mainTree.Nodes["root"];

                TreeViewToRowNodeRelation(root, ref ret);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }


            return ret;
        }

        public void TreeViewToRowNodeRelation(TreeNode node, ref List<RowNodeRelation> list)
        {
            try
            {
                if (null == node || null == list)
                    return;

                if (null != node.Nodes && node.Nodes.Count > 0)
                {
                    foreach (TreeNode child in node.Nodes)
                    {
                        list.Add(new RowNodeRelation((Guid)node.Tag, (Guid)child.Tag));

                        TreeViewToRowNodeRelation(child, ref list);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        public string DictionaryToString(SortedDictionary<string, object> dictionary)
        {
            if (dictionary == null)
                return "";

            StringBuilder buffer = new StringBuilder();
            try
            {
                buffer.Append("{");

                if (dictionary == null || dictionary.Count == 0)
                    buffer.Append(" ");
                else
                {
                    buffer.Append(string.Join("; ", dictionary.Select(e => string.Format("{0} = {1}", e.Key, e.Value)).ToArray()));
                }

                buffer.Append("}");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }

            return buffer.ToString();
        }

        private WeakImage[] ReferencesToArray()
        {
            List<WeakImage> ls_img = new List<WeakImage>();

            if (_references != null)
            {
                foreach (RowImage r in _references)
                {
                    if (r != null)
                    {
                        ls_img.Add(r.Item1);
                    }
                }
            }
            return ls_img.ToArray();
        }

        public DataTable RowResultsToDatatable(IList<RowResults> results, bool toXml)
        {
            DataTable dt = null;

            try
            {
                dt = new DataTable("Details");

                if (toXml)
                {
                    dt.Columns.AddRange(new DataColumn[] { 
                        new DataColumn("Algorithm", typeof(string)),
                        new DataColumn("Configs", typeof(string)),
                        new DataColumn("ImageSrc", typeof(string)),
                        new DataColumn("ImageDest", typeof(string)),
                        new DataColumn("ImageRef", typeof(string)),
                        new DataColumn("Measure", typeof(string)),
                        new DataColumn("Value", typeof(double)),
                        new DataColumn("Duration", typeof(TimeSpan))
                    });
                }
                else
                {
                    dt.Columns.AddRange(new DataColumn[] { 
                        new DataColumn("Algorithm", typeof(string)),
                        new DataColumn("Configs", typeof(string)),
                        new DataColumn("ImageSrc", typeof(Image)),
                        new DataColumn("ImageDest", typeof(Image)),
                        new DataColumn("ImageRef", typeof(Image)),
                        new DataColumn("Measure", typeof(string)),
                        new DataColumn("Value", typeof(double)),
                        new DataColumn("Duration", typeof(TimeSpan))
                    });
                }


                //List<String> colsMeasures = new List<string>();
                //foreach (RowResults r in results)
                //{
                //    foreach (String key in r.Item6.Keys)
                //    {
                //        if (!colsMeasures.Contains(key))
                //            colsMeasures.Add(key);
                //    }
                //}
                //foreach (string key in colsMeasures)
                //{
                //    dt.Columns.Add(new DataColumn(key, typeof(double)));
                //}

                DataRow dr;

                foreach (RowResults r in results)
                {
                    for (int i = 0; i < Math.Max(r.Item4.Count, 1); ++i)
                    {
                        dr = dt.NewRow();

                        dr.SetField<string>("Algorithm", r.Item5.FilterType.FullName);
                        dr.SetField<string>("Configs", DictionaryToString(r.Item6));
                        dr.SetField<TimeSpan>("Duration", r.Item7);

                        if (toXml)
                        {
                            dr.SetField<string>("ImageSrc", Facilities.ToBase64String((Image)r.Item2));
                            dr.SetField<string>("ImageDest", Facilities.ToBase64String((Image)r.Item3));
                        }
                        else
                        {
                            dr.SetField<Image>("ImageSrc", (Image)r.Item2);
                            dr.SetField<Image>("ImageDest", (Image)r.Item3);
                        }


                        if (i < r.Item4.Count)
                        {
                            if (toXml)
                            {
                                dr.SetField<string>("ImageRef", Facilities.ToBase64String((Image)r.Item4[i].Reference));
                            }
                            else
                            {
                                dr.SetField<Image>("ImageRef", (Image)r.Item4[i].Reference);
                            }

                            dr.SetField<string>("Measure", r.Item4[i].Key);
                            dr.SetField<double>("Value", r.Item4[i].Value);
                        }

                        dt.Rows.Add(dr);
                    }
                }

                // Open new form ...

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }

            return dt;
        }

        private DataTable RowResultsToDatatable2(IList<RowResults> results, bool toXml)
        {
            DataTable dt = null;
            try
            {
                dt = new DataTable("Details2");

                if (toXml)
                {
                    dt.Columns.AddRange(new DataColumn[] { 
                        new DataColumn("Algorithm", typeof(string)),
                        new DataColumn("Configs", typeof(string)),
                        new DataColumn("ImageSrc", typeof(string)),
                        new DataColumn("ImageDest", typeof(string)),
                        new DataColumn("Duration", typeof(TimeSpan))
                    });
                }
                else
                {
                    dt.Columns.AddRange(new DataColumn[] { 
                        new DataColumn("Algorithm", typeof(string)),
                        new DataColumn("Configs", typeof(string)),
                        new DataColumn("ImageSrc", typeof(Image)),
                        new DataColumn("ImageDest", typeof(Image)),
                        new DataColumn("Duration", typeof(TimeSpan))
                    });
                }

                List<String> colsMeasures = new List<string>(); string k;

                foreach (RowResults r in results)
                {
                    if (null != r.Item4)
                    {
                        foreach (MetricResult mr in r.Item4)
                        {
                            k = mr.Key;
                            if (!colsMeasures.Contains(k))
                                colsMeasures.Add(k);
                        }
                    }

                }

                foreach (string key in colsMeasures)
                {
                    dt.Columns.Add(new DataColumn(key, typeof(double)));
                }

                DataRow dr;

                foreach (RowResults r in results)
                {
                    dr = dt.NewRow();

                    dr.SetField<string>("Algorithm", r.Item5.FilterType.FullName);
                    dr.SetField<string>("Configs", DictionaryToString(r.Item6));
                    dr.SetField<TimeSpan>("Duration", r.Item7);

                    if (toXml)
                    {
                        dr.SetField<string>("ImageSrc", Facilities.ToBase64String((Image)r.Item2));
                        dr.SetField<string>("ImageDest", Facilities.ToBase64String((Image)r.Item3));
                    }
                    else
                    {
                        dr.SetField<Image>("ImageSrc", (Image)r.Item2);
                        dr.SetField<Image>("ImageDest", (Image)r.Item3);
                    }
                    if (null != r.Item4)
                    {
                        for (int i = 0; i < r.Item4.Count; ++i)
                        {
                            if (dt.Columns.Contains(r.Item4[i].Key))
                            {
                                dr.SetField<double>(r.Item4[i].Key, r.Item4[i].Value);
                            }
                        }
                    }
                    dt.Rows.Add(dr);
                }

                // Open new form ...
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }

            return dt;
        }

        private void AddNodesToTreeView(List<RowNode> nodes)
        {
            try
            {
                AddNodesToTreeView(nodes, mainTree.Nodes["root"]);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        private void AddNodesToTreeView(List<RowNode> nodes, TreeNode node)
        {
            try
            {
                IEnumerable<RowNode> q = from n in _nodes
                                         where n.Item3 == (Guid)node.Tag
                                         select n;

                foreach (RowNode r in q)
                {
                    TreeNode n = node.Nodes.Add(r.Item2);
                    n.Tag = r.Item1;
                    n.ContextMenuStrip = treeContextMenu;
                    AddNodesToTreeView(nodes, n);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        public void Flush(bool isEnd, bool clear)
        {
            try
            {
                if (ckAutoSave.Checked)
                {
                    if (!Directory.Exists(txDir.Text))
                    {
                        Directory.CreateDirectory(txDir.Text);
                    }

                    if ((ckEnd.Checked && isEnd) || (!ckEnd.Checked && _results.Count > (int)(numericVal.Value)))
                    {
                        string filenameBase = DateTime.Now.ToString("yyyyMMddHHmmssfff");

                        // Saves...
                        if (ckDetails.Checked)
                        {
                            DataTable dt = RowResultsToDatatable(_results, true);
                            dt.WriteXml(Path.Combine(txDir.Text, filenameBase + ".details.xml"), true);
                        }
                        if (ckDetails2.Checked)
                        {
                            DataTable dt = RowResultsToDatatable2(_results, true);
                            dt.WriteXml(Path.Combine(txDir.Text, filenameBase + ".details2.xml"), true);
                        }
                        if (ckCsv.Checked)
                        {
                            RowResults.ExportToCsv(Path.Combine(txDir.Text, filenameBase + ".csv"), _results.ToList());
                        }
                        if (ckXml.Checked)
                        {
                            RowResults.ExportToXml(Path.Combine(txDir.Text, filenameBase + ".xml"), _results.ToList());
                        }
                        if (ckHtml.Checked)
                        {
                            ExportResultsToHtml(Path.Combine(txDir.Text, filenameBase + "_images"), _results.ToList());
                        }
                        if (ckReset.Checked && clear)
                        {
                            _results.Clear();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        #endregion

        #region File
        // Load / Save Batch File

        public void LoadFile(string fname)
        {
            try
            {
                // Clean
                mainTree.Nodes["root"].Nodes.Clear();


                List<RowNode> tmp_nodes = null;
                List<RowBatchFilter> tmp_filters = null;
                List<RowImage> tmp_images = null;
                List<RowImage> tmp_references = null;

                BatchFilter.Load(fname, out tmp_nodes, out tmp_filters, out tmp_images, out tmp_references);

                _nodes = new BindingList<RowNode>(tmp_nodes);

                _filters = new BindingList<RowBatchFilter>(tmp_filters);

                _references = new BindingList<RowImage>(tmp_references);
                gridReferences.DataSource = _references;

                _images = new BindingList<RowImage>(tmp_images);
                gridImages.DataSource = _images;

                _results = new BindingList<RowResults>();
                gridResults.DataSource = _results;

                AddNodesToTreeView(tmp_nodes);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().FullName);
            }
        }

        public void SaveFile(string fname)
        {
            try
            {
                // Force save of algorithms
                TreeNode selection = mainTree.SelectedNode;
                mainTree.SelectedNode = mainTree.Nodes["root"];
                mainTree.SelectedNode = selection;

                BatchFilter.Save(fname, _nodes.ToList(), _filters.ToList(), _images.ToList(), _references.ToList());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().FullName);
            }
        }

        #endregion

        #region Exports
        // Exports implemented outside the framework ...

        private static void ExportResultsToHtml(string baseDir, List<RowResults> results)
        {
            try
            {
                Directory.CreateDirectory(baseDir);
                string baseName = DateTime.Now.ToString("yyyyMMdd_HHmmssfff");

                string INDEX_FILE = baseName + "_index.html";
                string IMG_FOLDER = baseName + "img";
                string TH_FOLDER = baseName + "thumbs";

                string directory_img = Path.Combine(baseDir, IMG_FOLDER);
                string directory_thumbs = Path.Combine(baseDir, TH_FOLDER);

                Directory.CreateDirectory(directory_img);
                Directory.CreateDirectory(directory_thumbs);

                // Dummy and nasty html code ... as long as it works ...
                StringBuilder dummyHtml = new StringBuilder();

                //prolog
                dummyHtml.Append("<html><head></head><body><table border='1'>");

                dummyHtml.Append("<tr>");
                dummyHtml.Append("<th>FilterName</th>");
                dummyHtml.Append("<th>Parameters</th>");
                dummyHtml.Append("<th>Filter-Chain</th>");
                dummyHtml.Append("<th>Image</th>");
                dummyHtml.Append("<th>Thumbnail</th>");
                dummyHtml.Append("</tr>");
                // content
                foreach (RowResults r in results)
                {
                    string filter = "", parameters = "", file_img = "";
                    Image img = null;

                    Facilities.FilterExecToString(r.Item5.FilterType, r.Item6, out filter, out parameters);

                    dummyHtml.Append("<tr>");
                    dummyHtml.Append("<td>");
                    dummyHtml.Append(filter);
                    dummyHtml.Append("</td>");

                    dummyHtml.Append("<td>");
                    dummyHtml.Append(parameters);
                    dummyHtml.Append("</td>");

                    dummyHtml.Append("<td>");

                    string sChain = "";
                    Image theImage = r.Item2.Image;
                    ArrayList bucket = Facilities.GetBucket<ArrayList>(
                                        ref theImage, Facilities.EXECUTED_FILTERS);

                    if (bucket != null)
                    {
                        foreach (object o in bucket)
                        {
                            sChain += (string.IsNullOrEmpty(sChain) ? "" : " ; ") + o.ToString();
                        }
                    }
                    dummyHtml.Append(sChain);
                    dummyHtml.Append("</td>");

                    if (r.Item3 != null)
                        img = r.Item3.Image;

                    dummyHtml.Append("<td>");

                    if (img == null)
                    {
                        dummyHtml.Append("(Erro!)");
                        dummyHtml.Append("</td>");
                        dummyHtml.Append("<td>");
                        dummyHtml.Append("(Erro!)");
                        dummyHtml.Append("</td>");
                    }
                    else
                    {
                        dummyHtml.Append("<a href='");

                        file_img = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ".bmp";

                        // Reduce width and height to a maximum of 75x75
                        // Without changing image width / height ratio
                        Image.GetThumbnailImageAbort nullCallback = delegate()
                        {
                            return true;
                        };

                        int newWidth = 0, newHeight = 0;

                        double destiny = 75 / Math.Max(75.0, Math.Max(img.Width, img.Height));

                        newWidth = (int)(destiny * img.Width);
                        newHeight = (int)(destiny * img.Height);

                        img.Save(Path.Combine(directory_img, file_img), img.RawFormat);
                        img.GetThumbnailImage(newWidth, newHeight, nullCallback, IntPtr.Zero).Save(
                                Path.Combine(directory_thumbs, file_img), img.RawFormat);

                        // use relative path here
                        dummyHtml.Append(IMG_FOLDER).Append("/").Append(file_img);
                        dummyHtml.Append("'>");
                        dummyHtml.Append("(Click to Open)");
                        dummyHtml.Append("</a>");
                        dummyHtml.Append("</td>");
                        dummyHtml.Append("<td>");
                        // use relative path here
                        dummyHtml.Append("<img src='").Append(TH_FOLDER).Append("/").Append(file_img).Append("'>");
                        dummyHtml.Append("</td>");
                    }


                    dummyHtml.Append("</tr>");
                }

                //epilog
                dummyHtml.Append("</table></body></html>");

                File.WriteAllText(Path.Combine(baseDir, INDEX_FILE), dummyHtml.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        #endregion

        #endregion

        #region Events

        #region TreeView

        private void mainTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                RowNode selectedRowNode = SelectedRowNode;

                splitContainer2.Panel1.Enabled = selectedRowNode != null;

                // Save changes
                BindingList<RowBatchFilter> datasource = (BindingList<RowBatchFilter>)gridAlgorithms.DataSource;

                if (datasource != null && datasource.Count > 0)
                {
                    Guid before = datasource.FirstOrDefault().Item5;

                    // "save" changes ...
                    // remove from original all from selected, re-insert ...
                    IEnumerable<RowBatchFilter> tmp1 = _filters.Where(a => a.Item5 != before);

                    tmp1 = tmp1.Concat(datasource);

                    _filters = new BindingList<RowBatchFilter>(tmp1.ToList());
                }

                // update filter ...
                if (null == selectedRowNode)
                {
                    // no filter
                    gridAlgorithms.DataSource = null;
                }
                else
                {
                    // selected is the new selection
                    Guid selected = selectedRowNode.Item1;

                    // filter
                    if (null != selected)
                    {
                        gridAlgorithms.DataSource = new BindingList<RowBatchFilter>(
                                _filters.Where(r => r.Item5 == selected).ToList());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        #endregion

        #region treeContextMenu

        private void addSiblingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (null == sender)
                    return;

                ToolStripMenuItem wrkSender = (ToolStripMenuItem)sender;

                if (null == wrkSender.Owner)
                    return;

                ContextMenuStrip mnu = (ContextMenuStrip)wrkSender.Owner;

                if (null == mnu.SourceControl)
                    return;

                TreeView view = (TreeView)mnu.SourceControl;

                if (null == view.SelectedNode)
                    return;
                if (null == view.SelectedNode.Parent)
                    return;

                TreeNode parent = view.SelectedNode.Parent;

                view.SelectedNode = AddNode(ref parent);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        private void addChildToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (null == sender)
                    return;

                ToolStripMenuItem wrkSender = (ToolStripMenuItem)sender;

                if (null == wrkSender.Owner)
                    return;

                ContextMenuStrip mnu = (ContextMenuStrip)wrkSender.Owner;

                if (null == mnu.SourceControl)
                    return;

                TreeView view = (TreeView)mnu.SourceControl;

                if (null == view.SelectedNode)
                    return;

                TreeNode parent = view.SelectedNode;

                view.SelectedNode = AddNode(ref parent);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                _transferArea = (object)mainTree.SelectedNode;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (null != _transferArea)
                {
                    TreeNode selected = mainTree.SelectedNode;
                    mainTree.SelectedNode = mainTree.Nodes["root"];

                    CopyInto((TreeNode)_transferArea, selected);

                    mainTree.SelectedNode = selected;

                    _transferArea = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        private void removeNodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (null == sender)
                    return;

                ToolStripMenuItem wrkSender = (ToolStripMenuItem)sender;

                if (null == wrkSender.Owner)
                    return;

                ContextMenuStrip mnu = (ContextMenuStrip)wrkSender.Owner;

                if (null == mnu.SourceControl)
                    return;

                TreeView view = (TreeView)mnu.SourceControl;

                if (null == view.SelectedNode)
                    return;

                TreeNode selected = view.SelectedNode;
                view.SelectedNode = view.Nodes["root"];

                if (selected != mainTree.Nodes["root"])
                {
                    RemoveNode(ref selected);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        private void changeTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                TreeNode node = mainTree.SelectedNode;

                string newValue = InputBox.AskValue(this, value: node.Text);

                if (!string.IsNullOrEmpty(newValue))
                {
                    node.Text = newValue;
                    RowNode rem = _nodes.Where(n => n.Item1 == (Guid)node.Tag).FirstOrDefault();
                    if (null != rem)
                    {
                        _nodes.Remove(rem);
                        _nodes.Add(new RowNode(rem.Item1, newValue, rem.Item3));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        #endregion

        #region Form

        private void TreeBatch_Load(object sender, EventArgs e)
        {
            try
            {
                mainTree.Nodes["root"].ContextMenuStrip = treeContextMenu;
                TreeNode root = mainTree.Nodes["root"];
                root.Tag = Guid.Empty;

                AddNode(ref root);

                mainTree.Nodes["root"].ExpandAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        #endregion

        #region Execution Handlers
        // The handler invokes a method in the form's thread, so it can interact directly
        // with the form's controls 

        // EXECUTED

        private void bt_FilterExecuted(BatchFilter sender, WeakImage input, WeakImage output, Filter filter, SortedDictionary<string, object> configs, TimeSpan duration)
        {
            try
            {
                FilterExecutedDelegate inv = new FilterExecutedDelegate(bt_FilterExecuted_threadsafe_);
                this.Invoke(inv, sender, input, output, filter, configs, duration);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        private object filterExec_lock = new object();
        private void bt_FilterExecuted_threadsafe_(BatchFilter sender, WeakImage input, WeakImage output, Filter filter, SortedDictionary<string, object> configs, TimeSpan duration)
        {
            try
            {
                Monitor.TryEnter(filterExec_lock, -1);


                ++_executedFiltros;
                lbXofY.Text = string.Format("{0}/{1}", _executedFiltros, _totFiltros);
                progressExec.Value = (int)(Math.Min(1.0, _executedFiltros / (_totFiltros * 1.0)) * progressExec.Maximum);


                // Add row to GridResults ...
                _results.Add(new RowResults(filter.Attributes["ShortName"].ToString(), input, output, null, filter, configs, duration));

                Flush(false, true);

                gridResults.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
            finally
            {
                Monitor.Exit(filterExec_lock);
            }
        }

        // MEASURED

        private void bt_FilterMeasured(BatchFilter sender, WeakImage input, WeakImage output, Filter filter, SortedDictionary<string, object> configs, List<MetricResult> measures, TimeSpan duration)
        {
            try
            {
                FilterMeasuredDelegate inv = new FilterMeasuredDelegate(bt_FilterMeasured_threadsafe_);
                this.Invoke(inv, sender, input, output, filter, configs, measures, duration);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        private object filterMeas_lock = new object();
        private void bt_FilterMeasured_threadsafe_(BatchFilter sender, WeakImage input, WeakImage output, Filter filter, SortedDictionary<string, object> configs, List<MetricResult> measures, TimeSpan duration)
        {

            try
            {
                Monitor.TryEnter(filterMeas_lock, -1);

                ++_executedFiltros;
                lbXofY.Text = string.Format("{0}/{1}", _executedFiltros, _totFiltros);
                progressExec.Value = (int)(Math.Min(1.0, _executedFiltros / (_totFiltros * 1.0)) * progressExec.Maximum);


                if (ckForgetNoMeasures.Checked)
                {
                    // Forget the row
                    if (measures == null || measures.Count == 0)
                        return;
                }

                // Add row to GridResults ...
                _results.Add(new RowResults(filter.Attributes["ShortName"].ToString(), input, output, measures, filter, configs, duration));

                Flush(false, true);

                gridResults.Refresh();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
            finally
            {
                Monitor.Exit(filterMeas_lock);
            }
        }

        // FINISHED

        private void bt_BatchFinished(BatchFilter sender)
        {
            try
            {
                BatchFinishedDelegate inv = new BatchFinishedDelegate(bt_BatchFinished_threadsafe_);
                this.Invoke(inv, sender);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        private void bt_BatchFinished_threadsafe_(BatchFilter sender)
        {
            try
            {
                Flush(true, false);
                MessageBox.Show(this, "Finished!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        #endregion

        #region Execution ToolStrip

        private void mnuStart_Click(object sender, EventArgs e)
        {
            try
            {
                if (_results != null && _results.Count > 0)
                {

                    switch (MessageBox.Show("Clean current results?", "Question", MessageBoxButtons.YesNoCancel))
                    {
                        case System.Windows.Forms.DialogResult.Yes:
                            _results.Clear();
                            break;
                        case System.Windows.Forms.DialogResult.No:

                            break;
                        case System.Windows.Forms.DialogResult.Cancel:
                            return;
                    }
                }

                // Start batch
                _bt = new BatchFilter(TreeViewToRowNodeRelation(), _images.ToList(), _filters.ToList());

                if (rdAfterExecMeas.Checked)
                {
                    _bt.FilterMeasured += new FilterMeasuredDelegate(bt_FilterMeasured);
                }
                if (rdAfterExec.Checked)
                {
                    _bt.FilterExecuted += new FilterExecutedDelegate(bt_FilterExecuted);
                }
                _bt.BatchFinished += new BatchFinishedDelegate(bt_BatchFinished);
                // --

                _totFiltros = _bt.CountFilters;
                if (_totFiltros == 0)
                {
                    MessageBox.Show("No filters to execute ...");
                    return;
                }

                _executedFiltros = 0;
                lbXofY.Text = string.Format("{0}/{1}", _executedFiltros, _totFiltros);
                progressExec.Value = 0;

                // --
                _bt.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        private void mnuPause_Click(object sender, EventArgs e)
        {
            try
            {
                if (null != _bt)
                    // Pause batch
                    _bt.Pause();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        private void mnuStop_Click(object sender, EventArgs e)
        {
            try
            {
                if (null != _bt)
                    // Stop batch
                    _bt.Stop();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        #endregion

        #region File ToolStrip

        private void mnuNew_Click(object sender, EventArgs e)
        {
            try
            {
                TreeBatch tree = new TreeBatch();
                tree.MdiParent = MdiParent;
                tree.Show();
                tree.WindowState = FormWindowState.Maximized;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        private void mnuOpen_Click(object sender, EventArgs e)
        {
            try
            {
                openDialog.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        private void mnuSave_Click(object sender, EventArgs e)
        {
            try
            {
                saveFileDialog1.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        #endregion

        #region Algorithms ToolStrip

        private void btAddAlgorithms_Click(object sender, EventArgs e)
        {
            try
            {
                AlgorithmsForm frm = new AlgorithmsForm(true);
                frm.ShowDialog(this);
                if (frm.DialogResult == DialogResult.OK)
                {
                    Filter[] selection = frm.Selection;

                    BindingList<RowBatchFilter> datasource = (BindingList<RowBatchFilter>)gridAlgorithms.DataSource;

                    if (null != selection)
                    {
                        List<MetricExecBase> metrics;

                        foreach (Filter f in selection)
                        {
                            // Generate metrics by default!
                            metrics = null;

                            Metric m = Factory.GetMetricsFromFilter(f.FilterType);

                            if (null != m)
                            {
                                metrics = new List<MetricExecBase>();

                                SortedDictionary<string, Metric.MetricDelegate> ms = m.GetListMetrics();

                                // brute-force adding
                                foreach (string key in ms.Keys)
                                {
                                    // input - output
                                    metrics.Add(new MetricExec(key, ms[key]));

                                    // reference 
                                    foreach (RowImage r in _references)
                                    {
                                        metrics.Add(new MetricExecReference(key, ms[key], r.Item1));
                                    }
                                }

                            }

                            datasource.Add(new RowBatchFilter(
                                    f.Attributes["ShortName"].ToString(),
                                    ((FilterCore)Activator.CreateInstance(f.FilterType)).GetDefaultConfigs(),
                                    f,
                                    metrics,
                                    SelectedRowNode.Item1));
                        }
                    }
                }
                gridAlgorithms.Refresh();
                frm.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        private void btCleanMetrics_Click(object sender, EventArgs e)
        {
            try
            {
                // Clean metrics for selected rows
                DataGridViewSelectedRowCollection selection = gridAlgorithms.SelectedRows;

                if (selection != null)
                {
                    foreach (DataGridViewRow r in selection)
                    {
                        RowBatchFilter row = (RowBatchFilter)r.DataBoundItem;

                        if (row != null)
                        {
                            // new clean metrics row ...
                            row.Item4 = new List<MetricExecBase>();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        private void btConfigureHash_Click(object sender, EventArgs e)
        {
            try
            {
                OpenHashConfigCurrentRow();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        private void btReplicate_Click(object sender, EventArgs e)
        {
            try
            {
                if (null != gridAlgorithms.SelectedRows && gridAlgorithms.SelectedRows.Count > 0)
                {
                    ReplicateForm repl = new ReplicateForm();
                    repl.ShowDialog(this);
                    if (repl.DialogResult == System.Windows.Forms.DialogResult.OK)
                    {
                        decimal val_orig = repl.Selection;
                        foreach (DataGridViewRow r in gridAlgorithms.SelectedRows)
                        {
                            RowBatchFilter template = (RowBatchFilter)r.DataBoundItem;
                            int i = gridAlgorithms.CurrentRow.Index + 1;
                            SortedDictionary<string, object> sorted_dict_copy;
                            decimal val = val_orig;

                            while (val-- > 0)
                            {
                                sorted_dict_copy = new SortedDictionary<string, object>(template.Item2);

                                ((BindingList<RowBatchFilter>)gridAlgorithms.DataSource).Add(new RowBatchFilter(
                                        template.Item1,
                                        sorted_dict_copy,
                                        template.Item3,
                                        template.Item4 == null ? null : new List<MetricExecBase>(template.Item4),
                                        template.Item5));
                            }
                        }
                    }
                    repl.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        #endregion

        #region References ToolStrip

        private void btOpenFileRefs_Click(object sender, EventArgs e)
        {
            try
            {
                selectReferences.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        #endregion

        #region Images ToolStrip

        private void btOpenFileImages_Click(object sender, EventArgs e)
        {
            try
            {
                selectImages.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        #endregion

        #region Metrics ToolStrip

        private void btSave_Click(object sender, EventArgs e)
        {
            try
            {
                saveResultsDialog.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        private void btSave2_Click(object sender, EventArgs e)
        {
            try
            {
                saveResults2Dialog.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        private void btDetailed_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = RowResultsToDatatable(_results, false);

                DataTableBindForm frm = new DataTableBindForm(dt);
                frm.MdiParent = MdiParent;
                frm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        private void btDetailed2_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = RowResultsToDatatable2(_results, false);

                DataTableBindForm frm = new DataTableBindForm(dt);
                frm.MdiParent = MdiParent;
                frm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        private void btExportToCSV_Click(object sender, EventArgs e)
        {
            try
            {
                exportCsvDialog.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        private void btExportToXML_Click(object sender, EventArgs e)
        {
            try
            {
                exportXmlDialog.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        private void btExportToHTML_Click(object sender, EventArgs e)
        {
            try
            {
                if (_results == null)
                    return;

                if (htmlSaveFolderDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                {
                    string directory = htmlSaveFolderDialog.SelectedPath;

                    if (!string.IsNullOrEmpty(directory))
                    {
                        try
                        {
                            ExportResultsToHtml(directory, _results.ToList());

                            MessageBox.Show(this, "Finished exporting!");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        #endregion

        #region Dialogs

        private void selectReferences_FileOk(object sender, CancelEventArgs e)
        {
            try
            {
                // Insert images' _filename; 
                foreach (string fname in selectReferences.FileNames)
                {
                    if (null == _references.FirstOrDefault(r => 1 == 0))
                    {
                        _references.Add(new RowImage(new WeakImage(Image.FromFile(fname))));
                    }
                }

                gridReferences.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        private void selectImages_FileOk(object sender, CancelEventArgs e)
        {
            try
            {
                // Insert images' _filename; 
                foreach (string fname in selectImages.FileNames)
                {
                    if (null == _images.FirstOrDefault(r => 1 == 0))
                    {
                        _images.Add(new RowImage(new WeakImage(Image.FromFile(fname))));
                    }
                }
                gridImages.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        private void saveResultsDialog_FileOk(object sender, CancelEventArgs e)
        {
            try
            {
                DataTable dt = RowResultsToDatatable(_results, true);
                dt.WriteXml(saveResultsDialog.FileName, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().FullName);
            }
        }

        private void saveResults2Dialog_FileOk(object sender, CancelEventArgs e)
        {
            try
            {
                DataTable dt = RowResultsToDatatable2(_results, true);
                dt.WriteXml(saveResults2Dialog.FileName, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().FullName);
            }
        }

        private void openDialog_FileOk(object sender, CancelEventArgs e)
        {
            try
            {
                LoadFile(openDialog.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            try
            {
                SaveFile(saveFileDialog1.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        private void exportCsvDialog_FileOk(object sender, CancelEventArgs e)
        {
            try
            {
                if (null != _results)
                {
                    try
                    {
                        string filename = ((SaveFileDialog)sender).FileName;
                        if (string.IsNullOrEmpty(filename))
                            return;

                        RowResults.ExportToCsv(filename, _results.ToList());

                        MessageBox.Show(this, "Finished exporting!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, ex.GetType().FullName);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        private void exportXmlDialog_FileOk(object sender, CancelEventArgs e)
        {
            try
            {
                if (null != _results)
                {
                    try
                    {
                        string filename = ((SaveFileDialog)sender).FileName;
                        if (string.IsNullOrEmpty(filename))
                            return;

                        RowResults.ExportToXml(filename, _results.ToList());

                        MessageBox.Show(this, "Finished exporting!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, ex.GetType().FullName);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        #endregion

        #region Grids

        private void gridAlgorithms_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int c_index = Configs.Index;
                switch (e.ColumnIndex)
                {
                    case 0: // Algorithms
                        OpenAlgorithmCurrentRow();

                        break;
                    case 1: // Configs
                        OpenHashConfigCurrentRow();

                        break;

                    case 2:
                        OpenMeasurementCurrentRow();

                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        private void gridImages_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataGridView wrkGrid = (DataGridView)sender;

                if (e.RowIndex < 0 || e.ColumnIndex < 0)
                    return;

                RowImage tuple = (RowImage)wrkGrid.Rows[e.RowIndex].DataBoundItem;
                ImageForm frm;

                if (wrkGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.GetType().Equals(typeof(Image)) ||
                    wrkGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.GetType().Equals(typeof(Bitmap))
                    )
                {
                    frm = new ImageForm((Image)wrkGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                    frm.MdiParent = MdiParent;
                    frm.Show();
                }
                else if (wrkGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.GetType().Equals(typeof(WeakImage)))
                {
                    frm = new ImageForm(((WeakImage)wrkGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value).Image);
                    frm.MdiParent = MdiParent;
                    frm.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        private void gridResults_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0 || e.ColumnIndex < 0)
                    return;

                RowResults tuple = (RowResults)gridResults.Rows[e.RowIndex].DataBoundItem;
                ImageForm frm;

                if (gridResults.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.GetType().Equals(typeof(Image)) ||
                    gridResults.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.GetType().Equals(typeof(Bitmap)))
                {
                    frm = new ImageForm((Image)(gridResults.Rows[e.RowIndex].Cells[e.ColumnIndex].Value));
                    frm.MdiParent = MdiParent;
                    frm.Show();
                }
                else if (gridResults.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.GetType().Equals(typeof(WeakImage)))
                {
                    frm = new ImageForm(((WeakImage)gridResults.Rows[e.RowIndex].Cells[e.ColumnIndex].Value).Image);
                    frm.MdiParent = MdiParent;
                    frm.Show();
                }
                else
                {
                    switch (e.ColumnIndex)
                    {
                        case 3:

                            MeasuresViewerForm m = new MeasuresViewerForm(tuple.Item4);
                            m.MdiParent = MdiParent;
                            m.Show();

                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        #endregion

        #region PanelAutoSavings

        private void ckAutoSave_CheckedChanged(object sender, EventArgs e)
        {
            ckReset.Enabled = ckAutoSave.Checked;

            ckDetails.Enabled = ckAutoSave.Checked;
            ckDetails2.Enabled = ckAutoSave.Checked;
            ckCsv.Enabled = ckAutoSave.Checked;
            ckXml.Enabled = ckAutoSave.Checked;
            ckHtml.Enabled = ckAutoSave.Checked;

            ckEnd.Enabled = ckAutoSave.Checked;
            numericVal.Enabled = ckAutoSave.Checked;

            btDir.Enabled = ckAutoSave.Checked;
            btOpenFolder.Enabled = ckAutoSave.Checked;
        }

        private void btDir_Click(object sender, EventArgs e)
        {
            try
            {
                if (autoSaveFolderDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                {
                    if (Directory.Exists(autoSaveFolderDialog.SelectedPath))
                    {
                        txDir.Text = autoSaveFolderDialog.SelectedPath;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }

        private void numericVal_ValueChanged(object sender, EventArgs e)
        {
            ckEnd.Checked = false;
        }

        #endregion

        private void btOpenFolder_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txDir.Text))
                    System.Diagnostics.Process.Start("explorer", txDir.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName, ex.Message);
            }
        }


        #endregion

    }
}
