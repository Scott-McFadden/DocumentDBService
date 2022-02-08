using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;

namespace DocumentDBService.Controllers
{
    public static class ResponseOptions
    {
        public static HttpResponseMessage Forbidden(string message )
        {
               var ret = new HttpResponseMessage(HttpStatusCode.Forbidden) { Content = new StringContent(message) };
            return ret;
            
        }

        public static HttpResponseMessage Ok(object message)
        {
            return new HttpResponseMessage()
            {
                Content = new StringContent(message.ToString()),
                StatusCode = HttpStatusCode.OK
            };
        }
        public static HttpResponseMessage  Error(Exception ex)
        {
            var ret =  new HttpResponseMessage();
            ret.Content = new StringContent(ex.Message);

            if (ex.Message.Contains("Duplicate"))
                ret.StatusCode = HttpStatusCode.Forbidden;    
            else if (ex.Message.Contains("does not allow"))
                ret.StatusCode = HttpStatusCode.MethodNotAllowed;
            else if (ex.Message.Contains("an id property"))
                ret.StatusCode = HttpStatusCode.PreconditionRequired;
            else ret.StatusCode = HttpStatusCode.InternalServerError;

            return ret;

        }
        public static HttpResponseMessage MethodNotAllowed(object message)
        {
            return new HttpResponseMessage()
            {
                Content = new StringContent(message.ToString()),
                StatusCode = HttpStatusCode.MethodNotAllowed
            };
        }
        public static HttpResponseMessage NotFound(object message)
        {
            return new HttpResponseMessage()
            {
                Content = new StringContent(message.ToString()),
                StatusCode = HttpStatusCode.NotFound
            };
        }
    }
}
