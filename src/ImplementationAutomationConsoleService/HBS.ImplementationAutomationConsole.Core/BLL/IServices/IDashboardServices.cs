using HBS.ImplementationAutomationConsole.Core.Models;

namespace HBS.ImplementationAutomationConsole.Core.BLL.IServices
{
    public interface IDashboardServices
    {
        public Task<Object> GetDashboardHistoryInfo(HistoryServerParamsDto serverParams);
        public Task InsertDashboardInfo(DashboardPostDto dashboardDto);
        public Task<object> GetDashboardInfo(ServerParamsDto parameters);
    }
}