using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should.Fluent;

namespace PGK.Extensions.Tests
{
	[TestClass]
	public class FloatExtensionsTest
	{
		[TestMethod]
		public void TestInRange()
		{
			// Arrange
			var value = 5.5f;
			var defaultValue = 9.9f;
			// Act
			var no = value.InRange(6.6f, 10.01f);
			var yes = value.InRange(0.0f, 10.01f);
			var result = value.InRange(6.6f, 10.01f, defaultValue);
			// Assert
			no.Should().Be.False();
			yes.Should().Be.True();
			result.Should().Equal(defaultValue);
		}

		[TestMethod]
		public void TestDays()
		{
			float val = 4.229900f;
			TimeSpan span1 = val.Days();
			TimeSpan span2 = TimeSpan.FromDays(val);

			span1.Should().Equal(span2);
		}

		[TestMethod]
		public void TestHours()
		{
			float val = 60.31276f;
			TimeSpan span1 = val.Hours();
			TimeSpan span2 = TimeSpan.FromHours(val);

			span1.Should().Equal(span2);
		}

		[TestMethod]
		public void TestMilliseconds()
		{
			float val = 75781.90f;
			TimeSpan span1 = val.Milliseconds();
			TimeSpan span2 = TimeSpan.FromMilliseconds(val);

			span1.Should().Equal(span2);
		}

		[TestMethod]
		public void TestMinutes()
		{
			float val = 952.2108f;
			TimeSpan span1 = val.Minutes();
			TimeSpan span2 = TimeSpan.FromMinutes(val);

			span1.Should().Equal(span2);
		}

		[TestMethod]
		public void TestSeconds()
		{
			float val = 3545.296f;
			TimeSpan span1 = val.Seconds();
			TimeSpan span2 = TimeSpan.FromSeconds(val);

			span1.Should().Equal(span2);
		}
	}
}
