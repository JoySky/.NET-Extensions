using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should.Fluent;

namespace PGK.Extensions.Tests
{
	[TestClass]
	public class DictionaryExtensionsTests
	{
		[TestMethod]
		public void TestSort()
		{
			// Arrange
			var notSortedDictionary = new Dictionary<int, string>
									  {
										{1, "a"},
										{3, "c"},
										{2, "b"}
									  };
			var sortedDictionary = new Dictionary<int, string>
									  {
										{1, "a"},
										{2, "b"},
										{3, "c"},
									  };
			// Act
			var result = notSortedDictionary.Sort();
			var sortByCompareResult = notSortedDictionary.Sort(new ComparableExtensions.AscendingComparer<int>());

			// Assert
			result.Should().Equal(sortedDictionary);
			result.Should().Not.Equal(notSortedDictionary);
			sortByCompareResult.Should().Equal(sortedDictionary);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Sort_NullSource()
		{
			Dictionary<int, string> testValue = null;
			var result = testValue.Sort();
			Assert.IsTrue(false, "If we've reached here, we didn't get expected exception");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Sort_NullSourceWithComparer()
		{
			Dictionary<int, string> testValue = null;
			var result = testValue.Sort(new ComparableExtensions.AscendingComparer<int>());
			Assert.IsTrue(false, "If we've reached here, we didn't get expected exception");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Sort_NullComparer()
		{
			var testValue = new Dictionary<int, string>();
			IComparer<int> comparer = null;
			var result = testValue.Sort(comparer);
			Assert.IsTrue(false, "If we've reached here, we didn't get expected exception");
		}


		[TestMethod]
		public void TestSortByValue()
		{
			// Arrange
			var notSortedDictionary = new Dictionary<int, string>
									  {
										{1, "a"},
										{3, "c"},
										{2, "b"}
									  };
			var sortedDictionary = new Dictionary<int, string>
									  {
										{1, "a"},
										{2, "b"},
										{3, "c"},
									  };
			// Act
			var result = notSortedDictionary.SortByValue();

			// Assert
			result.Should().Equal(sortedDictionary);
			result.Should().Not.Equal(notSortedDictionary);
		}

		[TestMethod]
		public void TestInvert()
		{
			// Arrange
			var notInvertedDictionary = new Dictionary<int, string>
									  {
										{1, "a"},
										{2, "b"},
										{3, "c"},
									  };
			var invertedDictionary = new Dictionary<string, int>
									  {
										{"a", 1},
										{"b", 2},
										{"c", 3}
									  };
			// Act
			var result = notInvertedDictionary.Invert();

			// Assert
			result.Should().Equal(invertedDictionary);
			result.GetType().Should().Not.Equal(notInvertedDictionary);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Invert_Null()
		{
			Dictionary<int, string> testValue = null;
			var result = testValue.Invert();
			Assert.IsTrue(false, "If we've reached here, we didn't get expected exception");
		}

		[TestMethod]
		public void GetFirstValue()
		{
			const string defaultValue = "Default";
			var dictionary = new Dictionary<int, string>
									  {
										{1, "a"},
										{2, "b"},
										{3, "c"},
									  };

			dictionary.GetFirstValue(defaultValue, 3, 2, 1).Should().Equal(dictionary[3]);
			dictionary.GetFirstValue(defaultValue, 9, 8, 1, 3).Should().Equal(dictionary[1]);
			dictionary.GetFirstValue(defaultValue, 9, 8).Should().Equal(defaultValue);
		}

		[TestMethod]
		public void TestToHashTable()
		{
			// Arrange
			var ht = new Hashtable
					 {
						{"house", "Dwelling"},
						{"car", "Means of transport"},
						{"book", "Collection of printed words"},
						{"apple", "Edible fruit"}
					 };
			// Add elements to the table 

			var dic = new Dictionary<string, string>
					  {
							{"house", "Dwelling"},
							{"car", "Means of transport"},
							{"book", "Collection of printed words"},
							{"apple", "Edible fruit"}
					  };
			// Act
			var result = dic.ToHashTable();

			// Assert
			result.Should().Equal(ht);
			result.Should().Not.Equal(dic);
		}

		[TestMethod]
		public void TestGetOrDefault()
		{
			var dictionary = new Dictionary<string, string> { { "foo", "bar" } };
			var bar = dictionary.GetOrDefault("foo");
			bar.Should().Be.Equals("bar");

			var noBar = dictionary.GetOrDefault("foo?");
			noBar.Should().Be.Equals(default(string));
		}

		[TestMethod]
		public void TestGetOrThrowKeyIsFound()
		{
			var dictionary = new Dictionary<string, string> { { "foo", "bar" } };
			dictionary.GetOrThrow("foo", new ApplicationException());
		}

		[TestMethod]
		[ExpectedException(typeof(ApplicationException))]
		public void TestGetOrThrowKeyIsNotFound()
		{
			var dictionary = new Dictionary<string, string> { { "foo", "bar" } };
			dictionary.GetOrThrow("foo?", new ApplicationException());
		}
	}
}