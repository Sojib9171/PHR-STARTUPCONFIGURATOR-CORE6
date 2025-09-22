using HBS.ImplementationAutomationConsole.Core.DataAccess.Data;
using HBS.ImplementationAutomationConsole.Core.DataAccess.IRepository;
using HBS.ImplementationAutomationConsole.Core.Models;
using System.Data;

namespace HBS.ImplementationAutomationConsole.Core.DataAccess.Repository
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly IBaseDataAccess _iBaseDataAccess;
        public DashboardRepository(IBaseDataAccess iBaseDataAccess)
        {
            this._iBaseDataAccess = iBaseDataAccess;
        }

        public async Task InsertDashboardData(DashboardPostDto dashboardDto)
        {
            string query = @"EXEC [SP_IA_INSRT_IN_IA_DASHBRD_INFO] @MODULE_ID, @SUBSECTION_ID, @APPROVAL_STATUS, @APPROVAL_BY, @APPROVAL_COMMENT, @APPROVAL_SIGNATURE, @APPROVAL_DATE, @APPROVAL_DATA,@SIGNOFF_DATA";
            List<Param> @params = new List<Param>()
            {
                new Param{ SqlDbType = SqlDbType.Int, ParamName="@MODULE_ID", SqlValue = dashboardDto.ModuleID},
                new Param{ SqlDbType = SqlDbType.NVarChar, ParamName="@SUBSECTION_ID", SqlValue = dashboardDto.SubsectionID},
                new Param{ SqlDbType = SqlDbType.NVarChar, ParamName="@APPROVAL_STATUS", SqlValue = dashboardDto.ApprovalStatus},
                new Param{ SqlDbType = SqlDbType.NVarChar, ParamName="@APPROVAL_BY", SqlValue = dashboardDto.Name},
                new Param{ SqlDbType = SqlDbType.NVarChar, ParamName="@APPROVAL_COMMENT", SqlValue = dashboardDto.Comment},
                new Param{ SqlDbType = SqlDbType.NVarChar, ParamName="@APPROVAL_SIGNATURE", SqlValue = dashboardDto.UserName},
                new Param{ SqlDbType = SqlDbType.DateTime, ParamName="@APPROVAL_DATE", SqlValue = dashboardDto.ApprovalDate},
                new Param{ SqlDbType = SqlDbType.VarBinary, ParamName="@APPROVAL_DATA", SqlValue = dashboardDto.ApprovalData},
                new Param{ SqlDbType = SqlDbType.VarBinary, ParamName="@SIGNOFF_DATA", SqlValue = dashboardDto.SignOffPdfData}
            };
            await _iBaseDataAccess.ExecuteQueryAsync(query, @params);
        }

        public async Task<(int, string, string)> GetModuleAndSubsectioninfoFromSubsectionName(string subsectionName)
        {
            string query = @"EXEC [SP_IA_GET_DATA_WITH_SUBSC_NAME] @SUBSECTION_NAME";

            List<Param> @params = new List<Param>()
            {
                new Param{ SqlDbType = SqlDbType.VarChar, ParamName="@SUBSECTION_NAME", SqlValue = subsectionName}
            };

            int moduleId = 0;
            string subsectionID = string.Empty;
            string moduleName = string.Empty;

            using (var reader = await _iBaseDataAccess.ExecuteReaderAsync(query, @params))
            {
                while (reader.Read())
                {
                    moduleId = reader.GetInt32(0);
                    subsectionID = reader.GetString(1);
                    moduleName = reader.GetString(2);
                }
            }
            return new(moduleId, subsectionID, moduleName);
        }

        public async Task<(int total, int totalDisplay, IList<DashboardGetDto> records)> GetDashboardInformation(int pageIndex, int pageSize, string searchText, string sortText, string moduleName)
        {
            string query = @"EXEC [SP_IA_GET_DASHBOARD_DATA_GROUP] @PAGE_INDEX, @PAGE_SIZE, @SEARCH_TEXT, @ORDER_BY,@MODULE_NAME";

            List<Param> @params = new List<Param>()
            {
                new Param{ SqlDbType = SqlDbType.Int, ParamName="@PAGE_INDEX", SqlValue = pageIndex},
                new Param{ SqlDbType = SqlDbType.Int, ParamName="@PAGE_SIZE", SqlValue = pageSize},
                new Param{ SqlDbType = SqlDbType.NVarChar, ParamName="@SEARCH_TEXT", SqlValue = searchText},
                new Param{ SqlDbType = SqlDbType.NVarChar, ParamName="@ORDER_BY", SqlValue = sortText},
                new Param{ SqlDbType = SqlDbType.NVarChar, ParamName="@MODULE_NAME", SqlValue = moduleName}
            };

            IList<DashboardGetDto> dashboardData = new List<DashboardGetDto>();

            int totalRecords = 0;
            int filteredRecords = 0;

            using (var reader = await _iBaseDataAccess.ExecuteReaderAsync(query, @params))
            {
                var countReadFlag = false;

                while (reader.Read())
                {
                    var dashboardRowInfo = new DashboardGetDto
                    {
                        RecordID = !reader.IsDBNull(0) ? reader.GetInt32(0):0,
                        MainsectionName = !reader.IsDBNull(1) ? reader.GetString(1):string.Empty,
                        SubsectionName = !reader.IsDBNull(2) ? reader.GetString(2) : string.Empty,
                        ApprovalStatus = !reader.IsDBNull(3) ? reader.GetString(3):null,
                        Name = !reader.IsDBNull(5) ? reader.GetString(5) : string.Empty,
                        VendorApprovalStatus = !reader.IsDBNull(5) ? reader.GetString(5) : null,
                        VendorName = !reader.IsDBNull(6) ? reader.GetString(6) : string.Empty,
                    };
                    dashboardData.Add(dashboardRowInfo);
                    if (!countReadFlag)
                    {
                        totalRecords = reader.GetInt32(7);
                        filteredRecords = reader.GetInt32(8);
                        countReadFlag = true;
                    }
                }
            }
            return (totalRecords, filteredRecords, dashboardData);
        }

        public async Task<(int total, int totalDisplay, IList<DashboardGetDto> records)> GetDashboardHistoryInformation(string subsectionName, int pageIndex, int pageSize, string searchText, string sortText)
        {
            string query = @"EXEC [SP_IA_GET_DASHBOARD_HISTORY] @SUBSECTION_NAME, @PAGE_INDEX, @PAGE_SIZE, @SEARCH_TEXT, @ORDER_BY";

            List<Param> @params = new List<Param>()
            {
                new Param{ SqlDbType = SqlDbType.NVarChar, ParamName="@SUBSECTION_NAME", SqlValue = subsectionName},
                new Param{ SqlDbType = SqlDbType.Int, ParamName="@PAGE_INDEX", SqlValue = pageIndex},
                new Param{ SqlDbType = SqlDbType.Int, ParamName="@PAGE_SIZE", SqlValue = pageSize},
                new Param{ SqlDbType = SqlDbType.NVarChar, ParamName="@SEARCH_TEXT", SqlValue = searchText},
                new Param{ SqlDbType = SqlDbType.NVarChar, ParamName="@ORDER_BY", SqlValue = sortText}
            };

            IList<DashboardGetDto> dashboardData = new List<DashboardGetDto>();

            int totalRecords = 0;
            int filteredRecords = 0;

            using (var reader = await _iBaseDataAccess.ExecuteReaderAsync(query, @params))
            {
                var countReadFlag = false;
                while (reader.Read())
                {
                    var dashboardRowInfo = new DashboardGetDto
                    {
                        ApprovalDate = reader.GetDateTime(0).Date,
                        ApprovalStatus = reader.GetString(1),
                        Name = reader.GetString(2),
                        ApprovalComment = reader.GetString(3),
                        ApprovalData = (byte[])reader.GetValue(4),
                        SignOffPdfData = (byte[])reader.GetValue(5)
                    };
                    dashboardData.Add(dashboardRowInfo);
                    if (!countReadFlag)
                    {
                        totalRecords = reader.GetInt32(6);
                        filteredRecords = reader.GetInt32(7);
                        countReadFlag = true;
                    }
                }
            }
            return (totalRecords, filteredRecords, dashboardData);
        }
    }
}