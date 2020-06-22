using ByondTopic;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SS13ServerApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SS13Controller : ControllerBase
    {
        [HttpGet]
        [OpenApiIgnore]
        public ActionResult Get() => Redirect("/swagger/index.html#/SS13");

        [HttpGet("status")]
        public ActionResult GetServerInfo([FromQuery] string address = "whipit.de", [FromQuery] ushort port = 1337)
        {

            if (!Utils.IsValidAddress(address))
            {
                ObjectResult ResponseObject = BadRequest(new
                {
                    Title = "You cheeky mofo",
                    Status = 405,
                    Errors = new Dictionary<string, string[]> { { "address", new[] { $"The value '{address}' is not valid" } } }
                });
                ResponseObject.StatusCode = 405;
                return ResponseObject;
            }
            try
            {
                TopicSource topic = new TopicSource(address, port);
                QueryResponse queryResponse = topic.Query("status");
                return Ok(new SS13Status(queryResponse));
            }
            catch (System.Net.Sockets.SocketException)
            {
                return BadRequest(new
                {
                    Title = "One or more validation errors occured",
                    Status = 400,
                    Errors = new Dictionary<string, string[]> { { "address", new[] { $"The value '{address}' is not valid" } } }
                });
            }
        }
    }
}
