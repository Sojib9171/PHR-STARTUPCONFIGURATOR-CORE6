using HBS.ImplementationAutomationConsole.Core.BLL.IServices;
using HBS.ImplementationAutomationConsole.Core.DataAccess.IRepository;
using HBS.ImplementationAutomationConsole.Core.Models;

namespace HBS.ImplementationAutomationConsole.Core.BLL.Services
{
    public class LoginServices : ILoginServices
    {

        private readonly ILoginRepository iloginRepository;

        public LoginServices(ILoginRepository iloginRepository)
        {
            this.iloginRepository = iloginRepository;
        }

        public async Task<Users> GetUserbyId(string userId)
        {
            return await iloginRepository.GetUserbyId(userId);
        }
        public async Task<bool> CheckPassword(string userId, string password)
        {
            return await iloginRepository.CheckPassword(userId, password);
        }

        public async Task<bool> HasUserLoggedInBefore(string userId)
        {
            return await iloginRepository.HasUserLoggedInBefore(userId);
        }

        public async Task UpdateUserLoggedInBeforeStatus(string userId)
        {
            await iloginRepository.UpdateUserLoggedInBeforeStatus(userId);
        }
    }
}