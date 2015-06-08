using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PGK.Extensions.Tests
{
    [TestClass]
    public class ListExtensionTest
    {

        [TestMethod]
        public void TestJoinT()
        {
            List<int> list = new List<int>();

            for (int i = 0; i < 100; i++)
            {
                list.Add(i);
            }

            // result needs to be checked up manualy
            //Assert.Inconclusive(list.Join<int>(", "));

            string expected = "";
            for (int i = 0; i < 100; i++)
            {
                expected += i;
                if (i != 99)
                    expected += ", ";
            }
            Assert.AreEqual(list.Join<int>(", "), expected);
        }

		[TestMethod]
		public void JoinT_Single()
		{
			var list = new List<int>();
			list.Add(50);
			Assert.AreEqual("50", list.Join(","));
		}

		[TestMethod]
		public void JoinT_Null()
		{
			// NOTE: This test the implementation as originally written.
			// However, it probably not correct.  We probably want to throw a ArgumentNullExecption
			List<int> list = null;
			Assert.AreEqual("", list.Join(","));
		}

		[TestMethod]
		public void JoinT_Empty()
		{
			var list = new List<int>();
			Assert.AreEqual("", list.Join(","));
		}
        /// <summary>
        ///A test for InsertUnique
        ///</summary>
        /// <remarks>
        /// 	Contributed by dkillewo, http://www.codeplex.com/site/users/view/dkillewo
        /// </remarks>
        [TestMethod]
        public void InsertUniqueTest()
        {
            var actual = new List<int> { 1, 2, 3, 4, 5 };
            var expected = new List<int>(actual);
            actual.InsertUnique<int>(1, 3);

            Assert.AreEqual(actual.Count, expected.Count, "A not unique item was inserted");

            for (int i = 0; i < actual.Count; i++)
                Assert.AreEqual(expected[i], actual[i]);

            actual.InsertUnique<int>(2, 6);

            Assert.AreEqual(actual.Count, expected.Count + 1);
            Assert.AreEqual(actual[2], 6, "Item not correctly inserted");
        }

        /// <summary>
        ///A test for InsertRanfeUnique
        ///</summary>
        /// <remarks>
        /// 	Contributed by dkillewo, http://www.codeplex.com/site/users/view/dkillewo
        /// </remarks>
        [TestMethod]
        public void InsertRangeUniqueTest()
        {
            var actual = new List<int> { 1, 2, 3, 4, 5 };
            var expected = new List<int>(actual);
            expected.InsertRange(3, new int[] { 7, 9, 10 });
            var nValues = new List<int> { 2, 4, 5, 7, 9, 10 };

            actual.InsertRangeUnique(3, nValues);

            Assert.AreEqual(expected.Count, actual.Count, "Wrong amount of items inserted");

            for (int i = 0; i < actual.Count; i++)
                Assert.AreEqual(expected[i], actual[i], "Incorrect at index {0}".FormatWith(i));
        }

        /// <summary>
        ///A test for IndexOfTest
        ///</summary>
        /// <remarks>
        /// 	Contributed by dkillewo, http://www.codeplex.com/site/users/view/dkillewo
        /// </remarks>
        [TestMethod]
        public void IndexOfTest()
        {
            var actual = new List<int> { 1, 2, 3, 4, 5 };
            Assert.AreEqual(2, actual.IndexOf(n => n == 3));
            Assert.AreNotEqual(3, actual.IndexOf(n => n == 6));
        }

        /// <summary>
        ///A test for MergeTest
        ///</summary>
        /// <remarks>
        /// 	Contributed by dkillewo, http://www.codeplex.com/site/users/view/dkillewo
        /// </remarks>
        [TestMethod]
        public void MergeTest()
        {
            var list1 = new List<int> {1, 2, 3, 4, 5};
            var list2 = new List<int> {6, 7, 8, 9, 10};
            var expected = new List<int>(list1);
            expected.InsertRange(list1.Count, list2);
            var actual = list1.Merge(list2);
            var invalidList = new List<int>(expected);
            invalidList.Remove(4);

            Should.Core.Assertions.Assert.Equal(expected,actual);
            Should.Core.Assertions.Assert.Equal(expected,list1);
            Should.Core.Assertions.Assert.NotEqual(invalidList, actual);
        }

    }
}
