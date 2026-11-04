using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Remp.Models.Configuration;

namespace Remp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VersionController : ControllerBase
    {
        private AppInfo _appInfo;
        private ILogger<VersionController> _logger;

        public VersionController(IOptions<AppInfo> appInfo, ILogger<VersionController> logger)
        {
            _appInfo = appInfo.Value;
            _logger = logger;
        }

        /// <summary>
        /// Return AppInfo from configuration
        /// </summary>
        /// <response code="200">Returns application information</response>
        /// <response code="401">Authentication is required</response>
        /// <response code="500">Application information is invalid</response>
        /// <returns>AppInfo</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AppInfo))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize]
        public ActionResult<AppInfo> GetAppVersion()
        {
            _logger.LogInformation("Fetching AppInfo from appsettings...");

            if (_appInfo.AppVersion <= 0 || string.IsNullOrWhiteSpace(_appInfo.AppName))
            {
                throw new InvalidOperationException("Failed to load AppInfo: AppVersion or AppName is invalid.");
            }

            return Ok(_appInfo);
        }
    }
}
