using HBS.ImplementationAutomationConsole.Core.BLL.IServices;
using HBS.ImplementationAutomationConsole.Core.DataAccess.Data;
using HBS.ImplementationAutomationConsole.Core.DataAccess.IRepository;
using HBS.ImplementationAutomationConsole.Core.Models;
using System.Data;

namespace HBS.ImplementationAutomationConsole.Core.BLL.Services
{
    public class WizardDataServices : IWizardDataServices
    {

        private readonly IWizardDataRepository _iwizardDataRepository;
        private readonly IExcelDataRepository _iexcelDataRepository;
        private readonly ITemplateRepository _itemplateRepository;
        private readonly IBaseDataAccess _ibaseDataAccess;
        private readonly IDashboardRepository _idashboardRepository;

        public WizardDataServices(IDashboardRepository idashboardRepository, IBaseDataAccess ibaseDataAccess, IWizardDataRepository iwizardDataRepository, IExcelDataRepository iexcelDataRepository, ITemplateRepository itemplateRepository)
        {
            _iwizardDataRepository = iwizardDataRepository;
            _iexcelDataRepository = iexcelDataRepository;
            _itemplateRepository = itemplateRepository;
            _ibaseDataAccess = ibaseDataAccess;
            _idashboardRepository = idashboardRepository;
        }

        public async Task<bool> CheckForDuplicateLeaveTypeStatutoryLeave(string responseText, string userId)
        {
            var response = await _iwizardDataRepository.CheckForDuplicateLeaveTypeStatutoryLeave(responseText, userId);
            return response;
        }

        public async Task<bool> CheckForDuplicateLeaveTypeShortLeave(string responseText, string userId)
        {
            var response = await _iwizardDataRepository.CheckForDuplicateLeaveTypeShortLeave(responseText,userId);
            return response;
        }

        public async Task<bool> CheckForDuplicateLeaveTypeStatutoryLeaveFirst(string responseText)
        {
            var response = await _iwizardDataRepository.CheckForDuplicateLeaveTypeStatutoryLeaveFirst(responseText);
            return response;
        }

        public async Task<bool> CheckForDuplicateLeaveTypeShortLeaveFirst(string responseText)
        {
            var response = await _iwizardDataRepository.CheckForDuplicateLeaveTypeShortLeaveFirst(responseText);
            return response;
        }

        public async Task DeleteDataFromDraftAndMainTable(string subsectionName)
        {
            await _iwizardDataRepository.DeleteDataFromDraftAndMainTable(subsectionName);
        }

        public async Task DeleteDataFromDependentTable(string subsectionName)
        {
            await _iwizardDataRepository.DeleteDataFromDependentTable(subsectionName);
        }

        public async Task DeleteFromDraftTable(int tableRowId, string subsectionName)
        {
            await _iwizardDataRepository.DeleteFromDraftTable(tableRowId, subsectionName);
        }


        public async Task DeleteFromMainTable(int tableRowId, string subsectionName)
        {
            await _iwizardDataRepository.DeleteFromMainTable(tableRowId, subsectionName);
        }

        public async Task<IList<int>> GetAbsenceWizardPendingOptionsRowNumber(string subsectionName)
        {
            var response=await _iwizardDataRepository.GetAbsenceWizardPendingOptionsRowNumber(subsectionName);
            return response;

        }

        public async Task<object> GetAbsenceWizardSelectedOptionsSummary(int rowId, string subsectionName)
        {
            var subsectionInfo = await _idashboardRepository.GetModuleAndSubsectioninfoFromSubsectionName(subsectionName);
            var subsectionId = subsectionInfo.Item2;

            var questionList = await _iwizardDataRepository.GetAbsenceWizardQuestionDetails(subsectionId);
            var responseList = await _iwizardDataRepository.GetAbsenceWizardResponsedByRowId(rowId, subsectionName);

            var combinedArray = new object[questionList.Count];
            for (int i = 0; i < questionList.Count; i++)
            {
                combinedArray[i] = new
                {
                    question_no = questionList[i].Question_No,
                    question_statement = questionList[i].Question_Statement,
                    question_type = questionList[i].Question_Type,
                    response = (responseList[i].ToString().EndsWith(".00"))? responseList[i].ToString().Replace(".00","").Trim() : responseList[i].ToString().Trim()
                };
            }
            return combinedArray;
        }

        public async Task<AbsenceWizardDrftStatusDto> GetIsDraftStatusForAbsence(string userId, string subsectionName)
        {
            var isDraftModel = await _iwizardDataRepository.GetIsDraftStatus(userId, subsectionName);
            return isDraftModel;
        }

        public async Task<object> GetPendingLeaveTypesData(AbsenceServerParamsDto? parameters)
        {
            var InfoList = await _iwizardDataRepository.GetPendingLeaveTypesData(
            parameters.Page,
            parameters.PerPage,
            parameters.SearchText,
            parameters.SortField + " " + parameters.SortType,
            parameters.Subsection,
            parameters.RowIds);

            var dataTableObjects = new
            {
                recordsTotal = InfoList.totalDisplay,
                data = (from record in InfoList.records
                        select new
                        {
                            record_id = record.RecordID,
                            leaveTypeCode= record.LeaveTypeCode
                        }
                ).ToArray()
            };
            return dataTableObjects;
        }

        public async Task<IList<string>> GetWizardDropdownOptions(string subsectionName, int questionNo)
        {
            var moduleAndSubsectionInfo = await _idashboardRepository.GetModuleAndSubsectioninfoFromSubsectionName(subsectionName);
            var moduleID = moduleAndSubsectionInfo.Item1;
            var subsectionID = moduleAndSubsectionInfo.Item2;
            var dropdownOptions = await _iwizardDataRepository.GetDropDownOtipnsWithQuestionNo(moduleID, subsectionID, questionNo);
            return dropdownOptions;
        }

        public async Task<object> GetWizardQuestionDetails(string subsection)
        {
            var questionList = await _iwizardDataRepository.GetWizardQuestionDetails(subsection);

            var data = (from record in questionList
                        select new
                        {
                            question_no = record.Question_No,
                            question_statement = record.Question_Statement,
                            question_type = record.Question_Type
                        }
                       ).ToArray();

            return data;
        }

        public async Task<int> InsertAndGetLastInsertedId(int questionNo, string responseText, string subsectionName)
        {
            var id = await _iwizardDataRepository.InsertAndGetLastInsertedId(questionNo + 1, responseText, subsectionName);
            return id;
        }

        public async Task InsertIntoDraftTable(string userID, int tableRowId, bool isDraft, DateTime dateValue, string subsectionName, bool isApproved)
        {
            await _iwizardDataRepository.InsertIntoDraftTable(userID, tableRowId, isDraft, dateValue, subsectionName,isApproved);
        }

        public async Task UpdateDraftAndApprovalStatusForAbsence(int[] rowIDs, string subsection)
        {
            await _iwizardDataRepository.UpdateDraftAndApprovalStatusForAbsence(rowIDs,subsection);
        }

        public async Task UpdateDraftStatusForAbsence(int tableRowId, string subsectionName)
        {
            await _iwizardDataRepository.UpdateDraftStatusForAbsence(tableRowId,subsectionName);
        }

        public async Task UpdateNumericColumnByRowId(int recordId, int questionNo, decimal response, string? subsectionName)
        {
            await _iwizardDataRepository.UpdateNumericColumnByRowId(recordId, questionNo + 1, response, subsectionName);
        }

        public async Task UpdateOptionsColumnByRowId(int recordId, int questionNo, bool response, string subsectionName)
        {
            await _iwizardDataRepository.UpdateOptionsColumnByRowId(recordId, questionNo + 1, response, subsectionName);
        }

        public async Task UpdateTextColumnByRowId(int recordId, int questionNo, string? response, string subsectionName)
        {
            await _iwizardDataRepository.UpdateTextColumnByRowId(recordId, questionNo + 1, response, subsectionName);
        }

        public async Task UploadWizardData(List<List<WizarDataInfoDto>> wizardTypes, string subsectionName)
        {
            var templateId = await _itemplateRepository.GetTemplateIdFromSubsectionName(subsectionName);
            var columnMaps = await _iexcelDataRepository.GetColumnNames(templateId);
            List<string> columnNames = columnMaps.Select(tuple => tuple.Item2).ToList();
            var dataTable = await WizardToDataTable(wizardTypes, columnNames);
            var tableName = await _iexcelDataRepository.GetTableNameFromSubsectionName(subsectionName);
            await _iexcelDataRepository.DeleteDataFromTable(tableName);
            await _ibaseDataAccess.BulkInsert(dataTable.CreateDataReader(), tableName, columnMaps);
        }

        public async Task<DataTable> WizardToDataTable(List<List<WizarDataInfoDto>> wizardTypes, List<string> columnNames)
        {
            var rowCount = wizardTypes.Count;
            var colCount = wizardTypes[0].Count;
            var dataTable = new DataTable();
            foreach (var column in columnNames)
            {
                dataTable.Columns.Add(column);
            }

            for (int i = 0; i < rowCount; i++)
            {
                var dataRow = dataTable.NewRow();
                for (int j = 0; j < colCount; j++)
                {
                    var colValue = wizardTypes[i][j].Response.ToString();

                    if (wizardTypes[i][j].Question_type == "Text" || wizardTypes[i][j].Question_type == "Drop down-Text")
                    {
                        if (string.IsNullOrEmpty(colValue))
                        {
                            dataRow[j] = null;
                            continue;
                        }
                        dataRow[j] = colValue;
                    }

                    else if (wizardTypes[i][j].Question_type == "Yes/No")
                    {
                        if (string.IsNullOrEmpty(colValue))
                        {
                            dataRow[j] = null;
                            continue;
                        }
                        dataRow[j] = colValue == "Yes" ? 1 : 0;
                    }

                    else if (wizardTypes[i][j].Question_type == "Numeric")
                    {
                        if (string.IsNullOrEmpty(colValue))
                        {
                            dataRow[j] = null;
                            continue;
                        }
                        dataRow[j] = int.Parse(wizardTypes[i][j].Response.ToString());
                    }
                }
                dataTable.Rows.Add(dataRow);
            }
            return dataTable;
        }

        public async Task<bool> checkDuplicateApplySeqForStatutoryLeave(string responseText, string userId)
        {
           var response= await _iwizardDataRepository.checkDuplicateApplySeqForStatutoryLeave(responseText, userId);
           return response;
        }

        public async Task<bool> CheckIfSmallerThanPreviousColumnValue(string responseText, int questionNo, int rowId, string subsectionName)
        {
            var response = await _iwizardDataRepository.CheckIfSmallerThanPreviousColumnValue(responseText, questionNo, rowId,subsectionName);
            return response;
        }

        public async Task<bool> CheckIfGreaterThanMaximumForShortLeave(string responseText, int questionNo, int rowId)
        {
            var response = await _iwizardDataRepository.CheckIfGreaterThanMaximumForShortLeave(responseText, questionNo, rowId);
            return response;
        }

        public async Task<bool> CheckIfLessThanMaximumForShortLeave(string responseText, int questionNo, int rowId)
        {
            var response = await _iwizardDataRepository.CheckIfLessThanMaximumForShortLeave(responseText, questionNo, rowId);
            return response;
        }

        public async Task<bool> CheckIfLessThanMinimumForShortLeave(string responseText, int questionNo, int rowId)
        {
            var response = await _iwizardDataRepository.CheckIfLessThanMinimumForShortLeave(responseText, questionNo, rowId);
            return response;
        }

        public async Task<bool> CheckIfZeroOrNullValue(int questionNo, int rowId, string subsectionName)
        {
            var response = await _iwizardDataRepository.CheckIfZeroOrNullValue( questionNo, rowId, subsectionName);
            return response;
        }

        public async Task<bool> CheckIfPreviousColumnIsNotNullForShortLeave(int questionNo, int rowId)
        {
            var response = await _iwizardDataRepository.CheckIfPreviousColumnIsNotNullForShortLeave(questionNo, rowId);
            return response;
        }

        public async Task<bool> CheckIfPreviousColumnIsNotNullForStatLeave(int questionNo, int rowId)
        {
            var response = await _iwizardDataRepository.CheckIfPreviousColumnIsNotNullForStatLeave(questionNo, rowId);
            return response;
        }

        public async Task<bool> checkIfPreviousTwoColumnsAreNotNullForShortLeave(int questionNo, int rowId)
        {
            var response = await _iwizardDataRepository.checkIfPreviousTwoColumnsAreNotNullForShortLeave(questionNo, rowId);
            return response;
        }

        public async Task<bool> CheckIfColumnBeforeTheLastIsNotNullForShortLeave(int questionNo, int rowId)
        {
            var response = await _iwizardDataRepository.CheckIfColumnBeforeTheLastIsNotNullForShortLeave(questionNo, rowId);
            return response;
        }

        public async Task DeleteSelectedRowsFromDraftAndMainTable(List<int> rowIds, string subsectionName)
        {
            await _iwizardDataRepository.DeleteSelectedRowsFromDraftAndMainTable(rowIds,subsectionName);
        }

        public async Task<bool> CheckExistingLeaveTypeShortLeave(string responseText)
        {
            return await _iwizardDataRepository.CheckExistingLeaveTypeShortLeave(responseText);
        }

        public async Task<bool> CheckExistingLeaveTypeStatutoryLeave(string responseText)
        {
            return await _iwizardDataRepository.CheckExistingLeaveTypeStatutoryLeave(responseText);
        }
    }
}