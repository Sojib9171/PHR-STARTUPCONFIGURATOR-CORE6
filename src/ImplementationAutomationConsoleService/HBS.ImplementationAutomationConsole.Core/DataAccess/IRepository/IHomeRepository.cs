using HBS.ImplementationAutomationConsole.Core.Models;

namespace HBS.ImplementationAutomationConsole.Core.DataAccess.IRepository
{
    public interface IHomeRepository
    {
        public Task<double> GetEimApprovalPercentage();
        public Task<double> GetAbsenceApprovalPercentage();
        public Task<string> GetClientURL(string subdomainName);
        public Task<double> GetAttendanceApprovalPercentage();
        public Task<List<ModuleOrderDto>> GetModuleOrdersWithModuleId();
    }
}