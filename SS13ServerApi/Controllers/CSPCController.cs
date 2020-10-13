using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using SS13ServerApi.Attributes;

namespace SS13ServerApi.Controllers
{
    [Route("[controller]")]
    [RequestRateLimit(MaxAmount = 20, Seconds = 1)]
    [ApiController]
    // Curse ServerPack Creator
    public class CSPCController : ControllerBase
    {
        [HttpGet("new")]
        [OpenApiOperation(Constants.CSPCSummary, Constants.CSPCDescription)]
        [OpenApiController("TEST")]
        public async Task<ActionResult> CreateNewRequest([FromQuery] ushort id = 0)
        {
            // TODO: Think about how I wanna cache responses

            if (id == 0)
            {
                // Return badrequest to tell the consumer they fucked up
                return BadRequest(new
                {
                    Title = "One or more validation errors occured",
                    Status = 400,
                    //ErrorMessage = e.Message,
                    Errors = new Dictionary<string, string[]> { { "id", new[] { $"The value '{id}' is not valid" } } }
                });
            }

            return Ok();
            
        }
    }
}
