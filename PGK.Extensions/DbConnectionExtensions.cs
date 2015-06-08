using System.Data;
using System.Linq;

namespace PGK.Extensions
{
    /// <summary>
    /// Extensions for IDbConnection
    /// </summary>
    public static class DbConnectionExtensions
    {
        /// <summary>
        /// Returns true if the database connection is in one of the states received.
        /// </summary>
        public static bool StateIsWithin(this IDbConnection connection, params ConnectionState[] states)
        {
            return (connection != null && 
                    (states != null && states.Length > 0) && 
                    (states.Where(x => (connection.State & x) == x).Count() > 0));
        }

        /// <summary>
        /// Returns true if the database connection is in the specified state.
        /// </summary>
        public static bool IsInState(this IDbConnection connection, ConnectionState state)
        {
            return (connection != null && 
                    (connection.State & state) == state);
        }

        /// <summary>
        /// Open the Database connection if not already opened.
        /// </summary>
        public static void OpenIfNot(this IDbConnection connection)
        {
            if (!connection.IsInState(ConnectionState.Open))
                connection.Open();
        }
    }
}
