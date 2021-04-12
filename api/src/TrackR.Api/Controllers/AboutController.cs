using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TrackR.Api.Controllers
{
    [Route("api/v1/about")]
    [ApiController]
    [ExcludeFromCodeCoverage]
    public class AboutController : ControllerBase
    {
        [HttpGet]
        [AllowAnonymous]
        public ActionResult<string> GetAboutInfo()
        {
            return "";
        }
    }
}