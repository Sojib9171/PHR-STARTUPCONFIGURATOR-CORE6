using HBS.ImplementationAutomationConsole.Core.DataAccess.Data;
using HBS.ImplementationAutomationConsole.Core.DataAccess.IRepository;
using HBS.ImplementationAutomationConsole.Core.Models;
using System.Data;

namespace HBS.ImplementationAutomationConsole.Core.DataAccess.Repository
{
    public class SectionEnableDisableRepository:ISectionEnableDisableRepository
    {
        private readonly IBaseDataAccess _ibaseDataAccess;
        public SectionEnableDisableRepository(IBaseDataAccess ibaseDataAccess)
        {
            _ibaseDataAccess = ibaseDataAccess;
        }

        public async Task InsertConfigControlInfo(ConfigControlPostDto configControlPostDto)
        {
            string query = @"EXEC [SP_IA_INSRT_IN_CNFG_CNTRL_INFO] @APPROVAL_STATUS, @APPROVAL_BY, @APPROVAL_COMMENT, @APPROVAL_SIGNATURE, @APPROVAL_DATE,@SIGNOFF_DATA";
            List<Param> @params = new List<Param>()
            {
                new Param{ SqlDbType = SqlDbType.NVarChar, ParamName="@APPROVAL_STATUS", SqlValue = configControlPostDto.ApprovalStatus},
                new Param{ SqlDbType = SqlDbType.NVarChar, ParamName="@APPROVAL_BY", SqlValue = configControlPostDto.Name},
                new Param{ SqlDbType = SqlDbType.NVarChar, ParamName="@APPROVAL_COMMENT", SqlValue = configControlPostDto.Comment},
                new Param{ SqlDbType = SqlDbType.NVarChar, ParamName="@APPROVAL_SIGNATURE", SqlValue = configControlPostDto.UserName},
                new Param{ SqlDbType = SqlDbType.DateTime, ParamName="@APPROVAL_DATE", SqlValue = configControlPostDto.ApprovalDate},
                new Param{ SqlDbType = SqlDbType.VarBinary, ParamName="@SIGNOFF_DATA", SqlValue = configControlPostDto.SignOffPdfData},
            };
            await _ibaseDataAccess.ExecuteQueryAsync(query, @params);
        }

        public async Task UpdateOrdersForModule(DataTable table)
        {
            string query = "EXEC [SP_IA_UPDAT_MODULE_ORDER] @MODULE_ARRAY";

            List<Param> @params = new List<Param>()
            {
                new Param{ SqlDbType = SqlDbType.Structured, ParamName="@MODULE_ARRAY", SqlValue = table}
            };

            string? userId = await _ibaseDataAccess.GetDbUserName();

            var userDataType = "[" + userId + "].HS_HR_IA_DBTYPE_MODULE_ORDER";

            await _ibaseDataAccess.ExecuteQueryAsync(query, @params, 30, userDataType);
        }

        public async Task UpdateOrdersForSubsection(DataTable table)
        {
            string query = "EXEC [SP_IA_UPDATE_SUBSEC_ORDER] @SUBSECTION_ARRAY";

            List<Param> @params = new List<Param>()
            {
                new Param{ SqlDbType = SqlDbType.Structured, ParamName="@SUBSECTION_ARRAY", SqlValue = table}
            };

            string? userId = await _ibaseDataAccess.GetDbUserName();

            var userDataType = "[" + userId + "].HS_HR_IA_DBTYPE_SUBSEC_ORDER";

            await _ibaseDataAccess.ExecuteQueryAsync(query, @params, 30, userDataType);
        }

        public async Task<object> GetConfigControlApprovalStatus()
        {
            string query = "EXEC [SP_IA_GET_CNF_CNTRL_APRV_STS]";

            var isApproved = false;
            var approvalStatus = string.Empty;
            using (var reader = await _ibaseDataAccess.ExecuteReaderAsync(query))
            {
                while (reader.Read())
                {
                    approvalStatus = (!reader.IsDBNull(0) ? reader.GetString(0) : string.Empty);
                    isApproved = (!reader.IsDBNull(0) ? (reader.GetString(0) == "Approved" || reader.GetString(0) == "Rejected" ? true : false) : false);
                }
            }
            return new
            {
                approvalStatus = approvalStatus,
                isApproved = isApproved
            };
        }

        public async Task<(int total, int totalDisplay, IList<ConfigControlHistroyGetDto> records)> GetConfigControlHistory(int page, int perPage, string searchText, string sortText)
        {
            string query = @"EXEC [SP_IA_GET_CNFG_CNTRL_HISTORY] @PAGE_INDEX, @PAGE_SIZE, @SEARCH_TEXT, @ORDER_BY";

            List<Param> @params = new List<Param>()
            {
                new Param{ SqlDbType = SqlDbType.Int, ParamName="@PAGE_INDEX", SqlValue = page},
                new Param{ SqlDbType = SqlDbType.Int, ParamName="@PAGE_SIZE", SqlValue = perPage},
                new Param{ SqlDbType = SqlDbType.NVarChar, ParamName="@SEARCH_TEXT", SqlValue = searchText},
                new Param{ SqlDbType = SqlDbType.NVarChar, ParamName="@ORDER_BY", SqlValue = sortText}
            };

            IList<ConfigControlHistroyGetDto> vendorDashboardData = new List<ConfigControlHistroyGetDto>();

            int totalRecords = 0;
            int filteredRecords = 0;

            using (var reader = await _ibaseDataAccess.ExecuteReaderAsync(query, @params))
            {
                var countReadFlag = false;
                while (reader.Read())
                {
                    var rowInfo = new ConfigControlHistroyGetDto
                    {
                        RecordID = reader.GetInt32(0),
                        ApprovalDate = reader.GetDateTime(1).Date,
                        ApprovalStatus = reader.GetString(2),
                        Name = reader.GetString(3),
                        ApprovalComment = reader.GetString(4),
                        SignOffPdfData = (byte[])reader.GetValue(5),
                    };
                    vendorDashboardData.Add(rowInfo);
                    if (!countReadFlag)
                    {
                        totalRecords = reader.GetInt32(6);
                        filteredRecords = reader.GetInt32(7);
                        countReadFlag = true;
                    }
                }
            }
            return (totalRecords, filteredRecords, vendorDashboardData);
        }

        public async Task<bool> GetAdvanceConfigActiveStatus()
        {
            string query = "EXEC [SP_IA_GET_CNF_CNTRL_APRV_STS]";
            var isApproved = false;
            using (var reader = await _ibaseDataAccess.ExecuteReaderAsync(query))
            {
                while (reader.Read())
                {
                    isApproved = (!reader.IsDBNull(0) ? (reader.GetString(0) == "Approved" ? true : false) : false);
                }
            }
            return isApproved;
        }

        public async Task<bool> GetAdvanceConfigActiveStatusForSidebar()
        {
            string query = "EXEC [SP_IA_GET_CNF_CTRL_APV_STS_SDE]";
            var isApproved = false;
            using (var reader = await _ibaseDataAccess.ExecuteReaderAsync(query))
            {
                while (reader.Read())
                {
                    isApproved = (!reader.IsDBNull(0) ? (reader.GetInt32(0) == 1 ? true : false) : false);
                }
            }
            return isApproved;
        }

        public async Task<bool> CheckIfAllSubsectionIsEnabled()
        {
            string query = "EXEC [SP_IA_IS_ALL_SUBSEC_ENABLED]";
            var allSubsectionApproved = false;
            using (var reader = await _ibaseDataAccess.ExecuteReaderAsync(query))
            {
                while (reader.Read())
                {
                    allSubsectionApproved = (!reader.IsDBNull(0) ? (reader.GetInt32(0) == 1 ? true : false) : false);
                }
            }
            return allSubsectionApproved;
        }
    }
}
