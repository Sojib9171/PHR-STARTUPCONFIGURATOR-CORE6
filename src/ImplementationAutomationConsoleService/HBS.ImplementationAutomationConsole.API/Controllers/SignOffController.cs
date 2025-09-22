using HBS.ImplementationAutomationConsole.Core.BLL.IServices;
using HBS.ImplementationAutomationConsole.Core.Extensions;
using HBS.ImplementationAutomationConsole.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SelectPdf;
using ILogger = Serilog.ILogger;

namespace HBS.ImplementationAutomationConsole.API.Controllers
{
    [ApiController]
    public class SignOffController : ControllerBase
    {
        private readonly ISignOffService _signOffService;
        private readonly ILogger _logger;
        public SignOffController(ISignOffService signOffService, ILogger logger)
        {
            _signOffService = signOffService;
            _logger = logger;
        }

        [HttpPost]
        [Authorize(Policy = "User")]
        [Route("/api/generateSignOffDocument")]
        public async Task<IActionResult> GetSignOffDocument([FromBody] DashboardPostDto dashboardDto)
        {
            try
            {
                var moduleName = await _signOffService.GetModuleNameFromSubsection(dashboardDto.SubsectionName);
                var htmlString = await _signOffService.GetSignOffHtmlString(dashboardDto.Name,dashboardDto.ApprovalDate,dashboardDto.ApprovalStatus
                    ,moduleName,dashboardDto.SubsectionName,dashboardDto.Comment,dashboardDto.UserName);

                var converter = new HtmlToPdf();
                PdfDocument doc = converter.ConvertHtmlString(htmlString);
                byte[] pdfBytes = doc.Save();
                doc.Close();
                return Ok(pdfBytes);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Policy = "User")]
        [Route("/api/generateSignOffDocumentForCommonConfig")]
        public async Task<IActionResult> GenerateSignOffDocumentForCommonConfig([FromBody] CommonConfigPostDto commonConfigPostDto)
        {
            try
            {
                if (commonConfigPostDto.UserName.CheckForValidStringResponse(true, true, true, 100))
                {
                    throw new Exception("Invalid User Name");
                }

                if (commonConfigPostDto.Name.CheckForValidStringResponse(false, true, true, 200))
                {
                    throw new Exception("Invalid Name");
                }

                if(commonConfigPostDto.ApprovalStatus.CheckForValidStringResponse(true, true, true, 50))
                {
                    throw new Exception("Invalid Approval Status");
                }

                if (commonConfigPostDto.Comment.CheckForValidStringResponse(false, false, true, 500))
                {
                    throw new Exception("Invalid Approval Status");
                }

                var htmlString = await _signOffService.GetSignOffHtmlStringForConfiguration(commonConfigPostDto.Name, commonConfigPostDto.ApprovalDate, commonConfigPostDto.ApprovalStatus
                    , commonConfigPostDto.Comment, commonConfigPostDto.UserName,"Common Configuration");

                var converter = new HtmlToPdf();
                PdfDocument doc = converter.ConvertHtmlString(htmlString);
                byte[] pdfBytes = doc.Save();
                doc.Close();
                return Ok(pdfBytes);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        [Authorize(Policy = "User Admin")]
        [Route("/api/generateSignOffDocumentForConfigControl")]
        public async Task<IActionResult> GenerateSignOffDocumentForConfigControl([FromBody] ConfigControlPostDto configControlPostDto)
        {
            try
            {
                if (configControlPostDto.UserName.CheckForValidStringResponse(true, true, true, 100))
                {
                    throw new Exception("Invalid User Name");
                }

                if (configControlPostDto.Name.CheckForValidStringResponse(false, true, true, 200))
                {
                    throw new Exception("Invalid Name");
                }

                if (configControlPostDto.ApprovalStatus.CheckForValidStringResponse(true, true, true, 50))
                {
                    throw new Exception("Invalid Approval Status");
                }

                if (configControlPostDto.Comment.CheckForValidStringResponse(false, false, true, 500))
                {
                    throw new Exception("Invalid Approval Status");
                }

                var htmlString = await _signOffService.GetSignOffHtmlStringForConfiguration(configControlPostDto.Name, configControlPostDto.ApprovalDate, configControlPostDto.ApprovalStatus
                    , configControlPostDto.Comment, configControlPostDto.UserName, "Configuration Control");

                var converter = new HtmlToPdf();
                PdfDocument doc = converter.ConvertHtmlString(htmlString);
                byte[] pdfBytes = doc.Save();
                doc.Close();
                return Ok(pdfBytes);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Authorize(Policy = "Admin")]
        [Route("/api/downloadSignOffPdfWithRecordID")]
        public async Task<IActionResult> DownloadSignOffPdfWithRecordID(int recordId)
        {
            try
            {
                var response = await _signOffService.GetSignOffPdfAsByteArray(recordId);
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