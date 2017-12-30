using AutoRest.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace AutoRest.Controllers
{
    /// <summary>
    ///     A general controller for accessing a database.
    /// </summary>
    [Route("api/[controller]")]
    public class TablesController : Controller
    {
        private readonly IDbAdapter _dbLogic;

        public TablesController(IDbAdapter dbLogic)
        {
            _dbLogic = dbLogic;
        }

        /// <summary>
        ///     Get database table row(s).
        /// </summary>
        /// <param name="tableName">Table name</param>
        /// <param name="orderBy">Order by column</param>
        /// <param name="offset">Page offset</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Table rows as a JSON string</returns>
        [HttpGet("{tableName}/{orderBy?}/{offset?}/{pageSize?}")]
        public IActionResult Get(string tableName, string orderBy, int offset = 0, int pageSize = 200)
        {
            if (string.IsNullOrEmpty(orderBy))
                return BadRequest("OrderBy parameter was omitted, specify a column name, e.g. the identity column.");

            string filter = null;

            // Get filter if it exists
            if (Request.Query.ContainsKey("filter"))
            {
                filter = Request.Query["filter"];
            }

            var res = _dbLogic.Select(tableName, orderBy, filter, offset, pageSize);

            if (res.Rows == null) return BadRequest(res.Message);

            return Json(res.Rows);
        }

        /// <summary>
        ///     Post a new table row.
        /// </summary>
        /// <param name="tableName">Table name</param>
        /// <param name="obj">JSON object</param>
        /// <returns>HTTP code 200 on success</returns>
        [HttpPost("{tableName}")]
        public IActionResult Post(string tableName, [FromBody] JObject obj)
        {
            var res = _dbLogic.Insert(tableName, obj);

            if (res.AffectedRows == 0) return BadRequest("No rows affected.");

            if (res.AffectedRows < 0) return BadRequest(res.Message);

            return Ok();
        }

        /// <summary>
        ///     Put/Patch table row(s).
        /// </summary>
        /// <param name="tableName">Table name</param>
        /// <param name="keyName">Key name</param>
        /// <param name="keyValue">Key value</param>
        /// <param name="obj">JSON object</param>
        /// <returns>HTTP code 200 on success</returns>
        [HttpPut("{tableName}/{keyName}/{keyValue}")]
        [HttpPatch("{tableName}/{keyName}/{keyValue}")]
        public IActionResult Put(string tableName, string keyName, string keyValue, [FromBody] JObject obj)
        {
            var res = _dbLogic.Update(tableName, keyName, keyValue, obj);

            if (res.AffectedRows == 0) return BadRequest("No rows affected.");

            if (res.AffectedRows < 0) return BadRequest(res.Message);

            return Ok();
        }

        /// <summary>
        ///     Delete table row(s).
        /// </summary>
        /// <param name="tableName">Table name</param>
        /// <param name="keyName">Key name</param>
        /// <param name="keyValue">Key value</param>
        /// <returns>HTTP code 200 on success</returns>
        [HttpDelete("{tableName}/{keyName}/{keyValue}")]
        public IActionResult Delete(string tableName, string keyName, string keyValue)
        {
            var res = _dbLogic.Delete(tableName, keyName, keyValue);

            if (res.AffectedRows == 0) return BadRequest("No rows affected.");

            if (res.AffectedRows < 0) return BadRequest(res.Message);

            return Ok();
        }
    }
}