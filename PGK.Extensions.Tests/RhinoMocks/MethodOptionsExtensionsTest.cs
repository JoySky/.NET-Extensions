using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PGK.Extensions.RhinoMocks;
using Rhino.Mocks;
using Rhino.Mocks.Exceptions;
using Should.Fluent;
using ShouldAssert = Should.Core.Assertions.Assert;

namespace PGK.Extensions.Tests.RhinoMocks
{
	[TestClass]
    public class MethodOptionsExtensionsTest
	{
        [TestMethod, ExpectedException(typeof(ExpectationViolationException))]
        public void AssignFirstArgumentThrowsExceptionWhenNoArgument()
        {
            DateTime firstArg;
		    var mock = MockRepository.GenerateStub<ISomeInterface>();

            mock.Stub(x => x.MethodWithoutArgument()).AssignFirstArgument<DateTime>(val => firstArg = val);

            // Act should throw
            mock.MethodWithoutArgument();
		}

        [TestMethod, ExpectedException(typeof(ExpectationViolationException))]
        public void AssignFirstArgumentThrowsExceptionWhenWrongArgumentType()
        {
            long firstArg;
		    var mock = MockRepository.GenerateStub<ISomeInterface>();

            mock.Stub(x => x.MethodWithoutArgument()).AssignFirstArgument<long>(val => firstArg = val);

            // Act should throw because 1st param is a Date and not a Long
            mock.MethodWithoutArgument();
		}

        [TestMethod]
        public void AssignFirstArgument()
        {
            DateTime firstArg = DateTime.MinValue;
            var argValue = DateTime.Now;
		    var mock = MockRepository.GenerateStub<ISomeInterface>();

            mock.Stub(x => x.MethodWithFirstArgumentAsDate(Arg<DateTime>.Is.Anything, Arg<String>.Is.Anything, Arg<Int32>.Is.Anything)).
                 AssignFirstArgument<DateTime>(val => firstArg = val);
            mock.Replay();

            // Act
            mock.MethodWithFirstArgumentAsDate(argValue, "", 0);
            // Assert
            firstArg.Should().Equal(argValue);
        }

        [TestMethod, ExpectedException(typeof(ExpectationViolationException))]
        public void AssignArgumentThrowsExceptionWhenNoArgumentAtIndex()
        {
            const int outOfBoundIndex = 4;
            DateTime firstArg;
		    var mock = MockRepository.GenerateStub<ISomeInterface>();

            mock.Stub(x => x.MethodWithoutArgument()).AssignArgument<DateTime>(outOfBoundIndex, val => firstArg = val);

            // Act should throw
            mock.MethodWithoutArgument();
		}

        [TestMethod, ExpectedException(typeof(ExpectationViolationException))]
        public void AssignArgumentThrowsExceptionWhenWrongArgumentType()
        {
            long firstArg;
		    var mock = MockRepository.GenerateStub<ISomeInterface>();

            mock.Stub(x => x.MethodWithoutArgument()).AssignArgument<long>(0, val => firstArg = val);

            // Act should throw because 1st param is a Date and not a Long
            mock.MethodWithoutArgument();
		}

        [TestMethod]
        public void AssignArgument()
        {
            const int argValue = 999;
            var thirdArg = int.MinValue;
		    var mock = MockRepository.GenerateStub<ISomeInterface>();

            mock.Stub(x => x.MethodWithFirstArgumentAsDate(Arg<DateTime>.Is.Anything, Arg<String>.Is.Anything, Arg<int>.Is.Anything)).
                 AssignArgument<int>(2, val => thirdArg = val);
            mock.Replay();

            // Act
            mock.MethodWithFirstArgumentAsDate(DateTime.Now, "", argValue);
            // Assert
            thirdArg.Should().Equal(argValue);
        }

        public interface ISomeInterface
        {
            void MethodWithoutArgument();
            void MethodWithFirstArgumentAsDate(DateTime arg1, string arg2, int arg3);
        }
	}

}
