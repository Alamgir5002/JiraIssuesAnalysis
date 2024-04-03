using IssueAnalysisExtended.Repository;
using IssueAnalysisExtended.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication7.Models;
using WebApplication7.Repository;
using WebApplication7.Services;

namespace WebApplication7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IssueController : ControllerBase
    {
        private IssuesService issuesService;
        private readonly SyncedReleasesRespository syncedReleasesRespository;
        public IssueController(IssuesService issuesService, SyncedReleasesRespository syncedReleasesRespository)
        {
            this.issuesService = issuesService;
            this.syncedReleasesRespository = syncedReleasesRespository; 
        }

        [HttpGet("/all/releases")]
        public async Task<IActionResult> GetReleasesFromSource()
        {
            try
            {
                List<Release> releasesList = await issuesService.FetchReleasesFromSource();
                return Ok(releasesList);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("/{fixVersion}")]
        public async Task<IActionResult> GetIssuesData(string fixVersion)
        {
            try
            {
                var response = await issuesService.FetchIssuesAgainstRelease(fixVersion);
                var issueResponse = issuesService.processIssuesList(response);
                return Ok(issueResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("allIssues")]
        public List<Issue> GetAllIssues()
        {
            return issuesService.GetAllIssuesList();
        }

        [HttpGet("allIssues/{fixVersion}")]

        public async Task<IActionResult> GetIssuesAgainstFixVersion(string fixVersion)
        {
            var resp = await issuesService.GetAllIssuesFromDatabase(fixVersion);

            return Ok(resp);
        }

        [HttpGet("/release/{fixVersion}")]
        public async Task<IActionResult> GetIssuesAgainstSyncedOrLiveRelease(string fixVersion)
        {
            var syncedRelease = await syncedReleasesRespository.GetRelease(fixVersion);
            if (syncedRelease != null)
            {
                var resp = await issuesService.GetAllIssuesFromDatabase(fixVersion);
                return Ok(resp);
            }
            else
            {
                try
                {
                    var response = await issuesService.FetchIssuesAgainstRelease(fixVersion);
                    var issueResponse = issuesService.processIssuesList(response);
                    await syncedReleasesRespository.AddSyncedReleaseAsync(new IssueAnalysisExtended.Models.SyncedRelease { Name = fixVersion });
                    return Ok(issueResponse);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
    }
}
