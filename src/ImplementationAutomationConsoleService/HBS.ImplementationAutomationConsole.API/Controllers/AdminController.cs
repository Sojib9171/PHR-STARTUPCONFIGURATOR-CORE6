using HBS.ImplementationAutomationConsole.Core.BLL.IServices;
using HBS.ImplementationAutomationConsole.Core.Extensions;
using HBS.ImplementationAutomationConsole.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ILogger = Serilog.ILogger;

namespace HBS.ImplementationAutomationConsole.API.Controllers
{
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminServices _iadminServices;
        private readonly ILogger _logger;
        public AdminController(IAdminServices iadminServices, ILogger logger)
        {
            _iadminServices = iadminServices;
            _logger = logger;
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        [Route("/api/uploadVendorApprovalData")]
        public async Task<IActionResult> UploadAdminData([FromBody] VendorPostDto vendorDto) // Uploading Approval Data in Vendor Approval Table After Admin Approval Or Rejection
        {
            try
            {
                if(vendorDto.VendorName.CheckForValidStringResponse(false,true,false))
                {
                    throw new Exception("Invalid User Name");
                }
                if (vendorDto.VendorApprovalStatus.CheckForValidStringResponse(false, true, false))
                {
                    throw new Exception("Invalid Approval Status");
                }
                await _iadminServices.InsertVendorInfo(vendorDto); 
                return Ok("Data successfully inserted");
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return BadRequest(ex.Message);
            }
        }


        [HttpGet]
        [Authorize(Policy = "Admin")]
        [Route("/api/getPendingApprovalListforVendor")]
        public async Task<IActionResult> GetPendingApprovalListForVendor(string serverParams) // Get Pending Approval Lists for Admin in the Admin Approval Section
        {
            try
            {
                var parameters = JsonConvert.DeserializeObject<ServerParamsDto>(serverParams);
                var response = await _iadminServices.GetApprovalFromVendor(parameters);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Authorize(Policy = "Admin")]
        [Route("/api/getApprovalSummaryListForVendor")]
        public async Task<IActionResult> GetApprovalSummaryListForVendor(string serverParams)
        {
            try
            {
                var parameters = JsonConvert.DeserializeObject<ServerParamsDto>(serverParams);
                var response = await _iadminServices.GetApprovedDataListForVendor(parameters); //Get Approved or Reject Summary List for Admin
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Authorize(Policy = "Admin")]
        [Route("/api/downloadExcelWithRecordID")]
        public async Task<IActionResult> DownloadExcelWithData(int recordId)
        {
            try
            {
                var response = await _iadminServices.GetExcelDataAsByteArray(recordId);
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
        [Route("/api/getVendorDashboardHistory")]
        public async Task<IActionResult> GetVendorDashboardData(string serverParams)
        {
            try
            {
                var parameters = JsonConvert.DeserializeObject<HistoryServerParamsDto>(serverParams);
                var response = await _iadminServices.GetVendorDashboardHistoryInfo(parameters);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return BadRequest(ex.Message);
            }
        }


        [HttpGet]
        [Authorize(Policy = "Admin")]
        [Route("/api/getRemainingApprovalCount")]
        public async Task<IActionResult> GetRemainingApprovalCount()
        {
            try
            {
                var response = await _iadminServices.GetRemainingApprovalCount();
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Authorize(Policy = "Admin")]
        [Route("/api/getVendorApprovalSummaryHistory")]
        public async Task<IActionResult> GetVendorApprovalSummaryHistory(string serverParams)
        {
            try
            {
                var parameters = JsonConvert.DeserializeObject<HistoryServerParamsDto>(serverParams);
                var response = await _iadminServices.GetVendorApprovalSummaryHistory(parameters);

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