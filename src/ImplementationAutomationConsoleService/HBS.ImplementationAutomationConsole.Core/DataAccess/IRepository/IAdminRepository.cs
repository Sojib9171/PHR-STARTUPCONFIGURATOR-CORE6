using HBS.ImplementationAutomationConsole.Core.Models;

namespace HBS.ImplementationAutomationConsole.Core.DataAccess.IRepository
{
    public interface IAdminRepository
    {
        public Task<(int total, int totalDisplay, IList<VendorApprovalGetDto> records)> GetPendingApprovalForAdmin(int pageIndex, int pageSize, string searchText, string orderText);
        public Task<byte[]> GetExcelByteArray(int recordID);
        public Task InsertVendorInfo(VendorPostDto vendorDto);
        public Task<(int total, int totalDisplay, IList<DashboardGetDto> records)> GetVendorDashboardHistoryInfo(string subsectionName, int pageIndex, int pageSize, string searchText, string sortText);
        public Task<int> GetRemainingAdminApprovalCount();
        public Task<(int total, int totalDisplay, IList<VendorApprovalGetDto> records)> GetApprovalSummaryForAdmin(int page, int perPage, string searchText, string orderText);
        public Task<(int total, int totalDisplay, IList<VendorApprovalGetDto> records)> GetVendorApprovalSummaryHistory(string subsection, int page, int perPage, string searchText, string orderText);
        public Task<string> GetSubsectionNameFromUserRecordId(int userRecordId);
        public Task InsertDataIntoMainTable(string subsectionName);
    }
}