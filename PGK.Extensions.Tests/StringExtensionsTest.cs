using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should.Fluent;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace PGK.Extensions.Tests
{
	[TestClass]
	public class StringExtensionsTest
	{
		[TestMethod]
		public void TestIsEmpty()
		{
			/* positive testing */
			String testValue = String.Empty;
			Assert.IsTrue(testValue.IsEmpty());
			/* negative testing */
			testValue = "non empty";
			Assert.IsFalse(testValue.IsEmpty());

            /* null testing */
            testValue = null;
            Assert.IsTrue(testValue.IsEmpty());
		}

		[TestMethod]
		public void TestIsNotEmpty()
		{
			/* positive testing */
			String testValue = "the quick brown fox jumps over the lazy dog.";
			Assert.IsTrue(testValue.IsNotEmpty());
			/* negative testing */
			testValue = String.Empty;
			Assert.IsFalse(testValue.IsNotEmpty());

		}

		[TestMethod]
		public void TestIfEmpty()
		{
			/* positive testing */
			String testValue = String.Empty;
			String defaultValue = "default";
			Assert.AreEqual(testValue.IfEmpty(defaultValue), defaultValue);
			/* negative testing */
			testValue = "not default";
			defaultValue = "default";
			Assert.AreNotEqual(testValue.IfEmpty(defaultValue), defaultValue);

		}

		[TestMethod]
		public void TrimToMaxLength()
		{
			/* positive testing */
			String testValue = "the quick brown fox jumps over the lazy dog.";
			String resultValue = "the quick brown fox";
			Assert.AreEqual(testValue.TrimToMaxLength(19), resultValue);
		}
		[TestMethod]
		public void TrimToMaxLength_Short()
		{
			String testValue = "the quick brown ";
			String expected = "the quick brown ";
			var resultValue = testValue.TrimToMaxLength(19);
			Assert.AreEqual(expected, resultValue);
		}

		[TestMethod]
		public void TrimToMaxLength_SuffixShort()
		{
			String testValue = "the quick brown ";
			String expected = "the quick brown ";
			var resultValue = testValue.TrimToMaxLength(19, "...");
			Assert.AreEqual(expected, resultValue);
		}

		[TestMethod]
		public void Remove_Strings()
		{
			String testValue = "the quick brown fox jumps over the lazy dog.";
			var removee = new string[] { "the ", "over ", "brown " };
			var expected = "quick fox jumps lazy dog.";
			var result = testValue.Remove(removee);
			Assert.AreEqual(expected, result);
		}

		[TestMethod]
		public void Remove_Letters()
		{
			String testValue = "the quick brown fox jumps over the lazy dog.";
			var expected =  "th qck brwn fx jmps vr th lzy dg.";
			var result = testValue.Remove('a', 'e', 'i', 'o', 'u');
			Assert.AreEqual(expected, result);
		}

		[TestMethod]
	  public void IfEmptyOrWhiteSpace_Empty()
	  {
			var testValue = String.Empty;
			var def = "_default_";
			var result = testValue.IfEmptyOrWhiteSpace(def);
		  Assert.AreEqual(def, result);
	  }

		[TestMethod]
	  public void IfEmptyOrWhiteSpace_Spaces()
	  {
			var testValue = "   ";;
			var def = "_default_";
			var result = testValue.IfEmptyOrWhiteSpace(def);
		  Assert.AreEqual(def, result);
	  }
		[TestMethod]
	  public void IfEmptyOrWhiteSpace_Tab()
	  {
			var testValue = "\t";;
			var def = "_default_";
			var result = testValue.IfEmptyOrWhiteSpace(def);
		  Assert.AreEqual(def, result);
	  }
		[TestMethod]
	  public void IfEmptyOrWhiteSpace_Null()
	  {
			string testValue =null;
			var def = "_default_";
			var result = testValue.IfEmptyOrWhiteSpace(def);
		  Assert.AreEqual(def, result);
	  }
		[TestMethod]
	  public void IfEmptyOrWhiteSpace_String()
	  {
			var testValue = "Hello World";
			var def = "_default_";
			var result = testValue.IfEmptyOrWhiteSpace(def);
		  Assert.AreEqual(testValue, result);
	  }

		[TestMethod]
	  public void ToUpperFirstLetter_AllLower()
	  {
			var testValue = "hello world";
			var result = testValue.ToUpperFirstLetter();
			Assert.AreEqual("Hello world", result);
	  }
		[TestMethod]
	  public void ToUpperFirstLetter_AllUpper()
	  {
			var testValue = "HELLO WORLD";
			var result = testValue.ToUpperFirstLetter();
			Assert.AreEqual(testValue, result);
	  }

		[TestMethod]
	  public void ToUpperFirstLetter_Empty()
	  {
			var testValue = String.Empty;
			var result = testValue.ToUpperFirstLetter();
			Assert.AreEqual(String.Empty, result);
	  }

		[TestMethod]
	  public void ToUpperFirstLetter_Null()
	  {
			string testValue = null;
			var result = testValue.ToUpperFirstLetter();
			Assert.AreEqual(string.Empty, result);
	  }


		[TestMethod]
		public void Left_Simple()
		{
			var testValue = "123456789a123456789b";
			var result = testValue.Left(15);
			Assert.AreEqual("123456789a12345", result);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void Left_TooLong()
		{
			var testValue = "123456789a123456789b";
			var result = testValue.Left(25);
			Assert.IsTrue(false, "If we've reached here, we didn't get expected exception");
		}
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Left_Null()
		{
			string testValue = null;
			var result = testValue.Left(25);
			Assert.IsTrue(false, "If we've reached here, we didn't get expected exception");
		}
		[TestMethod]
		public void Right_Simple()
		{
			var testValue = "123456789a123456789b";
			var result = testValue.Right(15);
			Assert.AreEqual("6789a123456789b", result);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void Right_TooLong()
		{
			var testValue = "123456789a123456789b";
			var result = testValue.Right(25);
			Assert.IsTrue(false, "If we've reached here, we didn't get expected exception");
		}
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Right_Null()
		{
			string testValue = null;
			var result = testValue.Right(25);
			Assert.IsTrue(false, "If we've reached here, we didn't get expected exception");
		}

		[TestMethod]
		public void TrimToMaxLength_Null()
		{
			/* positive testing */
			String testValue = null;
			String expected = null;
			Assert.AreEqual(testValue.TrimToMaxLength(19), expected);
		}

		[TestMethod]
		public void TrimToMaxLength_Suffix()
		{
			/* positive testing */
			String testValue = "the quick brown fox jumps over the lazy dog.";
			String expected = "the quick brown fox...";
			var resultValue = testValue.TrimToMaxLength(19, "...");
			Assert.AreEqual(expected, resultValue);
		}

		[TestMethod]
		public void Reverse()
		{
			/* positive testing */
			String testValue = "dnpextensions";
			String validationValue = "snoisnetxepnd";
			Assert.AreEqual(testValue.Reverse(), validationValue);
		}

		[TestMethod]
		public void Reverse_Empty()
		{
			var testValue = String.Empty;
			Assert.AreEqual(testValue, testValue.Reverse());
		}

		[TestMethod]
		public void Reverse_Short()
		{
			var testValue = "A";
			Assert.AreEqual(testValue, testValue.Reverse());
		}

		[TestMethod]
		public void ConcatWith()
		{
			var testValue = "Hello";
			var result = testValue.ConcatWith(", ", "World", "!");
			var expected = "Hello, World!";
			Assert.AreEqual(expected, result);
		}

		[TestMethod]
		public void ToGuid()
		{
			var testValue = "123456789a123456789b123456789c12";
			var result = testValue.ToGuid();
			Assert.IsInstanceOfType(result, typeof(Guid));
			var ba = result.ToByteArray();
			// the above render in byte[] form: a 32-bit value, little-endian first, two 16-bit values(little endian), and 8 8-bit values.
			var baExpected = new byte[] { 0x78, 0x56, 0x34, 0x12, 0x12, 0x9a, 0x56, 0x34, 0x78, 0x9b, 0x12, 0x34, 0x56, 0x78, 0x9c, 0x12 };
			Assert.IsTrue(ba.SequenceEqual(baExpected));
		}

		[TestMethod]
		[ExpectedException(typeof(FormatException))]
		public void ToGuid_BadValue()
		{
			var testValue = "123456789z123456789z123456789z12";
			var result = testValue.ToGuid();
			Assert.IsTrue(false, "If we've reached here, we didn't get expected exception");
		}

		[TestMethod]
		public void ToGuidSave_GoodValue()
		{
			var testValue = "123456789a123456789b123456789c12";
			var result = testValue.ToGuidSave();
			Assert.IsInstanceOfType(result, typeof(Guid));
			var ba = result.ToByteArray();
			// the above render in byte[] form: a 32-bit value, little-endian first, two 16-bit values(little endian), and 8 8-bit values.
			var baExpected = new byte[] { 0x78, 0x56, 0x34, 0x12, 0x12, 0x9a, 0x56, 0x34, 0x78, 0x9b, 0x12, 0x34, 0x56, 0x78, 0x9c, 0x12 };
			Assert.IsTrue(ba.SequenceEqual(baExpected));
		}

		[TestMethod]
		public void ToGuidSave_BadValue()
		{
			var testValue = "123456789z123456789z123456789z12";
			var result = testValue.ToGuidSave();
			Assert.IsInstanceOfType(result, typeof(Guid));
			Assert.AreEqual(Guid.Empty, result);
		}

		[TestMethod]
		public void ToGuidSave_BadValueWithDefault()
		{
			var testValue = "123456789z123456789z123456789z12";
			var expected = Guid.NewGuid();
			var result = testValue.ToGuidSave(expected);
			Assert.IsInstanceOfType(result, typeof(Guid));
			Assert.AreEqual(expected, result);
		}

		[TestMethod]
		public void ToGuidSave_EmptyValueWithDefault()
		{
			var testValue = string.Empty;
			var expected = Guid.NewGuid();
			var result = testValue.ToGuidSave(expected);
			Assert.IsInstanceOfType(result, typeof(Guid));
			Assert.AreEqual(expected, result);
		}

		[TestMethod]
	  public void GetBefore()
	  {
			String testValue = "the quick brown fox jumps over the lazy dog.";
			var result = testValue.GetBefore("fox");
			var expected = "the quick brown ";
			Assert.AreEqual(expected, result);
	  }

		[TestMethod]
	  public void GetBefore_NotFound()
	  {
			String testValue = "the quick brown fox jumps over the lazy dog.";
			var result = testValue.GetBefore("xxx");
			var expected = string.Empty;
			Assert.AreEqual(expected, result);
	  }
		[TestMethod]
	  public void GetBefore_Beginning()
	  {
			String testValue = "the quick brown fox jumps over the lazy dog.";
			var result = testValue.GetBefore("the");
			var expected = string.Empty;
			Assert.AreEqual(expected, result);
	  }

		[TestMethod]
		public void GetBetween()
		{
			String testValue = "the quick brown fox jumps over the lazy dog.";
			var result = testValue.GetBetween("quick", "fox");
			var expected = "brown";
			Assert.AreEqual(expected, result);
			
		}
		[TestMethod]
		public void GetBetween_FirstNotFound()
		{
			String testValue = "the quick brown fox jumps over the lazy dog.";
			var result = testValue.GetBetween("qqqqq", "fox");
			var expected = string.Empty;
			Assert.AreEqual(expected, result);
		}

		[TestMethod]
		public void GetBetween_SecondNotFound()
		{
			String testValue = "the quick brown fox jumps over the lazy dog.";
			var result = testValue.GetBetween("quick", "xxx");
			var expected = string.Empty;
			Assert.AreEqual(expected, result);
		}
		[TestMethod]
		public void GetBetween_SecondBeforeFirst()
		{
			String testValue = "the quick brown fox jumps over the lazy dog.";
			var result = testValue.GetBetween("fox", "quick");
			var expected = string.Empty;
			Assert.AreEqual(expected, result);
		}

		[TestMethod]
		public void GetAfter()
		{
			String testValue = "the quick brown fox jumps over the lazy dog.";
			var result = testValue.GetAfter("over");
			var expected = "the lazy dog.";
			Assert.AreEqual(expected, result);
		}

		[TestMethod]
		public void GetAfter_NotFound()
		{
			String testValue = "the quick brown fox jumps over the lazy dog.";
			var result = testValue.GetAfter("xxx");
			var expected = string.Empty;
			Assert.AreEqual(expected, result);
		}
		[TestMethod]
		public void GetAfter_End()
		{
			String testValue = "the quick brown fox jumps over the lazy dog.";
			var result = testValue.GetAfter("dog.");
			var expected = string.Empty;
			Assert.AreEqual(expected, result);
		}

		[TestMethod]
		public void EnsureStartsWith_changing()
		{
			/* positive testing */
			var extension = "txt";
			var validationValue = ".txt";

			// Test with change
			Assert.AreEqual(validationValue, extension.EnsureStartsWith("."));
		}

		[TestMethod]
		public void TestEnsureStartsWith_NotChanging()
		{
			var validationValue = ".txt";
			// test without change : do nothing
			Assert.AreEqual(validationValue, validationValue.EnsureStartsWith("."));
		}


		[TestMethod]
		public void EnsureEndsWith_changing()
		{
			/* positive testing */
			var url = "http://www.pgk.de";
			var validationValue = "http://www.pgk.de/";
			Assert.AreEqual(validationValue, url.EnsureEndsWith("/"));
		}

		[TestMethod]
		public void EnsureEndsWith_NotChanging()
		{
			/* positive testing */
			var validationValue = "http://www.pgk.de/";
			Assert.AreEqual(validationValue, validationValue.EnsureEndsWith("/"));
		}


		[TestMethod]
		public void Repeat_1char()
		{
			/* positive testing */
			var character = "a";
			var validationValue = "aaaaa";
			Assert.AreEqual(character.Repeat(5), validationValue);
		}

		[TestMethod]
		public void Repeat_Nchars()
		{
			/* positive testing */
			var character = "abc";
			var validationValue = "abcabcabcabcabc";
			Assert.AreEqual(character.Repeat(5), validationValue);
		}


		[TestMethod]
		public void TestIsnumeric_Integer()
		{
			SetDotAsDecimalSeparator();

			/* positive testing */
			var validationValue = "12345";
			Assert.IsTrue(validationValue.IsNumeric());
		}
		[TestMethod]
		public void TestIsNumeric_Floating()
		{
			SetDotAsDecimalSeparator();

			/* positive testing */
			var validationValue = "1123.45";
			Assert.IsTrue(validationValue.IsNumeric());
		}
		[TestMethod]
		public void TestIsnumeric_BadString()
		{
			SetDotAsDecimalSeparator();

			/* negative testing */
			var validationValue = "123$a45";
			Assert.IsFalse(validationValue.IsNumeric());
		}

		[TestMethod]
		public void TestExtractDigits()
		{
			/* positive testing */
			var testValue = @"1aO2%&3(45=\";
			var validationValue = "12345";
			Assert.AreEqual(testValue.ExtractDigits(), validationValue);
		}

		[TestMethod]
		public void TestAdjustString()
		{
			string testValue = @"%&btf678&//(b hbg";
			string validationValue = @"btf678bhbg";
			Assert.AreEqual(testValue.AdjustInput(), validationValue);
		}

		[TestMethod]
		public void TestRemoveAllSpecialCharacters()
		{
			string testValue = @"%&btf678&//(b hbg";
			string validationValue = @"btf678bhbg";
			Assert.AreEqual(testValue.RemoveAllSpecialCharacters(), validationValue);
		}

		[TestMethod]
		public void TestIsEmptyOrWhiteSpace()
		{
			string notEmpty = "NotEmpty";
			string withSpaces = "     ";
			string empty = string.Empty;

			Assert.IsFalse(notEmpty.IsEmptyOrWhiteSpace());
			Assert.IsTrue(withSpaces.IsEmptyOrWhiteSpace());
			Assert.IsTrue(empty.IsEmptyOrWhiteSpace());
		}

		[TestMethod]
		public void ToTitleCase()
		{
			var testValue = "this is a test";
			var expectedResult = "This Is A Test";

			var result = testValue.ToTitleCase();

			Assert.AreEqual(expectedResult, result);
		}

		[TestMethod]
		public void ToTitleCase_MixedCase()
		{
			var weirdTestValue = "this IS a tEst";
			var expectedResult = "This IS A Test";    // UppperCase characters is the source string after the first of each word are lowered, unless the word is exactly 2 character.

			var result = weirdTestValue.ToTitleCase();

			Assert.AreEqual(expectedResult, result);
		}

		[TestMethod]
		public void SpaceOnUpper()
		{
			var value = "MyCamelCaseString";
			var expectedResult = "My Camel Case String";

			var numberTestValue = "MyCamel101CaseString";
			var expectedNumberTestResult = "My Camel 101 Case String";

			var allCaseTestValue = "MYCAMELCASESTRING";

			var result = value.SpaceOnUpper();
			var numberResult = numberTestValue.SpaceOnUpper();
			var allCaseresult = allCaseTestValue.SpaceOnUpper();

			Assert.AreEqual(result, expectedResult);
			Assert.AreNotEqual(result, value);

			// should put space even for number
			Assert.AreEqual(numberResult, expectedNumberTestResult);

			// shouldn't change
			Assert.AreEqual(allCaseresult, allCaseTestValue);
		}

		[TestMethod]
		public void PadBoth_NoTruncate()
		{
			string baseString = "asdfqwer";
			string expectedResult = "--asdfqwer--";
			Assert.AreEqual(expectedResult, baseString.PadBoth(12, '-'));
		}
		[TestMethod]
		public void PadBoth_TooShortTruncate()
		{
			string baseString = "asdfqwer";
			var expectedResult = "asdf";
			Assert.AreEqual(expectedResult, baseString.PadBoth(4, '-', true));
		}
		[TestMethod]
		public void PadBoth_TooShortNoTruncate()
		{
			string testValue = "asdfqwer";
			var result = testValue.PadBoth(4, '-');
			var expectedResult = testValue;
			Assert.AreEqual(expectedResult, result);
		}

		[TestMethod]
		public void PadBoth_Equal()
		{
			string testValue = "asdfqwer";
			var result = testValue.PadBoth(8, '-');
			var expectedResult = testValue;
			Assert.AreEqual(expectedResult, result);
		}

		[TestMethod]
		public void ToXDocument()
		{
			var testValue = "<elem1><elem2 attr1='xxxx'/><elem3>inner text</elem3></elem1>";
			var result = testValue.ToXDocument();
			Assert.IsInstanceOfType(result, typeof(XDocument));
			Assert.AreEqual("elem1", result.Root.Name);
			var elem2 = result.Root.Descendants().First();
			Assert.AreEqual("elem2", elem2.Name);
			Assert.AreEqual("xxxx", elem2.Attributes().First().Value);
		}

		[TestMethod]
		[ExpectedException(typeof(XmlException))]
		public void ToXDocument_BadXML()
		{
			var testValue = "<elem1><elem2 attr1='xxxx'><elem3></elem1>";
			var result = testValue.ToXDocument();
			Assert.IsTrue(false, "If we've reached here, we didn't get expected exception");
		}
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ToXDocument_NullXML()
		{
			string testValue = null;
			var result = testValue.ToXDocument();
			Assert.IsTrue(false, "If we've reached here, we didn't get expected exception");
		}

		[TestMethod]
		public void ToXmlDOM()
		{
			var testValue = "<elem1><elem2 attr1='xxxx'/><elem3>inner text</elem3></elem1>";
			var result = testValue.ToXmlDOM();
			Assert.IsInstanceOfType(result, typeof(XmlDocument));
			Assert.AreEqual("elem1", result.DocumentElement.Name);
			var elem2 = result.DocumentElement.ChildNodes[0];
			Assert.AreEqual("elem2", elem2.Name);
			Assert.AreEqual("xxxx", elem2.Attributes[0].Value);
		}

		[TestMethod]
		[ExpectedException(typeof(XmlException))]
		public void ToXmlDOM_BadXML()
		{
			var testValue = "<elem1><elem2 attr1='xxxx'><elem3></elem1>";
			var result = testValue.ToXmlDOM();
			Assert.IsTrue(false, "If we've reached here, we didn't get expected exception");
		}
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ToXmlDOM_NullXML()
		{
			string testValue = null;
			var result = testValue.ToXmlDOM();
			Assert.IsTrue(false, "If we've reached here, we didn't get expected exception");
		}
 
		[TestMethod]
		public void ToXElement()
		{
			var testValue = "<elem1><elem2 attr1='xxxx'/><elem3>inner text</elem3></elem1>";
			var result = testValue.ToXElement();
			Assert.IsInstanceOfType(result, typeof(XElement));
			Assert.AreEqual("elem1", result.Name);
			var elem2 = result.Descendants().First();
			Assert.AreEqual("elem2", elem2.Name);
			Assert.AreEqual("xxxx", elem2.Attributes().First().Value);
		}

		[TestMethod]
		[ExpectedException(typeof(XmlException))]
		public void ToXElement_BadXML()
		{
			var testValue = "<elem1><elem2 attr1='xxxx'><elem3></elem1>";
			var result = testValue.ToXElement();
			Assert.IsTrue(false, "If we've reached here, we didn't get expected exception");
		}
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ToXElement_NullXML()
		{
			string testValue = null;
			var result = testValue.ToXElement();
			Assert.IsTrue(false, "If we've reached here, we didn't get expected exception");
		}

		[TestMethod]
		public void ToXPath()
		{
			var testValue = "<elem1><elem2 attr1='xxxx'/><elem3>inner text</elem3></elem1>";
			var result = testValue.ToXPath();
			Assert.IsInstanceOfType(result, typeof(XPathNavigator));
			result.MoveToFirstChild();
			Assert.AreEqual("elem1", result.Name);
			var iter = result.Select(@"/elem1/elem2[@attr1]");
			iter.MoveNext();
			Assert.AreEqual("elem2", iter.Current.Name);
			iter = result.Select(@"/elem1/elem2/@attr1");
			iter.MoveNext();
			Assert.AreEqual("xxxx", iter.Current.Value);
		}

		[TestMethod]
		[ExpectedException(typeof(XmlException))]
		public void ToXPath_BadXML()
		{
			var testValue = "<elem1><elem2 attr1='xxxx'><elem3></elem1>";
			var result = testValue.ToXPath();
			Assert.IsTrue(false, "If we've reached here, we didn't get expected exception");
		}
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ToXPath_NullXML()
		{
			string testValue = null;
			var result = testValue.ToXPath();
			Assert.IsTrue(false, "If we've reached here, we didn't get expected exception");
		}




		[TestMethod]
		public void SecureStringConversion()
		{
			string testValue = @"asdf";
			SecureString validationValue = testValue.ToSecureString();
			Assert.AreEqual(testValue, validationValue.ToUnsecureString());
		}

		[TestMethod]
		public void ContainsEquivalenceTo()
		{
			string value;

			value = null;
			value.ContainsEquivalenceTo("something else").Should().Be.False();
			value.ContainsEquivalenceTo(null).Should().Be.True();

			value = "string containting the VaLuE with different case for each letter";
			value.ContainsEquivalenceTo("value").Should().Be.True();
			value.ContainsEquivalenceTo("VALUE").Should().Be.True();
			value.ContainsEquivalenceTo("VaLuE").Should().Be.True();

			value.ContainsEquivalenceTo("Not value").Should().Be.False();
		}

		[TestMethod]
		public void EquivalentTo()
		{
			string value;

			value = null;
			value.EquivalentTo("something else").Should().Be.False();
			value.EquivalentTo(null).Should().Be.True();

			value = "VaLuE";
			value.EquivalentTo("value").Should().Be.True();
			value.EquivalentTo("VALUE").Should().Be.True();
			value.EquivalentTo("VaLuE").Should().Be.True();
		}

		/// <summary>Tests the SubstringFrom(int index) from StringExtensions.cs</summary>
		[TestMethod]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TestSubstringFrom()
		{
			string testValue = null;

			testValue = "dnpextensions";

			Assert.AreEqual(testValue.SubstringFrom(0), "dnpextensions");
			Assert.AreEqual(testValue.SubstringFrom(1), "npextensions");
			Assert.AreEqual(testValue.SubstringFrom(5), "tensions");
			Assert.AreEqual(testValue.SubstringFrom(20), string.Empty);
		}

		private static void SetDotAsDecimalSeparator()
		{
			var copy = Thread.CurrentThread.CurrentCulture.Clone().CastTo<CultureInfo>();

			copy.NumberFormat.NumberDecimalSeparator = ".";
			Thread.CurrentThread.CurrentCulture = copy;
		}

		[TestMethod]
		public void ReplaceAll_Predicate()
		{
			var str = "White Red Blue Green Yellow Black Gray";
			var achromaticColors = new[] {"White", "Black", "Gray"};
			str = str.ReplaceAll(achromaticColors, v => "[" + v + "]");
			Assert.AreEqual(str, "[White] Red Blue Green Yellow [Black] [Gray]");
		}

		[TestMethod]
		public void ReplaceAll_ManyToOne()
		{
			var str = "White Red Blue Green Yellow Black Gray";
			var achromaticColors = new[] { "White", "Black", "Gray" };
			str = str.ReplaceAll(achromaticColors, "[AchromaticColor]");
			Assert.AreEqual(str, "[AchromaticColor] Red Blue Green Yellow [AchromaticColor] [AchromaticColor]");
		}

		[TestMethod]
		public void ReplaceAll_ManyToMany()
		{
			var str = "White Red Blue Green Yellow Black Gray";
			var achromaticColors = new[] {"White", "Black", "Gray"};
			var exquisiteColors = new[] {"FloralWhite", "Bistre", "DavyGrey"};
			str = str.ReplaceAll(achromaticColors, exquisiteColors);
			Assert.AreEqual(str, "FloralWhite Red Blue Green Yellow Bistre DavyGrey");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void ReplaceAll_ManyToMany_TooLong()
		{
			var str = "White Red Blue Green Yellow Black Gray";
			var achromaticColors = new[] { "White", "Black", "Gray" };
			var exquisiteColors = new[] { "FloralWhite", "Bistre", "DavyGrey", "mismatched value"};
			str = str.ReplaceAll(achromaticColors, exquisiteColors);
			Assert.IsTrue(false, "If we've reached here, we didn't get expected exception");
		}
		[TestMethod]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void ReplaceAll_ManyToMany_TooShort()
		{
			var str = "White Red Blue Green Yellow Black Gray";
			var achromaticColors = new[] { "White", "Black", "Gray", "mismatched value" };
			var exquisiteColors = new[] { "FloralWhite", "Bistre", "DavyGrey" };
			str = str.ReplaceAll(achromaticColors, exquisiteColors);
			Assert.IsTrue(false, "If we've reached here, we didn't get expected exception");
		}

		[TestMethod]
		public void IsMatchingTo_Matching()
		{
				var s = "12345";
				Assert.IsTrue(s.IsMatchingTo(@"^\d+$"));
		}

		[TestMethod]
		public void IsMatchingTo_NotMatching()
		{
			var s = "12q345";
			Assert.IsFalse(s.IsMatchingTo(@"^\d+$"));
		}

		[TestMethod]
		public void IsMatchingTo_Matching_Options()
		{
			var s = "abcdef";
			Assert.IsTrue(s.IsMatchingTo(@"^[A-Z]+$", RegexOptions.IgnoreCase));
		}


		[TestMethod]
		public void TestExtractArguments()
		{
			var str = "My name is Aleksey Nagovitsyn. I'm from Russia.";
			IEnumerable<string> args;

			args = str.ExtractArguments(@"My name is {1} {0}. I'm from {2}.");
			
			Assert.AreEqual(args.Count(), 3);
			Assert.AreEqual(args.ElementAt(0), "Nagovitsyn");
			Assert.AreEqual(args.ElementAt(1), "Aleksey");
			Assert.AreEqual(args.ElementAt(2), "Russia");

			args = str.ExtractArguments(@"My name is {1} {0}. I'm from {2}.", StringExtensions.ComparsionTemplateOptions.Whole);
			Assert.AreEqual(args.Count(), 3);
			Assert.AreEqual(args.ElementAt(0), "Nagovitsyn");
			Assert.AreEqual(args.ElementAt(1), "Aleksey");
			Assert.AreEqual(args.ElementAt(2), "Russia");

			args = str.ExtractArguments(@"My name is {1} {0}.");
			Assert.AreEqual(args.Count(), 2);
			Assert.AreEqual(args.ElementAt(0), "Nagovitsyn");
			Assert.AreEqual(args.ElementAt(1), "Aleksey");

			args = str.ExtractArguments(@"I'm from {0}.");
			Assert.AreEqual(args.Count(), 1);
			Assert.AreEqual(args.ElementAt(0), "Russia");

			args = str.ExtractArguments(@"I'm FROM {0}.", StringExtensions.ComparsionTemplateOptions.Default, RegexOptions.IgnoreCase);
			Assert.AreEqual(args.Count(), 1);
			Assert.AreEqual(args.ElementAt(0), "Russia");

			args = str.ExtractArguments(@"I'm from {0}.", StringExtensions.ComparsionTemplateOptions.FromStart);
			Assert.AreEqual(args.Count(), 0);

			args = str.ExtractArguments(@"I'm from {0}.", StringExtensions.ComparsionTemplateOptions.AtTheEnd);
			Assert.AreEqual(args.Count(), 1);
			Assert.AreEqual(args.ElementAt(0), "Russia");
		}

        [TestMethod]
        public void ContainsAnyTest()
        {
            string test = "abc";

            if(test.ContainsAny("x", "y", "z"))
                Assert.Fail("The string 'abc' does not contain 'x', 'y' or 'z'");

            if (!test.ContainsAny("a", "y", "z"))
                Assert.Fail("The string 'abc' does contain 'a'");

            if(!test.ContainsAny(StringComparison.CurrentCultureIgnoreCase, "A", "y", "z"))
                Assert.Fail("The string 'abc' does contain 'a'");

            if (!test.ContainsAny(string.Empty))
                Assert.Fail("A non-null string will always contain empty string");

        }

        [TestMethod]
        public void ContainsAllTest()
        {
            string test = "abc";

            if (test.ContainsAll("x", "y", "z"))
                Assert.Fail("The string 'abc' does not contain 'x', 'y' and 'z'");

            if (!test.ContainsAll("a", "b", "c"))
                Assert.Fail("The string 'abc' does contain 'a', 'b' and 'c'");

            if(!test.ContainsAll(StringComparison.CurrentCultureIgnoreCase, "A", "B", "C"))
                Assert.Fail("The string 'abc' does contain 'a', 'b' and 'c'");
        }

        [TestMethod]
        public void EqualsAnyTest()
        {
            string test = "a";

            if (test.EqualsAny(StringComparison.CurrentCultureIgnoreCase, "x", "y", "z"))
                Assert.Fail("The string 'a' does not equal 'x', 'y' or 'z'");

            if (!test.EqualsAny(StringComparison.CurrentCultureIgnoreCase, "a", "y", "z"))
                Assert.Fail("The string 'a' is equal to 'a'");

            if (!test.EqualsAny(StringComparison.CurrentCultureIgnoreCase, "A", "y", "z"))
                Assert.Fail("The string 'a' is equal to 'a'");
        }

        [TestMethod]
        public void IsLike()
        {
            // begins with
            Assert.IsTrue("testing".IsLike("test*"));

            // ends with
            Assert.IsTrue("testing".IsLike("*ing"));

            // begins and ends with
            Assert.IsTrue("testing".IsLike("t*g"));

            // contains
            Assert.IsTrue("testing".IsLike("*estin*"));

            // Any 
            Assert.IsTrue("testing".IsLike("*"));

            // multiple contains
            Assert.IsTrue("testing".IsLike("t*s*i*g"));

            // whole word
            Assert.IsTrue("testing".IsLike("testing*"));
            Assert.IsFalse("testing".IsLike("testing*testing"));

            // whole word
            Assert.IsTrue("testing".IsLike("*testing"));

            // whole word
            Assert.IsTrue("testing".IsLike("*testing*"));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TruncateTest()
        {
            string test = "abcdefghijklmnopqrstuvwxyz";
            string t = test.Truncate(10, true);
            Assert.AreEqual(t, "abcdefg...");

            test = "abcdefghij";
            t = test.Truncate(10, true);
            Assert.AreEqual(t, test);

            test = null;
            t = test.Truncate(10, true);
            Assert.AreEqual(t, test);

            test = string.Empty;
            t = test.Truncate(10, true);
            Assert.AreEqual(t, test);

            test = "a";
            t = test.Truncate(10, true);
            Assert.AreEqual(t, test);

            test = "abcd";
            t = test.Truncate(1, false);
            Assert.AreEqual(t, "a");

            test = "abc";
            t = test.Truncate(1, true);
            Assert.AreEqual(t, "...");
        }
	}

}
