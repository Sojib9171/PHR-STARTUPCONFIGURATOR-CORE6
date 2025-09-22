using HBS.ImplementationAutomationConsole.Core.BLL.IServices;
using HBS.ImplementationAutomationConsole.Core.Models;
using HBS.ImplementationAutomationConsole.Core.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;

namespace HBS.ImplementationAutomationConsole.API.Controllers
{
    [ApiController]
    public class ExcelDataController : ControllerBase
    {
        private readonly IExcelDataServices _iexcelDataServices;
        private readonly ILogger _logger;
        public ExcelDataController(
            IExcelDataServices iexcelDataServices, ILogger logger)
        {
            _iexcelDataServices = iexcelDataServices;
            _logger = logger;
        }

        [HttpPost]
        [Authorize(Policy = "User")]
        [Route("/api/uploadData")]
        public async Task<IActionResult> UploadEmployeeFile([FromForm] IFormFile file, [FromForm] string subsectionName)
        
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    throw new Exception("File not selected");
                }

                if(subsectionName.CheckForValidStringResponse(false, true, true, 150))
                {
                    throw new Exception("Subsection name is not valid");
                }

                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    await _iexcelDataServices.UploadData(stream,subsectionName);
                }
                return Ok("Data inserted");
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Policy = "User")]
        [Route("/api/clearTable")]
        public async Task<IActionResult> DeleteallFromTable([FromBody]DeleteBySubsectionDto model)
        {
            try
            {
                if (model.subsectionName.CheckForValidStringResponse(false, true, true, 150))
                {
                    throw new Exception("Subsection name is not valid");
                }
                await _iexcelDataServices.DeleteAllFromTable(model.subsectionName);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return BadRequest(ex.Message);
            }
        }


        [HttpGet]
        [Authorize(Policy = "User")]
        [Route("/api/downloadExcelWithData")]
        public async Task<IActionResult> DownloadExcelWithData(string subsectionName)
        {
            try
            {
                if (subsectionName.CheckForValidStringResponse(false, true, true, 150))
                {
                    throw new Exception("Subsection name is not valid");
                }

                var response = await _iexcelDataServices.DownloadExcel(subsectionName);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Authorize(Policy = "User")]
        [Route("/api/updateDependentColumnsBySubsection")]
        public async Task<IActionResult> UpdateDependentColumnsBySubsection(string subsectionName)
        {
            try
            {
                if (subsectionName.CheckForValidStringResponse(false, true, true, 150))
                {
                    throw new Exception("Subsection name is not valid");
                }
                if (subsectionName=="Short Leave")
                    await _iexcelDataServices.UpdateDependentColumns(subsectionName);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}