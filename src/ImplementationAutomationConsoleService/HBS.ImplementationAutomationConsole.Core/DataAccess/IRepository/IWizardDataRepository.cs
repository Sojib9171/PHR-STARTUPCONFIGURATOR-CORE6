using HBS.ImplementationAutomationConsole.Core.Models;

namespace HBS.ImplementationAutomationConsole.Core.DataAccess.IRepository
{
    public interface IWizardDataRepository
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
        public Task<IList<WizardQuestionDto>> GetAbsenceWizardQuestionDetails(string subsectionId);
        public Task<List<object>> GetAbsenceWizardResponsedByRowId(int rowId, string subsectionName);
        public Task<IList<string>> GetDropDownOtipnsWithQuestionNo(int moduleID, string subsectionID, int questionNo);
        public Task<AbsenceWizardDrftStatusDto> GetIsDraftStatus(string userId,string subsectionName);
        public Task<(int total, int totalDisplay, IList<LeaveTypePendingGetDto> records)> GetPendingLeaveTypesData(int page, int perPage, string searchText, string orderText, string subsection, int[] rowIds);
        public Task<IList<WizardQuestionDto>> GetWizardQuestionDetails(string subsection);
        public Task<int> InsertAndGetLastInsertedId(int id, string responseText,string subsectionName);
        public Task InsertIntoDraftTable(string userID, object tableRowId, bool isDraft, DateTime dateValue, string subsectionName,bool isApproved);
        public Task UpdateDraftAndApprovalStatusForAbsence(int[] rowIDs, string subsection);
        public Task UpdateDraftStatusForAbsence(int tableRowId, string subsectionName);
        public Task UpdateNumericColumnByRowId(int recordId, int columnNo, decimal response, string? subsectionName);
        public Task UpdateOptionsColumnByRowId(int recordId, int columnNo, bool response, string subsectionName);
        public Task UpdateTextColumnByRowId(int recordId, int columnNo, string? response, string subsectionName);
        public Task<bool> CheckIfSmallerThanPreviousColumnValue(string responseText, int questionNo, int rowId,string subsectionName);
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