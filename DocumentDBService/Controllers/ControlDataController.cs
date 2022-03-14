﻿using DocumentDbDAL;
using DocumentDbDAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoreLinq;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentDBService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ControlDataController : ControllerBase
    {
        private IDBServices dbServices;

        public ControlDataController(IDBServices DBService)
        {
            dbServices = DBService;

        }

        
        [HttpGet]
        [Route("ControlData/{queryDefName}/GetFields")]
        public ActionResult GetFields(string queryDefName)
        {
            Log.Information($"GetFields = {queryDefName}");
            QueryDefModel QueryDef;
            try
            {
                QueryDef = QueryDefService.Get(queryDefName);
                return Ok(QueryDef.fields.Where(b => !(b.name.IsEmpty())).Select(c => c.name));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in GetFields");
                return BadRequest(ex);
            }
        }
        [HttpGet]
        [Route("ControlData/GetLookUpValues/{name}")]
        public ActionResult GetLookUpValues(string name)
        {
            Log.Information($"GetLookUpValues = {name}");
            List<DomainLookUpModel> lookup;
            try
            {
                lookup = dbServices.lookupdb.Get($" where [Category] = 'QueryDef' and [Name] = '{name}'");
                return Ok(lookup.Select(c => new { c.id, c.Value }));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in GetLookUpValues");
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("ControlData/GetLookUpCategories")]
        public ActionResult GetLookUpCategories()
        {
            Log.Information($"GetLookUpCategories = ");
            List<DomainLookUpModel> lookup;
            try
            {
                lookup = dbServices.lookupdb.Get();
                return Ok(lookup.DistinctBy(b=> b.Category).Select(c=> c.Category));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in GetLookUpCategories");
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("ControlData/GetByCategory/{category}")]
        public ActionResult GetByCategory(string category)
        {
            Log.Information($"GetLookUpValues = {category}");
            List<DomainLookUpModel> lookup;
            try
            {
                lookup = dbServices.lookupdb.Get($" where [category] = '{category}'");
                return Ok(lookup);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in GetByCategory");
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// get querydef 
        /// </summary>
        /// <param name="queryDefName">querydef name</param>
        /// <returns>querydef json object</returns>
        [HttpGet]
        [Route("QueryDef/{queryDefName}")]
        public ActionResult GetQueryDef(string queryDefName)
        {
            Log.Information($"GetFields = {queryDefName}");
            QueryDefModel QueryDef;
            try
            {
                QueryDef = QueryDefService.Get(queryDefName);
                return Ok(QueryDef );
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in GetFields");
                return BadRequest(ex);
            }
        }
    }
}
