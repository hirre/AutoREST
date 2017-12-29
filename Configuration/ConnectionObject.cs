using AutoRest.Interfaces;

namespace AutoRest.Configuration
{
    /// <summary>
    ///     Holds the connection string object.
    /// </summary>
    public class ConnectionObject : IDbConnection
    {
        public ConnectionObject(string connStr)
        {
            ConnectionString = connStr;
        }

        /// <summary>
        ///     The connection string.
        /// </summary>
        public string ConnectionString { get; }
    }
}