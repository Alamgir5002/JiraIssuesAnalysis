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
        private CustomFieldsService customFieldsService;
        private IssueRepository issueRepository;
        public IssueController(IssuesService issuesService, CustomFieldsService customFieldsService, IssueRepository issueRepository)
        {
            this.issuesService = issuesService;
            this.customFieldsService = customFieldsService; 
            this.issueRepository = issueRepository; 
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
                issuesService.processIssuesList(response);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost("/addCustomField")]
        public async Task<IActionResult> AddCustomField(CustomField customFields)
        {
            try
            {
                var response = await customFieldsService.AddNewCustomField(customFields);   
                return Ok(response);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("/allCustomFields")]
        public async Task<IActionResult> GetAllCustomFields()
        {
            try
            {
                var response = await customFieldsService.GetAllCustomFieldsAsync();
                return Ok(response);    
            }
            catch(Exception exception)
            {
                return BadRequest(exception.Message);   
            }
        }

        [HttpGet("allIssues")]
        public List<Issue> GetAllIssues()
        {
            return issueRepository.GetAllIssues().ToList();
        }
        [HttpGet("allIssues/{fixVersion}")]

        public List<Issue> GetIssuesAgainstFixVersion(string fixVersion)
        {
            return issueRepository.GetAllIssuesAgainstFixVersion(fixVersion).ToList();
        }
    }
}
