using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace DatabaseServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ControlDataController : ControllerBase
    {
        public ControlDataController()
        {
            Log.Information("ControlDataController constructed");
        }

        [HttpGet]
        public ActionResult GetQueryDefs()
        {

            return Ok();
        }
    }
}
