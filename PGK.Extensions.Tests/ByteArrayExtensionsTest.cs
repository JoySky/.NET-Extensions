using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace PGK.Extensions.Tests
{
	
	
	/// <summary>
	///This is a test class for ByteArrayExtensionsTest and is intended
	///to contain all ByteArrayExtensionsTest Unit Tests
	///</summary>
	[TestClass()]
	public class ByteArrayExtensionsTest
	{

		/// <summary>
		///A test for FindArrayInArray
		///</summary>
		/// <remarks>
		/// 	Contributed by dkillewo, http://www.codeplex.com/site/users/view/dkillewo
		/// </remarks>
		[TestMethod()]
		public void FindArrayInArrayTest()
		{
			var subBytes = new byte[] { 1, 2, 3, 1, 4, 3 };
			var bytes = new List<byte> {3, 4, 2, 1, 4, 5, 2};
			bytes.AddRange(subBytes);
			bytes.AddRange(new byte[]{7,4,5,8,3,1});

			var pos = bytes.ToArray().FindArrayInArray(subBytes);

			Assert.AreEqual(7,pos);

		}
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void FindArrayInArray_NullTarget()
		{
			var srcBytes = new byte[] { 1, 2, 3, 1, 4, 3 };

			var pos = srcBytes.FindArrayInArray(null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void FindArrayInArray_NullSource()
		{
			byte[] srcBytes = null;
			var subBytes = new byte[] { 1, 2, 3, 1, 4, 3 };

			srcBytes.FindArrayInArray(subBytes);
		}
		[TestMethod]
		public void FindArrayInArray_AtStart()
		{
			var srcBytes = new byte[] { 1, 2, 3, 4, 1, 2, 3, 4 };
			var targetBytes = new byte[] { 1, 2, 3, 4};

			var pos = srcBytes.FindArrayInArray(targetBytes);
			Assert.AreEqual(0, pos);
		}
		[TestMethod]
		public void FindArrayInArray_AtEnd()
		{
			var srcBytes = new byte[] { 1, 2, 3, 5, 1, 2, 3, 4 };
			var targetBytes = new byte[] { 1, 2, 3, 4 };

			var pos = srcBytes.FindArrayInArray(targetBytes);
			Assert.AreEqual(4, pos);
		}

		[TestMethod]
		public void FindArrayInArray_InNearMatch()
		{
			var srcBytes = new byte[] {0, 1, 2, 1, 2, 1, 2, 3, 5 };
			var targetBytes = new byte[] { 1, 2, 1, 2, 3 };

			var pos = srcBytes.FindArrayInArray(targetBytes);
			Assert.AreEqual(3, pos);
		}

		[TestMethod]
		public void FindArrayInArray_PerfectMatch()
		{
			var srcBytes = new byte[] { 1, 2, 1, 2, 3 };
			var targetBytes = new byte[] { 1, 2, 1, 2, 3 };

			var pos = srcBytes.FindArrayInArray(targetBytes);
			Assert.AreEqual(0, pos);
		}

		[TestMethod]
		public void FindArrayInArray_EmptyTarget()
		{
			var srcBytes = new byte[] { 1, 2, 1, 2, 3 };
			var targetBytes = new byte[] { };

			var pos = srcBytes.FindArrayInArray(targetBytes);
			Assert.AreEqual(0, pos);
			
		}

		[TestMethod]
		public void FindArrayInArray_NoMatch()
		{
			var srcBytes = new byte[] { 1, 2, 1, 2, 3 };
			var targetBytes = new byte[] {4,5,6,7 };

			var pos = srcBytes.FindArrayInArray(targetBytes);
			Assert.AreEqual(-1, pos);

		}
	}
}
