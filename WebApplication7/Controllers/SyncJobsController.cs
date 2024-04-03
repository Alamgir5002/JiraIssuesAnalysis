using Hangfire;
using IssueAnalysisExtended.Services.SyncService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IssueAnalysisExtended.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SyncJobsController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly JiraSyncService _jiraSyncService;
        public SyncJobsController(JiraSyncService jiraSyncService) 
        {
            _jiraSyncService = jiraSyncService;
        }

        [HttpPost("/bootstrapReleases")]
        public async Task<IActionResult> AddBootstrapJobForReleases(List<string> releaseNames)
        {
            try
            {
                _jiraSyncService.AddBootstrapJobForReleases(releaseNames);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogCritical("Failed to configure bootstrap job: {ex}", ex.Message.ToString());
                return BadRequest(ex.Message);
            }
        }
    }
}
