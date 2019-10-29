using System.Threading.Tasks;
using AutoRest.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AutoRest.Controllers
{
    /// <summary>
    ///     A general controller for accessing a database.
    /// </summary>
    [Route("api/[controller]")]
    public class TablesController : ControllerBase
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
        /// <returns>Table rows as a JSON string</returns>
        [HttpGet("{tableName}/{orderBy?}")]
        public IActionResult Get(string tableName, string orderBy)
        {
            if (string.IsNullOrEmpty(orderBy))
                return BadRequest("OrderBy parameter was omitted, specify a column name, e.g. the identity column.");

            string filter = null;
            string includes = null;
            var ascending = true;
            var outerJoin = false;
            var offset = 0;
            var pageSize = 200;

            // Get outer join statement
            if (Request.Query.ContainsKey("outerjoin"))
            {
                if (!bool.TryParse(Request.Query["outerjoin"], out outerJoin))
                {
                    outerJoin = false;
                }
            }

            // Get ascending column statement
            if (Request.Query.ContainsKey("asc"))
            {
                if (!bool.TryParse(Request.Query["asc"], out ascending))
                {
                    ascending = true;
                }
            }

            // Get offset statement
            if (Request.Query.ContainsKey("offset"))
            {
                if (!int.TryParse(Request.Query["offset"], out offset))
                {
                    offset = 0; // Probably not needed
                }
            }

            // Get page size statement
            if (Request.Query.ContainsKey("pagesize"))
            {
                if (!int.TryParse(Request.Query["pagesize"], out pageSize) || pageSize <= 0)
                {
                    pageSize = 200;
                }
            }

            // Get filter statement
            if (Request.Query.ContainsKey("filter")) filter = Request.Query["filter"];

            // Get include statement
            if (Request.Query.ContainsKey("include")) includes = Request.Query["include"];

            // Run query
            var res = _dbLogic.Select(tableName, orderBy, ascending, filter, includes, outerJoin, offset, pageSize);

            if (res.Rows == null) return BadRequest(res.Message);

            return Ok(JsonConvert.SerializeObject(res.Rows));
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