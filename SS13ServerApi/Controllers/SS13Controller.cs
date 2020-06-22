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

#if DEBUG
        [HttpGet("test")]
        public ActionResult Test([FromQuery] string address = "whipit.de", [FromQuery] ushort port = 1337, [FromQuery] string query = "status")
        {
            try
            {
                TopicSource topic = new TopicSource(address, port);
                var queryResponse = topic.Query(query);
                return Ok(queryResponse.ToString());
            }
            catch (System.Net.Sockets.SocketException e)
            {
                return BadRequest(new
                {
                    Title = "One or more validation errors occured",
                    Status = 400,
                    ErrorMessage = e.Message,
                    Errors = new Dictionary<string, string[]> { { "address", new[] { $"The value '{address}' is not valid" } } }
                });
            }
        }
#endif

        [HttpGet("status")]
        public ActionResult GetServerInfo([FromQuery] string address = "whipit.de", [FromQuery] ushort port = 1337)
        {
            if (!Utils.IsValidAddress(address))
            {
                return BadRequest();
            }
            try
            {
                TopicSource topic = new TopicSource(address, port);
                var queryResponse = topic.Query("status");
                return Ok(new SS13Status(queryResponse.AsText));
            }
            catch (System.Net.Sockets.SocketException e)
            {
                return BadRequest(new
                {
                    Title = "One or more validation errors occured",
                    Status = 400,
                    ErrorMessage = e.Message,
                    Errors = new Dictionary<string, string[]> { { "address", new[] { $"The value '{address}' is not valid" } } }
                });
            }
        }
    }
}
