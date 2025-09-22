using HBS.ImplementationAutomationConsole.Core.Models;

namespace HBS.ImplementationAutomationConsole.Core.BLL.IServices
{
    public interface ICommonConfigServices
    {
        public Task<object> GetCommonConfigRadioOptions(int questionNo);
        public Task<object> GetCommonConfigQuestionDetails();
        public Task<int> InsertAndGetLastInsertedId(int questionNo, string responseText);
        public Task InsertIntoDraftTable(string userID, int id, bool isDraft, DateTime dateValue);
        public Task UpdateTextColumnByRowId(int recordId, int questionNo, string responseText);
        public Task UpdateImageColumnByRowId(int recordId, int questionNo, byte[] bytes);
        public Task UpdateOptionsColumnByRowId(int recordId, int questionNo, bool response);
        public Task<Object> GetCommonConfigSummary(int rowId);
        public Task UpdateDraftStatusForCommonConfig(int rowId);
        public Task InsertCommonConfigInfo(CommonConfigPostDto commonConfigPostDto);
        public Task DeleteFromDraftTable(int tableRowId);
        public Task DeleteFromMainTable(int tableRowId);
        public Task<CommonConfigDrftStatusDto> GetIsDraftStatus(string userId);
        public Task UpdateImageNameColumnByRowId(int recordId, string? logoImageName);
        public Task UpdateMobileImageNameColumnByRowId(int recordId, string? mobileLogoImageName);
        public Task<bool> GetCommonConfigApprovalStatus(string userId);
        public Task<object> GetCommonConfigHistory(CommonConfigHistoryServerParamsDto? parameters);
    }
}