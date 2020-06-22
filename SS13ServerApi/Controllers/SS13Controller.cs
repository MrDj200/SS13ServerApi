using ByondTopic;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SS13ServerApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SS13Controller : ControllerBase
    {
        [HttpGet("status")]
        public ActionResult GetServerInfo([FromQuery] string address = "whipit.de", [FromQuery] ushort port = 1337)
        {
            try
            {
                TopicSource topic = new TopicSource(address, port);
                QueryResponse queryResponse = topic.Query("status");
                return Ok(queryResponse.ProperJson());
            }
            catch (System.Net.Sockets.SocketException)
            {
                return BadRequest(new
                {
                    Title = "One ore more validation errors occured",
                    Status = 400,
                    Errors = new Dictionary<string, string[]> { { "address", new[] { $"The value '{address}' is not valid" } } }
                });
            }
        }
    }
}
