using ClosedXML.Excel;
using HBS.ImplementationAutomationConsole.Core.BLL.IServices;
using HBS.ImplementationAutomationConsole.Core.DataAccess.IRepository;
using HBS.ImplementationAutomationConsole.Core.Models;
using Microsoft.Extensions.Configuration;

namespace HBS.ImplementationAutomationConsole.Core.BLL.Services
{
    public class DashboardServices : IDashboardServices
    {

        private readonly IDashboardRepository _idashboardRepository;
        private readonly ITemplateRepository _itemplateRepository;
        private readonly IExcelDataRepository _iexcelDataRepository;
        private readonly IBaseServices _ibaseServices;

        public DashboardServices(IDashboardRepository idashboardRepository, ITemplateRepository itemplateRepository, IExcelDataRepository excelDataRepository, IBaseServices baseServices)
        {
            _idashboardRepository = idashboardRepository;
            _itemplateRepository = itemplateRepository;
            _iexcelDataRepository = excelDataRepository;
            _ibaseServices = baseServices;
        }

        public async Task<Object> GetDashboardHistoryInfo(HistoryServerParamsDto serverParams)
        {

            var dashboardInfoList = await _idashboardRepository.GetDashboardHistoryInformation(
                    serverParams.Subsection,
                    serverParams.Page,
                    serverParams.PerPage,
                    serverParams.searchText,
                    serverParams.SortField + " " + serverParams.SortType);

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
                            approval_data = Convert.ToBase64String(record.ApprovalData),
                            signoff_data = Convert.ToBase64String(record.SignOffPdfData),
                        }
                ).ToArray()
            };
            return dataTableObjects;
        }

        public async Task InsertDashboardInfo(DashboardPostDto dashboardDto)
        {
            var moduleAndSubsectionInfo = await _idashboardRepository.GetModuleAndSubsectioninfoFromSubsectionName(dashboardDto.SubsectionName);
            dashboardDto.ModuleID = moduleAndSubsectionInfo.Item1;
            dashboardDto.SubsectionID = moduleAndSubsectionInfo.Item2;

            var templateId = await _itemplateRepository.GetTemplateIdFromSubsectionName(dashboardDto.SubsectionName);
            var templateData = await _itemplateRepository.GetTemplateByteArray(templateId);
            var templateByteArray = templateData.Item1;
            IXLWorkbook workbook;

            using (MemoryStream stream = new MemoryStream(templateByteArray))
            {
                workbook = new XLWorkbook(stream);
                var worksheet = workbook.Worksheets.Last();
                (int dataStartRowNumber, int lastColumnNumber) = await _ibaseServices.GetWorkSheetRowAndColumnCount(worksheet);
                var dataTable = await _iexcelDataRepository.GetAllDataFromTableAsDataTable(dashboardDto.SubsectionName);
                await _ibaseServices.InsertDatatableInWorksheet(worksheet, dataTable, dataStartRowNumber);

                MemoryStream ms = new MemoryStream();
                workbook.SaveAs(ms);
                dashboardDto.ApprovalData = ms.ToArray();
            }

            await _idashboardRepository.InsertDashboardData(dashboardDto);
        }

        public async Task<object> GetDashboardInfo(ServerParamsDto parameters)
        {
            var dashboardInfoList = await _idashboardRepository.GetDashboardInformation(
                    parameters.Page,
                    parameters.PerPage,
                    parameters.SearchText,
                    parameters.SortField + " " + parameters.SortType,
                    parameters.ModuleName);

            var dataTableObjects = new
            {
                recordsTotal = dashboardInfoList.totalDisplay,
                data = (from record in dashboardInfoList.records
                        select new
                        {
                            record_id = record.RecordID,
                            module_name = record.MainsectionName.ToString(),
                            subsection_name = record.SubsectionName.ToString(),
                            approval_status = string.IsNullOrEmpty(record.ApprovalStatus) ? "Not Initiated" : record.ApprovalStatus.ToString(),
                            approval_by = record.Name.ToString(),
                            vendor_approval_status = string.IsNullOrEmpty(record.VendorApprovalStatus) ? "Not Initiated" : record.VendorApprovalStatus.ToString(),
                            vendor_approval_by = record.VendorName.ToString(),
                        }
                ).ToArray()
            };
            return dataTableObjects;
        }
    }
}