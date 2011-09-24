using Framework.Transforms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Framework.Core.Filters.Frequency;

namespace Tests
{
    using FourierTestPair = System.Tuple<double[,], Framework.Core.Filters.Frequency.ComplexImage>;
    using Framework.Core.Transforms;

    /// <summary>
    ///This is a test class for FourierTransformTest and is intended
    ///to contain all FourierTransformTest Unit Tests
    ///</summary>
    [TestClass()]
    public class FourierTransformTest
    {

        #region Auto generated code

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
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code beforeLoad running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code beforeLoad running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        #endregion

        #region Quality Tests

        /// <summary>
        /// Performs the following tests on the transform instance:
        ///  * checks the size of the data after a transform or a reverse-transform
        ///  * checks the data after a transform or a reverse-transform
        ///  * checks null output
        ///</summary>
        ///<remarks>
        /// Comparision of doubles conformant with the documentation at: http://msdn.microsoft.com/en-us/library/ya2zha7s.aspx
        ///</remarks>
        private void TransformTestTestData(
                TransformCoreBase<ComplexImage> df, int i, double[,] wellKnownData, ComplexImage wellKnownTransform)
        {
            // Perform transform
            ComplexImage calculatedTransform = df.ApplyTransformBase(wellKnownData);

            // ** Check Reference
            Assert.IsNotNull(calculatedTransform, "Test #{0}: Generated transform is null :(.");

            // ** Check Sizes
            Assert.AreEqual(wellKnownData.GetLength(0), calculatedTransform.Width,
                    "Test #{0}: Width of transform does not match width of original data.", i);
            Assert.AreEqual(wellKnownData.GetLength(1), calculatedTransform.Height,
                    "Test #{0}: Height of transform does not match width of original data.", i);


            // ** Check Data
            int x, y;

            const double tollerance = 0.0001;

            for (x = 0; x < wellKnownData.GetLength(0); ++x)
            {
                for (y = 0; y < wellKnownData.GetLength(1); ++y)
                {
                    Assert.AreEqual(wellKnownTransform[x, y].real, calculatedTransform[x, y].real,
                            tollerance, "Test #{0}: Expected {1}, but found {2} in the transform @({3}, {4}).real .",
                            i, wellKnownTransform[x, y].real, calculatedTransform[x, y].real, x, y);

                    Assert.AreEqual(wellKnownTransform[x, y].imag, calculatedTransform[x, y].imag,
                            tollerance, "Test #{0}: Expected {1}, but found {2} in the transform @({3}, {4}).imag .",
                            i, wellKnownTransform[x, y].imag, calculatedTransform[x, y].imag, x, y);
                }
            }

            // Perform reverse-transform
            double[,] calculatedData = df.ApplyReverseTransformBase(calculatedTransform);

            // ** Check Reference
            Assert.IsNotNull(calculatedData, "Test #{0}: Generated reverse-transform is null :(.");

            // ** Check Sizes
            Assert.AreEqual(calculatedTransform.Width, calculatedData.GetLength(0),
                    "Width of reverse-transform does not match width of transform.");
            Assert.AreEqual(calculatedTransform.Height, calculatedData.GetLength(1),
                    "Height of reverse-transform does not match width of transform.");

            // ** Check Data
            for (x = 0; x < wellKnownData.GetLength(0); ++x)
            {
                for (y = 0; y < wellKnownData.GetLength(1); ++y)
                {
                    // Be careful with rounding error in the double datatype >:(

                    Assert.AreEqual(wellKnownData[x, y], calculatedData[x, y],
                           tollerance, "Test #{0}: Expected {1}, but found {2} in the reverse-transform @({3}, {4}).",
                           i, wellKnownData[x, y], calculatedData[x, y], x, y);

                }
            }
        }

        
        /// <summary>
        /// Performs TransformTestTestData using the DiscreteFourierTransform as a class.
        /// </summary>
        [TestMethod()]
        public void DiscreteFourierTransformTest()
        {
            DiscreteFourierTransform df = new DiscreteFourierTransform();
            int i = 1;

            FourierTestPair[] testToDo = new FourierTestPair[] { 
                        new FourierTestPair(
                                new double[,] { { 1.0, 1.0 }, { 1.0, 1.0 } }, 
                                new ComplexImage(new double[,] { { 0.0, 0.0 }, { 0.0, 4.0 } } )) // 2x2
                        ,new FourierTestPair(
                                new double[,] { { 2.0, 2.0 }, { 1.0, 1.0 } }, 
                                new ComplexImage(new double[,] { { 0.0, 2.0 }, { 0.0, 6.0 } } )) //2x2
                        ,new FourierTestPair(
                                new double[,] { { 1.0, 1.0, 1.0 }, { 1.0, 1.0, 1.0 }, { 1.0, 1.0, 1.0 } }, 
                                new ComplexImage(new Complex[,] { 
                                        { new Complex(1.0, 0.0), new Complex(1.0, 1.7321), new Complex(1.0, - 1.7321) }, 
                                        { new Complex(1.0, 1.7321), new Complex(-2.0, 3.4641), new Complex(4.0, 0) }, 
                                        { new Complex(1.0, -1.7321), new Complex(4.0, 0.0), new Complex(-2.0, - 3.4641) } } )) //3x3
                        ,new FourierTestPair(
                                new double[,] { { 2.0, 2.0, 2.0 }, { 1.0, 1.0, 1.0 }, { 0.0, 0.0, 0.0 } }, 
                                new ComplexImage(new Complex[,] { 
                                        { new Complex(1.0, 0.0), new Complex(1.0, 1.7321), new Complex(1.0, - 1.7321) }, 
                                        { new Complex(2.5, 0.8660), new Complex(1.0, 5.1962), new Complex(4.0, - 3.4641) }, 
                                        { new Complex(2.5, -0.8660), new Complex(4.0, 3.4641), new Complex(1.0, - 5.1962) } } )) //3x3
                        ,new FourierTestPair(
                                new double[,] { { 3.0, 3.0, 3.0, 3.0 },  { 2.0, 2.0, 2.0, 2.0 }, { 1.0, 1.0, 1.0, 1.0 }, { 0.0, 0.0, 0.0, 0.0 } }, 
                                new ComplexImage(new Complex[,] { 
                                        { new Complex( 0.0, 0.0), new Complex(0.0, 0.0), new Complex(8.0, 0.0) , new Complex(0.0, 0.0)}, 
                                        { new Complex( 0.0, 0.0), new Complex(0.0, 0.0), new Complex(8.0, 8.0) , new Complex(0.0, 0.0)}, 
                                        { new Complex( 0.0, 0.0), new Complex(0.0, 0.0), new Complex(24.0, 0.0) , new Complex(0.0, 0.0)},
                                        { new Complex( 0.0, 0.0), new Complex(0.0, 0.0), new Complex(8.0, -8.0) , new Complex(0.0, 0.0)}} )) //4x4
                };

            /*
             * TEMPLATE:
             * 
                new FourierTestPair(
                        new double[,] { { 1.0, 1.0 }, { 1.0, 1.0 } }, 
                        new ComplexImage(new double[,] { { 4.0, 0.0 }, { 0.0, 0.0 } } ))
            */

            foreach (FourierTestPair test in testToDo)
            {
                TransformTestTestData(df, i, test.Item1, test.Item2);
                i += 1;
            }

        }

        /// <summary>
        /// Performs TransformTestTestData using the FastFourierTransform as a class.
        /// </summary>
        [TestMethod()]
        public void FastFourierTransformTest()
        {
            FastFourierTransform df = new FastFourierTransform();
            int i = 1;

            FourierTestPair[] testToDo = new FourierTestPair[] { 
                        new FourierTestPair(
                                new double[,] { { 1.0, 1.0 }, { 1.0, 1.0 } }, 
                                new ComplexImage(new double[,] { { 0.0, 0.0 }, { 0.0, 4.0 } } )) // 2x2
                        ,new FourierTestPair(
                                new double[,] { { 2.0, 2.0 }, { 1.0, 1.0 } }, 
                                new ComplexImage(new double[,] { { 0.0, 2.0 }, { 0.0, 6.0 } } )) //2x2
                        //// FastFourierTransform can't handle images that do not have sizes base-2
                        //,new FourierTestPair(
                        //        new double[,] { { 1.0, 1.0, 1.0 }, { 1.0, 1.0, 1.0 }, { 1.0, 1.0, 1.0 } }, 
                        //        new ComplexImage(new Complex[,] { 
                        //                { new Complex(1.0, 0.0), new Complex(1.0, 1.7321), new Complex(1.0, - 1.7321) }, 
                        //                { new Complex(1.0, 1.7321), new Complex(-2.0, 3.4641), new Complex(4.0, 0) }, 
                        //                { new Complex(1.0, -1.7321), new Complex(4.0, 0.0), new Complex(-2.0, - 3.4641) } } )) //3x3
                        //,new FourierTestPair(
                        //        new double[,] { { 2.0, 2.0, 2.0 }, { 1.0, 1.0, 1.0 }, { 0.0, 0.0, 0.0 } }, 
                        //        new ComplexImage(new Complex[,] { 
                        //                { new Complex(1.0, 0.0), new Complex(1.0, 1.7321), new Complex(1.0, - 1.7321) }, 
                        //                { new Complex(2.5, 0.8660), new Complex(1.0, 5.1962), new Complex(4.0, - 3.4641) }, 
                        //                { new Complex(2.5, -0.8660), new Complex(4.0, 3.4641), new Complex(1.0, - 5.1962) } } )) //3x3
                        ,new FourierTestPair(
                                new double[,] { { 3.0, 3.0, 3.0, 3.0 },  { 2.0, 2.0, 2.0, 2.0 }, { 1.0, 1.0, 1.0, 1.0 }, { 0.0, 0.0, 0.0, 0.0 } }, 
                                new ComplexImage(new Complex[,] { 
                                        { new Complex( 0.0, 0.0), new Complex(0.0, 0.0), new Complex(8.0, 0.0) , new Complex(0.0, 0.0)}, 
                                        { new Complex( 0.0, 0.0), new Complex(0.0, 0.0), new Complex(8.0, 8.0) , new Complex(0.0, 0.0)}, 
                                        { new Complex( 0.0, 0.0), new Complex(0.0, 0.0), new Complex(24.0, 0.0) , new Complex(0.0, 0.0)},
                                        { new Complex( 0.0, 0.0), new Complex(0.0, 0.0), new Complex(8.0, -8.0) , new Complex(0.0, 0.0)}} )) //4x4
                };

            /*
             * TEMPLATE:
             * 
                new FourierTestPair(
                        new double[,] { { 1.0, 1.0 }, { 1.0, 1.0 } }, 
                        new ComplexImage(new double[,] { { 4.0, 0.0 }, { 0.0, 0.0 } } ))
            */

            foreach (FourierTestPair test in testToDo)
            {
                TransformTestTestData(df, i, test.Item1, test.Item2);
                i += 1;
            }

        }

        #endregion

    }
}
