using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should.Fluent;

namespace PGK.Extensions.Tests
{
	[TestClass]
	public class IntExtensionsTest
	{
		[TestMethod]
		public void TestIsEven()
		{
			/* positive testing */
			int testValue = 4;
			Assert.IsTrue(testValue.IsEven());
			/* negative testing */
			testValue = 3;
			Assert.IsFalse(testValue.IsEven());
		}

		[TestMethod]
		public void TestIsOdd()
		{
			/* positive testing */
			int testValue = 3;
			Assert.IsTrue(testValue.IsOdd());
			/* negative testing */
			testValue = 4;
			Assert.IsFalse(testValue.IsOdd());
		}

		[TestMethod]
		public void TestInRange()
		{
			// Arrange
			var value = 5;
			var defaultValue = 9;
			// Act
			var no = value.InRange(6, 10);
			var yes = value.InRange(0, 10);
			var result = value.InRange(6, 10, defaultValue);
			// Assert
			no.Should().Be.False();
			yes.Should().Be.True();
			result.Should().Equal(defaultValue);
		}

		[TestMethod]
		public void TestIsPrime()
		{
			int[] primes = new int[]
			{
				3, 7, 11, 17, 23, 29, 37, 47, 59, 71, 89, 107, 131, 163, 197, 239,
				293, 353, 431, 521, 631, 761, 919, 1103, 1327, 1597, 1931, 2333, 2801, 3371, 4049, 4861,
				5839, 7013, 8419, 10103, 12143, 14591, 17519, 21023, 25229, 30293, 36353, 43627, 52361, 62851, 75431, 90523,
				108631, 130363, 156437, 187751, 225307, 270371, 324449, 389357, 467237, 560689, 672827, 807403, 968897, 1162687, 1395263, 1674319,
				2009191, 2411033, 2893249, 3471899, 4166287, 4999559, 5999471, 7199369
			};

			for (int i = 0; i < primes.Length; i++)
			{
				Assert.IsTrue(primes[i].IsPrime());
			}

			int[] nonPrimes = new int[]
			{
				-2, 1, 0, 4, 8, 12, 18, 24, 28, 36, 48, 56, 78, 86, 106, 136, 168, 198, 236
			};

			for (int i = 0; i < nonPrimes.Length; i++)
			{
				Assert.IsFalse(nonPrimes[i].IsPrime());
			}
		}

		[TestMethod]
		public void TestToOrdinal()
		{
			string s3000001 = "3000001st";
			int i3000001 = 3000001;

			Assert.AreEqual(s3000001, i3000001.ToOrdinal());
		}

		[TestMethod]
		public void TestDays()
		{
			int val = -6;
			TimeSpan span1 = val.Days();
			TimeSpan span2 = TimeSpan.FromDays(val);

			span1.Should().Equal(span2);
		}

		[TestMethod]
		public void TestHours()
		{
			int val = -8;
			TimeSpan span1 = val.Hours();
			TimeSpan span2 = TimeSpan.FromHours(val);

			span1.Should().Equal(span2);
		}

		[TestMethod]
		public void TestMilliseconds()
		{
			int val = 10159;
			TimeSpan span1 = val.Milliseconds();
			TimeSpan span2 = TimeSpan.FromMilliseconds(val);

			span1.Should().Equal(span2);
		}

		[TestMethod]
		public void TestMinutes()
		{
			int val = 155;
			TimeSpan span1 = val.Minutes();
			TimeSpan span2 = TimeSpan.FromMinutes(val);

			span1.Should().Equal(span2);
		}

		[TestMethod]
		public void TestSeconds()
		{
			int val = 2708;
			TimeSpan span1 = val.Seconds();
			TimeSpan span2 = TimeSpan.FromSeconds(val);

			span1.Should().Equal(span2);
		}

		[TestMethod]
		public void TestTicks()
		{
			int val = 992055;
			TimeSpan span1 = val.Ticks();
			TimeSpan span2 = TimeSpan.FromTicks(val);

			span1.Should().Equal(span2);
		}
	}
}
