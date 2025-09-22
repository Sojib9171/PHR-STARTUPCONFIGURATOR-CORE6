using HBS.ImplementationAutomationConsole.Core.BLL.IServices;
using HBS.ImplementationAutomationConsole.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SelectPdf;
using ILogger = Serilog.ILogger;
using HBS.ImplementationAutomationConsole.Core.Extensions;

namespace HBS.ImplementationAutomationConsole.API.Controllers
{
    [ApiController]
    public class CommonConfigController : ControllerBase
    {
        private readonly ICommonConfigServices _iCommonConfigServices;
        private readonly ISignOffService _isignOffServices;
        private readonly ILogger _logger;

        public CommonConfigController(
            ICommonConfigServices iCommonConfigServices, ISignOffService isignOffServices, ILogger logger)
        {
            _iCommonConfigServices = iCommonConfigServices;
            _isignOffServices = isignOffServices;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Policy = "User")]
        [Route("/api/getCommonConfigQuestionDetails")]
        public async Task<IActionResult> GetCommonConfigQuestionDetails()
        {
            try
            {
                var questionDetails = await _iCommonConfigServices.GetCommonConfigQuestionDetails();
                return Ok(questionDetails);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Authorize(Policy = "User")]
        [Route("/api/getCommonConfigQuestionDetailsWithAnswers")]
        public async Task<IActionResult> GetCommonConfigQuestionDetailsWithAnswers(int rowId)
        {
            try
            {
                var questionDetails = await _iCommonConfigServices.GetCommonConfigSummary(rowId);
                return Ok(questionDetails);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Policy = "User")]
        [Route("/api/insertFirstRecord")]
        public async Task<IActionResult> InsertFirstDataForCommonConfig([FromBody] CommonConfigDto commonConfigDto)
        {
            try
            {
                if (commonConfigDto.UserID.CheckForValidStringResponse(true, true, true, 40))
                {
                    throw new Exception("Invalid User ID");
                }
                if (commonConfigDto.QuestionType.CheckForValidStringResponse(false, false, true, 200))
                {
                    throw new Exception("Invalid Question Type");
                }

                var Id = await _iCommonConfigServices.InsertAndGetLastInsertedId(commonConfigDto.QuestionNo, commonConfigDto.ResponseText);
                await _iCommonConfigServices.InsertIntoDraftTable(commonConfigDto.UserID, Id, true, commonConfigDto.DateValue);
                return Ok(Id);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Authorize(Policy = "User")]
        [Route("/api/getRadioOptions")]
        public async Task<IActionResult> GetCommonConfigRadioOptions(int questionNo)
        {
            try
            {
                var response = await _iCommonConfigServices.GetCommonConfigRadioOptions(questionNo);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Policy = "User")]
        [Route("/api/updateRecordForCommonConfig")]
        public async Task<IActionResult> UpdateRecordForCommonConfig([FromBody] CommonConfigUpdateDto commonConfigUpdateDto)
        {
            try
            {
                if (commonConfigUpdateDto.QuestionType.CheckForValidStringResponse(false, false, true, 200))
                {
                    throw new Exception("Invalid Question Type");
                }
                //if(!string.IsNullOrEmpty(commonConfigUpdateDto.LogoImageName) && commonConfigUpdateDto.LogoImageName.CheckForValidStringResponse(false,true,false))
                //{
                //    throw new Exception("Invalid Logo Image Name");
                //}
                //if (!string.IsNullOrEmpty(commonConfigUpdateDto.MobileLogoImageName) && commonConfigUpdateDto.MobileLogoImageName.CheckForValidStringResponse(false, true, false))
                //{
                //    throw new Exception("Invalid Mobile Logo Image Name");
                //}

                if (commonConfigUpdateDto.QuestionType == "Select Option" || commonConfigUpdateDto.QuestionType == "Text")
                {
                    await _iCommonConfigServices.UpdateTextColumnByRowId(commonConfigUpdateDto.RecordId, commonConfigUpdateDto.QuestionNo, commonConfigUpdateDto.ResponseText.ToString());
                }
                else if (commonConfigUpdateDto.QuestionType == "Image")
                {
                    byte[] bytes = Convert.FromBase64String(commonConfigUpdateDto.ResponseText.ToString());
                    await _iCommonConfigServices.UpdateImageColumnByRowId(commonConfigUpdateDto.RecordId, commonConfigUpdateDto.QuestionNo, bytes);
                    if(!string.IsNullOrEmpty(commonConfigUpdateDto.LogoImageName))
                    {
                        await _iCommonConfigServices.UpdateImageNameColumnByRowId(commonConfigUpdateDto.RecordId, commonConfigUpdateDto.LogoImageName);

                    }
                    if (!string.IsNullOrEmpty(commonConfigUpdateDto.MobileLogoImageName))
                    {
                        await _iCommonConfigServices.UpdateMobileImageNameColumnByRowId(commonConfigUpdateDto.RecordId, commonConfigUpdateDto.MobileLogoImageName);
                    }
                }
                else if (commonConfigUpdateDto.QuestionType == "Yes/No")
                {
                    var response = Convert.ToBoolean(commonConfigUpdateDto.ResponseText.ToString());
                    await _iCommonConfigServices.UpdateOptionsColumnByRowId(commonConfigUpdateDto.RecordId, commonConfigUpdateDto.QuestionNo, response);
                }
                else
                {
                    throw new Exception("Invalid Question Type");
                }
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
        [Route("/api/getCommonConfigSummary")]
        public async Task<IActionResult> GetCommonConfigSummary(int rowId)
        {
            try
            {
                var response = await _iCommonConfigServices.GetCommonConfigSummary(rowId);
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
        [Route("/api/getCommonConfigApprovalStatus")]
        public async Task<IActionResult> GetCommonConfigApprovalStatus(string userId)
        {
            try
            {
                if(userId.CheckForValidStringResponse(true,true,true,40))
                {
                    throw new Exception("Invalid username");
                }
                var response = await _iCommonConfigServices.GetCommonConfigApprovalStatus(userId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Policy = "User")]
        [Route("/api/updateDraftStatusForCommonConfig")]
        public async Task<IActionResult> UpdateDraftStatusForCommonConfig([FromBody] CommonConfigDrftDto commonConfigDrftDto)
        {
            try
            {
                await _iCommonConfigServices.UpdateDraftStatusForCommonConfig(commonConfigDrftDto.TableRowId);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Policy = "User")]
        [Route("/api/uploadCommonConfigData")]
        public async Task<IActionResult> UploadCommonConfigData([FromBody] CommonConfigPostDto commonConfigPostDto)
        {
            try
            {
                if (commonConfigPostDto.UserName.CheckForValidStringResponse(false, true, false))
                {
                    throw new Exception("Invalid User Name");
                }
                if (commonConfigPostDto.Name.CheckForValidStringResponse(false, true, false))
                {
                    throw new Exception("Invalid Name");
                }
                if (commonConfigPostDto.ApprovalStatus.CheckForValidStringResponse(false, true, false))
                {
                    throw new Exception("Invalid Approval Status");
                }

                var htmlString = await _isignOffServices.GetSignOffHtmlStringForConfiguration(commonConfigPostDto.Name, commonConfigPostDto.ApprovalDate, commonConfigPostDto.ApprovalStatus, commonConfigPostDto.Comment, commonConfigPostDto.UserName, "Common Configuration");

                var converter = new HtmlToPdf();
                PdfDocument doc = converter.ConvertHtmlString(htmlString);
                byte[] pdfBytes = doc.Save();
                doc.Close();

                commonConfigPostDto.SignOffPdfData = pdfBytes;

                await _iCommonConfigServices.InsertCommonConfigInfo(commonConfigPostDto);
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
        [Route("/api/deleteDataByRowId")]
        public async Task<IActionResult> DeleteDataByRowId([FromBody]DeleteByRowOnlyDto model)
        {
            try
            {
                await _iCommonConfigServices.DeleteFromDraftTable(model.tableRowId);
                await _iCommonConfigServices.DeleteFromMainTable(model.tableRowId);
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
        [Route("/api/getIsDraftStatus")]
        public async Task<IActionResult> GetIsDraftStatus(string userId)
        {
            try
            {
                if(userId.CheckForValidStringResponse(true,true,true,40))
                {
                    throw new Exception("Invalid User Id");
                }
                var response = await _iCommonConfigServices.GetIsDraftStatus(userId);
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
        [Route("/api/getCommonConfigHistory")]
        public async Task<IActionResult> GetCommonConfigHistory(string serverParams)
        {
            try
            {
                var parameters = JsonConvert.DeserializeObject<CommonConfigHistoryServerParamsDto>(serverParams);
                var response = await _iCommonConfigServices.GetCommonConfigHistory(parameters);

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