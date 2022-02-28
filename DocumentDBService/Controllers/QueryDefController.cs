using DocumentDbDAL;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Serilog;
using System;
using System.Net;
using System.Net.Http; 

namespace DocumentDBService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QueryDefController : ControllerBase
    {
        private IDBServices dbServices;
        private IExecuteQueryDef executeQueryDef;
        private JArray resultArray;
        private JObject result;

        public QueryDefController(IDBServices DBService, IExecuteQueryDef ExecuteQueryDef)
        {
            dbServices = DBService;
            executeQueryDef = ExecuteQueryDef;
        }

        /// <summary>
        /// gets the available queryDefs
        /// </summary>
        /// <returns>returns list of id, name and description for each query definition</returns>
        [HttpGet]
        public IActionResult Get()
        {
            Log.Information($"Get   ");
            var ret = QueryDefService.GetName();
            return Ok(ret);
        }

        /// <summary>
        /// Gets all matching the query
        /// </summary>
        /// <param name="queryDefName">name of query definition</param>
        /// <param name="query">query string suitable for target engine</param>
        /// <returns>list of matching results</returns>
        [HttpGet]
        [Route("{queryDefName}/Query/{query}")] 
        public IActionResult ExecuteGet(string queryDefName, string query = "")
        {
            Log.Information($"ExecuteGet = name: {queryDefName}, query = {query} ");
            if (QueryDefService.DoesNotHave(queryDefName))
            {
                return new ErrorResponse($"query {queryDefName} was not found in defintions");
            }
            try
            {
                executeQueryDef.QueryDefName = queryDefName;

                resultArray = executeQueryDef.Get(query);
                result = new JObject(new JProperty("results", resultArray));
            }
            catch (Exception ex)
            {
                Log.Error($"ExecuteGet with name={queryDefName} and query={query}", ex);
                return new ErrorResponse(ex);
            }

            return Ok(result);
        }

        /// <summary>
        /// Gets all 
        /// </summary>
        /// <param name="queryDefName">name of query definition</param>
        /// <returns>list of matching results</returns>
        [HttpGet]
        [Route("{queryDefName}")]
        public IActionResult ExecuteGetAll(string queryDefName )
        {
            Log.Information($"ExecuteGetAll = name: {queryDefName}" );
            if (QueryDefService.DoesNotHave(queryDefName))
            {
                return new ErrorResponse($"query {queryDefName} was not found in defintions");
            }
            try
            {
                executeQueryDef.QueryDefName = queryDefName;

                resultArray = executeQueryDef.Get("" );
                result = new JObject(new JProperty("results", resultArray));
            }
            catch (Exception ex)
            {
                Log.Error($"ExecuteGetAll with name={queryDefName}  ", ex);
                return new ErrorResponse(ex);
            }

            return Ok(result);
        }
        /// <summary>
        /// Gets a single record using the named querydef
        /// </summary>
        /// <param name="queryDefName">name of query definition</param>
        /// <param name="id">string representation of the id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{queryDefName}/GetOne/{id}")]
        public IActionResult ExecuteGetOne(string queryDefName, string id)
        {
            Log.Information($"ExecuteGetOne = name: {queryDefName} with id: {id}  ");
            if (QueryDefService.DoesNotHave(queryDefName))
            {
                return new ErrorResponse($"query {queryDefName} was not found in defintions");
            }
            try
            {
                executeQueryDef.QueryDefName = queryDefName;
                result = executeQueryDef.GetOne(id);
            }
            catch (Exception ex)
            {
                Log.Error($"ExecuteGetOne = name: {queryDefName} with id: {id} ", ex);
                return new ErrorResponse(ex);
            }
            return Ok(result);
        }

        /// <summary>
        /// Adds a new record,  Fields in the body object must exactly the dbName field in the defintions fields node.
        /// </summary>
        /// <param name="queryDefName">name of the query defintion</param>
        /// <param name="body">json object representing the object to be added.  Must match querydef field list</param>
        /// <returns>number of records afffected (1 if successful)</returns>
        [HttpPost]
        [Route("{queryDefName}")]
        public IActionResult ExecuteAdd(string queryDefName, [FromBody] object body)
        {
            Log.Information($"ExecuteAdd = name: {queryDefName} with id: {body}  ");
            if (QueryDefService.DoesNotHave(queryDefName))
            {
                return new ErrorResponse($"query {queryDefName} was not found in defintions");
            }
            int ret = 0;
            try
            {
                executeQueryDef.QueryDefName = queryDefName;
                JObject jsonBody = JObject.Parse(body.ToString());

                ret = executeQueryDef.Insert(jsonBody);

            }
            catch (Exception ex)
            {
                Log.Error($"ExecuteAdd = name: {queryDefName} with id: {body}  ", ex);

                return new ErrorResponse(ex);
            } 
            return Ok(ret);
        }


        /// <summary>
        /// Updates a new record,  Fields in the body object must exactly the dbName field in the defintions fields node.
        /// </summary>
        /// <param name="queryDefName">name of the query defintion</param>
        /// <param name="body">json object representing the object to be added.  Must match querydef field list</param>
        /// <returns>number of records afffected (1 if successful)</returns>
        [HttpPut]
        [Route("{queryDefName}")]
        public IActionResult ExecuteUpdate(string queryDefName, [FromBody] object body)
        {
            Log.Information($"ExecuteUpdate = name: {queryDefName} with id: {body}  ");
            if (QueryDefService.DoesNotHave(queryDefName))
            {
                return new ErrorResponse($"query {queryDefName} was not found in defintions");
            }
            int ret = 0;
            try
            {
                executeQueryDef.QueryDefName = queryDefName;
                JObject jsonBody = JObject.Parse(body.ToString());

                ret = executeQueryDef.Update(jsonBody);

            }
            catch (Exception ex)
            {
                Log.Error($"ExecuteUpdate = name: {queryDefName} with id: {body}  ", ex);

                return new ErrorResponse(ex);
            }
            return Ok(ret);
        }

        /// <summary>
        /// Delete Record using Query Defintion
        /// </summary>
        /// <param name="queryDefName">name of query def to use</param>
        /// <param name="id">primary key as string</param>
        /// <returns>number of rows affect - should be 1</returns>
        [HttpDelete]
        [Route("{queryDefName}/{id}")]
        public IActionResult ExecuteDelete(string queryDefName, string id)
        {
            Log.Information($"ExecuteDelete = name: {queryDefName} with id: {id}  ");
            if (QueryDefService.DoesNotHave(queryDefName))
            {
                return new ErrorResponse($"query {queryDefName} was not found in defintions");
            }
            int ret = 0;
            try
            {
                executeQueryDef.QueryDefName = queryDefName; 
                ret = executeQueryDef.Delete(id); 
            }
            catch (Exception ex)
            {
                Log.Error($"ExecuteAdd = name: {queryDefName} with id: {id}  ", ex); 
                return new ErrorResponse(ex);
            } 
            return Ok(ret);
        }
    }
}
