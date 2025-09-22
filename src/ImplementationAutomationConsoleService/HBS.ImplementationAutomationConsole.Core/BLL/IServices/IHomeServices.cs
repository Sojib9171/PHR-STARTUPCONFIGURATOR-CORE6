using HBS.ImplementationAutomationConsole.Core.Models;

namespace HBS.ImplementationAutomationConsole.Core.BLL.IServices
{
    public interface IHomeServices
    {
        public Task<double> GetEimApprovalPercentage();
        public Task<double> GetAbsenceApprovalPercentage();
        public Task<string> GetClientURL(string subdomainName);
        public Task<double> GetAttendanceApprovalPercentage();
        public Task<bool> CheckAllSubsectionApproved(IList<SidebarDto> subsectList);
        public Task<List<ModuleOrderDto>> GetModuleOrdersWithModuleId();
    }
}