namespace AutoRest.Interfaces
{
    /// <summary>
    ///     Interface for database connection.
    /// </summary>
    public interface IDbConnection
    {
        /// <summary>
        ///     The connection string.
        /// </summary>
        string ConnectionString { get; }
    }
}