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
    public class SectionEnableDisableController : Controller
    {
        private readonly ILogger _logger;
        private readonly ISectionEnableDisableServices _iSectionEnableDisableServices;
        private readonly ISignOffService _isignOffServices;
        public SectionEnableDisableController(ILogger logger, ISignOffService isignOffServices, ISectionEnableDisableServices sectionEnableDisableServices)
        {
            _logger = logger;
            _iSectionEnableDisableServices = sectionEnableDisableServices;
            _isignOffServices = isignOffServices;
        }

        [HttpPost]
        [Authorize(Policy = "User Admin")]
        [Route("/api/updateOrdersForModule")]
        public async Task<IActionResult> UpdateOrdersForModule([FromBody] ModuleEnableDisableDto models)
        {
            try
            {
                foreach (var module in models.Modules)
                {
                    if (module.ModuleName.CheckForValidStringResponse(false,true,true,150))
                    {
                        throw new Exception("Invalid Module Name");
                    }
                }

                await _iSectionEnableDisableServices.UpdateOrdersForModule(models.Modules);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Authorize(Policy = "User Admin")]
        [Route("/api/getConfigControlApprovalStatus")]
        public async Task<IActionResult> GetConfigControlApprovalStatus()
        {
            try
            {
                var response = await _iSectionEnableDisableServices.GetConfigControlApprovalStatus();
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
        [Route("/api/getAdvanceConfigActiveStatus")]
        public async Task<IActionResult> GetAdvanceConfigActiveStatus()
        {
            try
            {
                var response = await _iSectionEnableDisableServices.GetAdvanceConfigActiveStatus();
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
        [Route("/api/getAdvanceConfigActiveStatusForSidebar")]
        public async Task<IActionResult> GetAdvanceConfigActiveStatusForSidebar()
        {
            try
            {
                var response = await _iSectionEnableDisableServices.GetAdvanceConfigActiveStatusForSidebar();
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Policy = "User Admin")]
        [Route("/api/updateOrdersForSubsection")]
        public async Task<IActionResult> UpdateOrdersForSubsection([FromBody] SubsectionEnableDisableDto models)
        {
            try
            {
                foreach (var subsec in models.EimObjects)
                {
                    if (subsec.SubsectionName.CheckForValidStringResponse(false, true, true, 150))
                    {
                        throw new Exception("Invalid Subsection Name");
                    }
                    if (subsec.SubsectionId.CheckForValidStringResponse(true, true, true, 6))
                    {
                        throw new Exception("Invalid Subsection Name");
                    }
                }

                foreach (var subsec in models.AbsenceObjects)
                {
                    if (subsec.SubsectionName.CheckForValidStringResponse(false, true, true, 150))
                    {
                        throw new Exception("Invalid Subsection Name");
                    }
                    if (subsec.SubsectionId.CheckForValidStringResponse(true, true, true, 6))
                    {
                        throw new Exception("Invalid Subsection Name");
                    }
                }

                foreach (var subsec in models.AttendanceObjects)
                {
                    if (subsec.SubsectionName.CheckForValidStringResponse(false, true, true, 150))
                    {
                        throw new Exception("Invalid Subsection Name");
                    }
                    if (subsec.SubsectionId.CheckForValidStringResponse(true, true, true, 6))
                    {
                        throw new Exception("Invalid Subsection Name");
                    }
                }

                await _iSectionEnableDisableServices.UpdateOrdersForEIM(models.EimObjects);

                if (models.AbsenceObjects.Count != 0)
                    await _iSectionEnableDisableServices.UpdateOrdersForAbsence(models.AbsenceObjects);

                if (models.AttendanceObjects.Count != 0)
                    await _iSectionEnableDisableServices.UpdateOrdersForAttendance(models.AttendanceObjects);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Policy = "User Admin")]
        [Route("/api/uploadConfigControlData")]
        public async Task<IActionResult> UploadConfigControlData([FromBody] ConfigControlPostDto configControlPostDto)
        {
            try
            {
                if (configControlPostDto.ApprovalStatus.CheckForValidStringResponse(true, true, true, 50))
                {
                    throw new Exception("Invalid Approval Status");
                }

                if (configControlPostDto.Name.CheckForValidStringResponse(false, true, false, 0))
                {
                    throw new Exception("Invalid Approval Status");
                }
                if (configControlPostDto.UserName.CheckForValidStringResponse(false, true, false, 0))
                {
                    throw new Exception("Invalid User Name");
                }

                var htmlString = await _isignOffServices.GetSignOffHtmlStringForConfiguration(configControlPostDto.Name, configControlPostDto.ApprovalDate, configControlPostDto.ApprovalStatus, configControlPostDto.Comment, configControlPostDto.UserName, "Configuration Control");

                var converter = new HtmlToPdf();
                PdfDocument doc = converter.ConvertHtmlString(htmlString);
                byte[] pdfBytes = doc.Save();
                doc.Close();

                configControlPostDto.SignOffPdfData = pdfBytes;

                await _iSectionEnableDisableServices.InsertConfigControlInfo(configControlPostDto);
                return Ok("Data successfully inserted");
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Authorize(Policy = "User Admin")]
        [Route("/api/getConfigControlHistory")]
        public async Task<IActionResult> GetConfigControlHistory(string serverParams)
        {
            try
            {
                var parameters = JsonConvert.DeserializeObject<ConfigControlHistoryServerParamsDto>(serverParams);
                var response = await _iSectionEnableDisableServices.GetConfigControlHistory(parameters);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Authorize(Policy = "User Admin")]
        [Route("/api/checkIfAllSubsectionIsEnabled")]
        public async Task<IActionResult> CheckIfAllSubsectionIsEnabled()
        {
            try
            {
                var response = await _iSectionEnableDisableServices.CheckIfAllSubsectionIsEnabled();
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