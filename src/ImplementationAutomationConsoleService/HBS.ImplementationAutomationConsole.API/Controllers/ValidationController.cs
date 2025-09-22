using HBS.ImplementationAutomationConsole.Core.BLL.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;

namespace HBS.ImplementationAutomationConsole.API.Controllers
{
    [ApiController]
    public class ValidationController : ControllerBase
    {
        private readonly IValidationServices _ivalidationServices;
        private readonly ILogger _logger;
        public ValidationController(
            IValidationServices ivalidationServices, ILogger logger)
        {
            _ivalidationServices = ivalidationServices;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Policy = "User")]
        [Route("/api/validation")]
        public async Task<IActionResult> GetValidationOverview(string subsectionName)
        {
            try
            {
                await _ivalidationServices.ValidateUploadedData(subsectionName);
                var counts = await _ivalidationServices.GetValidationCounts(subsectionName);
                return Ok(counts);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Authorize(Policy = "User")]
        [Route("/api/validatedExcelDownload")]
        public async Task<IActionResult> DownloadValidatedExcel(string subsectionName)
        {
            try
            {
                var response=await _ivalidationServices.DownloadValidatedExcel(subsectionName);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}