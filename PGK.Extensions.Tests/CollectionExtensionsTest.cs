using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PGK.Extensions.Tests
{
	[TestClass]
	public class CollectionExtensionsTest
	{

        [TestMethod]
        public void GetRandomItem()
        {
            var testValue = new List<int> { 1, 2, 4, 5, 7, 8 };
            
            var itemA = testValue.GetRandomItem();
            Assert.IsTrue(testValue.Contains(itemA));

            var itemB = testValue.GetRandomItem(DateTime.Now.Millisecond);
            Assert.IsTrue(testValue.Contains(itemB));

            var itemC = testValue.GetRandomItem(new Random());
            Assert.IsTrue(testValue.Contains(itemC));
        }


		[TestMethod]
		public void AddUnique()
		{
			var testValue = new List<int> { 1, 2, 4, 5, 7, 8 };
			Assert.IsTrue(testValue.AddUnique(1));
			Assert.IsFalse(testValue.AddUnique(3));
			Assert.IsTrue(testValue.AddUnique(5));
			Assert.IsFalse(testValue.AddUnique(6));
			var expected = new int[] { 1, 2, 4, 5, 7, 8, 3, 6 };
			Assert.IsTrue(testValue.SequenceEqual(expected));
		}
		[TestMethod]
		public void AddUniqueRange()
		{
			var testValue = new List<int> { 1, 2, 4, 5, 7, 8 };
			var addee = new int[] { 1, 3, 5, 6 };
			var expected = new int[] { 1, 2, 4, 5, 7, 8, 3, 6 };
			var added = testValue.AddRangeUnique(addee);
			Assert.AreEqual(2, added);
			Assert.IsTrue(testValue.SequenceEqual(expected));
		}

		[TestMethod]
		public void RemoveWhere()
		{
			var testValue = new List<int> { 1, 2, 4, 5, 7, 8 };
			testValue.RemoveWhere(n => (n & 1) == 1);
			var expected = new[] { 2, 4, 8 };
			Assert.IsTrue(testValue.SequenceEqual(expected));
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void RemoveWhere_Null()
		{
			ICollection<int> testValue = null;
			testValue.RemoveWhere(n => (n & 1) == 1);
			Assert.IsTrue(false, "If we've reached here, we didn't get expected exception");
		}

	}
}
