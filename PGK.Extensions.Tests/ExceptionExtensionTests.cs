using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should.Fluent;

namespace PGK.Extensions.Tests
{
	[TestClass]
	public class ExceptionExtensionTests
	{
		[TestMethod]
		public void TestMessages()
		{
			// Arrange
			var outerException = "outer exception";
			var innerException = "inner exception";
			var e = new Exception(outerException, new Exception(innerException));

			// Act
			var result = e.Messages();

			// Assert
			result.Should().Count.Exactly(2);
			result.First().Should().Contain(innerException);
			result.Last().Should().Contain(outerException);
		}
	}
}
