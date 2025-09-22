using DocumentFormat.OpenXml.Vml;
using HBS.ImplementationAutomationConsole.Core.Models;
using System.Data;

namespace HBS.ImplementationAutomationConsole.Core.DataAccess.IRepository
{
    public interface ICommonConfigRepository
    {
        public Task<IList<CommonConfigRadioOptionDto>> GetRadioOptionsWithQuestionNo(int questionNo);
        public Task<IList<WizardQuestionDto>> GetCommonConfigQuestionDetails();
        public Task<int> InsertAndGetLastInsertedId(int columnNumber, string responseText);
        public Task InsertIntoDraftTable(string userID, int tableRowId, bool isDraft, DateTime dateValue);
        public Task UpdateTextColumnByRowId(int rowId, int columnNo, string responseText);
        public Task UpdateImageColumnByRowId(int recordId, int columnNo, byte[] bytes);
        public Task UpdateOptionColumnByRowId(int recordId, int columnNo, bool response);
        public Task<List<object>> GetCommonConfigResponsedByRowId(int rowId);
        public Task UpdateDraftStatusForCommonConfig(int rowId);
        public Task InsertCommonConfigInfo(CommonConfigPostDto commonConfigPostDto);
        public Task DeleteFromDraftTable(int tableRowId);
        public Task DeleteFromMainTable(int tableRowId);
        public Task<CommonConfigDrftStatusDto> GetIsDraftStatus(string userId);
        public Task UpdateImageNameColumnByRowId(int recordId, string? logoImageName);
        public Task UpdateMobileImageNameColumnByRowId(int recordId,string? mobileLogoImageName);
        public Task<bool> GetCommonConfigApprovalStatus(string userId);
        public Task<(int total, int totalDisplay, IList<CommonConfigHistroyGetDto> records)> GetCommonConfigHistory(string userid, int page, int perPage, string searchText, string sortText);
    }
}