using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PGK.Extensions.Tests.TestObjects;

namespace PGK.Extensions.Tests
{
		[TestClass]
	public class ComparableExtensionsTests
	{
			[TestMethod]
			public void IsBetween_Value_Default_Middle()
			{
				Assert.IsTrue(5.IsBetween(3, 7));
			}

			[TestMethod]
			public void IsBetween_Value_Default_Low()
			{
				Assert.IsTrue(3.IsBetween(3, 7));
			}

			[TestMethod]
			public void IsBetween_Value_Default_High()
			{
				// NOTE: As Implemented, this is true.  However, in some context, we may want to include the low, but not the high end.
				Assert.IsTrue(7.IsBetween(3, 7));
			}

			[TestMethod]
			public void IsBetween_Value_Default_Outside()
			{
				Assert.IsFalse(15.IsBetween(3, 7));
			}
			//-----------------------------------------------

			[TestMethod]
			public void IsBetween_Value_Default_Middle_inverted()
			{
				Assert.IsTrue(5.IsBetween(7, 3));
			}

			[TestMethod]
			public void IsBetween_Value_Default_Low_inverted()
			{
				Assert.IsTrue(3.IsBetween(7, 3));
			}

			[TestMethod]
			public void IsBetween_Value_Default_High_inverted()
			{
				Assert.IsTrue(7.IsBetween(7, 3));
			}

			[TestMethod]
			public void IsBetween_Value_Default_Outside_inverted()
			{
				Assert.IsFalse(15.IsBetween(7, 3));
			}

			//-----------------------------------------------
			[TestMethod]
			public void IsBetween_Ref_Default_Middle()
			{
				var high = new Box(10, 15, 20);
				var low = new Box(5, 20, 20);
				var target = new Box(7, 20, 20);
				Assert.IsTrue(target.IsBetween(low, high));
			}

			[TestMethod]
			public void IsBetween_Ref_Default_Low()
			{
				var high = new Box(10, 15, 20);
				var low = new Box(5, 20, 20);
				var target = new Box(5, 20, 20);
				Assert.IsTrue(target.IsBetween(low, high));
			}

			[TestMethod]
			public void IsBetween_Ref_Default_High()
			{
				var high = new Box(10, 15, 20);
				var low = new Box(5, 20, 20);
				var target = new Box(10, 15, 20);
				Assert.IsTrue(target.IsBetween(low, high));
			}

			[TestMethod]
			public void IsBetween_Ref_Default_Outside()
			{
				var high = new Box(10, 15, 20);
				var low = new Box(5, 20, 20);
				var target = new Box(15, 25, 20);
				Assert.IsFalse(target.IsBetween(low, high));
			}
		//-----------------------------------------------
			[TestMethod]
			public void IsBetween_Ref_Specific_Middle()
			{
				var high = new Box(10, 20, 20);
				var low = new Box(5, 10, 20);
				var target = new Box(17, 15, 20);
				Assert.IsTrue(target.IsBetween(low, high, new Box.LengthFirst()));
			}

			[TestMethod]
			public void IsBetween_Ref_Specific_Low()
			{
				var high = new Box(10, 20, 20);
				var low = new Box(5, 10, 20);
				var target = new Box(5, 10, 20);
				Assert.IsTrue(target.IsBetween(low, high, new Box.LengthFirst()));
			}

			[TestMethod]
			public void IsBetween_Ref_Specific_High()
			{
				var high = new Box(10, 20, 20);
				var low = new Box(5, 10, 20);
				var target = new Box(10, 20, 20);
				Assert.IsTrue(target.IsBetween(low, high, new Box.LengthFirst()));
			}

			[TestMethod]
			public void IsBetween_Ref_Specific_Outside()
			{
				var high = new Box(10, 20, 20);
				var low = new Box(5, 10, 20);
				var target = new Box(7, 25, 20);
				Assert.IsFalse(target.IsBetween(low, high, new Box.LengthFirst()));
			}

			[TestMethod]
			[ExpectedException(typeof(ArgumentNullException))]
			public void IsBetween_Ref_Specific_Null()
			{
				var high = new Box(10, 20, 20);
				var low = new Box(5, 10, 20);
				var target = new Box(7, 25, 20);
				target.IsBetween(low, high, null);
				Assert.IsTrue(false, "Should not get here");
			}

			[TestMethod]
			public void AscendingComparer()
			{
				String testValue = "the quick brown fox jumps over the lazy dog.";
				var expected =      "        .abcdeeefghhijklmnoooopqrrsttuuvwxyz";
				var chars = testValue.ToCharArray();
				Array.Sort(chars, new ComparableExtensions.AscendingComparer<char>());
				var result = new string(chars);
				Assert.AreEqual(expected, result);
				
			}

			[TestMethod]
			public void DescendingComparer()
			{
				String testValue = "1357924680";
				var expected      = "9876543210";
				var chars = testValue.ToCharArray();
				Array.Sort(chars, new ComparableExtensions.DescendingComparer<char>());
				var result = new string(chars);
				Assert.AreEqual(expected, result);

			}
	}
}
