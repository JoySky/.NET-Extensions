using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PGK.Extensions.SystemDependencies;
using Should.Fluent;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace PGK.Extensions.Tests
{
	[TestClass]
	public class DateTimeExtensionTest
	{
	    
		[TestMethod]
		public void TestGetDaysInYear()
		{
			/* positive testing */
			var testValue = new DateTime(2010, 1, 1);
			var resultValue = 365;
			Assert.AreEqual(testValue.GetDays(), resultValue);
			/* negative testing */
			testValue = new DateTime(2012, 1, 1);
			Assert.AreNotEqual(testValue.GetDays(), resultValue);
		}

		[TestMethod]
		public void TestCalculateAge()
		{
			const int defaultValue = 25;
			const int smallValue = 24;
			const int bigValue = 26;
			var birthDate = new DateTime(1984, 12, 26);
			var dayBefore26Th = birthDate.AddYears(defaultValue + 1).AddDays(-1);

			// Arrange
			using (new SubstituteForSystemDate(dayBefore26Th))
			{
				var futureDate = new DateTime(2011, 01, 01);
			
				// Act
				var result = birthDate.CalculateAge();
				var futureResult = birthDate.CalculateAge(futureDate);

				// Assert
				result.Should().Equal(defaultValue);
				result.Should().Not.Equal(smallValue);
				result.Should().Not.Equal(bigValue);

				futureResult.Should().Equal(bigValue);
				futureResult.Should().Not.Equal(smallValue);
				futureResult.Should().Not.Equal(defaultValue);
			}
		}

		[TestMethod]
		public void TestGetCountDaysOfMonth()
		{
			// Arrange
			var value = new DateTime(2010, 1, 1);
			var defaultValue = 31;

			// Act
			var result = value.GetCountDaysOfMonth();

			// Assert
			result.Should().Equal(defaultValue);

			//Exercise
			new DateTime(2010, 2, 15).GetCountDaysOfMonth().Should().Equal(28);
			new DateTime(2012, 2, 20).GetCountDaysOfMonth().Should().Equal(29);
			new DateTime(2000, 2, 5).GetCountDaysOfMonth().Should().Equal(29);
			new DateTime(1900, 2, 5).GetCountDaysOfMonth().Should().Equal(28);

			new DateTime(2010, 12, 5).GetCountDaysOfMonth().Should().Equal(31);
			new DateTime(2010, 6, 5).GetCountDaysOfMonth().Should().Equal(30);

		}

		[TestMethod]
		public void TestIsEaster()
		{
			DateTime dtR = new DateTime(2011, 4, 24);
			DateTime dtW = new DateTime(2011, 4, 25);

			Assert.IsTrue(dtR.IsEaster());
			Assert.IsFalse(dtW.IsEaster());
		}

		[TestMethod]
		public void TestIsBefore()
		{
			var source = DateTime.Now;
			var other = source.AddMilliseconds(1);

			source.IsBefore(other).Should().Be.True();
			other.IsBefore(source).Should().Be.False();
		}

		[TestMethod]
		public void TestIsAfter()
		{
			var source = DateTime.Now;
			var other = source.AddMilliseconds(1);

			source.IsAfter(other).Should().Be.False();
			other.IsAfter(source).Should().Be.True();
		}
	}
}
