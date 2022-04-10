using DocumentDbDAL;
using DocumentDbDAL.Models;
using DocumentDbDAL.Services;
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
    [Route("[controller]")]
    [ApiController]
    public class FeatureFlagController : ControllerBase
    {
        private IDBServices dbServices;
         
        public FeatureFlagController(IDBServices DBService)
        {
            dbServices = DBService;
        }


        [HttpGet]
        [Route("1_0/")]
        public ActionResult Get()
        {
            Log.Information($"Get =  ");

            try
            { 
                return Ok(FeatureFlagService.Get());
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in Get ");
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("1_0/ison/{name}")]
        public ActionResult ison(string name)
        {
            Log.Information($"ison = name: {name}");

            try
            {
                return Ok(FeatureFlagService.IsOn(name));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in ison name");
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("1_0/isoff/{name}")]
        public ActionResult isoff(string name)
        {
            Log.Information($"ison = name: {name}");

            try
            {
                return Ok(FeatureFlagService.IsOff(name));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in isoff name");
                return BadRequest(ex);
            }
        }


        [HttpGet]
        [Route("1_0/Value/{name}")]
        public ActionResult GetValue(string name)
        {
            Log.Information($"GetValue = name: {name}");

            try
            {
                return Ok(FeatureFlagService.GetValue(name));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in GetValue name");
                return BadRequest(ex);
            }
        }


        [HttpPost]
        [Route("1_0/{name}/{value}")]
        public ActionResult SetValue(string name, string value)
        {
            Log.Information($"SetValue = name: {name}  value: {value}");

            try
            {
                return Ok(FeatureFlagService.Set(name, value));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in SetValue name");
                return BadRequest(ex);
            }
        }

        [HttpPost]
        [Route("1_0/{name}/{value}/perm")]
        public ActionResult SetValuePerm(string name, string value)
        {
            Log.Information($"SetValue = name: {name}  value: {value}");

            try
            {
                return Ok(FeatureFlagService.Set(name, value, true));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in SetValuePerm name");
                return BadRequest(ex);
            }
        }


        [HttpDelete]
        [Route("1_0/{name}")]
        public ActionResult Delete(string name)
        {
            Log.Information($"Delete name={name}  ");

            try
            {
                return Ok(FeatureFlagService.Remove(name));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in Delete");
                return BadRequest(ex);
            }
        }
        [HttpDelete]
        [Route("1_0/{name}/perm")]
        public ActionResult DeletePerm(string name)
        {
            Log.Information($"DeletePerm name={name}  ");

            try
            {
                return Ok(FeatureFlagService.Remove(name,true));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in DeletePerm");
                return BadRequest(ex);
            }
        }

        [HttpPatch]
        [Route("1_0/Refresh/{pwd}")]
        public ActionResult Refresh(string pwd )
        {
            Log.Information($"Refresh = {pwd} ");

            try
            {
                if (pwd.ToLower() == "reload")
                    FeatureFlagService.Refresh();
                else
                    throw new Exception("Not Authorized");
                return Ok();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in Refresh");
                return BadRequest(ex);
            }
        }
    }
}
