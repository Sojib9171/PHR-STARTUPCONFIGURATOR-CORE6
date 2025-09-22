using ClosedXML.Excel;

namespace HBS.ImplementationAutomationConsole.Core.BLL.IServices
{
    public interface IConfigControlServices
    {
        Task UpdateEnabledStatusForModules(List<int> activeModuleIds);
    }
}