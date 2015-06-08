using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PGK.Extensions.Tests
{
	[TestClass]
	public class TimeSpanExtensionsTests
	{
		[TestMethod]
		public void MultiplyBy_int()
		{
			var testValue = TimeSpan.FromSeconds(12);
			var result = testValue.MultiplyBy(5);
			Assert.AreEqual(1, result.Minutes);
			Assert.AreEqual(0, result.Seconds);
		}

		[TestMethod]
		public void MultiplyBy_double()
		{
			var testValue = TimeSpan.FromSeconds(18);
			var result = testValue.MultiplyBy(3.334);
			Assert.AreEqual(1, result.Minutes);
			Assert.AreEqual(0, result.Seconds);
		}

	}
}
