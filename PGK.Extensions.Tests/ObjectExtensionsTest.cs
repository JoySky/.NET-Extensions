using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should.Fluent;

namespace PGK.Extensions.Tests
{
	[TestClass]
	public class ObjectExtensionsTest
	{

		[TestMethod]
		public void EqualsAny_Found()
		{
			Assert.IsTrue(5.EqualsAny(1, 2, 3, 4, 5));
		}

		[TestMethod]
		public void EqualsAny_NotFound()
		{
			Assert.IsFalse(15.EqualsAny(1, 2, 3, 4, 5));
		}

		[TestMethod]
		public void EqualsAny_EmptyParam()
		{
			Assert.IsFalse(15.EqualsAny());
		}

		[TestMethod]
		public void EqualsAny_EmptyArray()
		{
			int[] target = { };
			Assert.IsFalse(15.EqualsAny(target));
		}

		[TestMethod]
		public void EqualsNone_Found()
		{
			Assert.IsFalse(5.EqualsNone(1, 2, 3, 4, 5));
		}

		[TestMethod]
		public void EqualsNone_NotFound()
		{
			Assert.IsTrue(15.EqualsNone(1, 2, 3, 4, 5));
		}

		[TestMethod]
		public void EqualsNone_EmptyParam()
		{
			Assert.IsTrue(15.EqualsNone());
		}

		[TestMethod]
		public void EqualsNone_EmptyArray()
		{
			int[] target = { };
			Assert.IsTrue(15.EqualsNone(target));
		}


		[TestMethod]
		public void CastTo_nonNumericToInt()
		{
			// Arrange
			string value = "test";

			// Act
			int result = value.CastTo<Int32>();

			// Assert
			result.Should().Equal(default(int));
		}

		[TestMethod]
		public void TestCastAs()
		{
			// Arrange
			var value = new List<string>();

			// Act
			var result = value.CastAs<List<string>>();

			// Assert
			result.Should().Equal(new List<string>());
		}

		[TestMethod]
		public void TestConvertToAndIgnoreException()
		{
			const string invalidValue = "test";
			const string stringValue = "1234";
			const int value = 1234;
			const int defaultValue = 999;

			invalidValue.ConvertToAndIgnoreException<int>().Should().Equal(0);
			invalidValue.ConvertToAndIgnoreException(defaultValue).Should().Equal(defaultValue);

			stringValue.ConvertToAndIgnoreException<int>().Should().Equal(value);
			stringValue.ConvertToAndIgnoreException(defaultValue).Should().Equal(value);
		}

		[TestMethod, ExpectedException(typeof(Exception))]
		public void TestConvertToAgainstNotConvertibleStringOnly()
		{
			"NotConvertibleToInt".ConvertTo<int>();
		}

		[TestMethod]
		public void TestConvertToAgainstStringDigit()
		{
			// Arrange
			var value = 123;
			var stringValue = "123";

			// Act
			var stringResult = stringValue.ConvertTo<int>();

			// Assert
			stringResult.Should().Equal(value);
			stringResult.Should().Not.Equal(0);
		}

		[TestMethod]
		public void ConvertTo_SameType()
		{
			object testValue = 5;
			var result = testValue.ConvertTo<int>();
			Assert.IsInstanceOfType(result, typeof(int));
			Assert.AreEqual(testValue, result);
		}

		[TestMethod]
		public void ConvertTo_ConvertorExists()
		{
			var testValue = 5;
			var result = testValue.ConvertTo<long>();
			Assert.IsInstanceOfType(result, typeof(long));
			Assert.AreEqual((long)testValue, result);
		}

		[TestMethod]
		public void ConvertTo_CanConvert_WithDefault()
		{
			var testValue = 5;
			var result = testValue.ConvertTo<long>(123);
			Assert.IsInstanceOfType(result, typeof(long));
			Assert.AreEqual((long)testValue, result);
		}
		[TestMethod]
		public void ConvertTo_CannotConvert_WithDefault()
		{
			object testValue = null;
			var result = testValue.ConvertTo<long>(123L);
			Assert.IsInstanceOfType(result, typeof(long));
			Assert.AreEqual(123L, result);
		}

		[TestMethod]
		public void CanConvertTo_ConvertTo()
		{
			object testValue = Color.Red;
			Assert.IsTrue(testValue.CanConvertTo<string>());
			
		}
		[TestMethod]
		public void CanConvertTo_ConvertFrom()
		{
			object testValue = "Red";
			Assert.IsTrue(testValue.CanConvertTo<Color>());
		}

		[TestMethod]
		public void CanConvertTo_Failure()
		{
			object testValue = Color.Red;
			Assert.IsFalse(testValue.CanConvertTo<Exception>());
		}

		[TestMethod]
		public void CanConvertTo_Null()
		{
			object testValue = null;
			Assert.IsFalse(testValue.CanConvertTo<object>());
		}
		[TestMethod]
		public void InvokeMethod()
		{
			int testValue = 5;
			var result = testValue.InvokeMethod<string>("ToString");
			Assert.AreEqual(testValue.ToString(), result);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void InvokeMethod_BadMethod()
		{
			int testValue = 5;
			var result = testValue.InvokeMethod<string>("NotAMethodOfInt32");
		}

		[TestMethod]
		public void InvokeMethod_WrongReturnType()
		{
			int testValue = 5;
			var result = testValue.InvokeMethod<int>("ToString");
			Assert.AreEqual(0, result);
		}
		[TestMethod]
		public void InvokeMethod_WithParamter()
		{
			int testValue = 5;
			var result = testValue.InvokeMethod<string>("ToString", "000");
			Assert.AreEqual(testValue.ToString("000"), result);
		}
		[TestMethod]
		public void InvokeMethod_NonGeneric()
		{
			int testValue = 5;
			object result = testValue.InvokeMethod("ToString");
			Assert.IsInstanceOfType(result, typeof(string));
			Assert.AreEqual(testValue.ToString(), result);
		}

		[TestMethod]
		public void GetPropertyValue()
		{
			var testValue = "Hello World";
			var result = testValue.GetPropertyValue<int>("Length");
			Assert.AreEqual(testValue.Length, result);
		}
		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void GetPropertyValue_BadProperty()
		{
			var testValue = "Hello World";
			var result = testValue.GetPropertyValue<int>("NotAPropertyOfString");
		}

		[TestMethod]
		public void GetPropertyValue_WrongReturnType()
		{
			var testValue = "Hello World";
			var result = testValue.GetPropertyValue<string>("Length");
			Assert.IsNull(result);
		}
		[TestMethod]
		public void GetPropertyValue_NonGeneric()
		{
			var testValue = "Hello World";
			var result = testValue.GetPropertyValue("Length");
			Assert.IsInstanceOfType(result, typeof(int));
			Assert.AreEqual(testValue.Length, result);
		}

		[TestMethod]
		public void SetPropertyValue()
		{
			var testValue = new Exception("Test1");
			Assert.IsNull(testValue.HelpLink);

			testValue.SetPropertyValue( "HelpLink", "Hello World");
			Assert.AreEqual("Hello World", testValue.HelpLink);
		}
		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void SetPropertyValue_BadProperty()
		{
			var testValue = new Exception("Test1");
			testValue.SetPropertyValue("NotAPropertyOfString", 1234);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void SetPropertyValue_WrongDataType()
		{
			var testValue = new Exception("Test1");
			testValue.SetPropertyValue("HelpLink", 1234);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void SetPropertyValue_CannotWrite()
		{
			var testValue = new Exception("Test1");
			testValue.SetPropertyValue("InnerException", new Exception());
		}


		[TestMethod]
		public void GetAttribute_FromObject()
		{
			var testValue = this;
			var result = testValue.GetAttribute<TestClassAttribute>();
			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result, typeof(TestClassAttribute));
		}

		[TestMethod]
		public void GetAttribute_FromType()
		{
			var testValue = typeof(ObjectExtensionsTest);
			var result = testValue.GetAttribute<TestClassAttribute>();
			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result, typeof(TestClassAttribute));
			var str = new Exception();
		}

		[TestMethod]
		public void GetAttribute_NotFound()
		{
			var testValue = this;
			var result = testValue.GetAttribute<TestMethodAttribute>();
			Assert.IsNull(result);
		}

		[CLSCompliant(false)]
		class SubClass : ObjectExtensionsTest {}

		[TestMethod]
      public void GetAttributes_FromObject()
      {
			var testValue = new SubClass();
			var result = testValue.GetAttributes<CLSCompliantAttribute>();
			Assert.IsNotNull(result);
			Assert.IsTrue(result.Any());
			var attr = result.First();
			Assert.IsInstanceOfType(attr, typeof(CLSCompliantAttribute));
      }

      public void GetAttributes_FromType()
      {
			var testValue = typeof(SubClass);
			var result = testValue.GetAttributes<CLSCompliantAttribute>();
			Assert.IsNotNull(result);
			Assert.IsTrue(result.Any());
			var attr = result.First();
			Assert.IsInstanceOfType(attr, typeof(CLSCompliantAttribute));
      }

      public void GetAttributes_LookUp()
      {
			var testValue = typeof(SubClass);
			var result = testValue.GetAttributes<TestClassAttribute>(true);
			Assert.IsNotNull(result);
			Assert.IsTrue(result.Any());
			var attr = result.First();
			Assert.IsInstanceOfType(attr, typeof(TestClassAttribute));
      }
      public void GetAttributes_DontLookUp()
      {
			var testValue = typeof(SubClass);
			var result = testValue.GetAttributes<TestClassAttribute>(false);
			Assert.IsNotNull(result);
			Assert.IsFalse(result.Any());
      }

        [TestMethod]
        public void ToXmlTest()
        {
            TestEntity test = new TestEntity();
            test.Property1 = "this";
            test.Property2 = "that";

            string xml = test.ToXml();
            Console.WriteLine(xml);
        }

        [TestClass]
        public class TestEntity
        {
            public string Property1 { get; set; }
            public string Property2 { get; set; }
        }
	}
}