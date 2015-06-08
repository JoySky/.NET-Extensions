using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Linq;

namespace PGK.Extensions.Test4
{


	/// <summary>
	///This is a test class for StringExtensionsTest and is intended
	///to contain all StringExtensionsTest Unit Tests
	///</summary>
	[TestClass()]
	public class StringExtensionsTest
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
		///A test for ToPlural
		///</summary>
		[TestMethod()]
		public void ToPluralTest()
		{
			Assert.AreEqual("test".ToPlural2(), "tests"); // standard
			Assert.AreEqual("goose".ToPlural2(), "geese"); // special nouns
			Assert.AreEqual("box".ToPlural2(), "boxes"); // -ch, x, s to -es
			Assert.AreEqual("boy".ToPlural2(), "boys"); // -y to -ies
			Assert.AreEqual("box of ball".ToPlural2(), "box of balls"); // of
			Assert.AreEqual("kiss".ToPlural2(), "kisses"); // -s to -es
			Assert.AreEqual("phenomenon".ToPlural2(), "phenomena"); // nouns that maintain their Latin or Greek form in the plural
			
			Assert.AreEqual("potato".ToPlural2(), "potatoes"); // -o to -oes
			Assert.AreEqual("memo".ToPlural2(), "memos"); // -o to -oes (exceptions)
			Assert.AreEqual("stereo".ToPlural2(), "stereos"); // -o to -oes (exceptions)

			Assert.AreEqual("knife".ToPlural2(), "knives"); // -f, fe to -es
			// these two fail
			// Assert.AreEqual("dwarf".ToPlural(), "dwarfs"); // -f, fe to -es (exceptions)
			// Assert.AreEqual("roof".ToPlural(), "roofs"); // -f, fe to -es (exceptions)
		}

        [TestMethod]
        public void ExtractPerformanceTest()
        {
            string test = string.Join(",", Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ 1234567890", 100));
            Stopwatch sw = new Stopwatch();

            sw.Start();

            for (int i = 0; i < 1000; i++)
            {
                string x = test.ExtractDigits();
            }

            sw.Stop();
            Console.WriteLine("ExtractDigits: {0} msecs", sw.ElapsedMilliseconds);

            sw.Reset();
            sw.Start();

            for (int i = 0; i < 1000; i++)
            {            
                string x = test.Extract(c => char.IsDigit(c));
            }

            sw.Stop();
            Console.WriteLine("Extract(c => char.IsDigit(c)): {0} msecs", sw.ElapsedMilliseconds);

            sw.Reset();
            sw.Start();

            for (int i = 0; i < 1000; i++)
            {
                string x = test.Extract(CharTypes.Digits);
            }

            sw.Stop();
            Console.WriteLine("Extract(CharTypes.Digits): {0} msecs", sw.ElapsedMilliseconds);

            sw.Reset();
            sw.Start();

            for (int i = 0; i < 1000; i++)
            {
                string x = test.Extract(CharTypes.Digits | CharTypes.Letters);
            }

            sw.Stop();
            Console.WriteLine("Extract(CharTypes.Digits | CharTypes.Letters): {0} msecs", sw.ElapsedMilliseconds);

            sw.Reset();
            sw.Start();

            for (int i = 0; i < 1000; i++)
            {
                string x = test.Extract(CharTypes.LettersOrDigits);
            }

            sw.Stop();
            Console.WriteLine("Extract(CharTypes.LettersOrDigits): {0} msecs", sw.ElapsedMilliseconds);

            sw.Reset();
            sw.Start();

            for (int i = 0; i < 1000; i++)
            {
                string x = test.Extract(CharTypes.LettersOrDigits | CharTypes.WhiteSpace);
            }

            sw.Stop();
            Console.WriteLine("Extract(CharTypes.LettersOrDigits | CharTypes.WhiteSpace): {0} msecs", sw.ElapsedMilliseconds);
        }

        /// <summary>
        /// Test digit extraction
        /// </summary>
        [TestMethod]
        public void ExtractDigitsTest()
        {
            Assert.AreEqual("test".Extract(CharTypes.Digits), string.Empty);
            Assert.AreEqual("1234".Extract(CharTypes.Digits), "1234");
            Assert.AreEqual("test 1234 test".Extract(CharTypes.Digits), "1234");
        }

        /// <summary>
        /// Test digit removal
        /// </summary>
        [TestMethod]
        public void RemoveDigitsTest()
        {
            Assert.AreEqual("test".Remove(CharTypes.Digits), "test");
            Assert.AreEqual("1234".Remove(CharTypes.Digits), string.Empty);
            Assert.AreEqual("test 1234 test".Remove(CharTypes.Digits), "test  test");
        }

        /// <summary>
        /// Test letter extraction
        /// </summary>
        [TestMethod]
        public void ExtractLettersTest()
        {
            Assert.AreEqual("test".Extract(CharTypes.Letters), "test");
            Assert.AreEqual("1234".Extract(CharTypes.Letters), string.Empty);
            Assert.AreEqual("test 1234 test".Extract(CharTypes.Letters), "testtest");
        }

        /// <summary>
        /// Test letter removal
        /// </summary>
        [TestMethod]
        public void RemoveLettersTest()
        {
            Assert.AreEqual("test".Remove(CharTypes.Letters), string.Empty);
            Assert.AreEqual("1234".Remove(CharTypes.Letters), "1234");
            Assert.AreEqual("test 1234 test".Remove(CharTypes.Letters), " 1234 ");
        }

        /// <summary>
        /// Test letter or digit extraction
        /// </summary>
        [TestMethod]
        public void ExtractLettersOrDigitsTest()
        {
            Assert.AreEqual("test".Extract(CharTypes.LettersOrDigits), "test");
            Assert.AreEqual("1234".Extract(CharTypes.LettersOrDigits), "1234");
            Assert.AreEqual("test 1234 test".Extract(CharTypes.LettersOrDigits), "test1234test");
        }

        /// <summary>
        /// Test letter or digit removal
        /// </summary>
        [TestMethod]
        public void RemoveLettersOrDigitsTest()
        {
            Assert.AreEqual("test".Remove(CharTypes.LettersOrDigits), string.Empty);
            Assert.AreEqual("1234".Remove(CharTypes.LettersOrDigits), string.Empty);
            Assert.AreEqual("test 1234 test".Remove(CharTypes.LettersOrDigits), "  ");
        }

        /// <summary>
        /// Test white space extraction
        /// </summary>
        [TestMethod]
        public void ExtractWhiteSpaceTest()
        {
            Assert.AreEqual("test".Extract(CharTypes.WhiteSpace), string.Empty);
            Assert.AreEqual("1234".Extract(CharTypes.WhiteSpace), string.Empty);
            Assert.AreEqual("test 1234 test".Extract(CharTypes.WhiteSpace), "  ");
        }

        /// <summary>
        /// Test white space removal
        /// </summary>
        [TestMethod]
        public void RemoveWhiteSpaceTest()
        {
            Assert.AreEqual(" test".Remove(CharTypes.WhiteSpace), "test");
            Assert.AreEqual("1234 ".Remove(CharTypes.WhiteSpace), "1234");
            Assert.AreEqual("test 1234 test".Remove(CharTypes.WhiteSpace), "test1234test");
        }

        /// <summary>
        /// Test letter or white space extraction
        /// </summary>
        [TestMethod]
        public void ExtractLettersOrWhiteSpaceTest()
        {
            Assert.AreEqual("test".Extract(CharTypes.Letters | CharTypes.WhiteSpace), "test");
            Assert.AreEqual("1234".Extract(CharTypes.Letters | CharTypes.WhiteSpace), string.Empty);
            Assert.AreEqual("test 1234 test".Extract(CharTypes.Letters | CharTypes.WhiteSpace), "test  test");
        }

        /// <summary>
        /// Test letter or white space removal
        /// </summary>
        [TestMethod]
        public void RemoveLettersOrWhiteSpaceTest()
        {
            Assert.AreEqual(" test".Remove(CharTypes.Letters | CharTypes.WhiteSpace), string.Empty);
            Assert.AreEqual("1234 ".Remove(CharTypes.Letters | CharTypes.WhiteSpace), "1234");
            Assert.AreEqual("test 1234 test".Remove(CharTypes.Letters | CharTypes.WhiteSpace), "1234");
        }

        /// <summary>
        /// Test XML character extraction
        /// </summary>
        [TestMethod]
        public void ExtractXmlCharTest()
        {
            Assert.AreEqual("test\u0002test".Extract(CharTypes.XmlChar), "testtest");
        }

        /// <summary>
        /// Test XML character removal
        /// </summary>
        [TestMethod]
        public void RemoveXmlCharTest()
        {
            Assert.AreEqual("test\u0002test".Remove(CharTypes.XmlChar), "\u0002");
        }

        /// <summary>
        /// Test char extraction using char array
        /// </summary>
        [TestMethod]
        public void ExtractCharList()
        {
            Assert.AreEqual("test".Extract('t'), "tt");
            Assert.AreEqual("test".Extract('e', 's'), "es");
            Assert.AreEqual("test".Extract("es"), "es");
        }

        /// <summary>
        /// Test null reference exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void ExtractNullReferenceExceptionText()
        {
            string test = null;
            test.Extract(CharTypes.Letters);
        }

	}
}
