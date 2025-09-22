using HBS.ImplementationAutomationConsole.Core.Models;

namespace HBS.ImplementationAutomationConsole.Core.DataAccess.IRepository
{
    public interface ISidebarRepository
    {
        public Task<string> GetCopyRightText();
        public Task<List<ModuleDto>> GetActiveModules();
        public Task<List<ActiveSubsectionDto>> GetActiveSubsectionsEim();
        public Task<List<ActiveSubsectionDto>> GetActiveSubsectionsLeave();
        public Task<List<ActiveSubsectionDto>> GetActiveSubsectionsAttendance();
        public Task<List<int>> GetActiveModuleIDs();
        public Task<IList<SidebarDto>> GetSubsectionItems();
        public Task<IList<DashboardApprovalDto>> GetDashboardApprovalItems();
        public Task<bool> CheckIfSubsectionApproved(string subsectionName);
    }
}