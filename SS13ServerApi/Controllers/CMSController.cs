using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using SS13ServerApi.Attributes;
using SS13ServerApi.ChangeMySkin.Endpoints;
using SS13ServerApi.ChangeMySkin.Responses;

namespace SS13ServerApi.Controllers
{
    [Route("[controller]")]
    [RequestRateLimit(MaxAmount = 20, Seconds = 1)]
    [ApiController]
    public class CMSController : ControllerBase
    {
#region Only runs in DEBUG mode:
#if DEBUG
        [HttpGet("test")]
        public ActionResult Test([FromQuery] string username, [FromQuery] string password)
        {
            var auth = new Authentication(username, password).PerformRequestAsync().Result;
            if (auth.IsSuccess)
            {
                StringBuilder fuck = new StringBuilder();
                fuck.Append($"{{\n\t\"accessToken\":{auth.AccessToken},\n\t\"clientToken\":{auth.ClientToken}\n}}");
                return Ok(fuck.ToString());
            }
            return BadRequest(auth.Error);
        }
#endif
#endregion
    }
}
