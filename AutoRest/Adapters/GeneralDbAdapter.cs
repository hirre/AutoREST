﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using AutoRest.Interfaces;
using Newtonsoft.Json.Linq;

namespace AutoRest.Adapters
{
    /// <summary>
    ///     General database adapter class.
    /// </summary>
    public class GeneralDbAdapter : IDbAdapter
    {
        protected IDbConnection DbConnObj;

        public GeneralDbAdapter(IDbConnection dbConnObj)
        {
            DbConnObj = dbConnObj;
        }

        /// <inheritdoc />
        public virtual (IEnumerable<IDictionary<string, object>> Rows, string Message)
            Select(string tableName, string orderBy, int offset = 0, int pageSize = 200)
        {
            var resList = new List<Dictionary<string, object>>();

            using (var conn = new SqlConnection(DbConnObj.ConnectionString))
            {
                try
                {
                    conn.Open();

                    var sql = GetSelectString(tableName, orderBy, offset, pageSize);

                    using (var cmd = new SqlCommand(sql, conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                                while (reader.Read())
                                {
                                    var obj = new Dictionary<string, object>();

                                    for (var i = 0; i < reader.FieldCount; i++) obj.Add(reader.GetName(i), reader[i]);

                                    resList.Add(obj);
                                }
                        }
                    }
                }
                catch (Exception e)
                {
                    return (null, e.Message);
                }
                finally
                {
                    conn.Close();
                }
            }

            return (resList, "");
        }

        /// <inheritdoc />
        public virtual (int AffectedRows, string Message) Insert(string tableName, JObject obj)
        {
            using (var conn = new SqlConnection(DbConnObj.ConnectionString))
            {
                try
                {
                    conn.Open();

                    var sql = GetInsertString(tableName, obj);

                    using (var cmd = new SqlCommand(sql, conn))
                    {
                        return (cmd.ExecuteNonQuery(), "");
                    }
                }
                catch (Exception e)
                {
                    return (-1, e.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        /// <inheritdoc />
        public virtual (int AffectedRows, string Message) Update(string tableName, string keyName, string keyValue,
            JObject obj)
        {
            using (var conn = new SqlConnection(DbConnObj.ConnectionString))
            {
                try
                {
                    conn.Open();

                    var sql = GetUpdateString(tableName, keyName, keyValue, obj);

                    using (var cmd = new SqlCommand(sql, conn))
                    {
                        return (cmd.ExecuteNonQuery(), "");
                    }
                }
                catch (Exception e)
                {
                    return (-1, e.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        /// <inheritdoc />
        public virtual (int AffectedRows, string Message) Delete(string tableName, string keyName, string keyValue)
        {
            using (var conn = new SqlConnection(DbConnObj.ConnectionString))
            {
                try
                {
                    conn.Open();

                    var sql = GetDeleteString(tableName, keyName, keyValue);

                    using (var cmd = new SqlCommand(sql, conn))
                    {
                        return (cmd.ExecuteNonQuery(), "");
                    }
                }
                catch (Exception e)
                {
                    return (-1, e.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        #region Protected virtual SQL string methods

        /// <summary>
        ///     Get select SQL string.
        /// </summary>
        /// <param name="tableName">Table name</param>
        /// <param name="orderBy">Order by column</param>
        /// <param name="offset">Page offset</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>The SQL string</returns>
        protected virtual string GetSelectString(string tableName, string orderBy, int offset, int pageSize)
        {
            throw new NotImplementedException(
                "Not implemented due to the fact that the SQL syntax varies depending on database server.");
        }

        /// <summary>
        ///     Get insert SQL string.
        /// </summary>
        /// <param name="tableName">Table name</param>
        /// <param name="obj">JSON object</param>
        /// <returns>The SQL string</returns>
        protected virtual string GetInsertString(string tableName, JObject obj)
        {
            var sql = $"INSERT INTO {tableName} (";

            sql = obj.Properties()
                .Aggregate(sql, (current, prop) => current + prop.Name + ", ");
            sql = sql.Substring(0, sql.LastIndexOf(",", StringComparison.Ordinal));
            sql += ") VALUES (";

            foreach (var prop in obj.Properties())
            {
                var val = prop.Value;

                if (!double.TryParse((string) val, out var _))
                    sql += "'" + val + "', ";
                else
                    sql += val + ", ";
            }

            sql = sql.Substring(0, sql.LastIndexOf(",", StringComparison.Ordinal));
            sql += ")";

            return sql;
        }

        /// <summary>
        ///     Get update SQL string.
        /// </summary>
        /// <param name="tableName">Table name</param>
        /// <param name="keyName">Key name</param>
        /// <param name="keyValue">Key value</param>
        /// <param name="obj">JSON object</param>
        /// <returns>The SQL string</returns>
        protected virtual string GetUpdateString(string tableName, string keyName, string keyValue,
            JObject obj)
        {
            var sql = $"UPDATE {tableName} SET ";

            foreach (var prop in obj.Properties())
            {
                var val = prop.Value;

                if (!double.TryParse((string) val, out var _))
                    sql += prop.Name + " = '" + val + "', ";
                else
                    sql += prop.Name + " = " + val + ", ";
            }

            sql = sql.Substring(0, sql.LastIndexOf(",", StringComparison.Ordinal));

            if (!double.TryParse(keyValue, out var _))
                sql += $" WHERE {keyName} = '{keyValue}'";
            else
                sql += $" WHERE {keyName} = {keyValue}";

            return sql;
        }

        /// <summary>
        ///     Get delete SQL string.
        /// </summary>
        /// <param name="tableName">Table name</param>
        /// <param name="keyName">Key name</param>
        /// <param name="keyValue">Key value</param>
        /// <returns>The SQL string</returns>
        protected virtual string GetDeleteString(string tableName, string keyName, string keyValue)
        {
            var sql = $"DELETE FROM {tableName} ";

            if (!double.TryParse(keyValue, out var _))
                sql += $" WHERE {keyName} = '{keyValue}'";
            else
                sql += $" WHERE {keyName} = {keyValue}";

            return sql;
        }

        #endregion
    }
}