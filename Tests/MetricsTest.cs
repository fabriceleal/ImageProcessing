using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using System.Diagnostics;

namespace Tests
{
    using Facilities = Framework.Core.Facilities;

    /// <summary>
    /// Summary description for MetricsTest
    /// </summary>
    [TestClass]
    public class MetricsTest
    {
        // "Well-Known" images, black (0) and white (255)
        // Histograms were calculated using Matlab

        /// <summary>
        /// Histogram (0 = 65024; 255 = 512)
        /// </summary>
        private Image _fullEdges;
        /// <summary>
        /// Histogram (0 = 65279; 255 = 257)
        /// </summary>
        private Image _halfEdges;
        /// <summary>
        /// Histogram (0 = 65016; 255 = 520)
        /// </summary>
        private Image _fullEdgesOuter1;
        /// <summary>
        /// Histogram (0 = 65008; 255 = 528)
        /// </summary>
        private Image _fullEdgesOuter2;
        /// <summary>
        /// Histogram (0 = 65032; 255 = 504)
        /// </summary>
        private Image _fullEdgesInner1;
        /// <summary>
        /// Histogram (0 = 65040; 255 = 496)
        /// </summary>
        private Image _fullEdgesInner2;
        /// <summary>
        /// Histogram (0 = 64796; 255 = 740)
        /// </summary>
        private Image _fullEdgesMore;
        /// <summary>
        /// Histogram (0 = 65156; 255 = 380)
        /// </summary>
        private Image _fullEdgesLess;

        struct PrivateStore
        {
            public string Name; public int Blacks; public int Whites;

            public PrivateStore(string name, int blacks, int whites)
            {
                Name = name; Whites = whites; Blacks = blacks;
            }
        }

        public MetricsTest()
        {
            _fullEdges = (Image)(Properties.Resources.edges_0.Clone());
            _fullEdges.Tag = new PrivateStore("Full Edges", 65024, 512);

            _halfEdges = (Image)(Properties.Resources.edges_1.Clone());
            _halfEdges.Tag = new PrivateStore("Half Edges", 65279, 257);

            _fullEdgesOuter1 = (Image)(Properties.Resources.edges_2.Clone());
            _fullEdgesOuter1.Tag = new PrivateStore("Full Edges Outer 1", 65016, 520);

            _fullEdgesOuter2 = (Image)(Properties.Resources.edges_3.Clone());
            _fullEdgesOuter2.Tag = new PrivateStore("Full Edges Outer 2", 65008, 528);

            _fullEdgesInner1 = (Image)(Properties.Resources.edges_4.Clone());
            _fullEdgesInner1.Tag = new PrivateStore("Full Edges Inner 1", 65032, 504);

            _fullEdgesInner2 = (Image)(Properties.Resources.edges_5.Clone());
            _fullEdgesInner2.Tag = new PrivateStore("Full Edges Inner 2", 65040, 496);

            _fullEdgesMore = (Image)(Properties.Resources.edges_6.Clone());
            _fullEdgesMore.Tag = new PrivateStore("Full Edges More", 64796, 740);

            _fullEdgesLess = (Image)(Properties.Resources.edges_7.Clone());
            _fullEdgesLess.Tag = new PrivateStore("Full Edges Less", 65156, 380);

        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        #region Base Tests

        [TestMethod]
        public void TestCountWhiteBytes()
        {
            try
            {
                Image[] a_img = new Image[] { _fullEdges, _halfEdges, _fullEdgesInner1, _fullEdgesInner2, 
                        _fullEdgesOuter1, _fullEdgesOuter2, _fullEdgesMore, _fullEdgesLess };
                // --

                foreach (Image i in a_img)
                {
                    // Test function for counting white bytes of the 1st parameter and function
                    // for counting white bytes of the 2nd parameter
                    double count_0 = Facilities.CountWhitebytesInput(i, null);
                    double count_1 = Facilities.CountWhitebytesOutput(null, i);

                    Assert.AreEqual(
                            count_0, count_1,
                            "Countings do not match for image {0}!", ((PrivateStore)i.Tag).Name);
                    // --

                    Assert.AreEqual(
                            count_0, ((PrivateStore)i.Tag).Whites,
                            "Countings do not match with histogram for image {0}!", ((PrivateStore)i.Tag).Name);
                    // --
                }
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected fail TestCountWhiteBytes: {0}", ex.Message);
            }
        }

        /// <summary>
        /// Testing function of matching white bytes 
        /// </summary>
        [TestMethod]
        public void TestMatchingBytes()
        {
            try
            {
                // Number of matching bytes between two equal images must equal the number of white bytes of the image itself
                Image[] a_img = new Image[] { _fullEdges, _halfEdges, _fullEdgesInner1, _fullEdgesInner2, 
                        _fullEdgesOuter1, _fullEdgesOuter2, _fullEdgesMore, _fullEdgesLess };
                // --

                foreach (Image i in a_img)
                {
                    double count = Facilities.CountMatchWhitebytes(i, i);

                    Assert.AreEqual(
                            count, ((PrivateStore)i.Tag).Whites,
                            "Countings do not match with histogram for image {0}!", ((PrivateStore)i.Tag).Name);
                    // --
                }
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected fail TestMatchingBytes: {0}", ex.Message);
            }
        }

        /// <summary>
        /// Tests 2 basic situation with the error tolerant metric: two equal and two different images
        /// </summary>
        [TestMethod()]
        public void TestMetricErrorTolerantBasic()
        {
            try
            {
                double value = Framework.Core.Facilities.CountErrorTolerant(_fullEdges, _fullEdges);
                Assert.AreEqual(value, 0.0, "Two equal images do not have ErrorTolerant == 0 !!! :O Is {0}", value);

                value = Framework.Core.Facilities.CountErrorTolerant(_fullEdges, _halfEdges);
                Assert.AreNotEqual(value, 0.0, "Two different images do have ErrorTolerant == 0 !!! :O ");

            }
            catch (Exception e)
            {
                Assert.Fail("Unexpected fail TestMetricErrorTolerant: {0}", e.Message);
            }
        }

        #endregion

        #region Test Excel Generated Metrics

        /// <summary>
        /// Output results with "well-known" images
        /// </summary>
        [TestMethod]
        public void TestExcelGeneratedMetrics()
        {
            try
            {
                Image[,] pairs = new Image[,]{
                    { _fullEdges, _fullEdges },
                    {_fullEdges, _halfEdges},
                    {_fullEdges, _fullEdgesOuter1},
                    {_fullEdges, _fullEdgesOuter2},
                    {_fullEdges, _fullEdgesInner1},
                    {_fullEdges, _fullEdgesInner2},
                    {_fullEdges, _fullEdgesMore},
                    {_fullEdges, _fullEdgesLess}
                };

                for (int i = 0; i < pairs.GetLength(0); ++i)
                {
                    Trace.WriteLine(string.Format("Image 1 = \"{0}\", Image 2 = \"{1}\"",
                            ((PrivateStore)pairs[i, 0].Tag).Name,
                            ((PrivateStore)pairs[i, 1].Tag).Name));
                    // --

                    double true_edges = Facilities.CountWhitebytesInput(pairs[i, 0], null);
                    Assert.IsFalse(true_edges < 0, "True edges less than zero!");

                    double detected_edges = Facilities.CountWhitebytesInput(pairs[i, 1], null);
                    Assert.IsFalse(detected_edges < 0, "Detected edges less than zero!");

                    Trace.WriteLine("\tEdges-------------------------");
                    Trace.WriteLine(string.Format("\tTrue Edges = {0}", true_edges));
                    Trace.WriteLine(string.Format("\tDetected Edges = {0}", detected_edges));

                    // Test excel generated metrics: FP, FN, Precision, Recall, F-Measure
                    double vp = Facilities.CountMatchWhitebytes(pairs[i, 0], pairs[i, 1]);
                    Assert.IsFalse(vp < 0, "True positives less than zero!");

                    double fp = detected_edges - vp;
                    Assert.IsFalse(fp < 0, "False positives less than zero!");

                    double fn = true_edges - vp;
                    Assert.IsFalse(fn < 0, "False negatives less than zero!");

                    double precision = 0;
                    double recall = 0;
                    double fmeasure = 0;

                    if (vp + fp != 0)
                    {
                        precision = vp / (vp + fp);
                    }

                    if (vp + fn != 0)
                    {
                        recall = vp / (vp + fn);
                    }

                    if (precision + recall != 0)
                    {
                        fmeasure = (2.0 * precision * recall) / (precision + recall);
                    }

                    Trace.WriteLine("\tMetrics-----------------------");
                    Trace.WriteLine(string.Format("\tVP = {0}", vp));
                    Trace.WriteLine(string.Format("\tFP = {0}", fp));
                    Trace.WriteLine(string.Format("\tFN = {0}", fn));
                    Trace.WriteLine(string.Format("\tPrecision = {0}", precision));
                    Trace.WriteLine(string.Format("\tRecall = {0}", recall));
                    Trace.WriteLine(string.Format("\tF-Measure = {0}", fmeasure));
                }

            }
            catch (Exception ex)
            {
                Assert.Fail("TestExcelGeneratedMetrics failed with exception {0}", ex.Message);
            }
        }

        #endregion

        #region Error Tolerant

        /// <summary>
        /// Output results with "well-known" images
        /// </summary>
        [TestMethod]
        public void TestErrorTolerant()
        {
            try
            {
                // 
                // 
                Image[,] pairs = new Image[,]{
                    { _fullEdges, _fullEdges },
                    {_fullEdges, _halfEdges},
                    {_fullEdges, _fullEdgesOuter1},
                    {_fullEdges, _fullEdgesOuter2},
                    {_fullEdges, _fullEdgesInner1},
                    {_fullEdges, _fullEdgesInner2},
                    {_fullEdges, _fullEdgesMore},
                    {_fullEdges, _fullEdgesLess}
                };

                for (int i = 0; i < pairs.GetLength(0); ++i)
                {
                    Trace.WriteLine(string.Format("Image 1 = \"{0}\", Image 2 = \"{1}\"",
                            ((PrivateStore)pairs[i, 0].Tag).Name,
                            ((PrivateStore)pairs[i, 1].Tag).Name));
                    // --

                    double ret = Facilities.CountErrorTolerant(pairs[i, 0], pairs[i, 1]);
                    Assert.IsFalse(ret < 0, "Error Tolerant less than zero!");

                    Trace.WriteLine("\tMetrics-----------------------");
                    Trace.WriteLine(string.Format("\tCountErrorTolerant = {0}", ret));
                }
            }
            catch (Exception ex)
            {
                Assert.Fail("TestErrorTolerant failed with exception {0}", ex.Message);
            }
        }

        #endregion

    }
}
