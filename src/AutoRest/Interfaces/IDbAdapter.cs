﻿using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace AutoRest.Interfaces
{
    /// <summary>
    ///     Interface for the database adapter.
    /// </summary>
    public interface IDbAdapter
    {
        /// <summary>
        ///     Selects rows from the database.
        /// </summary>
        /// <param name="tableName">Table name</param>
        /// <param name="orderBy">Order by column name</param>
        /// <param name="ascending">Ascending order</param>
        /// <param name="filter">Filter options</param>
        /// <param name="includes">Include statement</param>
        /// <param name="outerJoin">Indicates if outer join should be applied</param>
        /// <param name="offset">Row offset</param>
        /// <param name="pageSize">Page size</param>
        /// <returns></returns>
        (IEnumerable<IDictionary<string, object>> Rows, string Message)
            Select(string tableName, string orderBy, bool ascending = true, 
                string filter = null, string includes = null, bool outerJoin = false, int offset = 0, int pageSize = 200);

        /// <summary>
        ///     Inserts a row into the database.
        /// </summary>
        /// <param name="tableName">Table name</param>
        /// <param name="obj">JSON object</param>
        /// <returns>Number of rows affected</returns>
        (int AffectedRows, string Message) Insert(string tableName, JObject obj);

        /// <summary>
        ///     Updates rows in the database.
        /// </summary>
        /// <param name="tableName">Table name</param>
        /// <param name="keyName">Key name</param>
        /// <param name="keyValue">Key value</param>
        /// <param name="obj">JSON object</param>
        /// <returns>Number of affected rows</returns>
        (int AffectedRows, string Message) Update(string tableName,
            string keyName, string keyValue, JObject obj);

        /// <summary>
        ///     Deletes rows in the database.
        /// </summary>
        /// <param name="tableName">Table name</param>
        /// <param name="keyName">Key name</param>
        /// <param name="keyValue">Key value</param>
        /// <returns>Number of affected rows</returns>
        (int AffectedRows, string Message) Delete(string tableName, string keyName, string keyValue);
    }
}