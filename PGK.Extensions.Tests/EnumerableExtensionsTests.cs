using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PGK.Extensions.Tests.TestObjects;
using Should.Fluent;

namespace PGK.Extensions.Tests
{
	[TestClass]
	public class EnumerableExtensionsTests
	{
		[TestMethod]
		public void ConvertList_StringInt()
		{
			var values = new[] { "1", "2", "3" };
			var converted = values.ConvertList<string, int>();
			var expected = new int[] { 1, 2, 3 };
			Assert.IsTrue(expected.SequenceEqual(converted));
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ConvertList_Null()
		{
			int[] values = null;
			var converted = values.ConvertList<int, string>().ToArray();
			Assert.IsTrue(false, "Did not throw expected exception");
		}

		[TestMethod]
		public void ConvertList_Empty()
		{
			var values = new string[] { };
			var converted = values.ConvertList<string, int>();
			var expected = new int[] { };
			Assert.IsFalse(converted.Any());
			Assert.IsTrue(expected.SequenceEqual(converted));

		}

		[TestMethod]
		public void IgnoreNull_Basic()
		{
			int?[] src = { 1, 2, null, 3, null, 4 };
			int?[] expected = { 1, 2, 3, 4 };
			var filtered = src.IgnoreNulls();
			Assert.IsTrue(expected.SequenceEqual(filtered));
		}

		[TestMethod]
		public void IgnoreNull_NoNulls()
		{
			int[] src = { 1, 2, 3, 4 };
			int[] expected = { 1, 2, 3, 4 };
			var filtered = src.IgnoreNulls();
			Assert.IsTrue(expected.SequenceEqual(filtered));
		}
		[TestMethod]
		public void IgnoreNull_Null()
		{
			int[] nullArray = null;
			var filtered = nullArray.IgnoreNulls();
			Assert.IsNotNull(filtered);
			Assert.IsInstanceOfType(filtered, typeof(IEnumerable<int>));
			Assert.IsFalse(filtered.Any());
		}

		[TestMethod]
		public void IgnoreNulls_AllNulls()
		{
			int?[] nullsArray = { null, null, null };
			var filtered = nullsArray.IgnoreNulls();
			Assert.IsNotNull(filtered);
			Assert.IsInstanceOfType(filtered, typeof(IEnumerable<int?>));
			Assert.IsFalse(filtered.Any());
		}


		[TestMethod]
		public void FirstOrDefault_NotDefault()
		{
			IEnumerable<Double> list = new List<Double> { 1, 2 };

			list.FirstOrDefault(999).Should().Equal(1d);
		}
		[TestMethod]
		public void FirstOrDefault_Default()
		{
			IEnumerable<Double> emptyList = new List<Double>();
			emptyList.FirstOrDefault(999).Should().Equal(999d);
		}

		Box box1 = new Box(1, 1, 1);   // height length, width
		Box box2 = new Box(2, 2, 4);
		Box box3 = new Box(3, 5, 3);
		Box box4 = new Box(5, 4, 2);

		[TestMethod]
		public void MaxItem()
		{
			var boxes = new Box[] { box1, box2, null, box3, box4 };
			int width;
			var maxItem = boxes.MaxItem(b => b.Width, out width);
			Assert.AreEqual(box2, maxItem);
			Assert.AreEqual(4, width);
		}
		[TestMethod]
		public void MinItem()
		{
			var boxes = new Box[] { box1, box2, null, box3, box4 };
			int width;
			var minItem = boxes.MinItem(b => b.Width, out width);
			Assert.AreEqual(box1, minItem);
			Assert.AreEqual(1, width);
		}

		// Skipping Distinct, since I can't figure out how it hows

		[TestMethod]
		public void RemoveAll_Simple()
		{
			var values = new int[] { 1, 2, 3, 4, 5 };
			var expected = new int[] { 2, 4 };

			var results = values.AsEnumerable().RemoveAll(v => (v & 1) == 1);
			Assert.IsTrue(expected.SequenceEqual(results));
		}

		[TestMethod]
		public void WhereNot_Simple()
		{
			var values = new int[] { 1, 2, 3, 4, 5 };
			var expected = new int[] { 2, 4 };

			var results = values.AsEnumerable().RemoveWhere(v => (v & 1) == 1);
			Assert.IsTrue(expected.SequenceEqual(results));
		}

		[TestMethod]
		public void WhereNot_Null()
		{
			int[] values = null;
			var expected = new int[] { };

			var results = values.AsEnumerable().RemoveWhere(v => (v & 1) == 1);
			Assert.IsTrue(expected.SequenceEqual(results));
		}

		[TestMethod]
		public void ToCSV_Simple()
		{
			var values = new[] { 1, 2, 3, 4, 5 };
			string csv = values.ToCSV();
			var expected = "1,2,3,4,5";
			Assert.AreEqual(expected, csv);
		}

		[TestMethod]
		public void ToCSV_Alternate()
		{
			var values = new[] { 1, 2, 3, 4, 5 };
			string csv = values.ToCSV(';');
			var expected = "1;2;3;4;5";
			Assert.AreEqual(expected, csv);
		}

		[TestMethod]
		public void ToCSV_Null()
		{
			int[] values = null;
			string csv = values.ToCSV(';');
			var expected = "";
			Assert.AreEqual(expected, csv);
		}

		[TestMethod]
		public void Sum_For_ULong()
		{
			var boxes = new Box[] { box1, box2, box3, box4 };
			var widths = boxes.Sum(b => b.WidthAsULong);
			Assert.AreEqual((ulong)10, widths);
		}

		[TestMethod]
		public void Sum_For_Nullable_ULong()
		{
			var boxes = new Box[] { box1, box2, box3, box4 };
			var widths = boxes.Sum(b => b.WidthAsNullableULong);
			Assert.AreEqual((ulong)10, widths);
		}

		[TestMethod]
		public void Sum_For_UInt()
		{
			var boxes = new Box[] { box1, box2, box3, box4 };
			var widths = boxes.Sum(b => b.WidthAsUInt);
			Assert.AreEqual((uint)10, widths);
		}

		[TestMethod]
		public void Sum_For_Nullable_UInt()
		{
			var boxes = new Box[] { box1, box2, box3, box4 };
			var widths = boxes.Sum(b => b.WidthAsNullableUInt);
			Assert.AreEqual((uint)10, widths);
		}

		[TestMethod]
		public void Sum_For_ULong_Ignore_Null_Value()
		{
			var boxes = new Box[] { box1, box2, null, box3, box4 };
			var widths = boxes.Sum(b => b.WidthAsULong);
			Assert.AreEqual((ulong)10, widths);
		}

		[TestMethod]
		public void Sum_For_Nullable_ULong_Ignore_Null_Value()
		{
			var boxes = new Box[] { box1, box2, null, box3, box4 };
			var widths = boxes.Sum(b => b.WidthAsNullableULong);
			Assert.AreEqual((ulong)10, widths);
		}

		[TestMethod]
		public void Sum_For_UInt_Ignore_Null_Value()
		{
			var boxes = new Box[] { box1, box2, null, box3, box4 };
			var widths = boxes.Sum(b => b.WidthAsUInt);
			Assert.AreEqual((uint)10, widths);
		}

		[TestMethod]
		public void Sum_For_Nullable_UInt_Ignore_Null_Value()
		{
			var boxes = new Box[] { box1, box2, null, box3, box4 };
			var widths = boxes.Sum(b => b.WidthAsNullableUInt);
			Assert.AreEqual((uint)10, widths);
		}


		[TestMethod]
		public void IsNullOrEmptyVsIsNotEmpty()
		{
			IEnumerable<Double> list = new List<Double> { 1, 2 };
			IEnumerable<Double> emptyList = new List<Double>();
			IEnumerable<Double> nullList = null;

			list.IsNullOrEmpty().Should().Be.False();
			list.IsNotEmpty().Should().Be.True();

			emptyList.IsNullOrEmpty().Should().Be.True();
			emptyList.IsNotEmpty().Should().Be.False();

			nullList.IsNullOrEmpty().Should().Be.True();
			nullList.IsNotEmpty().Should().Be.False();
		}

		[TestMethod]
		public void TestAppend()
		{
			var ints = Enumerable.Range(0, 3);
			ints = ints.Append(3);

			new[] { 0, 1, 2, 3 }.SequenceEqual(ints).Should().Be.True();
		}

		[TestMethod]
		public void TestPrepend()
		{
			var ints = Enumerable.Range(1, 3);
			ints = ints.Prepend(0);

			new[] { 0, 1, 2, 3 }.SequenceEqual(ints).Should().Be.True();
		}

		[TestMethod]
		public void TestToArray()
		{
			var intStrings = Enumerable.Range(1, 3).ToArray(i => i.ToString());

			new[] { "1", "2", "3" }.SequenceEqual(intStrings).Should().Be.True();
		}

		[TestMethod]
		public void TestToList()
		{
			var intStrings = Enumerable.Range(1, 3).ToList(i => i.ToString());

			new[] { "1", "2", "3" }.SequenceEqual(intStrings).Should().Be.True();
		}

		[TestMethod]
		public void TestSum()
		{
			// test uint sum
			var uints = Enumerable.Range(1, 5).ToList(i => (uint)i);
			uints.Sum().Should().Equal(15U);

			// test ulong sum
			var ulongs = Enumerable.Range(1, 5).ToList(i => (ulong)i);
			ulongs.Sum().Should().Equal(15UL);

			// test uint? sum
			var nullableUints = new uint?[] { 1, 2, 3, 4, 5, null };
			nullableUints.Sum().Should().Equal(15U);

			nullableUints = new uint?[0];
			nullableUints.Sum().Should().Equal(0U);

			nullableUints = new uint?[] { null };
			nullableUints.Sum().Should().Equal(0U);

			// test ulong? sum
			var nullableUlongs = new ulong?[] { 1, 2, 3, 4, 5, null };
			nullableUlongs.Sum().Should().Equal(15UL);

			nullableUlongs = new ulong?[0];
			nullableUlongs.Sum().Should().Equal(0UL);

			nullableUlongs = new ulong?[] { null };
			nullableUlongs.Sum().Should().Equal(0UL);
		}

		[TestMethod]
		public void TestContactWith()
		{
			List<string> strings = new List<string> { "1", "2", "3", "4", "5" };
			List<double> doubles = new List<double> { 123.4567, 123.4, 123.0, 4, 5 };
			List<string> stringsWithNull = new List<string> { "1", "2", null, "4", "5" };
			List<double?> doublesWithNull = new List<double?> { 123.4567, 123.4, null, 4, 5 };

			// Test default separator (strings)
			"1,2,3,4,5".Should().Equal(strings.ConcatWith());
			"1,2,,4,5".Should().Equal(stringsWithNull.ConcatWith());

			// Test non-default separator (string-specific)
			"1?2?3?4?5".Should().Equal(strings.ConcatWith("?"));
			"1?2??4?5".Should().Equal(stringsWithNull.ConcatWith("?"));

			// Test default separator (doubles)
			"123.46,123.40,123.00,4.00,5.00".Should().Equal(doubles.ConcatWith(formatString: "0.00"));
			"123.46,123.40,,4.00,5.00".Should().Equal(doublesWithNull.ConcatWith(formatString: "0.00"));

			// Test non-default separator (doubles)
			"123.46?123.40?123.00?4.00?5.00".Should().Equal(doubles.ConcatWith("?", "0.00"));
			"123.46?123.40??4.00?5.00".Should().Equal(doublesWithNull.ConcatWith("?", "0.00"));
        }

        #region BlockCopy

        [TestMethod]
        public void BlockCopyTest()
        {
            string[] source = new string[15];
            for (int i = 0; i < source.Length; i++)
            {
                source[i] = "string " + i.ToString();
            }

            int n = 0;
            foreach (string[] block in source.BlockCopy(10))
            {
                for (int i = 0; i < block.Length; i++)
                {
                    Assert.AreEqual(string.Format("string {0}", i + n), block[i]);
                }
                n += 10;
            }
        }

        [TestMethod]
        public void BlockCopyWithPadding()
        {
            string[] source = new string[15];
            for (int i = 0; i < source.Length; i++)
            {
                source[i] = "string " + i.ToString();
            }

            foreach (string[] block in source.BlockCopy(10, true))
            {
                Assert.AreEqual(10, block.Length);
            }
        }

        #endregion
    }
}