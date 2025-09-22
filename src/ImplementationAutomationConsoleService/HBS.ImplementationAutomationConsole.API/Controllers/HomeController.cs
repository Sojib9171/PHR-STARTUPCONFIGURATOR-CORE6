using HBS.ImplementationAutomationConsole.Core.BLL.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;

namespace HBS.ImplementationAutomationConsole.API.Controllers
{
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IHomeServices _ihomeServices;
        private readonly ISidebarServices _isidebarServices;
        private readonly ILogger _logger;
        public HomeController(
            IHomeServices ihomeServices, ISidebarServices isidebarServices, ILogger logger)
        {
            _ihomeServices = ihomeServices;
            _isidebarServices = isidebarServices;
            _logger = logger;
        }


        [HttpGet]
        [Authorize(Policy = "User")]
        [Route("/api/getApprovalPercentage")]
        public async Task<IActionResult> GetApprovalPercentage()
        {
            try
            {
                var eimPercentage = await _ihomeServices.GetEimApprovalPercentage();
                var absencePercentage = await _ihomeServices.GetAbsenceApprovalPercentage();
                var attendancePercentage = await _ihomeServices.GetAttendanceApprovalPercentage();

                var response = new List<double>() { Math.Ceiling(eimPercentage), Math.Ceiling(absencePercentage), Math.Ceiling(attendancePercentage) };
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
        [Route("/api/CheckAllSubsectionApproved")]
        public async Task<IActionResult> CheckAllSubsectionApproved()
        {
            try
            {
                var subsectList = await _isidebarServices.GetSubsectionItems();
                var isAllSubsecApproved = await _ihomeServices.CheckAllSubsectionApproved(subsectList);
                return Ok(isAllSubsecApproved);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Authorize(Policy = "User")]
        [Route("/api/getClientURL")]
        public async Task<IActionResult> GetClientURL(string subdomainName)
        {
            try
            {
                var clientURL = await _ihomeServices.GetClientURL(subdomainName);
                return Ok(clientURL);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Authorize(Policy = "User")]
        [Route("/api/getModuleOrdersWithModuleId")]
        public async Task<IActionResult> GetModuleOrdersWithModuleId()
        {
            try
            {
                var response = await _ihomeServices.GetModuleOrdersWithModuleId();
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