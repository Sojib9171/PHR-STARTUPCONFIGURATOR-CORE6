using HBS.ImplementationAutomationConsole.Core.Models;

namespace HBS.ImplementationAutomationConsole.Core.BLL.IServices
{
    public interface ISidebarServices
    {
        public Task<IList<SidebarDto>> GetSubsectionItems();
        public Task<string> GetCopyRightText();
        public Task<List<ModuleDto>> GetActiveModules();
        public Task<List<ActiveSubsectionDto>> GetActiveSubsectionsEim();
        public Task<List<ActiveSubsectionDto>> GetActiveSubsectionsLeave();
        public Task<List<ActiveSubsectionDto>> GetActiveSubsectionsAttendance();
        public Task<List<int>> GetActiveModuleIDs();
        public Task<bool> CheckIfSubsectionApproved(string subsectionName);
        public Task<List<ModuleDto>> GetActiveModulesFromDashboard();
    }
}