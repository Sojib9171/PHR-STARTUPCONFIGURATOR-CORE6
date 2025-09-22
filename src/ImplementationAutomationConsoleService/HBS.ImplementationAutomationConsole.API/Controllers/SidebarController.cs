using HBS.ImplementationAutomationConsole.Core.BLL.IServices;
using HBS.ImplementationAutomationConsole.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;

namespace HBS.ImplementationAutomationConsole.API.Controllers
{
    [ApiController]
    public class SidebarController : ControllerBase
    {
        private readonly ISidebarServices _sidebarServices;
        private readonly ILogger _logger;
        public SidebarController(
            ISidebarServices sidebarServices, ILogger logger)
        {
            _sidebarServices = sidebarServices;
            _logger = logger;
        }

        [Route("/api/GetSidebarComponents")]
        [Authorize(Policy = "User")]
        [HttpGet]
        public async Task<IActionResult> GetSidebarComponents()
        {
            try
            {
                IList<SidebarDto> sideBarComponents = await _sidebarServices.GetSubsectionItems();
                return Ok(sideBarComponents);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [Route("/api/GetActiveModules")]
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetActiveModules()
        {
            try
            {
                var moduleList = await _sidebarServices.GetActiveModules();
                return Ok(moduleList);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [Route("/api/GetActiveModulesFromDashboard")]
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetActiveModulesFromDashboard()
        {
            try
            {
                var moduleList = await _sidebarServices.GetActiveModulesFromDashboard();
                return Ok(moduleList);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return BadRequest(ex.Message);
            }
        }


        [Route("/api/GetActiveModuleIds")]
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetActiveModuleIds()
        {
            try
            {
                var moduleList = await _sidebarServices.GetActiveModuleIDs();
                return Ok(moduleList.ToList());
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [Route("/api/GetActiveSubsectionsEim")]
        [Authorize(Policy = "User Admin")]
        [HttpGet]
        public async Task<IActionResult> GetActiveSubsectionsEim()
        {
            try
            {
                var subsecList = await _sidebarServices.GetActiveSubsectionsEim();
                return Ok(subsecList);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [Route("/api/GetActiveSubsectionsLeave")]
        [Authorize(Policy = "User Admin")]
        [HttpGet]
        public async Task<IActionResult> GetActiveSubsectionsLeave()
        {
            try
            {
                var subsecList = await _sidebarServices.GetActiveSubsectionsLeave();
                return Ok(subsecList);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [Route("/api/GetActiveSubsectionsAttendance")]
        [Authorize(Policy = "User Admin")]
        [HttpGet]
        public async Task<IActionResult> GetActiveSubsectionsAttendance()
        {
            try
            {
                var subsecList = await _sidebarServices.GetActiveSubsectionsAttendance();
                return Ok(subsecList);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [Route("/api/GetCopyRightText")]
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetCopyRightText()
        {
            try
            {
                var copyRightText = await _sidebarServices.GetCopyRightText();
                return Ok(copyRightText);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [Route("/api/checkIfSubsectionApproved")]
        [Authorize(Policy = "User")]
        [HttpGet]
        public async Task<IActionResult> CheckIfSubsectionApproved(string subsectionName)
        {
            try
            {
                var response = await _sidebarServices.CheckIfSubsectionApproved(subsectionName);
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