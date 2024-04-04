using IssueAnalysisExtended.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication7.Models;

namespace IssueAnalysisExtended.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly ILogger<AdminController> _logger;
        private readonly DatabaseContext _databaseContext;
        private readonly TruncationService _truncationService;

        public AdminController(ILogger<AdminController> logger, DatabaseContext databaseContext, TruncationService truncationService)
        {
            _logger = logger;
            _databaseContext = databaseContext;
            _truncationService = truncationService;
        }

        [HttpPost("truncate")]
        public async Task<IActionResult> truncateAllDbTables()
        {
            try
            {
                await _databaseContext.Database.ExecuteSqlRawAsync($"TRUNCATE TABLE CustomFields");
                await _databaseContext.Database.ExecuteSqlRawAsync($"TRUNCATE TABLE syncedReleases");
                await _databaseContext.Database.ExecuteSqlRawAsync($"TRUNCATE TABLE SourceCredentials");
                await _databaseContext.Database.ExecuteSqlRawAsync($"TRUNCATE TABLE Projects");

                await _databaseContext.Database.ExecuteSqlRawAsync($"TRUNCATE TABLE EstimatedAndSpentTimes");
                await _databaseContext.Database.ExecuteSqlRawAsync($"TRUNCATE TABLE IssueRelease");

                var deleteStatus = _truncationService.RemoveAllDataFromCircularDependantTables();

                if (deleteStatus == true) { return Ok(); }
                else { return BadRequest(); }
            }
            catch (Exception ex)
            {
                _logger.LogCritical("Couldn't truncate tables from DB: {ex}", ex.Message.ToString());
                throw;
            }
        }

        
    }
}
