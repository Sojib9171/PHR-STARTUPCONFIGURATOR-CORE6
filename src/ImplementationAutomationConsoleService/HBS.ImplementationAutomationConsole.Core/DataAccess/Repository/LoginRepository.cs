using HBS.ImplementationAutomationConsole.Core.DataAccess.Data;
using HBS.ImplementationAutomationConsole.Core.DataAccess.IRepository;
using HBS.ImplementationAutomationConsole.Core.Models;
using System.Data;

namespace HBS.ImplementationAutomationConsole.Core.DataAccess.Repository
{
    public class LoginRepository : ILoginRepository
    {
        private readonly IBaseDataAccess _iBaseDataAccess;
        public LoginRepository(IBaseDataAccess iBaseDataAccess)
        {
            this._iBaseDataAccess = iBaseDataAccess;
        }

        public async Task<Users> GetUserbyId(string userId)
        {
            string query = @"EXEC [SP_IA_GET_USER_BY_ID] @USER_ID";
            List<Param> @params = new List<Param>()
            {
                new Param{ SqlDbType = SqlDbType.NVarChar, ParamName="@USER_ID", SqlValue = userId},
            };
            var dt = await _iBaseDataAccess.GetDataTable(query, @params);

            if (dt.Rows.Count == 0)
            {
                return null;
            }

            var user = new Users().ConvertToSingleUserModel(dt);
            return user;
        }

        public async Task<bool> CheckPassword(string userId, string password)
        {
            string query = @"EXEC [SP_IA_CHECK_PASSWORD] @USER_ID,@PASSWORD";
            List<Param> @params = new List<Param>()
            {
                new Param{ SqlDbType = SqlDbType.NVarChar, ParamName="@USER_ID", SqlValue = userId},
                new Param{ SqlDbType = SqlDbType.NVarChar, ParamName="@PASSWORD", SqlValue = password},
            };

            return await _iBaseDataAccess.GetSingleInt(query, @params) == 1;
        }

        public async Task<Users> AuthenticateUser(string userId, string password)
        {
            string query = @"EXEC [AuthenticateUser] @USER_ID, @PASSWORD";
            List<Param> @params = new List<Param>()
            {
                new Param{ SqlDbType = SqlDbType.NVarChar, ParamName="@USER_ID", SqlValue = userId},
                new Param{ SqlDbType = SqlDbType.NVarChar, ParamName="@PASSWORD", SqlValue = password},
            };

            var data = await _iBaseDataAccess.GetDataTable(query, @params);
            Users user = new Users();

            if (data.Rows.Count > 1)
            {
                user = user.ConvertToSingleUserModel(data);
            }
            return user;
        }

        public async Task<bool> HasUserLoggedInBefore(string userId)
        {
            string query = @"EXEC [SP_IA_CHK_USER_LOG_IN_BEFORE] @USER_ID";
            List<Param> @params = new List<Param>()
            {
                new Param{ SqlDbType = SqlDbType.NVarChar, ParamName="@USER_ID", SqlValue = userId},
            };

            var hasUserLoggedInBefore = false;
            using (var reader = await _iBaseDataAccess.ExecuteReaderAsync(query, @params))
            {
                while (reader.Read())
                {
                    hasUserLoggedInBefore = !reader.IsDBNull(0) ? reader.GetBoolean(0) : false;
                }
            }
            return hasUserLoggedInBefore;
        }

        public async Task UpdateUserLoggedInBeforeStatus(string userId)
        {
            string query = @"EXEC [SP_IA_UPDT_USER_LOG_IN_BEFORE] @USER_ID";
            List<Param> @params = new List<Param>()
            {
                new Param{ SqlDbType = SqlDbType.NVarChar, ParamName="@USER_ID", SqlValue = userId},
            };

            await _iBaseDataAccess.ExecuteQueryAsync(query, @params);
        }
    }
}