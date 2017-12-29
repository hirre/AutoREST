using AutoRest.Interfaces;

namespace AutoRest.Adapters
{
    /// <summary>
    ///     A database adapter for Microsoft SQL Server.
    /// </summary>
    public class SqlServerDbAdapter : GeneralDbAdapter
    {
        public SqlServerDbAdapter(IDbConnection dbConnObj) : base(dbConnObj)
        {
        }

        /// <inheritdoc />
        protected override string GetSelectString(string tableName, string orderBy,
            int offset, int pageSize)
        {
            return $"SELECT * FROM {tableName} " +
                   $"ORDER BY {orderBy} " +
                   $"OFFSET {offset} ROWS " +
                   $"FETCH NEXT {pageSize} ROWS ONLY";
        }
    }
}