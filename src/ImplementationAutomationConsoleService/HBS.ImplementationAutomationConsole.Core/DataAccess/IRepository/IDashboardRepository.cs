using HBS.ImplementationAutomationConsole.Core.Models;

namespace HBS.ImplementationAutomationConsole.Core.DataAccess.IRepository
{
    public interface IDashboardRepository
    {
        public Task InsertDashboardData(DashboardPostDto dashboardDto);
        public Task<(int, string, string)> GetModuleAndSubsectioninfoFromSubsectionName(string subsectionName);
        public Task<(int total, int totalDisplay, IList<DashboardGetDto> records)>GetDashboardInformation(int pageIndex, int pageSize, string searchText, string orderText,string moduleName);
        public Task<(int total, int totalDisplay, IList<DashboardGetDto> records)> GetDashboardHistoryInformation(string subsectionName,int pageIndex, int pageSize, string searchText, string orderText);
    }
}