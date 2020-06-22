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
            ActionResult returner;
            TopicSource topic = new TopicSource(address, port);

            try
            {
                QueryResponse queryResponse = topic.Query("status");
                returner = Ok(queryResponse.AsDictionary);
            }
            catch (System.Net.Sockets.SocketException)
            {
                returner = BadRequest(new
                {
                    Title = "One ore more validation errors occured",
                    Status = 400,
                    Errors = new Dictionary<string, string[]> { { "address", new[] { $"The value '{address}' is not valid" } } }
                });
            }
            return returner;

        }
    }
}
