using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication7.Models;
using WebApplication7.Services;

namespace WebApplication7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private ProjectService projectService;
        public ProjectController(ProjectService projectService)
        {
            this.projectService = projectService;
        }

        //[HttpGet("/source/projects")]
        //public async Task<IActionResult> FetchAllProjectsFromSource()
        //{
        //    try
        //    {
        //        var response = await projectService.FetchAllProjectsFromSource();
        //        return Ok(response);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        //[HttpPost("/addSourceProject")]
        //public async Task<IActionResult> AddProjectSource(Project project)
        //{
        //    try
        //    {
        //        var resp = await projectService.AddSourceProject(project);
        //        return Ok(resp);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
    }
}
