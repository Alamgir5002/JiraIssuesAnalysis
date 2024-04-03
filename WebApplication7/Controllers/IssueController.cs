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
        public IssueController(IssuesService issuesService)
        {
            this.issuesService = issuesService;
        }

        [HttpGet("/releases/{projectId}")]
        public async Task<IActionResult> GetReleasesFromSource(string projectId)
        {
            List<Release> releasesList = await issuesService.FetchReleasesFromSource(projectId);
            return Ok(releasesList);
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
    }
}
