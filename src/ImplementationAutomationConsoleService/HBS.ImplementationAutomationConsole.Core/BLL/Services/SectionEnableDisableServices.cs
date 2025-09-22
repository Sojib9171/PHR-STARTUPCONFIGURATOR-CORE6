using HBS.ImplementationAutomationConsole.Core.BLL.IServices;
using HBS.ImplementationAutomationConsole.Core.DataAccess.IRepository;
using HBS.ImplementationAutomationConsole.Core.DataAccess.Repository;
using HBS.ImplementationAutomationConsole.Core.Models;
using System.Data;
using System.Reflection;

namespace HBS.ImplementationAutomationConsole.Core.BLL.Services
{
    public class SectionEnableDisableServices : ISectionEnableDisableServices
    {
        private readonly ISectionEnableDisableRepository _isectionEnableDisableRepository;

        public SectionEnableDisableServices(ISectionEnableDisableRepository isectionEnableDisableRepository)
        {
            _isectionEnableDisableRepository = isectionEnableDisableRepository;
        }

        public async Task UpdateOrdersForAbsence(List<SubsectionModel> absenceObjects)
        {
            for (int i = 0; i < absenceObjects.Count; i++)
            {
                absenceObjects[i].Order = i + 1;
            }
            var table = await CreateDataTableForSubsection(absenceObjects);
            await _isectionEnableDisableRepository.UpdateOrdersForSubsection(table);
        }

        public async Task UpdateOrdersForAttendance(List<SubsectionModel> attendanceObjects)
        {
            for (int i = 0; i < attendanceObjects.Count; i++)
            {
                attendanceObjects[i].Order = i + 1;
            }
            var table = await CreateDataTableForSubsection(attendanceObjects);
            await _isectionEnableDisableRepository.UpdateOrdersForSubsection(table);
        }

        public async Task UpdateOrdersForEIM(List<SubsectionModel> eimObjects)
        {
            for (int i = 0; i < eimObjects.Count; i++)
            {
                if (eimObjects[i].Order != 0)
                {
                    continue;
                }
                eimObjects[i].Order = i + 1;
            }
            var table = await CreateDataTableForSubsection(eimObjects);
            await _isectionEnableDisableRepository.UpdateOrdersForSubsection(table);
        }

        public async Task UpdateOrdersForModule(List<ModuleModel> modules)
        {
            for (int i = 0; i < modules.Count; i++)
            {
                modules[i].Order = i + 1;
            }

            DataTable table = new DataTable();
            table.Columns.Add("ModuleId", typeof(int));
            table.Columns.Add("Order", typeof(int));

            foreach (var item in modules)
            {
                table.Rows.Add(item.ModuleId, item.Order);
            }

            await _isectionEnableDisableRepository.UpdateOrdersForModule(table);
        }

        public async Task<DataTable> CreateDataTableForSubsection(List<SubsectionModel> subsections)
        {
            DataTable table = new DataTable();
            table.Columns.Add("SubsectionId", typeof(int));
            table.Columns.Add("Order", typeof(int));

            foreach (var item in subsections)
            {
                table.Rows.Add(item.SubsectionId, item.Order);
            }

            return table;
        }

        public async Task InsertConfigControlInfo(ConfigControlPostDto configControlPostDto)
        {
            await _isectionEnableDisableRepository.InsertConfigControlInfo(configControlPostDto);
        }

        public async Task<object> GetConfigControlApprovalStatus()
        {
            return await _isectionEnableDisableRepository.GetConfigControlApprovalStatus();
        }

        public async Task<object> GetConfigControlHistory(ConfigControlHistoryServerParamsDto? parameters)
        {
            var tableInfoList = await _isectionEnableDisableRepository.GetConfigControlHistory(
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
                            record_id = record.RecordID.ToString(),
                            approval_status = record.ApprovalStatus.ToString(),
                            approval_by = record.Name.ToString(),
                            approval_date = record.ApprovalDate.ToString("yyyy-MM-dd"),
                            signoff_data = record.SignOffPdfData,
                            approval_comment = record.ApprovalComment.ToString(),
                        }
                ).ToArray()
            };
            return dataTableObjects;
        }

        public async Task<bool> GetAdvanceConfigActiveStatus()
        {
            return await _isectionEnableDisableRepository.GetAdvanceConfigActiveStatus();
        }

        public async Task<bool> GetAdvanceConfigActiveStatusForSidebar()
        {
            return await _isectionEnableDisableRepository.GetAdvanceConfigActiveStatusForSidebar();
        }

        public async Task<bool> CheckIfAllSubsectionIsEnabled()
        {
            return await _isectionEnableDisableRepository.CheckIfAllSubsectionIsEnabled();
        }
    }
}
