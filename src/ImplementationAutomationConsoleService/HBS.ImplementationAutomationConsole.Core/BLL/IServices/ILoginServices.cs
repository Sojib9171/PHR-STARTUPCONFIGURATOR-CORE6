using HBS.ImplementationAutomationConsole.Core.Models;

namespace HBS.ImplementationAutomationConsole.Core.BLL.IServices
{
    public interface ILoginServices
    {
        public Task<Users> GetUserbyId(string userId);
        public Task<bool> CheckPassword(string userId, string password);
        public Task<bool> HasUserLoggedInBefore(string userId);
        public Task UpdateUserLoggedInBeforeStatus(string userId);
    }
}