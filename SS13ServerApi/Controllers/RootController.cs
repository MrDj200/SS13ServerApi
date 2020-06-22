using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace SS13ServerApi.Controllers
{
    [Route("/")]
    [ApiController]
    public class RootController : ControllerBase
    {
        [HttpGet]
        [OpenApiIgnore]
        public ActionResult Get() => Redirect("/swagger");
    }
}
