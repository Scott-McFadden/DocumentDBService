using DocumentDbDAL;
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
    public class DomainLookUpController : ControllerBase
    {
        private IDBServices dbServices;

        public DomainLookUpController(IDBServices DBService)
        {
            dbServices = DBService;

        }


        [HttpGet]

        public ActionResult Get()
        {
            Log.Information($"Get =  ");

            try
            {

                return Ok(DomainLookUpService.Get());
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in Get ");
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult GetLookUpValues(string id)
        {
            Log.Information($"Get = id: {id}");

            try
            {

                return Ok(DomainLookUpService.Get(Guid.Parse(id)));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in Get (id)");
                return BadRequest(ex);
            }
        }
        [HttpGet]
        [Route("{cat}/{name}/{value}")]
        public ActionResult Get(string cat, string name, string value)
        {
            Log.Information($"Get = cat: {cat}   name={name}    value={value}");

            try
            {

                return Ok(DomainLookUpService.Get(cat + "_" + name + "_" + value));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in Get (cat, name) ");
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("categories")]
        public ActionResult GetCategories()
        {
            Log.Information($"GetCategories  ");

            try
            {

                return Ok(DomainLookUpService.GetCategories());
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in GetCategories");
                return BadRequest(ex);
            }
        }
        [HttpGet]
        [Route("GetValues/{category}/{name}")]
        public ActionResult GetValues(string category, string name)
        {
            Log.Information($"GetValues = ");

            try
            {
                return Ok(DomainLookUpService.GetValues(category, name));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in GetValues");
                return BadRequest(ex);
            }
        }

        [HttpPut]
        [Route("Add/{cat}/{name}/{value}")]
        public async Task<ActionResult> PutValues(string cat, string name, string value)
        {
            Log.Information($"PutValues = {cat}, {name}, {value}");
            DomainLookUpModel model = new DomainLookUpModel() { Name = name, Value = value, Category = cat };
            try
            {
                return Ok(await Task.FromResult(DomainLookUpService.Add(model)));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in PutValues");
                return BadRequest(ex);
            }
        }

        [HttpDelete]
        [Route("Delete/{id}")]
        public async Task<ActionResult> Delete(string id)
        {

            Log.Information($"Delete = {id} ");
             
            try
            {
                return Ok(await Task.Run(() =>DomainLookUpService.Delete(Guid.Parse(id))));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in PutValues");
                return BadRequest(ex);
            }
        }

    }
}
