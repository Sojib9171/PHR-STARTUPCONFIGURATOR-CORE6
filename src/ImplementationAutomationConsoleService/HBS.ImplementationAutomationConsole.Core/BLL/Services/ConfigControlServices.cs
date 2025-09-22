using HBS.ImplementationAutomationConsole.Core.BLL.IServices;
using HBS.ImplementationAutomationConsole.Core.DataAccess.IRepository;

namespace HBS.ImplementationAutomationConsole.Core.BLL.Services
{
    public class ConfigControlServices : IConfigControlServices
    {

        private readonly IConfigControlRepository _iconfigControlRepository;
        private readonly IBaseServices _ibaseServices;

        public ConfigControlServices(IConfigControlRepository iconfigControlRepository, IBaseServices ibaseServices)
        {
            _iconfigControlRepository = iconfigControlRepository;
            _ibaseServices = ibaseServices;
        }

        public async Task UpdateEnabledStatusForModules(List<int> activeModuleIds)
        {
            throw new NotImplementedException();
        }
    }
}