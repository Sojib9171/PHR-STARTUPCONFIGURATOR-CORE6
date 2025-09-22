using HBS.ImplementationAutomationConsole.Core.Models;

namespace HBS.ImplementationAutomationConsole.Core.BLL.IServices
{
    public interface IWizardDataServices
    {
        public Task<bool> CheckForDuplicateLeaveTypeStatutoryLeave(string responseText, string userId);
        public Task<bool> CheckForDuplicateLeaveTypeShortLeave(string responseText, string userId);
        public Task<bool> CheckForDuplicateLeaveTypeStatutoryLeaveFirst(string responseText);
        public Task<bool> CheckForDuplicateLeaveTypeShortLeaveFirst(string responseText);
        public Task<bool> checkDuplicateApplySeqForStatutoryLeave(string responseText, string userId);
        public Task DeleteDataFromDraftAndMainTable(string subsectionName);
        public Task DeleteDataFromDependentTable(string subsectionName);
        public Task DeleteFromDraftTable(int tableRowId, string subsectionName);
        public Task DeleteFromMainTable(int tableRowId, string subsectionName);
        public Task<IList<int>> GetAbsenceWizardPendingOptionsRowNumber(string subsectionName);
        public Task<object> GetAbsenceWizardSelectedOptionsSummary(int rowId, string subsectionName);
        public Task<AbsenceWizardDrftStatusDto> GetIsDraftStatusForAbsence(string userId, string subsectionName);
        public Task<object> GetPendingLeaveTypesData(AbsenceServerParamsDto? parameters);
        public Task<IList<string>> GetWizardDropdownOptions(string subsectionName, int questionNo);
        public Task<object> GetWizardQuestionDetails(string subsection);
        public Task<int> InsertAndGetLastInsertedId(int questionNo, string responseText,string subsectionName);
        public Task InsertIntoDraftTable(string userID, int tableRowId, bool isDraft, DateTime dateValue,string subsectionName,bool isApproved);
        public Task UpdateDraftAndApprovalStatusForAbsence(int[] rowIDs, string subsection);
        public Task UpdateDraftStatusForAbsence(int tableRowId, string subsectionName);
        public Task UpdateNumericColumnByRowId(int recordId, int questionNo, decimal response, string? subsectionName);
        public Task UpdateOptionsColumnByRowId(int recordId, int questionNo, bool response, string subsectionName);
        public Task UpdateTextColumnByRowId(int recordId, int questionNo, string? response, string subsectionName);
        public Task UploadWizardData(List<List<WizarDataInfoDto>> wizardTypes, string subsectionName);
        public Task<bool> CheckIfSmallerThanPreviousColumnValue(string responseText, int questionNo,int rowId,string subsectionName);
        public Task<bool> CheckIfGreaterThanMaximumForShortLeave(string responseText, int questionNo, int rowId);
        public Task<bool> CheckIfLessThanMaximumForShortLeave(string responseText, int questionNo, int rowId);
        public Task<bool> CheckIfLessThanMinimumForShortLeave(string responseText, int questionNo, int rowId);
        public Task<bool> CheckIfZeroOrNullValue(int questionNo, int rowId, string subsectionName);
        public Task<bool> CheckIfPreviousColumnIsNotNullForShortLeave(int questionNo, int rowId);
        public Task<bool> CheckIfPreviousColumnIsNotNullForStatLeave(int questionNo, int rowId);
        public Task<bool> checkIfPreviousTwoColumnsAreNotNullForShortLeave(int questionNo, int rowId);
        public Task<bool> CheckIfColumnBeforeTheLastIsNotNullForShortLeave(int questionNo, int rowId);
        public Task DeleteSelectedRowsFromDraftAndMainTable(List<int> rowIds, string subsectionName);
        public Task<bool> CheckExistingLeaveTypeShortLeave(string responseText);
        public Task<bool> CheckExistingLeaveTypeStatutoryLeave(string responseText);
    }
}