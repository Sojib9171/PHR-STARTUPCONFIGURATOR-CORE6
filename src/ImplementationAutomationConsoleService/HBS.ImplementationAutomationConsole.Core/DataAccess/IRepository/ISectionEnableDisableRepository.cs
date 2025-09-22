using HBS.ImplementationAutomationConsole.Core.Models;
using System.Data;

namespace HBS.ImplementationAutomationConsole.Core.DataAccess.IRepository
{
    public interface ISectionEnableDisableRepository
    {
        public Task InsertConfigControlInfo(ConfigControlPostDto configControlPostDto);
        public Task UpdateOrdersForModule(DataTable table);
        public Task UpdateOrdersForSubsection(DataTable table);
        public Task<object> GetConfigControlApprovalStatus();
        public Task<(int total, int totalDisplay, IList<ConfigControlHistroyGetDto> records)> GetConfigControlHistory(int page, int perPage, string searchText, string sortText);
        public Task<bool> GetAdvanceConfigActiveStatus();
        public Task<bool> GetAdvanceConfigActiveStatusForSidebar();
        public Task<bool> CheckIfAllSubsectionIsEnabled();
    }
}
