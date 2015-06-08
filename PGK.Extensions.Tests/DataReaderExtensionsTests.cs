using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using System.Data;

namespace PGK.Extensions.Tests
{
    /// <summary>
    /// Summary description for DataReaderExtensionsTests
    /// </summary>
    [TestClass]
    public class DataReaderExtensionsTests
    {
        public DataReaderExtensionsTests()
        {
            //
            // TODO: Add constructor logic here
            //
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

        [TestMethod]
        public void IndexOfTest()
        {
            DataTable dt = new DataTable("test");
            dt.Columns.Add(new DataColumn("C1", typeof(int)));
            dt.Columns.Add(new DataColumn("C2", typeof(string)));

            using (IDataReader dr = dt.CreateDataReader())
            {
                Assert.IsTrue(dr.IndexOf("C2") == 1);
                Assert.IsTrue(dr.IndexOf("A1") == -1);
            }
        }
    }
}
