using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Semogly.Core.Api.AccountContext.Controllers
{
    [Route("api/status")]
    [ApiController]
    [Authorize]
    public class StatusController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new
            {
                status = "Ok"
            });
        }
    }
}
