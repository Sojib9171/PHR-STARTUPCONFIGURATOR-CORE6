using HBS.ImplementationAutomationConsole.Core.Models;

namespace HBS.ImplementationAutomationConsole.Core.BLL.IServices
{
    public interface ISectionEnableDisableServices
    {
        public Task InsertConfigControlInfo(ConfigControlPostDto configControlPostDto);
        public Task UpdateOrdersForAbsence(List<SubsectionModel> absenceObjects);
        public Task UpdateOrdersForAttendance(List<SubsectionModel> attendanceObjects);
        public Task UpdateOrdersForEIM(List<SubsectionModel> eimObjects);
        public Task UpdateOrdersForModule(List<ModuleModel> models);
        public Task<object> GetConfigControlApprovalStatus();
        public Task<object> GetConfigControlHistory(ConfigControlHistoryServerParamsDto? parameters);
        public Task<bool> GetAdvanceConfigActiveStatus();
        public Task<bool> GetAdvanceConfigActiveStatusForSidebar();
        public Task<bool> CheckIfAllSubsectionIsEnabled();
    }
}