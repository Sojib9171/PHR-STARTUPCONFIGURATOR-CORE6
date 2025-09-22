using HBS.ImplementationAutomationConsole.Core.DataAccess.Data;
using HBS.ImplementationAutomationConsole.Core.DataAccess.IRepository;
using HBS.ImplementationAutomationConsole.Core.Models;
using System.Data;

namespace HBS.ImplementationAutomationConsole.Core.DataAccess.Repository
{
    public class HomeRepository : IHomeRepository
    {
        private readonly IBaseDataAccess _iBaseDataAccess;
        public HomeRepository(IBaseDataAccess iBaseDataAccess)
        {
            this._iBaseDataAccess = iBaseDataAccess;
        }


        public async Task<double> GetEimApprovalPercentage()
        {
            string query = @"EXEC [SP_IA_GET_EMI_APPRV_COUNT]";
            double eimApprovalPercentage = 0;

            using (var reader = await _iBaseDataAccess.ExecuteReaderAsync(query))
            {
                while (reader.Read())
                {
                    eimApprovalPercentage = reader.GetDouble(0);
                }
            }
            return eimApprovalPercentage * 100;
        }

        public async Task<double> GetAbsenceApprovalPercentage()
        {
            string query = @"EXEC [SP_IA_GET_ABSNC_APPRV_COUNT]";
            double eimApprovalPercentage = 0;

            using (var reader = await _iBaseDataAccess.ExecuteReaderAsync(query))
            {
                while (reader.Read())
                {
                    eimApprovalPercentage = reader.GetDouble(0);
                }
            }
            return eimApprovalPercentage * 100;
        }

        public async Task<string> GetClientURL(string subdomainName)
        {
            string query = @"EXEC [SP_IA_GET_CLIENT_URL] @SUBDOMAIN_NAME";
            var @params = new List<Param>()
            {
                new Param{SqlDbType=SqlDbType.NVarChar, ParamName="@SUBDOMAIN_NAME", SqlValue=subdomainName}
            };

            var clientUrl = string.Empty;
            using (var reader = await _iBaseDataAccess.ExecuteReaderAsync(query, @params))
            {
                while (reader.Read())
                {
                    clientUrl = reader.GetString(0);
                }
            }
            return clientUrl;
        }


        public async Task<double> GetAttendanceApprovalPercentage()
        {
            string query = @"EXEC [SP_IA_GET_ATTN_APPRV_COUNT]";
            double attendanceApprovalPercentage = 0;

            using (var reader = await _iBaseDataAccess.ExecuteReaderAsync(query))
            {
                while (reader.Read())
                {
                    attendanceApprovalPercentage = reader.GetDouble(0);
                }
            }
            return attendanceApprovalPercentage * 100;
        }

        public async Task<List<ModuleOrderDto>> GetModuleOrdersWithModuleId()
        {
            string query = @"EXEC [SP_IA_GET_MODULE_ORDERS]";

            var moduleOrderList = new List<ModuleOrderDto>();
            using (var reader = await _iBaseDataAccess.ExecuteReaderAsync(query))
            {
                while (reader.Read())
                {
                    var moduleOrder = new ModuleOrderDto()
                    {
                        Module_Id = reader.GetString(0),
                        View_Order = !reader.IsDBNull(1) ? reader.GetInt32(1) : 0,
                    };
                    moduleOrderList.Add(moduleOrder);
                }
            }
            return moduleOrderList;
        }
    }
}