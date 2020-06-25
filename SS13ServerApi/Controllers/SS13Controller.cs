using ByondTopic;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using SS13ServerApi.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SS13ServerApi.Controllers
{
    [Route("[controller]")]
    [RequestRateLimit(MaxAmount = 20, Seconds = 1)]
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
        [HttpGet("hubInfoJson")]
        [OpenApiOperation(Constants.SS13HubToJsonSummary, Constants.SS13HubToJsonDescription)]
        public async Task<ActionResult> GetHubInfoJson()
        {
            string hubUri = "http://www.byond.com/games/Exadv1/SpaceStation13?format=text";
            string response = await Utils.MakeGetRequest(hubUri);

            var entries = IndentedTextParser.Parse(response.Split(Environment.NewLine));
            var returnBuilder = new StringBuilder();
            returnBuilder.Append("[");
            foreach (var item in entries)
            {
                returnBuilder.Append(item.ToString() + ",");
            }
            returnBuilder.Remove(returnBuilder.ToString().LastIndexOf(','), 1); // Removing last ',' to make it valid json
            returnBuilder.Append("]");
            return Ok(returnBuilder.ToString());
        }

        [HttpGet("status")]
        [OpenApiOperation(Constants.SS13StatusSummary, Constants.SS13StatusDescription)]
        public async Task<ActionResult> GetServerInfo([FromQuery] string address = "whipit.de", [FromQuery] ushort port = 1337)
        {
            // TODO: Think about how I wanna cache responses
            if (!await Utils.IsValidAddress(address))
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
                // Return badrequest to tell the consumer they fucked up
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
