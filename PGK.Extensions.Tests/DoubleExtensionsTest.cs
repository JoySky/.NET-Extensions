using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should.Fluent;

namespace PGK.Extensions.Tests
{
	[TestClass]
	public class DoubleExtensionsTest
	{
		[TestMethod]
		public void TestInRange()
		{
			// Arrange
			var value = 5.5;
			var defaultValue = 9.9;
			// Act
			var no = value.InRange(6.6, 10.01);
			var yes = value.InRange(0.0, 10.01);
			var result = value.InRange(6.6, 10.01, defaultValue);
			// Assert
			no.Should().Be.False();
			yes.Should().Be.True();
			result.Should().Equal(defaultValue);
		}

		[TestMethod]
		public void TestDays()
		{
			double val = 2.199044;
			TimeSpan span1 = val.Days();
			TimeSpan span2 = TimeSpan.FromDays(val);

			span1.Should().Equal(span2);
		}

		[TestMethod]
		public void TestHours()
		{
			double val = 65.54515;
			TimeSpan span1 = val.Hours();
			TimeSpan span2 = TimeSpan.FromHours(val);

			span1.Should().Equal(span2);
		}

		[TestMethod]
		public void TestMilliseconds()
		{
			double val = 7388.00;
			TimeSpan span1 = val.Milliseconds();
			TimeSpan span2 = TimeSpan.FromMilliseconds(val);

			span1.Should().Equal(span2);
		}

		[TestMethod]
		public void TestMinutes()
		{
			double val = 624.1849;
			TimeSpan span1 = val.Minutes();
			TimeSpan span2 = TimeSpan.FromMinutes(val);

			span1.Should().Equal(span2);
		}

		[TestMethod]
		public void TestSeconds()
		{
			double val = 2130.827;
			TimeSpan span1 = val.Seconds();
			TimeSpan span2 = TimeSpan.FromSeconds(val);

			span1.Should().Equal(span2);
		}
	}
}
