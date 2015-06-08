using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should.Fluent;

namespace PGK.Extensions.Tests
{
	[TestClass]
    public class StringBuilderExtensionsTests
	{
		[TestMethod]
		public void AppendLineWithFormat()
		{
		    var builder = new StringBuilder();
		    string result;

            // Arrange
		    builder.AppendLine("Something to format ({0}.{1}) with a new line after", "String", "Format");
		    // Act
            result = builder.ToString();
		    // Assert
            result.Should().Not.Be.NullOrEmpty();
		    result.Split(Environment.NewLine).Should().Count.Exactly(2);
            result.Split(Environment.NewLine)[0].Should().Equal("Something to format (String.Format) with a new line after");
            result.Split(Environment.NewLine)[1].Should().Be.Empty();
		}

        [TestMethod]
        public void AppendIfTest()
        {
            string test = "abc123xyz456";

            StringBuilder sb = new StringBuilder(); 
            Char c; 
            for (int i = 0; i < test.Length; i++) 
            {
                c = test[i];
                sb.AppendIf(Char.IsDigit(c), c);
            }
            
            Assert.IsTrue(sb.ToString() == "123456");
        }

        [TestMethod]
        public void AppendLineIfTest0()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLineIf(true, "True");
            sb.AppendLineIf(false, "false");
            sb.AppendLineIf(true, true);

            Assert.IsTrue(sb.ToString() == "True" + Environment.NewLine + "True" + Environment.NewLine);
        }

        [TestMethod]
        public void AppendLineIfTest1()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLineIf(true, "{0}", true).AppendLineIf(false, "{0}", false).AppendLineIf(true, "{0}", true);

            Assert.IsTrue(sb.ToString() == "True" + Environment.NewLine + "True" + Environment.NewLine);
        }

        [TestMethod]
        public void AppendFormatIf()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormatIf(true, "{0}", true);
            sb.AppendFormatIf(false, "{0}", false);
            sb.AppendFormatIf(true, "{0}", true);

            Assert.IsTrue(sb.ToString() == "TrueTrue");
        }
	}
}
