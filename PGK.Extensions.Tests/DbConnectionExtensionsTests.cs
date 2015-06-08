using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using Should.Fluent;

namespace PGK.Extensions.Tests
{
	[TestClass]
    public class DbConnectionExtensionsTests
	{
		[TestMethod]
		public void StateIsWithin()
		{
            var connection = MockRepository.GenerateStub<IDbConnection>();

		    connection.Stub(x => x.State).Return(ConnectionState.Closed);

		    connection.StateIsWithin(ConnectionState.Executing, ConnectionState.Fetching).Should().Be.False();
            connection.StateIsWithin(ConnectionState.Open).Should().Be.False();
            
            connection.StateIsWithin(ConnectionState.Broken, ConnectionState.Closed).Should().Be.True();
            connection.StateIsWithin(ConnectionState.Closed).Should().Be.True();
		}

		[TestMethod]
		public void StateIsWithinReturnsFalseWhenNoStates()
		{
            var connection = MockRepository.GenerateStub<IDbConnection>();

		    connection.StateIsWithin().Should().Be.False();
		}

		[TestMethod]
		public void StateIsWithinReturnsFalseWhenConnectionIsNull()
		{
            IDbConnection connection = null;

		    connection.StateIsWithin().Should().Be.False();
		}

		[TestMethod]
        public void IsInState()
		{
            var connection = MockRepository.GenerateStub<IDbConnection>();

            connection.Stub(x => x.State).Return(ConnectionState.Fetching);

            connection.IsInState(ConnectionState.Executing).Should().Be.False();
            connection.IsInState(ConnectionState.Fetching).Should().Be.True();
        }

		[TestMethod]
        public void OpenIfNot()
		{
            var openedConnection = MockRepository.GenerateStub<IDbConnection>();
            var closedConnection = MockRepository.GenerateStub<IDbConnection>();
            // Arrange
            openedConnection.Stub(x => x.State).Return(ConnectionState.Open);
            closedConnection.Stub(x => x.State).Return(ConnectionState.Closed);
            //Act
            openedConnection.OpenIfNot();
            closedConnection.OpenIfNot();
            // Assert
            openedConnection.AssertWasNotCalled(x => x.Open());
            closedConnection.AssertWasCalled(x => x.Open());
        }
	}
}
