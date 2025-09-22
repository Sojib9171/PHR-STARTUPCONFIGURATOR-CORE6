using HBS.ImplementationAutomationConsole.Core.BLL.IServices;
using HBS.ImplementationAutomationConsole.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ILogger = Serilog.ILogger;

namespace HBS.ImplementationAutomationConsole.API.Controllers
{
    [ApiController]
    public class WizardDataController : ControllerBase
    {
        private readonly IWizardDataServices _iwizardDataServices;
        private readonly ILogger _logger;
        public WizardDataController(
            IWizardDataServices iwizardDataServices, ILogger logger)
        {
            _iwizardDataServices = iwizardDataServices;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Policy = "User")]
        [Route("/api/getWizardQuestionDetails")]
        public async Task<IActionResult> GetWizardQuestionDetails(string subsectionName)
        {
            try
            {
                var questionDetails = await _iwizardDataServices.GetWizardQuestionDetails(subsectionName);
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
        [Route("/api/uploadWizardData")]
        public async Task<IActionResult> UploadWizardData([FromBody] WizardDataDto WizardObject)
        {
            try
            {
                await _iwizardDataServices.UploadWizardData(WizardObject.WizardTypes, WizardObject.SubsectionName);
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
        [Route("/api/getDropdownOptions")]
        public async Task<IActionResult> GetDropdownOptions(string parameters)
        {
            try
            {
                var wizardDropdownDto = JsonConvert.DeserializeObject<WizardDropdownDto>(parameters);
                var response = await _iwizardDataServices.GetWizardDropdownOptions(wizardDropdownDto.subsectionName, wizardDropdownDto.questionNo);
                return Ok(response.ToList());
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        [Authorize(Policy = "User")]
        [Route("/api/insertFirstRecordForAbsence")]
        public async Task<IActionResult> InsertFirstRecordForAbsence([FromBody] AbsenceWizardDto absenceWizardDto)
        {
            try
            {
                var Id = await _iwizardDataServices.InsertAndGetLastInsertedId(absenceWizardDto.QuestionNo, absenceWizardDto.ResponseText, absenceWizardDto.SubsectionName);
                await _iwizardDataServices.InsertIntoDraftTable(absenceWizardDto.UserID, Id, true, absenceWizardDto.DateValue, absenceWizardDto.SubsectionName,false);
                return Ok(Id);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Policy = "User")]
        [Route("/api/updateRecordForAbsenceWizard")]
        public async Task<IActionResult> UpdateRecordForAbsenceWizard([FromBody] AbsenceWizardUpdateDto absenceWizardUpdateDto)
        {
            try
            {
                if (absenceWizardUpdateDto.QuestionType == "Drop down-Text" || absenceWizardUpdateDto.QuestionType == "Text")
                {
                    await _iwizardDataServices.UpdateTextColumnByRowId(absenceWizardUpdateDto.RecordId, absenceWizardUpdateDto.QuestionNo, absenceWizardUpdateDto.ResponseText.ToString(), absenceWizardUpdateDto.SubsectionName);
                }

                else if (absenceWizardUpdateDto.QuestionType == "Yes/No")
                {
                    var response = Convert.ToBoolean(absenceWizardUpdateDto.ResponseText.ToString());
                    await _iwizardDataServices.UpdateOptionsColumnByRowId(absenceWizardUpdateDto.RecordId, absenceWizardUpdateDto.QuestionNo, response, absenceWizardUpdateDto.SubsectionName);
                }

                else if (absenceWizardUpdateDto.QuestionType == "Numeric")
                {
                    var response=Decimal.One;
                    if (string.IsNullOrEmpty(absenceWizardUpdateDto.ResponseText.ToString()))
                    {
                        response = -1;
                    }
                    else
                    {
                        response = Decimal.Parse(absenceWizardUpdateDto.ResponseText.ToString());
                    }

                    await _iwizardDataServices.UpdateNumericColumnByRowId(absenceWizardUpdateDto.RecordId, absenceWizardUpdateDto.QuestionNo, response, absenceWizardUpdateDto.SubsectionName);
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
        [Route("/api/getAbsenceWizardSelectedOptionsSummary")]
        public async Task<IActionResult> GetAbsenceWizardSelectedOptionsSummary(int rowId,string subsectionName)
        {
            try
            {
                var response = await _iwizardDataServices.GetAbsenceWizardSelectedOptionsSummary(rowId, subsectionName);
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
        [Route("/api/deleteAbsenceDataByRowId")]
        public async Task<IActionResult> DeleteDataByRowId([FromBody] DeleteByRowDto model)
        {
            try
            {
                await _iwizardDataServices.DeleteFromDraftTable(model.tableRowId, model.subsectionName);
                await _iwizardDataServices.DeleteFromMainTable(model.tableRowId, model.subsectionName);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        [Authorize]
        [Route("/api/deleteDataFromDraftAndMainTable")]
        public async Task<IActionResult> DeleteDataFromDraftAndMainTable([FromBody]DeleteBySubsectionDto model)
        {
            try
            {
                await _iwizardDataServices.DeleteDataFromDraftAndMainTable(model.subsectionName);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Authorize]
        [Route("/api/deleteDataFromDependentTable")]
        public async Task<IActionResult> DeleteDataFromDependentTable([FromBody] DeleteBySubsectionDto model)
        {
            try
            {
                if(model.subsectionName=="Short Leave")
                    await _iwizardDataServices.DeleteDataFromDependentTable(model.subsectionName);
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
        [Route("/api/deleteMultipleAbsenceRecords")]
        public async Task<IActionResult> DeleteMultipleAbsenceRecords([FromBody] AbsenceWizardMultipleDeleteDto model)
        {
            try
            {
                var rowIds = model.selectedRows.Select(x => x.record_id).ToList();
                await _iwizardDataServices.DeleteSelectedRowsFromDraftAndMainTable(rowIds,model.subsectionName);
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
        [Route("/api/getPendingLeaveTypes")]
        public async Task<IActionResult> GetPendingLeaveTypes(string serverParams)
        {
            try
            {
                var parameters = JsonConvert.DeserializeObject<AbsenceServerParamsDto>(serverParams);
                var response = await _iwizardDataServices.GetPendingLeaveTypesData(parameters);

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
        [Route("/api/updateDraftStatusForAbsence")]
        public async Task<IActionResult> UpdateDraftStatusForAbsence([FromBody] AbsenceWizardDrftDto absenceWizardDrftDto)
        {
            try
            {
                await _iwizardDataServices.UpdateDraftStatusForAbsence(absenceWizardDrftDto.TableRowId,absenceWizardDrftDto.SubsectionName);
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
        [Route("/api/getAbsenceWizardPendingOptionsRowNumber")]
        public async Task<IActionResult> GetAbsenceWizardPendingOptionsRowNumber(string subsectionName)
        {
            try
            {
                var response = await _iwizardDataServices.GetAbsenceWizardPendingOptionsRowNumber(subsectionName);
                return Ok(response.ToArray());
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Authorize(Policy = "User")]
        [Route("/api/getIsDraftStatusForAbsence")]
        public async Task<IActionResult> GetIsDraftStatusForAbsence(string userId,string subsectionName)
        {
            try
            {
                var response = await _iwizardDataServices.GetIsDraftStatusForAbsence(userId, subsectionName);
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
        [Route("/api/checkForDuplicateLeaveTypeStatutoryLeave")]
        public async Task<IActionResult> CheckForDuplicateLeaveTypeStatutoryLeave(string responseText, string userId)
        {
            try
            {
                var response = await _iwizardDataServices.CheckForDuplicateLeaveTypeStatutoryLeave(responseText,userId);
                if (response == false)
                {
                    response = await _iwizardDataServices.CheckExistingLeaveTypeStatutoryLeave(responseText);
                }
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
        [Route("/api/checkForDuplicateLeaveTypeShortLeave")]
        public async Task<IActionResult> CheckForDuplicateLeaveTypeShortLeave(string responseText, string userId)
        {
            try
            {
                var response = await _iwizardDataServices.CheckForDuplicateLeaveTypeShortLeave(responseText,userId);
                if (response==false)
                {
                    response = await _iwizardDataServices.CheckExistingLeaveTypeShortLeave(responseText);
                }
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
        [Route("/api/checkForDuplicateLeaveTypeStatutoryLeaveFirst")]
        public async Task<IActionResult> CheckForDuplicateLeaveTypeStatutoryLeaveFirst(string responseText)
        {
            try
            {
                var response = await _iwizardDataServices.CheckForDuplicateLeaveTypeStatutoryLeaveFirst(responseText);
                if (response == false)
                {
                    response = await _iwizardDataServices.CheckExistingLeaveTypeStatutoryLeave(responseText);
                }
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
        [Route("/api/checkForDuplicateLeaveTypeShortLeaveFirst")]
        public async Task<IActionResult> CheckForDuplicateLeaveTypeShortLeaveFirst(string responseText)
        {
            try
            {
                var response = await _iwizardDataServices.CheckForDuplicateLeaveTypeShortLeaveFirst(responseText);
                if (response == false)
                {
                    response = await _iwizardDataServices.CheckExistingLeaveTypeShortLeave(responseText);
                }
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
        [Route("/api/checkDuplicateApplySeqForStatutoryLeave")]
        public async Task<IActionResult> checkDuplicateApplySeqForStatutoryLeave(string responseText,string userId)
        {
            try
            {
                var response = await _iwizardDataServices.checkDuplicateApplySeqForStatutoryLeave(responseText,userId);
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
        [Route("/api/checkIfSmallerThanPreviousColumnValue")]
        public async Task<IActionResult> CheckIfSmallerThanPreviousColumnValue(string responseText, int questionNo,int rowId, string subsectionName)
        {
            try
            {
                var response = await _iwizardDataServices.CheckIfSmallerThanPreviousColumnValue(responseText, questionNo,rowId,subsectionName);
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
        [Route("/api/checkIfPreviousColumnIsNotNullForStatLeave")]
        public async Task<IActionResult> CheckIfPreviousColumnIsNullValueForStatLeave( int questionNo, int rowId)
        {
            try
            {
                var response = await _iwizardDataServices.CheckIfPreviousColumnIsNotNullForStatLeave( questionNo, rowId);
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
        [Route("/api/checkIfZeroOrNullValue")]
        public async Task<IActionResult> CheckIfZeroOrNullValue(int questionNo, int rowId, string subsectionName)
        {
            try
            {
                var response = await _iwizardDataServices.CheckIfZeroOrNullValue(questionNo, rowId, subsectionName);
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
        [Route("/api/checkIfGreaterThanMaximumForShortLeave")]
        public async Task<IActionResult> CheckIfGreaterThanMaximumForShortLeave(string responseText, int questionNo, int rowId)
        {
            try
            {
                var response = await _iwizardDataServices.CheckIfGreaterThanMaximumForShortLeave(responseText, questionNo, rowId);
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
        [Route("/api/checkIfLessThanMaximumForShortLeave")]
        public async Task<IActionResult> CheckIfLessThanMaximumForShortLeave(string responseText, int questionNo, int rowId)
        {
            try
            {
                var response = await _iwizardDataServices.CheckIfLessThanMaximumForShortLeave(responseText, questionNo, rowId);
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
        [Route("/api/checkIfLessThanMinimumForShortLeave")]
        public async Task<IActionResult> CheckIfLessThanMinimumForShortLeave(string responseText, int questionNo, int rowId)
        {
            try
            {
                var response = await _iwizardDataServices.CheckIfLessThanMinimumForShortLeave(responseText, questionNo, rowId);
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
        [Route("/api/checkIfPreviousColumnIsNotNullForShortLeave")]
        public async Task<IActionResult> CheckIfPreviousColumnIsNotNullForShortLeave(int questionNo, int rowId)
        {
            try
            {
                var response = await _iwizardDataServices.CheckIfPreviousColumnIsNotNullForShortLeave(questionNo, rowId);
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
        [Route("/api/checkIfColumnBeforeTheLastIsNotNullForShortLeave")]
        public async Task<IActionResult> CheckIfColumnBeforeTheLastIsNotNullForShortLeave(int questionNo, int rowId)
        {
            try
            {
                var response = await _iwizardDataServices.CheckIfColumnBeforeTheLastIsNotNullForShortLeave(questionNo, rowId);
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
        [Route("/api/checkIfPreviousTwoColumnsAreNotNullForShortLeave")]
        public async Task<IActionResult> checkIfPreviousTwoColumnsAreNotNullForShortLeave(int questionNo, int rowId)
        {
            try
            {
                var response = await _iwizardDataServices.checkIfPreviousTwoColumnsAreNotNullForShortLeave( questionNo, rowId);
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