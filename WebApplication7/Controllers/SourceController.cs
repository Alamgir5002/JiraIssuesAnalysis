using IssueAnalysisExtended.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication7.Models;
using WebApplication7.Services;

namespace WebApplication7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SourceController : ControllerBase
    {
        private SourceService sourceService;
        public SourceController(SourceService sourceService)
        {
            this.sourceService = sourceService;
        }

        [HttpPost]
        public async Task<IActionResult> ValidateAndSaveSourceCredentials(SourceCredentials credentials)
        {
            try
            {
                var response = await sourceService.ValidateAndSaveCredentials(credentials); 
                return Ok(response);
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]

        public async Task<IActionResult> GetSourceDetails()
        {
            try
            {
                var resp = await sourceService.GetSourceCredentialsAsync();
                return Ok(resp);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);  
            }
        }

        [HttpGet("/projectsAndCustomFields")]
        public async Task<IActionResult> GetProjectsAndCustomFields()
        {
            try
            {
                var resp = await sourceService.GetSourceFields();
                return Ok(resp);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("/projectsAndCustomFields")]
        public async Task<IActionResult> PostProjectsAndCustomFields(SourceFieldsResponse sourceFieldsResponse)
        {
            try
            {
                var resp = await sourceService.AddOrUpdateFieldResponse(sourceFieldsResponse);
                return Ok(resp);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[HttpPost("/addCustomField")]
        //public async Task<IActionResult> AddCustomField(CustomField customFields)
        //{
        //    try
        //    {
        //        var response = await customFieldsService.AddNewCustomField(customFields);
        //        return Ok(response);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        //[HttpGet("/allCustomFields")]
        //public async Task<IActionResult> GetAllCustomFields()
        //{
        //    try
        //    {
        //        var response = await customFieldsService.GetAllCustomFieldsAsync();
        //        return Ok(response);
        //    }
        //    catch (Exception exception)
        //    {
        //        return BadRequest(exception.Message);
        //    }
        //}

        //[HttpGet("allSourceCustomFields")]
        //public async Task<IActionResult> GetAllCustomFieldsFromSource()
        //{
        //    try
        //    {
        //        var resp = await customFieldsService.GetAllCustomFieldsFromSource();
        //        return Ok(resp);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex);
        //        return BadRequest(ex.Message);
        //    }
        //}
    }
}
