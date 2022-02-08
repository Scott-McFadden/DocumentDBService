using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace DocumentDBService
{
    public class ErrorResponse : IActionResult
    {

        private readonly Exception _ex;
        private readonly HttpStatusCode _statusCode;

        public ErrorResponse(Exception ex)
        {
            _ex = ex;
        }
        public ErrorResponse(string ex)
        {
            _ex = new Exception(ex);
        }
         
        public async Task ExecuteResultAsync(ActionContext context)
        {
            string msg = _ex.Message;
            int statusCode = 200;

            if (_ex.Message.Contains("Duplicate"))
                statusCode = (int)HttpStatusCode.Forbidden;
            else if (_ex.Message.Contains("does not allow"))
                statusCode = (int)HttpStatusCode.MethodNotAllowed;
            else if (_ex.Message.Contains("an id property"))
                statusCode = (int)HttpStatusCode.PreconditionRequired;
            else if (_ex.Message.Contains("not found"))
                statusCode = (int)HttpStatusCode.NotFound;

            else if (_ex.GetType().Name == "SqlException")
            {
                msg = "SQLException: " + _ex.Message;
                statusCode = (int)HttpStatusCode.BadRequest;
            }
            else statusCode = (int)HttpStatusCode.InternalServerError;


            var ret = new ObjectResult(msg);
            ret.StatusCode = statusCode;
             
            await ret.ExecuteResultAsync(context);
        }
    }
}
