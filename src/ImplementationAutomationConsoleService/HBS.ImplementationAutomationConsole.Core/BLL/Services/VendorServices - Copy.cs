using HBS.ImplementationAutomationConsole.Core.BLL.IServices;
using HBS.ImplementationAutomationConsole.Core.DataAccess.IRepository;
using HBS.ImplementationAutomationConsole.Core.Models;
using System.Data;

namespace HBS.ImplementationAutomationConsole.Core.BLL.Services
{
    public class VendorServices : IVendorServices
    {

        private readonly IDashboardRepository _idashboardRepository;
        private readonly IVendorRepository _ivendorRepository;
        private readonly IBaseServices _ibaseServices;


        public VendorServices(IDashboardRepository idashboardRepository, IVendorRepository ivendorRepository, IBaseServices baseServices)
        {
            _idashboardRepository = idashboardRepository;
            _ibaseServices = baseServices;
            _ivendorRepository = ivendorRepository;
        }

        public async Task<Object> GetDashboardHistoryInfo(HistoryServerParamsDto parameters)
        {

            var dashboardInfoList = await _idashboardRepository.GetDashboardHistoryInformation(
                    parameters.Subsection,
                    parameters.Page,
                    parameters.PerPage,
                    parameters.searchText,
                    parameters.SortField + " " + parameters.SortType);

            var dataTableObjects = new
            {
                recordsTotal = dashboardInfoList.totalDisplay,
                data = (from record in dashboardInfoList.records
                        select new
                        {
                            approval_date = record.ApprovalDate.ToString("yyyy-MM-dd"),
                            approval_status = record.ApprovalStatus.ToString(),
                            approval_by = record.Name.ToString(),
                            comment = record.ApprovalComment.ToString(),
                            approval_data = Convert.ToBase64String(record.ApprovalData)
                        }
                ).ToArray()
            };
            return dataTableObjects;
        }

        public async Task<object> GetApprovalFromVendor(ServerParamsDto parameters)
        {
            var dashboardInfoList = await _ivendorRepository.GetPendingApprovalForAdmin(
                    parameters.Page,
                    parameters.PerPage,
                    parameters.searchText,
                    parameters.SortField + " " + parameters.SortType);

            var dataTableObjects = new
            {
                recordsTotal = dashboardInfoList.total,
                data = (from record in dashboardInfoList.records
                        select new
                        {
                            record_id = record.RecordID,
                            module_name = record.MainsectionName.ToString(),
                            subsection_name = record.SubsectionName.ToString(),
                            approval_status = record.ApprovalStatus.ToString(),
                            approval_by = record.Name.ToString(),
                            comment = record.ApprovalComment.ToString(),
                            approval_data = Convert.ToBase64String(record.ApprovalData),
                            approval_date = record.ApprovalDate.ToString("yyyy-MM-dd")
                        }
                ).ToArray()
            };
            return dataTableObjects;
        }

        public async Task<string> GetExcelDataAsByteArray(int recordID)
        {
            var response = await _ivendorRepository.GetExcelByteArray(recordID);
            var base64Str = Convert.ToBase64String(response);
            return base64Str;
        }

        public async Task InsertVendorInfo(VendorPostDto vendorDto)
        {
            await _ivendorRepository.InsertVendorInfo(vendorDto);
        }

        public async Task<object> GetVendorDashboardHistoryInfo(HistoryServerParamsDto parameters)
        {
            var dashboardInfoList = await _ivendorRepository.GetVendorDashboardHistoryInfo(
                    parameters.Subsection,
                    parameters.Page,
                    parameters.PerPage,
                    parameters.searchText,
                    parameters.SortField + " " + parameters.SortType);

            var dataTableObjects = new
            {
                recordsTotal = dashboardInfoList.totalDisplay,
                data = (from record in dashboardInfoList.records
                        select new
                        {
                            vendor_approval_status = record.VendorApprovalStatus.ToString(),
                            vendor_approval_by = record.VendorName.ToString(),
                            vendor_approval_date = record.VendorApprovalDate.ToString("yyyy-MM-dd"),
                            approval_data = Convert.ToBase64String(record.ApprovalData),
                            vendor_approval_comment = record.VendorApprovalComment.ToString(),
                        }
                ).ToArray()
            };
            return dataTableObjects;
        }

        public async Task<int> GetRemainingApprovalCount()
        {
            var count = await _ivendorRepository.GetRemainingAdminApprovalCount();
            return count;
        }

        public async Task<object> GetApprovedDataListForVendor(ServerParamsDto parameters)
        {
            var dashboardInfoList = await _ivendorRepository.GetApprovalSummaryForAdmin(
                parameters.Page,
                parameters.PerPage,
                parameters.searchText,
                parameters.SortField + " " + parameters.SortType);

            var dataTableObjects = new
            {
                recordsTotal = dashboardInfoList.total,
                data = (from record in dashboardInfoList.records
                        select new
                        {
                            record_id = record.RecordID,
                            module_name = record.MainsectionName.ToString(),
                            subsection_name = record.SubsectionName.ToString(),
                            vendor_approval_status = record.ApprovalStatus.ToString(),
                            vendor_approval_by = record.Name.ToString(),
                            //comment = record.ApprovalComment.ToString(),
                            //approval_data = Convert.ToBase64String(record.ApprovalData),
                            //approval_date = record.ApprovalDate.ToString("yyyy-MM-dd")
                        }
                ).ToArray()
            };
            return dataTableObjects;
        }

        public async Task<object> GetVendorApprovalSummaryHistory(HistoryServerParamsDto parameters)
        {
            var dashboardInfoList = await _ivendorRepository.GetVendorDashboardHistoryInfo(
                    parameters.Subsection,
                    parameters.Page,
                    parameters.PerPage,
                    parameters.searchText,
                    parameters.SortField + " " + parameters.SortType);

            var dataTableObjects = new
            {
                recordsTotal = dashboardInfoList.totalDisplay,
                data = (from record in dashboardInfoList.records
                        select new
                        {
                            vendor_approval_status = record.VendorApprovalStatus.ToString(),
                            vendor_approval_by = record.VendorName.ToString(),
                            vendor_approval_date = record.VendorApprovalDate.ToString("yyyy-MM-dd"),
                            approval_data = Convert.ToBase64String(record.ApprovalData),
                            vendor_approval_comment = record.VendorApprovalComment.ToString(),
                        }
                ).ToArray()
            };
            return dataTableObjects;
        }
    }
}