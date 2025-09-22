using HBS.ImplementationAutomationConsole.Core.BLL.IServices;
using HBS.ImplementationAutomationConsole.Core.Extensions;
using HBS.ImplementationAutomationConsole.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SelectPdf;
using ILogger = Serilog.ILogger;

namespace HBS.ImplementationAutomationConsole.API.Controllers
{
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardServices _idashboardServices;
        private readonly ISignOffService _isignOffServices;
        private readonly IWizardDataServices _iWizardDataServices;
        private readonly ILogger _logger;
        public DashboardController(
            IDashboardServices idashboardServices, ISignOffService isignOffService, IWizardDataServices wizardDataServices, ILogger logger)
        {
            _idashboardServices = idashboardServices;
            _isignOffServices = isignOffService;
            _iWizardDataServices = wizardDataServices;
            _logger = logger;
        }

        [HttpPost]
        [Authorize(Policy = "User")]
        [Route("/api/uploadDashboardData")]
        public async Task<IActionResult> UploadDashboardData([FromBody] DashboardPostDto dashboardDto)
        {
            try
            {
                if (dashboardDto.UserName.CheckForValidStringResponse(false, true, false))
                {
                    throw new Exception("Invalid User Name");
                }
                if (dashboardDto.Name.CheckForValidStringResponse(false, true, false))
                {
                    throw new Exception("Invalid Name");
                }
                if (dashboardDto.SubsectionName.CheckForValidStringResponse(false, true, false))
                {
                    throw new Exception("Invalid Seubsection Name");
                }
                if (dashboardDto.SubsectionID.CheckForValidStringResponse(false, true, false))
                {
                    throw new Exception("Invalid Subsection Id");
                }
                var moduleName = await _isignOffServices.GetModuleNameFromSubsection(dashboardDto.SubsectionName);
                var htmlString = await _isignOffServices.GetSignOffHtmlString(dashboardDto.Name, dashboardDto.ApprovalDate, dashboardDto.ApprovalStatus, moduleName, dashboardDto.SubsectionName, dashboardDto.Comment, dashboardDto.UserName);

                var converter = new HtmlToPdf();
                PdfDocument doc = converter.ConvertHtmlString(htmlString);
                byte[] pdfBytes = doc.Save();
                doc.Close();

                dashboardDto.SignOffPdfData = pdfBytes;

                await _idashboardServices.InsertDashboardInfo(dashboardDto);
                return Ok("Data successfully inserted");
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Policy = "User")]
        [Route("/api/uploadDashboardDataForWizard")]
        public async Task<IActionResult> UploadDashboardDataForWizard([FromBody] DashboardWizardPostDto dashboardWizardDto)
        {
            try
            {
                var moduleName = await _isignOffServices.GetModuleNameFromSubsection(dashboardWizardDto.SubsectionName);
                var htmlString = await _isignOffServices.GetSignOffHtmlString(dashboardWizardDto.Name, dashboardWizardDto.ApprovalDate, dashboardWizardDto.ApprovalStatus, moduleName, dashboardWizardDto.SubsectionName, dashboardWizardDto.Comment, dashboardWizardDto.UserName);

                var converter = new HtmlToPdf();
                PdfDocument doc = converter.ConvertHtmlString(htmlString);
                byte[] pdfBytes = doc.Save();
                doc.Close();

                dashboardWizardDto.SignOffPdfData = pdfBytes;

                var dashboardDto = new DashboardPostDto()
                {
                    SubsectionName = dashboardWizardDto.SubsectionName,
                    UserName = dashboardWizardDto.UserName,
                    Name = dashboardWizardDto.Name,
                    Comment = dashboardWizardDto.Comment,
                    ModuleID = dashboardWizardDto.ModuleID,
                    SubsectionID = dashboardWizardDto.SubsectionID,
                    ApprovalStatus = dashboardWizardDto.ApprovalStatus,
                    ApprovalSignature = dashboardWizardDto.ApprovalSignature,
                    ApprovalDate = dashboardWizardDto.ApprovalDate,
                    ApprovalData = dashboardWizardDto.ApprovalData,
                    SignOffPdfData = dashboardWizardDto.SignOffPdfData
                };

                await _idashboardServices.InsertDashboardInfo(dashboardDto);
                await _iWizardDataServices.UpdateDraftAndApprovalStatusForAbsence(dashboardWizardDto.RowIDs, dashboardDto.SubsectionName);
                return Ok("Data successfully inserted");
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Authorize(Policy = "User")]
        [Route("/api/getDashboardData")]
        public async Task<IActionResult> GetDashboardData(string serverParams)
        {
            try
            {
                var parameters = JsonConvert.DeserializeObject<ServerParamsDto>(serverParams);
                var response = await _idashboardServices.GetDashboardInfo(parameters);

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
        [Route("/api/getDashboardHistoryData")]
        public async Task<IActionResult> GetDashboardHistoryData(string serverParams)
        {
            try
            {
                var parameters = JsonConvert.DeserializeObject<HistoryServerParamsDto>(serverParams);
                var response = await _idashboardServices.GetDashboardHistoryInfo(parameters);

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