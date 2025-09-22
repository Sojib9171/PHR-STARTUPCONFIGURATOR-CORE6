using HBS.ImplementationAutomationConsole.Core.BLL.IServices;
using HBS.ImplementationAutomationConsole.Core.DataAccess.IRepository;
using HBS.ImplementationAutomationConsole.Core.Models;
using System.Data;

namespace HBS.ImplementationAutomationConsole.Core.BLL.Services
{
    public class CommonConfigServices : ICommonConfigServices
    {

        private readonly ICommonConfigRepository _iCommonConfigRepository;

        public CommonConfigServices(ICommonConfigRepository iCommonConfigRepository)
        {
            _iCommonConfigRepository = iCommonConfigRepository;
        }

        public async Task<object> GetCommonConfigRadioOptions(int questionNo)
        {
            var radioOptions = await _iCommonConfigRepository.GetRadioOptionsWithQuestionNo(questionNo);

            var data = (from record in radioOptions
                        select new
                        {
                            option_label = record.Option_Label,
                            option_value = record.Option_Value
                        }
           ).ToArray();

            return data;
        }

        public async Task<object> GetCommonConfigQuestionDetails()
        {
            var questionList = await _iCommonConfigRepository.GetCommonConfigQuestionDetails();

            var data = (from record in questionList
                        select new
                        {
                            question_no=record.Question_No,
                            question_statement=record.Question_Statement,
                            question_type=record.Question_Type
                        }
                       ).ToArray();

            return data;
        }

        public async Task<int> InsertAndGetLastInsertedId(int questionNo, string responseText)
        {
            var id = await _iCommonConfigRepository.InsertAndGetLastInsertedId(questionNo+1,responseText);
            return id;
        }

        public async Task InsertIntoDraftTable(string userID, int tableRowId, bool isDraft, DateTime dateValue)
        {
            await _iCommonConfigRepository.InsertIntoDraftTable(userID,tableRowId,isDraft, dateValue);
        }

        public async Task UpdateTextColumnByRowId(int recordId, int questionNo, string responseText)
        {
            await _iCommonConfigRepository.UpdateTextColumnByRowId(recordId,questionNo+1,responseText);
        }

        public async Task UpdateImageColumnByRowId(int recordId, int questionNo, byte[] bytes)
        {
            await _iCommonConfigRepository.UpdateImageColumnByRowId(recordId, questionNo + 1, bytes);
        }

        public async Task UpdateOptionsColumnByRowId(int recordId, int questionNo, bool response)
        {
            await _iCommonConfigRepository.UpdateOptionColumnByRowId(recordId, questionNo + 1, response);
        }

        public async Task<object> GetCommonConfigSummary(int rowId)
        {
            var questionList = await _iCommonConfigRepository.GetCommonConfigQuestionDetails();
            var responseList = await _iCommonConfigRepository.GetCommonConfigResponsedByRowId(rowId);

            var combinedArray = new object[questionList.Count];
            for (int i = 0; i < questionList.Count; i++)
            {
                combinedArray[i] = new
                {
                    question_no = questionList[i].Question_No,
                    question_statement= questionList[i].Question_Statement,
                    question_type= questionList[i].Question_Type,
                    response = responseList[i]
                };
            }
            return combinedArray;
        }

        public async Task UpdateDraftStatusForCommonConfig(int rowId)
        {
            await _iCommonConfigRepository.UpdateDraftStatusForCommonConfig(rowId);
        }

        public async Task InsertCommonConfigInfo(CommonConfigPostDto commonConfigPostDto)
        {
            await _iCommonConfigRepository.InsertCommonConfigInfo(commonConfigPostDto);
        }

        public async Task DeleteFromDraftTable(int tableRowId)
        {
            await _iCommonConfigRepository.DeleteFromDraftTable(tableRowId);
        }

        public async Task DeleteFromMainTable(int tableRowId)
        {
            await _iCommonConfigRepository.DeleteFromMainTable(tableRowId);
        }

        public async Task<CommonConfigDrftStatusDto> GetIsDraftStatus(string userId)
        {
            var isDraftModel = await _iCommonConfigRepository.GetIsDraftStatus(userId);
            return isDraftModel;
        }

        public async Task UpdateImageNameColumnByRowId(int recordId, string? logoImageName)
        {
            await _iCommonConfigRepository.UpdateImageNameColumnByRowId(recordId, logoImageName);
        }

        public async Task UpdateMobileImageNameColumnByRowId(int recordId, string? mobileLogoImageName)
        {
            await _iCommonConfigRepository.UpdateMobileImageNameColumnByRowId(recordId, mobileLogoImageName);
        }

        public async Task<bool> GetCommonConfigApprovalStatus(string userId)
        {
            return await _iCommonConfigRepository.GetCommonConfigApprovalStatus(userId);
        }

        public async Task<object> GetCommonConfigHistory(CommonConfigHistoryServerParamsDto? parameters)
        {
            var tableInfoList = await _iCommonConfigRepository.GetCommonConfigHistory(
                    parameters.userid,
                    parameters.Page,
                    parameters.PerPage,
                    parameters.searchText,
                    parameters.SortField + " " + parameters.SortType);

            var dataTableObjects = new
            {
                recordsTotal = tableInfoList.totalDisplay,
                data = (from record in tableInfoList.records
                        select new
                        {
                            record_id=record.RecordID.ToString(),
                            approval_status = record.ApprovalStatus.ToString(),
                            approval_by = record.Name.ToString(),
                            approval_date = record.ApprovalDate.ToString("yyyy-MM-dd"),
                            signoff_data = record.SignOffPdfData,
                            approval_comment = record.ApprovalComment.ToString(),
                            table_recordId=record.TableRecordId.ToString(),
                        }
                ).ToArray()
            };
            return dataTableObjects;
        }
    }
}