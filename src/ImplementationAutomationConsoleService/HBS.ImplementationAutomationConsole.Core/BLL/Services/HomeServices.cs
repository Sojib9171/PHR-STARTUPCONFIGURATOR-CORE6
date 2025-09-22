using HBS.ImplementationAutomationConsole.Core.BLL.IServices;
using HBS.ImplementationAutomationConsole.Core.DataAccess.IRepository;
using HBS.ImplementationAutomationConsole.Core.Models;

namespace HBS.ImplementationAutomationConsole.Core.BLL.Services
{
    public class HomeServices : IHomeServices
    {

        private readonly IHomeRepository _ihomeRepository;

        public HomeServices(IHomeRepository ihomeRepository)
        {
            _ihomeRepository = ihomeRepository;
        }

        public async Task<double> GetEimApprovalPercentage()
        {
            return await _ihomeRepository.GetEimApprovalPercentage();
        }

        public async Task<double> GetAbsenceApprovalPercentage()
        {
            return await _ihomeRepository.GetAbsenceApprovalPercentage();
        }

        public async Task<string> GetClientURL(string subdomainName)
        {
            string clientURL = await _ihomeRepository.GetClientURL(subdomainName);
            return clientURL;
        }

        public async Task<double> GetAttendanceApprovalPercentage()
        {
            return await _ihomeRepository.GetAttendanceApprovalPercentage();
        }

        public async Task<bool> CheckAllSubsectionApproved(IList<SidebarDto> subsectList)
        {
            if(subsectList.Count == 0)
            {
                return false;
            }
            var isAllSubsecApproved = subsectList.All(x=> x.Approval_Status=="Approved" && x.Vendor_ApprovalStatus=="Approved");
            return isAllSubsecApproved;
        }

        public async Task<List<ModuleOrderDto>> GetModuleOrdersWithModuleId()
        {
            return await _ihomeRepository.GetModuleOrdersWithModuleId();
        }
    }
}