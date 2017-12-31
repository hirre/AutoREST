using System.Collections.Generic;
using AutoRest.Interfaces;
using Microsoft.AspNetCore.Http.Features;

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
            string filter, string includes, bool outerJoin, int offset, int pageSize)
        {
            var orderDirection = ascending ? "ASC" : "DESC";
            var sql = $"SELECT * FROM {tableName} AS [{TableAlias}1]";

            // Apply include statement
            if (!string.IsNullOrEmpty(includes))
            {
                includes = includes.Replace("(", "");
                includes = includes.Replace(")", "");

                var incArr = includes.Split(',');
                var aliasCnt = 2;
                var incList = new List<(int AliasId, string SrcId, string Dst, string DstId)>();

                // Process each include
                foreach (var inc in incArr)
                {
                    var argArr = inc.Split(';');
                    var srcTableId = argArr[0];
                    var dstTable = argArr[1];
                    var dstTableId = argArr[2];

                    // Add to include list
                    incList.Add((aliasCnt++, srcTableId, dstTable, dstTableId));
                }

                // Create join statements
                foreach (var inc in incList)
                {
                    if (outerJoin)
                    {
                        sql +=
                            $" LEFT OUTER JOIN [{inc.Dst}] {TableAlias}{inc.AliasId} ON " +
                            $"[{TableAlias}1].{inc.SrcId} = [{TableAlias}{inc.AliasId}].{inc.DstId} ";
                    }
                    else
                    {
                        sql +=
                            $" INNER JOIN [{inc.Dst}] {TableAlias}{inc.AliasId} ON " +
                            $"[{TableAlias}1].{inc.SrcId} = [{TableAlias}{inc.AliasId}].{inc.DstId} ";
                    }
                    
                }
            }
            
            sql += " WHERE 1 = 1 ";
            
            // Apply filter
            if (!string.IsNullOrEmpty(filter))
            {
                sql += $" AND {filter} ";
            }
            
            sql +=  $"ORDER BY {orderBy} {orderDirection} " +
                    $"OFFSET {offset} ROWS " +
                    $"FETCH NEXT {pageSize} ROWS ONLY";

            return sql;
        }
    }
}