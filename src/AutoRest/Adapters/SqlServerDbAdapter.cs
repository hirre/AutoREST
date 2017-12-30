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
            string filter, int offset, int pageSize)
        {
            string sql;

            if (filter == null)
                sql = $"SELECT * FROM {tableName} " +
                      $"ORDER BY {orderBy} " +
                      $"OFFSET {offset} ROWS " +
                      $"FETCH NEXT {pageSize} ROWS ONLY";
            else
                sql = $"SELECT * FROM {tableName} " +
                      $"WHERE {CreateSqlFilterQuery(filter)} " +
                      $"ORDER BY {orderBy} " +
                      $"OFFSET {offset} ROWS " +
                      $"FETCH NEXT {pageSize} ROWS ONLY";

            return sql;
        }
    }
}