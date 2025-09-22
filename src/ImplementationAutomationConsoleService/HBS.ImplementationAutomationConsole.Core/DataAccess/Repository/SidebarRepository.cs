using HBS.ImplementationAutomationConsole.Core.DataAccess.Data;
using HBS.ImplementationAutomationConsole.Core.DataAccess.IRepository;
using HBS.ImplementationAutomationConsole.Core.Models;
using System.Data;

namespace HBS.ImplementationAutomationConsole.Core.DataAccess.Repository
{
    public class SidebarRepository : ISidebarRepository
    {
        private readonly IBaseDataAccess _iBaseDataAccess;
        public SidebarRepository(IBaseDataAccess iBaseDataAccess)
        {
            this._iBaseDataAccess = iBaseDataAccess;
        }

        public async Task<IList<SidebarDto>> GetSubsectionItems()
        {
            string query = @"EXEC [SP_IA_GET_SUBSECTION_DATA]";
            IList<SidebarDto> SubsectionData = new List<SidebarDto>();
            using (var reader = await _iBaseDataAccess.ExecuteReaderAsync(query))
            {
                while (reader.Read())
                {
                    var sidebarComponent = new SidebarDto
                    {
                        Subsection_Id = reader.GetString(0),
                        Subsection_Name = reader.GetString(1),
                        Module_Id = reader.GetInt32(2),
                        Approval_Status = reader.IsDBNull(3) ? "Not Approved" : reader.GetString(3),
                        Vendor_ApprovalStatus = reader.IsDBNull(4) ? "Not Approved" : reader.GetString(4),
                        Enabled = false
                    };
                    SubsectionData.Add(sidebarComponent);
                }
            }
            return SubsectionData;
        }

        public async Task<string> GetCopyRightText()
        {
            string query = @"EXEC [SP_IA_GET_CPRGHT_CNFG_ASST]";
            var copyRightText = string.Empty;
            using (var reader = await _iBaseDataAccess.ExecuteReaderAsync(query))
            {
                while (reader.Read())
                {
                    copyRightText = reader.GetString(0);
                }
            }
            return copyRightText;
        }

        public async Task<List<ModuleDto>> GetActiveModules()
        {
            string query = @"EXEC [SP_IA_GET_ACTIVE_MODULES]";
            var activeModules = new List<ModuleDto>();

            using (var reader = await _iBaseDataAccess.ExecuteReaderAsync(query))
            {
                while (reader.Read())
                {
                    activeModules.Add(new ModuleDto
                    {
                        moduleId = Convert.ToInt32(reader.GetString(0)),
                        moduleName = reader.GetString(1),
                        order = !reader.IsDBNull(2) ? reader.GetInt32(2) : 0,
                        is_enable=false
                    });
                }
            }
            return activeModules;
        }

        public async Task<List<int>> GetActiveModuleIDs()
        {
            string query = @"EXEC [SP_IA_GET_ACTIVE_MODULES]";
            var activeModuleIds = new List<int>();

            using (var reader = await _iBaseDataAccess.ExecuteReaderAsync(query))
            {
                while (reader.Read())
                {
                    activeModuleIds.Add(Convert.ToInt32(reader.GetString(0)));
                }
            }
            return activeModuleIds;
        }

        public async Task<List<ActiveSubsectionDto>> GetActiveSubsectionsEim()
        {
            string query = @"EXEC [SP_IA_GET_ACTIVE_SUBS_EIM]";
            var activeSubsecs = new List<ActiveSubsectionDto>();

            using (var reader = await _iBaseDataAccess.ExecuteReaderAsync(query))
            {
                while (reader.Read())
                {
                    activeSubsecs.Add(new ActiveSubsectionDto
                    {
                        SubsectionId = reader.GetString(0),
                        SubsectionName = reader.GetString(1),
                        Order = !reader.IsDBNull(2) ? reader.GetInt32(2) : 0,
                    });
                }
            }
            return activeSubsecs;
        }

        public async Task<List<ActiveSubsectionDto>> GetActiveSubsectionsLeave()
        {
            string query = @"EXEC [SP_IA_GET_ACTIVE_SUBS_LEA]";
            var activeSubsecs = new List<ActiveSubsectionDto>();

            using (var reader = await _iBaseDataAccess.ExecuteReaderAsync(query))
            {
                while (reader.Read())
                {
                    activeSubsecs.Add(new ActiveSubsectionDto
                    {
                        SubsectionId = reader.GetString(0),
                        SubsectionName=reader.GetString(1),
                        Order = !reader.IsDBNull(2) ? reader.GetInt32(2) : 0,
                    });
                }
            }
            return activeSubsecs;
        }

        public async Task<List<ActiveSubsectionDto>> GetActiveSubsectionsAttendance()
        {
            string query = @"EXEC [SP_IA_GET_ACTIVE_SUBS_ATT]";
            var activeSubsecs = new List<ActiveSubsectionDto>();

            using (var reader = await _iBaseDataAccess.ExecuteReaderAsync(query))
            {
                while (reader.Read())
                {
                    activeSubsecs.Add(new ActiveSubsectionDto
                    {
                        SubsectionId = reader.GetString(0),
                        SubsectionName = reader.GetString(1),
                        Order = !reader.IsDBNull(2) ? reader.GetInt32(2) : 0,
                    });
                }
            }
            return activeSubsecs;
        }

        public async Task<bool> CheckIfSubsectionApproved(string subsectionName)
        {
            string query = @"EXEC [SP_IA_CHK_SUBSEC_APPRVD] @SUBSECTION_NAME";
            List<Param> @params = new List<Param>()
            {
                new Param{ SqlDbType = SqlDbType.NVarChar, ParamName="@SUBSECTION_NAME", SqlValue = subsectionName},
            };

            var approved = false;

            using (var reader = await _iBaseDataAccess.ExecuteReaderAsync(query, @params))
            {
                while (reader.Read())
                {
                    approved = !reader.IsDBNull(0) ? (reader.GetInt32(0)==1?true:false):false;
                }
            }
            return approved;
        }

        public async Task<IList<DashboardApprovalDto>> GetDashboardApprovalItems()
        {
            string query = @"EXEC [SP_IA_GET_APRV_STS_MOD_ACTV]";
            IList<DashboardApprovalDto> SubsectionData = new List<DashboardApprovalDto>();
            using (var reader = await _iBaseDataAccess.ExecuteReaderAsync(query))
            {
                while (reader.Read())
                {
                    var sidebarComponent = new DashboardApprovalDto
                    {
                        SubsectionID = reader.GetString(0),
                        ModuleID = reader.GetInt32(1),
                        ApprovalStatus = !reader.IsDBNull(2) ? reader.GetString(2):"Not Approved" ,
                        VendorApprovalStatus = !reader.IsDBNull(3) ? reader.GetString(3): "Not Approved",
                    };
                    SubsectionData.Add(sidebarComponent);
                }
            }
            return SubsectionData;
        }
    }
}