using HBS.ImplementationAutomationConsole.Core.DataAccess.Data;
using HBS.ImplementationAutomationConsole.Core.DataAccess.IRepository;
using HBS.ImplementationAutomationConsole.Core.Models;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace HBS.ImplementationAutomationConsole.Core.DataAccess.Repository
{
    public class WizardDataRepository : IWizardDataRepository
    {
        private readonly IBaseDataAccess _iBaseDataAccess;
        private readonly IConfiguration _iconfiguration;
        public WizardDataRepository(IBaseDataAccess iBaseDataAccess, IConfiguration configuration)
        {
            _iBaseDataAccess = iBaseDataAccess;
            _iconfiguration = configuration;
        }

        public async Task DeleteFromDraftTable(int tableRowId, string subsectionName)
        {
            string query = subsectionName switch
            {
                "Short Leave" => "EXEC [SP_IA_DLT_SHRT_LEA_DRFT] @ID",
                "Statutory Leave" => "EXEC [SP_IA_DLT_STAT_LEA_DRFT]@ID",
                _ => "Default",
            };

            List<Param> @params = new List<Param>()
            {
                new Param{ SqlDbType = SqlDbType.Int, ParamName="@ID", SqlValue = tableRowId}
            };
            await _iBaseDataAccess.ExecuteQueryAsync(query, @params);
        }

        public async Task DeleteFromMainTable(int tableRowId, string subsectionName)
        {
            string query = subsectionName switch
            {
                "Short Leave" => "EXEC [SP_IA_DLT_SHRT_LEA_UPLD] @ID",
                "Statutory Leave" => "EXEC [SP_IA_DLT_STAT_LEA_UPLD] @ID",
                _ => "Default",
            };
            List<Param> @params = new List<Param>()
            {
                new Param{ SqlDbType = SqlDbType.Int, ParamName="@ID", SqlValue = tableRowId}
            };
            await _iBaseDataAccess.ExecuteQueryAsync(query, @params);
        }

        public async Task<IList<WizardQuestionDto>> GetAbsenceWizardQuestionDetails(string subsectionId)
        {
            string query = "EXEC [SP_IA_GET_ABSNC_WIZ_QSTIONS] @SUBSECTION_ID";
            List<Param> @params = new List<Param>()
            {
                new Param{SqlDbType=SqlDbType.NVarChar, ParamName="@SUBSECTION_ID", SqlValue=subsectionId}
            };

            var questionList = new List<WizardQuestionDto>();
            using (var reader = await _iBaseDataAccess.ExecuteReaderAsync(query, @params))
            {
                while (reader.Read())
                {
                    var question = new WizardQuestionDto
                    {
                        Question_No = reader.GetInt32(0),
                        Question_Statement = reader.GetString(1),
                        Question_Type = reader.GetString(2),
                    };
                    questionList.Add(question);
                }
            }
            return questionList;
        }

        public async Task<List<object>> GetAbsenceWizardResponsedByRowId(int rowId, string subsectionName)
        {
            var response = subsectionName switch
            {
                "Short Leave" => await GetAbsenceWizardResponsesForShortLeave(rowId),
                "Statutory Leave" => await GetAbsenceWizardResponsesForStatutoryLeave(rowId),
                _ => new List<object> { },
            };

            return response;
        }

        public async Task<List<object>> GetAbsenceWizardResponsesForShortLeave(int rowId)
        {
            string query = "EXEC [SP_IA_GET_SHRT_LEA_RESPONSE] @ID";

            List<Param> @params = new List<Param>()
            {
                new Param{ SqlDbType = SqlDbType.Int, ParamName="@ID", SqlValue = rowId}
            };

            var response = new List<object>();
            try
            {
                using (var reader = await _iBaseDataAccess.ExecuteReaderAsync(query, @params))
                {
                    while (reader.Read())
                    {
                        response.Add(!reader.IsDBNull(0) ? reader.GetString(0) : string.Empty);
                        response.Add(!reader.IsDBNull(1) ? reader.GetString(1) : string.Empty);
                        response.Add(!reader.IsDBNull(2) ? reader.GetDouble(2) : string.Empty);
                        response.Add(!reader.IsDBNull(3) ? reader.GetDouble(3) : string.Empty);
                        response.Add(!reader.IsDBNull(4) ? reader.GetDouble(4) : string.Empty);
                        response.Add(!reader.IsDBNull(5) ? reader.GetString(5) : string.Empty);

                        response.Add(!reader.IsDBNull(6) ? (reader.GetInt32(6) == 1 ? "Yes" : "No") : string.Empty);
                        response.Add(!reader.IsDBNull(7) ? reader.GetInt32(7) : string.Empty);
                        response.Add(!reader.IsDBNull(8) ? reader.GetDouble(8) : string.Empty);
                        response.Add(!reader.IsDBNull(9) ? reader.GetDecimal(9) : string.Empty);
                        response.Add(!reader.IsDBNull(10) ? reader.GetDecimal(10) : string.Empty);
                        response.Add(!reader.IsDBNull(11) ? reader.GetDecimal(11) : string.Empty);

                        response.Add(!reader.IsDBNull(12) ? reader.GetDecimal(12) : string.Empty);
                        response.Add(!reader.IsDBNull(13) ? (reader.GetInt16(13) == 1 ? "Yes" : "No") : string.Empty);
                        response.Add(!reader.IsDBNull(14) ? reader.GetString(14) : string.Empty);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }

        public async Task<List<object>> GetAbsenceWizardResponsesForStatutoryLeave(int rowId)
        {
            string query = "EXEC [SP_IA_GET_STAT_LEA_RESPONSE] @ID";

            List<Param> @params = new List<Param>()
            {
                new Param{ SqlDbType = SqlDbType.Int, ParamName="@ID", SqlValue = rowId}
            };

            var response = new List<object>();
            try
            {
                using (var reader = await _iBaseDataAccess.ExecuteReaderAsync(query, @params))
                {
                    while (reader.Read())
                    {
                        response.Add(!reader.IsDBNull(0) ? reader.GetString(0) : string.Empty);
                        response.Add(!reader.IsDBNull(1) ? reader.GetString(1) : string.Empty);

                        response.Add(!reader.IsDBNull(2) ? (reader.GetInt16(2) == 1 ? "Yes" : "No") : string.Empty);
                        response.Add(!reader.IsDBNull(3) ? (reader.GetInt32(3) == 1 ? "Yes" : "No") : string.Empty);
                        response.Add(!reader.IsDBNull(4) ? (reader.GetInt16(4) == 1 ? "Yes" : "No") : string.Empty);
                        response.Add(!reader.IsDBNull(5) ? (reader.GetInt32(5) == 1 ? "Yes" : "No") : string.Empty);
                        response.Add(!reader.IsDBNull(6) ? (reader.GetInt16(6) == 1 ? "Yes" : "No") : string.Empty);
                        response.Add(!reader.IsDBNull(7) ? (reader.GetInt16(7) == 1 ? "Yes" : "No") : string.Empty);
                        response.Add(!reader.IsDBNull(8) ? (reader.GetInt16(8) == 1 ? "Yes" : "No") : string.Empty);
                        response.Add(!reader.IsDBNull(9) ? (reader.GetInt16(9) == 1 ? "Yes" : "No") : string.Empty);
                        response.Add(!reader.IsDBNull(10) ? (reader.GetInt32(10) == 1 ? "Yes" : "No") : string.Empty);
                        response.Add(!reader.IsDBNull(11) ? (Convert.ToInt16(reader.GetDecimal(11)) == 1 ? "Yes" : "No") : string.Empty);
                        response.Add(!reader.IsDBNull(12) ? (reader.GetInt16(12) == 1 ? "Yes" : "No") : string.Empty);
                        response.Add(!reader.IsDBNull(13) ? (reader.GetInt16(13) == 1 ? "Yes" : "No") : string.Empty);
                        response.Add(!reader.IsDBNull(14) ? (Convert.ToInt16(reader.GetDecimal(14)) == 1 ? "Yes" : "No") : string.Empty);
                        response.Add(!reader.IsDBNull(15) ? (Convert.ToInt16(reader.GetDecimal(15)) == 1 ? "Yes" : "No") : string.Empty);
                        response.Add(!reader.IsDBNull(16) ? (Convert.ToInt16(reader.GetDecimal(16)) == 1 ? "Yes" : "No") : string.Empty);
                        response.Add(!reader.IsDBNull(17) ? (Convert.ToInt16(reader.GetDecimal(17)) == 1 ? "Yes" : "No") : string.Empty);
                        response.Add(!reader.IsDBNull(18) ? (Convert.ToInt16(reader.GetDecimal(18)) == 1 ? "Yes" : "No") : string.Empty);
                        response.Add(!reader.IsDBNull(19) ? (Convert.ToInt16(reader.GetDecimal(19)) == 1 ? "Yes" : "No") : string.Empty);
                        response.Add(!reader.IsDBNull(20) ? (Convert.ToInt16(reader.GetDecimal(20)) == 1 ? "Yes" : "No") : string.Empty);
                        response.Add(!reader.IsDBNull(21) ? (Convert.ToInt16(reader.GetDecimal(21)) == 1 ? "Yes" : "No") : string.Empty);
                        response.Add(!reader.IsDBNull(22) ? (Convert.ToInt16(reader.GetDecimal(22)) == 1 ? "Yes" : "No") : string.Empty);
                        response.Add(!reader.IsDBNull(23) ? (Convert.ToInt16(reader.GetDecimal(23)) == 1 ? "Yes" : "No") : string.Empty);
                        response.Add(!reader.IsDBNull(24) ? (Convert.ToInt16(reader.GetDecimal(24)) == 1 ? "Yes" : "No") : string.Empty);
                        response.Add(!reader.IsDBNull(25) ? (Convert.ToInt16(reader.GetDecimal(25)) == 1 ? "Yes" : "No") : string.Empty);
                        response.Add(!reader.IsDBNull(26) ? (reader.GetInt16(26) == 1 ? "Yes" : "No") : string.Empty);
                        response.Add(!reader.IsDBNull(27) ? (reader.GetInt32(27) == 1 ? "Yes" : "No") : string.Empty);

                        response.Add(!reader.IsDBNull(28) ? Convert.ToInt32(reader.GetDecimal(28)) : string.Empty);
                        response.Add(!reader.IsDBNull(29) ? reader.GetDecimal(29) : string.Empty);
                        response.Add(!reader.IsDBNull(30) ? reader.GetDecimal(30) : string.Empty);
                        response.Add(!reader.IsDBNull(31) ? reader.GetDecimal(31) : string.Empty);
                        response.Add(!reader.IsDBNull(32) ? reader.GetDecimal(32) : string.Empty);
                        response.Add(!reader.IsDBNull(33) ? reader.GetDecimal(33) : string.Empty);

                        response.Add(!reader.IsDBNull(34) ? reader.GetString(34) : string.Empty);
                        response.Add(!reader.IsDBNull(35) ? reader.GetString(35) : string.Empty);

                        response.Add(!reader.IsDBNull(36) ? reader.GetInt32(36) : string.Empty);
                        response.Add(!reader.IsDBNull(37) ? reader.GetString(37) : string.Empty);
                        response.Add(!reader.IsDBNull(38) ? reader.GetDecimal(38) : string.Empty);
                        response.Add(!reader.IsDBNull(39) ? reader.GetDecimal(39) : string.Empty);
                        response.Add(!reader.IsDBNull(40) ? reader.GetInt32(40) : string.Empty);

                        response.Add(!reader.IsDBNull(41) ? reader.GetString(41) : string.Empty);
                        response.Add(!reader.IsDBNull(42) ? reader.GetString(42) : string.Empty);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }

        public async Task<IList<string>> GetDropDownOtipnsWithQuestionNo(int moduleID, string subsectionID, int questionNo)
        {
            string query = "EXEC [SP_IA_GET_WZRD_DRPDWN_OPTIONS] @MODULE_ID, @SUBSECTION_ID, @QUESTION_NO";
            List<Param> @params = new List<Param>()
            {
                new Param{ SqlDbType = SqlDbType.Int, ParamName="@MODULE_ID", SqlValue = moduleID},
                new Param{ SqlDbType = SqlDbType.NVarChar, ParamName="@SUBSECTION_ID", SqlValue = subsectionID},
                new Param{ SqlDbType = SqlDbType.Int, ParamName="@QUESTION_NO", SqlValue = questionNo},
            };

            var optionList = new List<string>();
            using (var reader = await _iBaseDataAccess.ExecuteReaderAsync(query, @params))
            {

                while (reader.Read())
                {
                    var option = reader.GetString(0);
                    optionList.Add(option);
                }
            }
            return optionList;
        }

        public async Task<(int total, int totalDisplay, IList<LeaveTypePendingGetDto> records)> GetPendingLeaveTypesData(int page, int perPage, string searchText, string orderText, string subsection, int[] rowIds)
        {
            string query = subsection switch
            {
                "Short Leave" => "EXEC [SP_IA_GET_SHRT_LEA_PNDNG_ITEMS] @PAGE_INDEX, @PAGE_SIZE, @SEARCH_TEXT, @ORDER_BY, @INT_ARRAY",
                "Statutory Leave" => "EXEC [SP_IA_GET_STAT_LEA_PNDNG_ITEMS] @PAGE_INDEX, @PAGE_SIZE, @SEARCH_TEXT, @ORDER_BY, @INT_ARRAY",
                _ => "Default",
            };
            var rowDatatable = await CreateIntArrayParameter(rowIds);

            List<Param> @params = new List<Param>()
            {
                new Param{ SqlDbType = SqlDbType.Int, ParamName="@PAGE_INDEX", SqlValue = page},
                new Param{ SqlDbType = SqlDbType.Int, ParamName="@PAGE_SIZE", SqlValue = perPage},
                new Param{ SqlDbType = SqlDbType.NVarChar, ParamName="@SEARCH_TEXT", SqlValue = searchText},
                new Param{ SqlDbType = SqlDbType.NVarChar, ParamName="@ORDER_BY", SqlValue = orderText},
                new Param{ SqlDbType = SqlDbType.Structured, ParamName="@INT_ARRAY", SqlValue = rowDatatable}
            };

            List<LeaveTypePendingGetDto> absenceRowInfoList = new List<LeaveTypePendingGetDto>();
            int totalRecords = 0;
            int filteredRecords = 0;

            string? userId = _iconfiguration.GetConnectionString("DefaultConnection")?.Split(';')
                .FirstOrDefault(part => part.Trim().StartsWith("user id", StringComparison.OrdinalIgnoreCase))?
                .Split('=')[1]?.Trim();

            var userDataType = "[" + userId + "].IntArrayTableType";

            using (var reader = await _iBaseDataAccess.ExecuteReaderAsync(query, @params, null, userDataType))
            {
                var countReadFlag = false;

                while (reader.Read())
                {
                    var absenceRowInfo = new LeaveTypePendingGetDto
                    {
                        RecordID = !reader.IsDBNull(0) ? reader.GetInt32(0) : 0,
                        LeaveTypeCode = !reader.IsDBNull(1) ? reader.GetString(1) : string.Empty,
                    };
                    absenceRowInfoList.Add(absenceRowInfo);
                    if (!countReadFlag)
                    {
                        totalRecords = reader.GetInt32(2);
                        filteredRecords = reader.GetInt32(3);
                        countReadFlag = true;
                    }
                }
            }
            return (totalRecords, filteredRecords, absenceRowInfoList);
        }

        public async Task<DataTable> CreateIntArrayParameter(int[] idArray)
        {
            DataTable table = new DataTable();
            table.Columns.Add("ID", typeof(int));

            foreach (int id in idArray)
            {
                table.Rows.Add(id);
            }

            return table;
        }

        public async Task<IList<WizardQuestionDto>> GetWizardQuestionDetails(string subsection)
        {
            string query = "EXEC [SP_IA_GET_WZRD_QUESTIONS] @SUBSECTION_NAME";
            List<Param> @params = new List<Param>()
            {
                new Param{ SqlDbType = SqlDbType.VarChar, ParamName="@SUBSECTION_NAME", SqlValue = subsection}
            };

            var questionList = new List<WizardQuestionDto>();
            using (var reader = await _iBaseDataAccess.ExecuteReaderAsync(query, @params))
            {

                while (reader.Read())
                {
                    var question = new WizardQuestionDto
                    {
                        Question_No = reader.GetInt32(0),
                        Question_Statement = reader.GetString(1),
                        Question_Type = reader.GetString(2),
                    };
                    questionList.Add(question);
                }
            }
            return questionList;
        }

        public async Task<int> InsertAndGetLastInsertedId(int columnNumber, string responseText, string subsectionName)
        {
            string query = subsectionName switch
            {
                "Short Leave" => "EXEC [SP_IA_INSRT_ND_GET_ID_SHRT_LEA] @COLUMN_NO, @DATA",
                "Statutory Leave" => "EXEC [SP_IA_INSRT_ND_GET_ID_STAT_LEA] @COLUMN_NO, @DATA",
                _ => "Default",
            };

            //string query = "EXEC [SP_IA_INSRT_ND_GET_ID_SHRT_LEA] @COLUMN_NO, @DATA";
            List<Param> @params = new List<Param>()
            {
                new Param{ SqlDbType = SqlDbType.Int, ParamName="@COLUMN_NO", SqlValue = columnNumber},
                new Param{ SqlDbType = SqlDbType.NVarChar, ParamName="@DATA", SqlValue = responseText}
            };

            var id = 0;
            using (var reader = await _iBaseDataAccess.ExecuteReaderAsync(query, @params))
            {
                while (reader.Read())
                {
                    id = reader.GetInt32(0);
                }
            }
            return id;
        }

        public async Task InsertIntoDraftTable(string userID, object tableRowId, bool isDraft, DateTime dateValue, string subsectionName, bool isApproved)
        {
            string query = subsectionName switch
            {
                "Short Leave" => "EXEC [SP_IA_INSRT_SHRT_LEA_DRAFT] @USER_ID, @TABLE_RECORD_ID, @IS_DRAFT, @DATE_TIME, @IS_APPROVED",
                "Statutory Leave" => "EXEC [SP_IA_INSRT_STAT_LEA_DRAFT] @USER_ID, @TABLE_RECORD_ID, @IS_DRAFT, @DATE_TIME,@IS_APPROVED",
                _ => "Default",
            };

            //string query = "EXEC [SP_IA_INSRT_CMN_CNFG_DRAFT] @USER_ID, @TABLE_RECORD_ID, @IS_DRAFT, @DATE_TIME";
            List<Param> @params = new List<Param>()
            {
                new Param{ SqlDbType = SqlDbType.NVarChar, ParamName="@USER_ID", SqlValue = userID},
                new Param{ SqlDbType = SqlDbType.Int, ParamName="@TABLE_RECORD_ID", SqlValue = tableRowId},
                new Param{ SqlDbType = SqlDbType.Bit, ParamName="@IS_DRAFT", SqlValue = isDraft},
                new Param{ SqlDbType = SqlDbType.DateTime, ParamName="@DATE_TIME", SqlValue = dateValue},
                new Param{ SqlDbType = SqlDbType.Bit, ParamName="@IS_APPROVED", SqlValue = isApproved}
            };

            await _iBaseDataAccess.ExecuteQueryAsync(query, @params);
        }

        public async Task UpdateNumericColumnByRowId(int recordId, int columnNo, decimal response, string? subsectionName)
        {
            string query = subsectionName switch
            {
                "Short Leave" => "EXEC [SP_IA_UPDT_SHRT_WIZ_UPLD_NUM] @ID, @COLUMN_NO, @DATA",
                "Statutory Leave" => "EXEC [SP_IA_UPDT_STAT_WIZ_UPLD_NUM] @ID, @COLUMN_NO, @DATA",
                _ => "Default",
            };
            List<Param> @params = new List<Param>()
            {
                new Param{ SqlDbType = SqlDbType.Int, ParamName="@ID", SqlValue = recordId},
                new Param{ SqlDbType = SqlDbType.Int, ParamName="@COLUMN_NO", SqlValue = columnNo},
            };


            if (response == -1)
            {
                @params.Add(new Param { SqlDbType = SqlDbType.Decimal, ParamName = "@DATA", SqlValue = DBNull.Value });
            }
            else
            {
                @params.Add(new Param { SqlDbType = SqlDbType.Decimal, ParamName = "@DATA", SqlValue = response });
            }

            await _iBaseDataAccess.ExecuteQueryAsync(query, @params);
        }

        public async Task UpdateOptionsColumnByRowId(int recordId, int columnNo, bool response, string subsectionName)
        {
            string query = subsectionName switch
            {
                "Short Leave" => "EXEC [SP_IA_UPDT_SHRT_WIZ_UPLD_OPT] @ID, @COLUMN_NO, @DATA",
                "Statutory Leave" => "EXEC [SP_IA_UPDT_STAT_WIZ_UPLD_OPT] @ID, @COLUMN_NO, @DATA",
                _ => "Default",
            };
            List<Param> @params = new List<Param>()
            {
                new Param{ SqlDbType = SqlDbType.Int, ParamName="@ID", SqlValue = recordId},
                new Param{ SqlDbType = SqlDbType.Int, ParamName="@COLUMN_NO", SqlValue = columnNo},
                new Param{ SqlDbType = SqlDbType.Bit, ParamName="@DATA", SqlValue = response}
            };
            await _iBaseDataAccess.ExecuteQueryAsync(query, @params);
        }

        public async Task UpdateTextColumnByRowId(int recordId, int columnNo, string? response, string subsectionName)
        {
            string query = subsectionName switch
            {
                "Short Leave" => "EXEC [SP_IA_UPDT_SHRT_WIZ_UPLD_TXT] @ID, @COLUMN_NO, @DATA",
                "Statutory Leave" => "EXEC [SP_IA_UPDT_STAT_WIZ_UPLD_TXT] @ID, @COLUMN_NO, @DATA",
                _ => "Default",
            };
            //string query = "EXEC [SP_IA_UPDT_ABSNC_WIZ_UPLD_TXT] @ID, @COLUMN_NO, @DATA";
            List<Param> @params = new List<Param>()
            {
                new Param{ SqlDbType = SqlDbType.Int, ParamName="@ID", SqlValue = recordId},
                new Param{ SqlDbType = SqlDbType.Int, ParamName="@COLUMN_NO", SqlValue = columnNo},
            };

            if (string.IsNullOrEmpty(response))
            {
                @params.Add(new Param { SqlDbType = SqlDbType.NVarChar, ParamName = "@DATA", SqlValue = DBNull.Value });
            }
            else
            {
                @params.Add(new Param { SqlDbType = SqlDbType.NVarChar, ParamName = "@DATA", SqlValue = response });
            }
            await _iBaseDataAccess.ExecuteQueryAsync(query, @params);
        }

        public async Task UpdateDraftStatusForAbsence(int tableRowId, string subsectionName)
        {
            string query = subsectionName switch
            {
                "Short Leave" => "EXEC [SP_IA_UPDT_SHRT_LEA_DRFT] @ID",
                "Statutory Leave" => "EXEC [SP_IA_UPDT_STAT_LEA_DRFT] @ID",
                _ => "Default",
            };
            List<Param> @params = new List<Param>()
            {
                new Param{ SqlDbType = SqlDbType.Int, ParamName="@ID", SqlValue = tableRowId}
            };
            await _iBaseDataAccess.ExecuteQueryAsync(query, @params);
        }

        public async Task<IList<int>> GetAbsenceWizardPendingOptionsRowNumber(string subsectionName)
        {
            string query = subsectionName switch
            {
                "Short Leave" => "EXEC [SP_IA_GET_PNDNG_ID_FR_SHRT_LEA]",
                "Statutory Leave" => "EXEC [SP_IA_GET_PNDNG_ID_FR_STAT_LEA]",
                _ => "Default",
            };

            var idList = new List<int>();
            using (var reader = await _iBaseDataAccess.ExecuteReaderAsync(query))
            {
                while (reader.Read())
                {
                    idList.Add(reader.GetInt32(0));
                }
            }
            return idList;
        }

        public async Task UpdateDraftAndApprovalStatusForAbsence(int[] rowIDs, string subsection)
        {
            string query = subsection switch
            {
                "Short Leave" => "EXEC [SP_IA_UPDAT_SHRT_LEA_STS]  @INT_ARRAY",
                "Statutory Leave" => "EXEC [SP_IA_UPDAT_STAT_LEA_STS] @INT_ARRAY",
                _ => "Default",
            };
            var rowDatatable = await CreateIntArrayParameter(rowIDs);

            List<Param> @params = new List<Param>()
            {
                new Param{ SqlDbType = SqlDbType.Structured, ParamName="@INT_ARRAY", SqlValue = rowDatatable}
            };

            //string? userId = _iconfiguration.GetConnectionString("DefaultConnection")?.Split(';')
            //    .FirstOrDefault(part => part.Trim().StartsWith("user id", StringComparison.OrdinalIgnoreCase))?
            //    .Split('=')[1]?.Trim();

            string? userId = await _iBaseDataAccess.GetDbUserName();

            var userDataType = "[" + userId + "].IntArrayTableType";

            await _iBaseDataAccess.ExecuteQueryAsync(query, @params, 30, userDataType);
        }

        public async Task DeleteDataFromDraftAndMainTable(string subsectionName)
        {
            string query = subsectionName switch
            {
                "Short Leave" => "EXEC [SP_IA_DELT_SHRT_LEA_DATA]",
                "Statutory Leave" => "EXEC [SP_IA_DELT_STAT_LEA_DATA]",
                _ => "Default",
            };

            await _iBaseDataAccess.ExecuteQueryAsync(query);
        }

        public async Task DeleteDataFromDependentTable(string subsectionName)
        {
            string query = "EXEC [SP_IA_DELT_SHRT_DEP_DATA]";

            await _iBaseDataAccess.ExecuteQueryAsync(query);
        }

        public async Task<AbsenceWizardDrftStatusDto> GetIsDraftStatus(string userId, string subsectionName)
        {
            string query = subsectionName switch
            {
                "Short Leave" => "EXEC [SP_IA_GET_DRFT_STS_SHRT_LEA] @USER_ID",
                "Statutory Leave" => "EXEC [SP_IA_GET_DRFT_STS_STAT_LEA] @USER_ID",
                _ => "Default",
            };
            List<Param> @params = new List<Param>()
            {
                new Param{ SqlDbType = SqlDbType.NVarChar, ParamName="@USER_ID", SqlValue = userId}
            };

            var drftModel = new AbsenceWizardDrftStatusDto();
            using (var reader = await _iBaseDataAccess.ExecuteReaderAsync(query, @params))
            {
                while (reader.Read())
                {
                    drftModel.IsDraft = reader.GetBoolean(0);
                    drftModel.TableRowId = reader.GetInt32(1);
                }
            }
            return drftModel;
        }

        public async Task<bool> CheckForDuplicateLeaveTypeStatutoryLeave(string responseText, string userId)
        {
            string query = "EXEC [SP_IA_CHK_DUP_STAT_LEA_TYPE] @LEAVE_TYPE,@USER_ID ";

            List<Param> @params = new List<Param>()
            {
                new Param{ SqlDbType = SqlDbType.NVarChar, ParamName="@LEAVE_TYPE", SqlValue = responseText},
                new Param{ SqlDbType = SqlDbType.NVarChar, ParamName="@USER_ID", SqlValue = userId}
            };

            var response = false;
            using (var reader = await _iBaseDataAccess.ExecuteReaderAsync(query, @params))
            {
                while (reader.Read())
                {
                    response = !reader.IsDBNull(0);
                }
            }
            return response;
        }

        public async Task<bool> CheckForDuplicateLeaveTypeShortLeave(string responseText, string userId)
        {
            string query = "EXEC [SP_IA_CHK_DUP_SHRT_LEA_TYPE] @LEAVE_TYPE,@USER_ID ";

            List<Param> @params = new List<Param>()
            {
                new Param{ SqlDbType = SqlDbType.NVarChar, ParamName="@LEAVE_TYPE", SqlValue = responseText},
                new Param{ SqlDbType = SqlDbType.NVarChar, ParamName="@USER_ID", SqlValue = userId}
            };

            var response = false;
            using (var reader = await _iBaseDataAccess.ExecuteReaderAsync(query, @params))
            {
                while (reader.Read())
                {
                    response = !reader.IsDBNull(0);
                }
            }
            return response;
        }

        public async Task<bool> CheckForDuplicateLeaveTypeStatutoryLeaveFirst(string responseText)
        {
            string query = "EXEC [SP_IA_CHK_DUP_STAT_LEA_FRST] @LEAVE_TYPE";

            List<Param> @params = new List<Param>()
            {
                new Param{ SqlDbType = SqlDbType.NVarChar, ParamName="@LEAVE_TYPE", SqlValue = responseText},
            };

            var response = false;
            using (var reader = await _iBaseDataAccess.ExecuteReaderAsync(query, @params))
            {
                while (reader.Read())
                {
                    response = !reader.IsDBNull(0);
                }
            }
            return response;
        }

        public async Task<bool> CheckForDuplicateLeaveTypeShortLeaveFirst(string responseText)
        {
            string query = "EXEC [SP_IA_CHK_DUP_SHRT_LEA_FRST] @LEAVE_TYPE";

            List<Param> @params = new List<Param>()
            {
                new Param{ SqlDbType = SqlDbType.NVarChar, ParamName="@LEAVE_TYPE", SqlValue = responseText},
            };

            var response = false;
            using (var reader = await _iBaseDataAccess.ExecuteReaderAsync(query, @params))
            {
                while (reader.Read())
                {
                    response = !reader.IsDBNull(0);
                }
            }
            return response;
        }

        public async Task<bool> checkDuplicateApplySeqForStatutoryLeave(string responseText, string userId)
        {
            string query = "EXEC [SP_IA_CHK_DUP_APPLY_SEQ] @APPLY_SEQUENCE,@USER_ID";

            List<Param> @params = new List<Param>()
            {
                new Param{ SqlDbType = SqlDbType.Int, ParamName="@APPLY_SEQUENCE", SqlValue = responseText},
                new Param{ SqlDbType = SqlDbType.NVarChar, ParamName="@USER_ID", SqlValue = userId}
            };

            var response = false;
            using (var reader = await _iBaseDataAccess.ExecuteReaderAsync(query, @params))
            {
                while (reader.Read())
                {
                    response = !reader.IsDBNull(0);
                }
            }
            return response;
        }

        public async Task<bool> CheckIfSmallerThanPreviousColumnValue(string responseText, int questionNo, int rowId, string subsectionName)
        {
            string query = subsectionName switch
            {
                "Short Leave" => "EXEC [SP_IA_CHK_MAX_VAL_SHRT_LEA] @DATA, @COLUMN_NO, @ROW_ID",
                "Statutory Leave" => "EXEC [SP_IA_CHK_MAX_VAL_STAT_LEA] @DATA, @COLUMN_NO, @ROW_ID",
                _ => "Default",
            };
            List<Param> @params = new List<Param>()
            {
                new Param{SqlDbType = SqlDbType.Decimal, ParamName = "@DATA", SqlValue = decimal.Parse(responseText)},
                new Param{ SqlDbType = SqlDbType.Int, ParamName="@COLUMN_NO", SqlValue = questionNo},
                new Param{ SqlDbType = SqlDbType.Int, ParamName="@ROW_ID", SqlValue = rowId}
            };

            var response = false;
            using (var reader = await _iBaseDataAccess.ExecuteReaderAsync(query, @params))
            {
                while (reader.Read())
                {
                    response = !reader.IsDBNull(0);
                }
            }
            return response;
        }

        public async Task<bool> CheckIfGreaterThanMaximumForShortLeave(string responseText, int questionNo, int rowId)
        {
            string query = "EXEC [SP_IA_CHK_GRT_THN_MX_VAL_SHRT] @DATA, @COLUMN_NO, @ROW_ID";

            List<Param> @params = new List<Param>()
            {
                new Param{SqlDbType = SqlDbType.Decimal, ParamName = "@DATA", SqlValue = decimal.Parse(responseText)},
                new Param{ SqlDbType = SqlDbType.Int, ParamName="@COLUMN_NO", SqlValue = questionNo},
                new Param{ SqlDbType = SqlDbType.Int, ParamName="@ROW_ID", SqlValue = rowId}
            };

            var response = false;
            using (var reader = await _iBaseDataAccess.ExecuteReaderAsync(query, @params))
            {
                while (reader.Read())
                {
                    response = !reader.IsDBNull(0);
                }
            }
            return response;
        }

        public async Task<bool> CheckIfLessThanMaximumForShortLeave(string responseText, int questionNo, int rowId)
        {
            string query = "EXEC [SP_IA_CHK_LES_THN_MX_VAL_SHRT] @DATA, @COLUMN_NO, @ROW_ID";

            List<Param> @params = new List<Param>()
            {
                new Param{SqlDbType = SqlDbType.Decimal, ParamName = "@DATA", SqlValue = decimal.Parse(responseText)},
                new Param{ SqlDbType = SqlDbType.Int, ParamName="@COLUMN_NO", SqlValue = questionNo},
                new Param{ SqlDbType = SqlDbType.Int, ParamName="@ROW_ID", SqlValue = rowId}
            };

            var response = false;
            using (var reader = await _iBaseDataAccess.ExecuteReaderAsync(query, @params))
            {
                while (reader.Read())
                {
                    response = !reader.IsDBNull(0);
                }
            }
            return response;
        }

        public async Task<bool> CheckIfLessThanMinimumForShortLeave(string responseText, int questionNo, int rowId)
        {
            string query = "EXEC [SP_IA_CHK_LES_THN_MN_VAL_SHRT] @DATA, @COLUMN_NO, @ROW_ID";

            List<Param> @params = new List<Param>()
            {
                new Param{SqlDbType = SqlDbType.Decimal, ParamName = "@DATA", SqlValue = decimal.Parse(responseText)},
                new Param{ SqlDbType = SqlDbType.Int, ParamName="@COLUMN_NO", SqlValue = questionNo},   
                new Param{ SqlDbType = SqlDbType.Int, ParamName="@ROW_ID", SqlValue = rowId}
            };

            var response = false;
            using (var reader = await _iBaseDataAccess.ExecuteReaderAsync(query, @params))
            {
                while (reader.Read())
                {
                    response = !reader.IsDBNull(0);
                }
            }
            return response;
        }

        public async Task<bool> CheckIfZeroOrNullValue(int questionNo, int rowId, string subsectionName)
        {
            string query = subsectionName switch
            {
                "Short Leave" => "EXEC [SP_IA_CHK_NULL_SHRT_LEA] @COLUMN_NO, @ROW_ID",
                "Statutory Leave" => "EXEC [SP_IA_CHK_NULL_STAT_LEA] @COLUMN_NO, @ROW_ID",
                _ => "Default",
            };
            List<Param> @params = new List<Param>()
            {
                new Param{ SqlDbType = SqlDbType.Int, ParamName="@COLUMN_NO", SqlValue = questionNo},
                new Param{ SqlDbType = SqlDbType.Int, ParamName="@ROW_ID", SqlValue = rowId}
            };

            var response = false;
            using (var reader = await _iBaseDataAccess.ExecuteReaderAsync(query, @params))
            {
                while (reader.Read())
                {
                    response = !reader.IsDBNull(0);
                }
            }
            return response;
        }

        public async Task<bool> CheckIfPreviousColumnIsNotNullForShortLeave(int questionNo, int rowId)
        {
            string query = "EXEC [SP_IA_CHK_PRV_COL_NULL] @COLUMN_NO, @ROW_ID";

            List<Param> @params = new List<Param>()
            {
                new Param{ SqlDbType = SqlDbType.Int, ParamName="@COLUMN_NO", SqlValue = questionNo},
                new Param{ SqlDbType = SqlDbType.Int, ParamName="@ROW_ID", SqlValue = rowId}
            };

            var response = false;
            using (var reader = await _iBaseDataAccess.ExecuteReaderAsync(query, @params))
            {
                while (reader.Read())
                {
                    response = !reader.IsDBNull(0);
                }
            }
            return response;
        }

        public async Task<bool> CheckIfColumnBeforeTheLastIsNotNullForShortLeave(int questionNo, int rowId)
        {
            string query = "EXEC [SP_IA_CHK_PRV_COL_NULL] @COLUMN_NO, @ROW_ID";

            List<Param> @params = new List<Param>()
            {
                new Param{ SqlDbType = SqlDbType.Int, ParamName="@COLUMN_NO", SqlValue = questionNo-1},
                new Param{ SqlDbType = SqlDbType.Int, ParamName="@ROW_ID", SqlValue = rowId}
            };

            var response = false;
            using (var reader = await _iBaseDataAccess.ExecuteReaderAsync(query, @params))
            {
                while (reader.Read())
                {
                    response = !reader.IsDBNull(0);
                }
            }
            return response;
        }

        public async Task<bool> CheckIfPreviousColumnIsNotNullForStatLeave(int questionNo, int rowId)
        {
            string query = "EXEC [SP_IA_CHK_PRV_COL_NULL_STAT] @COLUMN_NO, @ROW_ID";

            List<Param> @params = new List<Param>()
            {
                new Param{ SqlDbType = SqlDbType.Int, ParamName="@COLUMN_NO", SqlValue = questionNo},
                new Param{ SqlDbType = SqlDbType.Int, ParamName="@ROW_ID", SqlValue = rowId}
            };

            var response = false;
            using (var reader = await _iBaseDataAccess.ExecuteReaderAsync(query, @params))
            {
                while (reader.Read())
                {
                    response = !reader.IsDBNull(0);
                }
            }
            return response;
        }

        public async Task<bool> checkIfPreviousTwoColumnsAreNotNullForShortLeave(int questionNo, int rowId)
        {
            string query = "EXEC [SP_IA_CHK_PRV_TWO_COL_NULL] @COLUMN_NO, @ROW_ID";

            List<Param> @params = new List<Param>()
            {
                new Param{ SqlDbType = SqlDbType.Int, ParamName="@COLUMN_NO", SqlValue = questionNo},
                new Param{ SqlDbType = SqlDbType.Int, ParamName="@ROW_ID", SqlValue = rowId}
            };

            var response = false;
            using (var reader = await _iBaseDataAccess.ExecuteReaderAsync(query, @params))
            {
                while (reader.Read())
                {
                    response = !reader.IsDBNull(0);
                }
            }
            return response;
        }

        public async Task DeleteSelectedRowsFromDraftAndMainTable(List<int> rowIds, string subsectionName)
        {
            string query = subsectionName switch
            {
                "Short Leave" => "EXEC [SP_IA_DEL_SHRT_MLTPL_ROW]  @INT_ARRAY",
                "Statutory Leave" => "EXEC [SP_IA_DEL_STAT_MLTPL_ROW] @INT_ARRAY",
                _ => "Default",
            };
            var rowDatatable = await CreateIntArrayParameter(rowIds.ToArray());

            List<Param> @params = new List<Param>()
            {
                new Param{ SqlDbType = SqlDbType.Structured, ParamName="@INT_ARRAY", SqlValue = rowDatatable}
            };

            string? userId = _iconfiguration.GetConnectionString("DefaultConnection")?.Split(';')
                .FirstOrDefault(part => part.Trim().StartsWith("user id", StringComparison.OrdinalIgnoreCase))?
                .Split('=')[1]?.Trim();

            var userDataType = "[" + userId + "].IntArrayTableType";

            await _iBaseDataAccess.ExecuteQueryAsync(query, @params, 30, userDataType);
        }

        public async Task<bool> CheckExistingLeaveTypeShortLeave(string responseText)
        {
            string query = "EXEC [SP_IA_CHK_EXST_SHRT_LEA_TYPE] @LEAVE_TYPE";

            List<Param> @params = new List<Param>()
            {
                new Param{ SqlDbType = SqlDbType.NVarChar, ParamName="@LEAVE_TYPE", SqlValue = responseText},
            };

            var response = false;
            using (var reader = await _iBaseDataAccess.ExecuteReaderAsync(query, @params))
            {
                while (reader.Read())
                {
                    response = !reader.IsDBNull(0);
                }
            }
            return response;
        }

        public async Task<bool> CheckExistingLeaveTypeStatutoryLeave(string responseText)
        {
            string query = "EXEC [SP_IA_CHK_EXST_STAT_LEA_TYPE] @LEAVE_TYPE";

            List<Param> @params = new List<Param>()
            {
                new Param{ SqlDbType = SqlDbType.NVarChar, ParamName="@LEAVE_TYPE", SqlValue = responseText},
            };

            var response = false;
            using (var reader = await _iBaseDataAccess.ExecuteReaderAsync(query, @params))
            {
                while (reader.Read())
                {
                    response = !reader.IsDBNull(0);
                }
            }
            return response;
        }
    }
}