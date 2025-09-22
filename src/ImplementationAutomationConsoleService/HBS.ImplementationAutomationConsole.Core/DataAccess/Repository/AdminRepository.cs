using HBS.ImplementationAutomationConsole.Core.DataAccess.Data;
using HBS.ImplementationAutomationConsole.Core.DataAccess.IRepository;
using HBS.ImplementationAutomationConsole.Core.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace HBS.ImplementationAutomationConsole.Core.DataAccess.Repository
{
    public class AdminRepository : IAdminRepository
    {
        private readonly IBaseDataAccess _iBaseDataAccess;
        public AdminRepository(IBaseDataAccess iBaseDataAccess)
        {
            this._iBaseDataAccess = iBaseDataAccess;
        }

        public async Task<(int total, int totalDisplay, IList<VendorApprovalGetDto> records)> GetPendingApprovalForAdmin(int pageIndex, int pageSize, string searchText, string sortText)
        {
            string query = @"EXEC [SP_IA_GET_USER_APRV_FOR_ADMN] @PAGE_INDEX, @PAGE_SIZE, @SEARCH_TEXT, @ORDER_BY";

            List<Param> @params = new List<Param>()
            {
                new Param{ SqlDbType = SqlDbType.Int, ParamName="@PAGE_INDEX", SqlValue = pageIndex},
                new Param{ SqlDbType = SqlDbType.Int, ParamName="@PAGE_SIZE", SqlValue = pageSize},
                new Param{ SqlDbType = SqlDbType.NVarChar, ParamName="@SEARCH_TEXT", SqlValue = searchText},
                new Param{ SqlDbType = SqlDbType.NVarChar, ParamName="@ORDER_BY", SqlValue = sortText}
            };

            IList<VendorApprovalGetDto> vendorApprovalData = new List<VendorApprovalGetDto>();

            int totalRecords = 0;
            int filteredRecords = 0;

            using (var reader = await _iBaseDataAccess.ExecuteReaderAsync(query, @params))
            {
                var countReadFlag = false;

                while (reader.Read())
                {
                    var rowInfo = new VendorApprovalGetDto
                    {
                        RecordID = reader.GetInt32(0),
                        MainsectionName = reader.GetString(1),
                        SubsectionName = reader.GetString(2),
                        ApprovalStatus = reader.GetString(3),
                        Name = reader.GetString(4),
                        ApprovalComment = reader.GetString(5),
                        ApprovalDate = reader.GetDateTime(6).Date,
                        ApprovalData = (byte[])reader.GetValue(7)
                    };
                    vendorApprovalData.Add(rowInfo);
                    if (!countReadFlag)
                    {
                        totalRecords = reader.GetInt32(8);
                        filteredRecords = reader.GetInt32(9);
                        countReadFlag = true;
                    }
                }
            }
            return (totalRecords, filteredRecords, vendorApprovalData);
        }

        public async Task<byte[]> GetExcelByteArray(int recordID)
        {
            string query = @"EXEC [SP_IA_GET_EXCEL_WITH_RECORD_ID] @RECORD_ID";
            List<Param> @params = new List<Param>()
            {
                new Param{ SqlDbType = SqlDbType.Int, ParamName="@RECORD_ID", SqlValue = recordID},
            };
            var excelData = new byte[] { };
            using (var reader = await _iBaseDataAccess.ExecuteReaderAsync(query, @params))
            {
                while (reader.Read())
                {
                    excelData = (byte[])reader.GetValue(0);
                }
            }
            return excelData;
        }

        public async Task InsertVendorInfo(VendorPostDto vendorDto)
        {
            string query = @"EXEC [SP_IA_INSRT_INTO_IA_VENDR_INFO] @VENDOR_NAME,@VENDOR_APPROVAL_STATUS,@VENDOR_APPROVAL_COMMENT,@VENDOR_APPROVAL_DATE,@USER_RECORD_ID";

            List<Param> @params = new List<Param>()
            {
                new Param{ SqlDbType = SqlDbType.NVarChar, ParamName="@VENDOR_NAME", SqlValue = vendorDto.VendorName},
                new Param{ SqlDbType = SqlDbType.NVarChar, ParamName="@VENDOR_APPROVAL_STATUS", SqlValue = vendorDto.VendorApprovalStatus},
                new Param{ SqlDbType = SqlDbType.NVarChar, ParamName="@VENDOR_APPROVAL_COMMENT", SqlValue = vendorDto.VendorApprovalComment},
                new Param{ SqlDbType = SqlDbType.DateTime, ParamName="@VENDOR_APPROVAL_DATE", SqlValue = vendorDto.VendorApprovalDate},
                new Param{ SqlDbType = SqlDbType.Int, ParamName="@USER_RECORD_ID", SqlValue = vendorDto.UserRecordId},
            };

            try
            {
                await _iBaseDataAccess.ExecuteQueryAsync(query, @params);
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<(int total, int totalDisplay, IList<DashboardGetDto> records)> GetVendorDashboardHistoryInfo(string subsectionName, int pageIndex, int pageSize, string searchText, string sortText)
        {
            string query = @"EXEC [SP_IA_GET_DSHBRD_VNDR_HISTORY] @SUBSECTION_NAME, @PAGE_INDEX, @PAGE_SIZE, @SEARCH_TEXT, @ORDER_BY";

            List<Param> @params = new List<Param>()
            {
                new Param{ SqlDbType = SqlDbType.NVarChar, ParamName="@SUBSECTION_NAME", SqlValue = subsectionName},
                new Param{ SqlDbType = SqlDbType.Int, ParamName="@PAGE_INDEX", SqlValue = pageIndex},
                new Param{ SqlDbType = SqlDbType.Int, ParamName="@PAGE_SIZE", SqlValue = pageSize},
                new Param{ SqlDbType = SqlDbType.NVarChar, ParamName="@SEARCH_TEXT", SqlValue = searchText},
                new Param{ SqlDbType = SqlDbType.NVarChar, ParamName="@ORDER_BY", SqlValue = sortText}
            };

            IList<DashboardGetDto> vendorDashboardData = new List<DashboardGetDto>();

            int totalRecords = 0;
            int filteredRecords = 0;

            using (var reader = await _iBaseDataAccess.ExecuteReaderAsync(query, @params))
            {
                var countReadFlag = false;
                while (reader.Read())
                {
                    var dashboardRowInfo = new DashboardGetDto
                    {
                        VendorApprovalDate = reader.GetDateTime(0).Date,
                        VendorApprovalStatus = reader.GetString(1),
                        VendorName = reader.GetString(2),
                        VendorApprovalComment = reader.GetString(3),
                        ApprovalData = (byte[])reader.GetValue(4),
                    };
                    vendorDashboardData.Add(dashboardRowInfo);
                    if (!countReadFlag)
                    {
                        totalRecords = reader.GetInt32(5);
                        filteredRecords = reader.GetInt32(6);
                        countReadFlag = true;
                    }
                }
            }
            return (totalRecords, filteredRecords, vendorDashboardData);
        }

        public async Task<int> GetRemainingAdminApprovalCount()
        {
            string query = @"EXEC [SP_IA_GET_REM_VLDTN_COUNT]";
            int count = 0;
            using (var reader = await _iBaseDataAccess.ExecuteReaderAsync(query))
            {
                while (reader.Read())
                {
                    count = !reader.IsDBNull(0) ? reader.GetInt32(0) : 0;
                }
            }
            return count;
        }

        public async Task<(int total, int totalDisplay, IList<VendorApprovalGetDto> records)> GetApprovalSummaryForAdmin(int page, int perPage, string searchText, string orderText)
        {
            string query = @"EXEC [SP_IA_GET_ADMN_APRV_DATA_GROUP] @PAGE_INDEX, @PAGE_SIZE, @SEARCH_TEXT, @ORDER_BY";

            List<Param> @params = new List<Param>()
            {
                new Param{ SqlDbType = SqlDbType.Int, ParamName="@PAGE_INDEX", SqlValue = page},
                new Param{ SqlDbType = SqlDbType.Int, ParamName="@PAGE_SIZE", SqlValue = perPage},
                new Param{ SqlDbType = SqlDbType.NVarChar, ParamName="@SEARCH_TEXT", SqlValue = searchText},
                new Param{ SqlDbType = SqlDbType.NVarChar, ParamName="@ORDER_BY", SqlValue = orderText}
            };

            IList<VendorApprovalGetDto> vendorApprovalSummaryData = new List<VendorApprovalGetDto>();

            int totalRecords = 0;
            int filteredRecords = 0;

            using (var reader = await _iBaseDataAccess.ExecuteReaderAsync(query, @params))
            {
                var countReadFlag = false;

                while (reader.Read())
                {
                    var rowInfo = new VendorApprovalGetDto
                    {
                        RecordID = reader.GetInt32(0),
                        MainsectionName = reader.GetString(1),
                        SubsectionName = reader.GetString(2),
                        ApprovalStatus = reader.GetString(3),
                        Name = reader.GetString(4),
                    };
                    vendorApprovalSummaryData.Add(rowInfo);
                    if (!countReadFlag)
                    {
                        totalRecords = reader.GetInt32(5);
                        filteredRecords = reader.GetInt32(6);
                        countReadFlag = true;
                    }
                }
            }
            return (totalRecords, filteredRecords, vendorApprovalSummaryData);
        }

        public async Task<(int total, int totalDisplay, IList<VendorApprovalGetDto> records)> GetVendorApprovalSummaryHistory(string subsection, int page, int perPage, string searchText, string orderText)
        {
            string query = @"EXEC [SP_IA_GET_ADMN_APRV_SUMRY_HIST] @SUBSECTION_NAME, @PAGE_INDEX, @PAGE_SIZE, @SEARCH_TEXT, @ORDER_BY";

            List<Param> @params = new List<Param>()
            {
                new Param{ SqlDbType = SqlDbType.NVarChar, ParamName="@SUBSECTION_NAME", SqlValue = subsection},
                new Param{ SqlDbType = SqlDbType.Int, ParamName="@PAGE_INDEX", SqlValue = page},
                new Param{ SqlDbType = SqlDbType.Int, ParamName="@PAGE_SIZE", SqlValue = perPage},
                new Param{ SqlDbType = SqlDbType.NVarChar, ParamName="@SEARCH_TEXT", SqlValue = searchText},
                new Param{ SqlDbType = SqlDbType.NVarChar, ParamName="@ORDER_BY", SqlValue = orderText}
            };

            IList<VendorApprovalGetDto> vendorApprovalSummaryHistoryData = new List<VendorApprovalGetDto>();


            int totalRecords = 0;
            int filteredRecords = 0;

            using (var reader = await _iBaseDataAccess.ExecuteReaderAsync(query, @params))
            {
                var countReadFlag = false;
                while (reader.Read())
                {
                    var rowInfo = new VendorApprovalGetDto
                    {
                        ApprovalDate = reader.GetDateTime(0).Date,
                        ApprovalStatus = reader.GetString(1),
                        Name = reader.GetString(2),
                        ApprovalComment = reader.GetString(3),
                        ApprovalData = (byte[])reader.GetValue(4)
                    };
                    vendorApprovalSummaryHistoryData.Add(rowInfo);
                    if (!countReadFlag)
                    {
                        totalRecords = reader.GetInt32(5);
                        filteredRecords = reader.GetInt32(6);
                        countReadFlag = true;
                    }
                }
            }
            return (totalRecords, filteredRecords, vendorApprovalSummaryHistoryData);
        }

        public async Task<string> GetSubsectionNameFromUserRecordId(int userRecordId)
        {
            string query = "EXEC [SP_IA_GET_SBSCTN_FRM_RECORDID] @USER_RECORD_ID";
            List<Param> @params = new List<Param>()
            {
                new Param{ SqlDbType = SqlDbType.Int, ParamName="@USER_RECORD_ID", SqlValue=userRecordId}
            };

            var subsectionName = string.Empty;

            using (var reader = await _iBaseDataAccess.ExecuteReaderAsync(query, @params))
            {
                while (reader.Read())
                {
                    subsectionName = reader.GetString(0);
                }
            }
            return subsectionName;
        }

        public async Task InsertDataIntoMainTable(string subsectionName)
        {
            string query;
            switch (subsectionName)
            {
                case "Employee Data":
                    query = "EXEC [SP_AI_UPLOAD_EMPLOYEE]";
                    break;

                case "Bank Details":
                    query = "EXEC [SP_AI_UPLOAD_EMPBANK]";
                    break;

                case "Reporting Hierarchy":
                    query = "EXEC [SP_AI_UPLOAD_REPORTTO]";
                    break;

                case "Dependent Information":
                    query = "EXEC [SP_AI_DEPINFO_UPLOAD]";
                    break;

                case "Emergency Contact":
                    query = "EXEC [SP_AI_EC_UPLOAD]";
                    break;

                case "Statutory Leave":
                    query = "EXEC [SP_STATUTORY_LEAVE_UPLOAD]";
                    break;

                case "Short Leave":
                    query = "EXEC [SP_SHORT_LEAVE_UPLOAD]";
                    break;

                case "Shift Information":
                    query = "EXEC [SP_AI_UPLOAD_SHIFT_INFO]";
                    break;

                case "Roster Information":
                    query = "EXEC [SP_AI_UPLOAD_ROSTER_INFO]";
                    break;

                default:
                    throw new Exception("Invalid Subsection");

            }

            var isUploadedinmainTable = string.Empty;
            var errorMessage = string.Empty;
            using (var reader = await _iBaseDataAccess.ExecuteReaderAsync(query))
            {
                while (reader.Read())
                {
                    isUploadedinmainTable = reader.GetString(0);
                    errorMessage=!reader.IsDBNull(1)? reader.GetString(1):string.Empty;
                }
            }

            if (isUploadedinmainTable == "False")
                throw new Exception(errorMessage);
        }
    }
}