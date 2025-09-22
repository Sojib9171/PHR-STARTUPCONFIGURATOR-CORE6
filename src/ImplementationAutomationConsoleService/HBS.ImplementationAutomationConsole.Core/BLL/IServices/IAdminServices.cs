using HBS.ImplementationAutomationConsole.Core.Models;

namespace HBS.ImplementationAutomationConsole.Core.BLL.IServices
{
    public interface IAdminServices
    {
        public Task<Object> GetDashboardHistoryInfo(HistoryServerParamsDto serverParams);
        public Task<object> GetApprovalFromVendor(ServerParamsDto parameters);
        public Task<string> GetExcelDataAsByteArray(int recordID);
        public Task InsertVendorInfo(VendorPostDto vendorDto);
        public Task<object> GetVendorDashboardHistoryInfo(HistoryServerParamsDto parameters);
        public Task<int> GetRemainingApprovalCount();
        public Task<object> GetApprovedDataListForVendor(ServerParamsDto parameters);
        public Task<object> GetVendorApprovalSummaryHistory(HistoryServerParamsDto parameters);
    }
}