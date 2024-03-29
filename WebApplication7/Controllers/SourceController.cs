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
    }
}
