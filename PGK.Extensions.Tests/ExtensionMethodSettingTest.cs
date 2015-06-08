using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;
using System.Text;
using Should.Fluent;

namespace PGK.Extensions.Tests
{
    
    
    /// <summary>
    ///This is a test class for ExtensionMethodSettingTest and is intended
    ///to contain all ExtensionMethodSettingTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ExtensionMethodSettingTest
    {


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
        //Use ClassInitialize to run code before running the first test in the class
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
        //Use TestInitialize to run code before running each test
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

        /// <summary>
        ///A test for DefaultCulture
        ///</summary>
        [TestMethod()]
        public void DefaultCultureTest()
        {
            //reset it to default just in case something has changed it.
            ExtensionMethodSetting.DefaultCulture = CultureInfo.CurrentCulture;

            //result should be equal to the Current Thread Culture.
            ExtensionMethodSetting.DefaultCulture.Name.Should().Equal(CultureInfo.CurrentCulture.Name);
            ExtensionMethodSetting.DefaultCulture.ThreeLetterISOLanguageName.Should().Equal(CultureInfo.CurrentCulture.ThreeLetterISOLanguageName);

            //Change the default culture to French Canadian
            ExtensionMethodSetting.DefaultCulture = new CultureInfo(Cultures.French_Canada.DisplayString());

            //Unless your system is set to French Canadian, the current thread culture should not be fr-CA
            ExtensionMethodSetting.DefaultCulture.Name.Should().Not.Equal(CultureInfo.CurrentCulture.Name);
            ExtensionMethodSetting.DefaultCulture.Name.Should().Not.Equal(CultureInfo.CurrentCulture.ThreeLetterISOLanguageName);
        }

        /// <summary>
        ///A test for DefaultEncoding
        ///</summary>
        [TestMethod()]
        public void DefaultEncodingTest()
        {
            
            //REset it to default UTF-8 just in case something has changed it.
            ExtensionMethodSetting.DefaultEncoding = Encoding.Default;

            //Default Encoding should be equal to the Default System Encoding.
            ExtensionMethodSetting.DefaultEncoding.EncodingName.Should().Equal(Encoding.Default.EncodingName);

            //Set it to ASCII
            ExtensionMethodSetting.DefaultEncoding = Encoding.ASCII;
            ExtensionMethodSetting.DefaultEncoding.EncodingName.Should().Not.Equal(Encoding.Default.EncodingName);

        }
    }
}
