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
        protected override string GetSelectString(string tableName, string orderBy, bool ascending,
            string filter, int offset, int pageSize)
        {
            string sql;
            var orderDirection = ascending ? "ASC" : "DESC";

            if (filter == null)
                sql = $"SELECT * FROM {tableName} " +
                      $"ORDER BY {orderBy} {orderDirection} " +
                      $"OFFSET {offset} ROWS " +
                      $"FETCH NEXT {pageSize} ROWS ONLY";
            else
                sql = $"SELECT * FROM {tableName} " +
                      $"WHERE {filter} " +
                      $"ORDER BY {orderBy} " +
                      $"OFFSET {offset} ROWS " +
                      $"FETCH NEXT {pageSize} ROWS ONLY";

            return sql;
        }
    }
}