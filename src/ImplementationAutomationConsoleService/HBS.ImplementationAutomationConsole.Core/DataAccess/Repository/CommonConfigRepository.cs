using HBS.ImplementationAutomationConsole.Core.DataAccess.Data;
using HBS.ImplementationAutomationConsole.Core.DataAccess.IRepository;
using HBS.ImplementationAutomationConsole.Core.Models;
using System.Data;

namespace HBS.ImplementationAutomationConsole.Core.DataAccess.Repository
{
    public class CommonConfigRepository : ICommonConfigRepository
    {
        private readonly IBaseDataAccess _iBaseDataAccess;
        public CommonConfigRepository(IBaseDataAccess iBaseDataAccess)
        {
            this._iBaseDataAccess = iBaseDataAccess;
        }

        public async Task<IList<CommonConfigRadioOptionDto>> GetRadioOptionsWithQuestionNo(int questionNo)
        {
            string query = "EXEC [SP_IA_GET_CMN_CNFG_RDIO_OPTNS] @QUESTION_NO";
            List<Param> @params = new List<Param>()
            {
                new Param{ SqlDbType = SqlDbType.Int, ParamName="@QUESTION_NO", SqlValue = questionNo},
            };

            var optionList = new List<CommonConfigRadioOptionDto>();
            using (var reader = await _iBaseDataAccess.ExecuteReaderAsync(query, @params))
            {
                while (reader.Read())
                {
                    var option = new CommonConfigRadioOptionDto()
                    {
                        Option_Label = reader.GetString(0),
                        Option_Value = reader.GetString(1),
                    };
                    optionList.Add(option);
                }
            }
            return optionList;
        }

        public async Task<IList<WizardQuestionDto>> GetCommonConfigQuestionDetails()
        {
            string query = "EXEC [SP_IA_GET_COMMN_CONFG_QSTIONS]";
            var questionList = new List<WizardQuestionDto>();
            using (var reader = await _iBaseDataAccess.ExecuteReaderAsync(query))
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

        public async Task<int> InsertAndGetLastInsertedId(int columnNumber, string responseText)
        {
            string query = "EXEC [SP_IA_INSRT_ND_GET_ID_CMN_CNFG] @COLUMN_NO, @DATA";
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

        public async Task InsertIntoDraftTable(string userID, int tableRowId, bool isDraft, DateTime dateValue)
        {
            string query = "EXEC [SP_IA_INSRT_CMN_CNFG_DRAFT] @USER_ID, @TABLE_RECORD_ID, @IS_DRAFT, @DATE_TIME";
            List<Param> @params = new List<Param>()
            {
                new Param{ SqlDbType = SqlDbType.NVarChar, ParamName="@USER_ID", SqlValue = userID},
                new Param{ SqlDbType = SqlDbType.Int, ParamName="@TABLE_RECORD_ID", SqlValue = tableRowId},
                new Param{ SqlDbType = SqlDbType.Bit, ParamName="@IS_DRAFT", SqlValue = isDraft},
                new Param{ SqlDbType = SqlDbType.DateTime, ParamName="@DATE_TIME", SqlValue = dateValue}
            };

            await _iBaseDataAccess.ExecuteQueryAsync(query, @params);
        }

        public async Task UpdateTextColumnByRowId(int rowId, int columnNo, string responseText)
        {
            string query = "EXEC [SP_IA_UPDT_CMN_CNFG_UPLD_TXT] @ID, @COLUMN_NO, @DATA";
            List<Param> @params = new List<Param>()
            {
                new Param{ SqlDbType = SqlDbType.Int, ParamName="@ID", SqlValue = rowId},
                new Param{ SqlDbType = SqlDbType.Int, ParamName="@COLUMN_NO", SqlValue = columnNo},
            };

            if(string.IsNullOrEmpty(responseText))
            {
                @params.Add(new Param { SqlDbType = SqlDbType.NVarChar, ParamName = "@DATA", SqlValue = DBNull.Value });
            }
            else
            {
                @params.Add(new Param { SqlDbType = SqlDbType.NVarChar, ParamName = "@DATA", SqlValue = responseText });
            }
            await _iBaseDataAccess.ExecuteQueryAsync(query, @params);
        }

        public async Task UpdateImageColumnByRowId(int rowId, int columnNo, byte[] bytes)
        {
            string query = "EXEC [SP_IA_UPDT_CMN_CNFG_UPLD_IMG] @ID, @COLUMN_NO, @DATA";
            List<Param> @params = new List<Param>()
            {
                new Param{ SqlDbType = SqlDbType.Int, ParamName="@ID", SqlValue = rowId},
                new Param{ SqlDbType = SqlDbType.Int, ParamName="@COLUMN_NO", SqlValue = columnNo},
                new Param{ SqlDbType = SqlDbType.VarBinary, ParamName="@DATA", SqlValue = bytes}
            };
            await _iBaseDataAccess.ExecuteQueryAsync(query, @params);
        }

        public async Task UpdateOptionColumnByRowId(int rowId, int columnNo, bool response)
        {
            string query = "EXEC [SP_IA_UPDT_CMN_CNFG_UPLD_OPTN] @ID, @COLUMN_NO, @DATA";
            List<Param> @params = new List<Param>()
            {
                new Param{ SqlDbType = SqlDbType.Int, ParamName="@ID", SqlValue = rowId},
                new Param{ SqlDbType = SqlDbType.Int, ParamName="@COLUMN_NO", SqlValue = columnNo},
                new Param{ SqlDbType = SqlDbType.Bit, ParamName="@DATA", SqlValue = response}
            };
            await _iBaseDataAccess.ExecuteQueryAsync(query, @params);
        }

        public async Task<List<object>> GetCommonConfigResponsedByRowId(int rowId)
        {

            string query = "EXEC [SP_IA_GET_CMN_CNFG_RESPONSE] @ID";
            List<Param> @params = new List<Param>()
            {
                new Param{ SqlDbType = SqlDbType.Int, ParamName="@ID", SqlValue = rowId}
            };

            var response = new List<object>();
            using (var reader = await _iBaseDataAccess.ExecuteReaderAsync(query, @params))
            {
                while (reader.Read())
                {
                    response.Add(!reader.IsDBNull(0) ? reader.GetString(0) : string.Empty);
                    response.Add(!reader.IsDBNull(1) ? reader.GetString(1) : string.Empty);
                    response.Add(!reader.IsDBNull(2) ? (reader.GetBoolean(2) ? 1 : 0) : -1);
                    response.Add(!reader.IsDBNull(3) ? reader.GetString(3) : string.Empty);
                    response.Add(!reader.IsDBNull(4) ? (reader.GetBoolean(4) ? 1 : 0) : -1);
                    response.Add(!reader.IsDBNull(5) ? reader.GetString(5) : string.Empty);
                }
            }
            return response;
        }

        public async Task UpdateDraftStatusForCommonConfig(int rowId)
        {
            string query = "EXEC [SP_IA_UPDT_CMN_CNFG_DRFT] @ID";
            List<Param> @params = new List<Param>()
            {
                new Param{ SqlDbType = SqlDbType.Int, ParamName="@ID", SqlValue = rowId}
            };
            await _iBaseDataAccess.ExecuteQueryAsync(query, @params);
        }

        public async Task InsertCommonConfigInfo(CommonConfigPostDto commonConfigPostDto)
        {
            string query = @"EXEC [SP_IA_INSRT_IN_CMN_CNFG_INFO] @APPROVAL_STATUS, @APPROVAL_BY, @APPROVAL_COMMENT, @APPROVAL_SIGNATURE, @APPROVAL_DATE,@SIGNOFF_DATA, @TABLE_ROW_ID";
            List<Param> @params = new List<Param>()
            {
                new Param{ SqlDbType = SqlDbType.NVarChar, ParamName="@APPROVAL_STATUS", SqlValue = commonConfigPostDto.ApprovalStatus},
                new Param{ SqlDbType = SqlDbType.NVarChar, ParamName="@APPROVAL_BY", SqlValue = commonConfigPostDto.Name},
                new Param{ SqlDbType = SqlDbType.NVarChar, ParamName="@APPROVAL_COMMENT", SqlValue = commonConfigPostDto.Comment},
                new Param{ SqlDbType = SqlDbType.NVarChar, ParamName="@APPROVAL_SIGNATURE", SqlValue = commonConfigPostDto.UserName},
                new Param{ SqlDbType = SqlDbType.DateTime, ParamName="@APPROVAL_DATE", SqlValue = commonConfigPostDto.ApprovalDate},
                new Param{ SqlDbType = SqlDbType.VarBinary, ParamName="@SIGNOFF_DATA", SqlValue = commonConfigPostDto.SignOffPdfData},
                new Param{ SqlDbType = SqlDbType.Int, ParamName="@TABLE_ROW_ID", SqlValue = commonConfigPostDto.TableRowId}
            };
            await _iBaseDataAccess.ExecuteQueryAsync(query, @params);
        }

        public async Task DeleteFromDraftTable(int tableRowId)
        {
            string query = "EXEC [SP_IA_DLT_CMN_CNFG_DRFT] @TABLE_RECORD_ID";
            List<Param> @params = new List<Param>()
            {
                new Param{ SqlDbType = SqlDbType.Int, ParamName="@TABLE_RECORD_ID", SqlValue = tableRowId}
            };
            await _iBaseDataAccess.ExecuteQueryAsync(query, @params);
        }

        public async Task DeleteFromMainTable(int tableRowId)
        {
            string query = "EXEC [SP_IA_DLT_CMN_CNFG_UPLD] @ID";
            List<Param> @params = new List<Param>()
            {
                new Param{ SqlDbType = SqlDbType.Int, ParamName="@ID", SqlValue = tableRowId}
            };
            await _iBaseDataAccess.ExecuteQueryAsync(query, @params);
        }

        public async Task<CommonConfigDrftStatusDto> GetIsDraftStatus(string userId)
        {
            string query = "EXEC [SP_IA_GET_DRFT_FRM_USER_ID] @USER_ID";
            List<Param> @params = new List<Param>()
            {
                new Param{ SqlDbType = SqlDbType.NVarChar, ParamName="@USER_ID", SqlValue = userId}
            };

            var drftModel = new CommonConfigDrftStatusDto() ;
            using (var reader = await _iBaseDataAccess.ExecuteReaderAsync(query, @params))
            {
                while (reader.Read())
                {
                    drftModel.IsDraft = reader.GetBoolean(0);
                    drftModel.TableRowId=reader.GetInt32(1);
                }
            }
            return drftModel;
        }

        public async Task UpdateImageNameColumnByRowId(int recordId, string? logoImageName)
        {
            string query = "EXEC [SP_IA_UPDT_CMN_CNFG_IMG_NAME] @ID, @LOGO_IMAGE_NAME";
            List<Param> @params = new List<Param>()
            {
                new Param{ SqlDbType = SqlDbType.Int, ParamName="@ID", SqlValue = recordId},
                new Param{ SqlDbType = SqlDbType.NVarChar, ParamName="@LOGO_IMAGE_NAME", SqlValue = logoImageName},
            };
            await _iBaseDataAccess.ExecuteQueryAsync(query, @params);
        }

        public async Task UpdateMobileImageNameColumnByRowId(int recordId, string? mobileLogoImageName)
        {
            string query = "EXEC [SP_IA_UPDT_CMN_CNFG_MBL_IMG_NM] @ID, @MOBILE_LOGO_IMAGE_NAME";
            List<Param> @params = new List<Param>()
            {
                new Param{ SqlDbType = SqlDbType.Int, ParamName="@ID", SqlValue = recordId},
                new Param{ SqlDbType = SqlDbType.NVarChar, ParamName="@MOBILE_LOGO_IMAGE_NAME", SqlValue = mobileLogoImageName}
            };
            await _iBaseDataAccess.ExecuteQueryAsync(query, @params);
        }

        public async Task<bool> GetCommonConfigApprovalStatus(string userId)
        {
            string query = "EXEC [SP_IA_GET_CMN_CNF_APRV_STS] @USER_ID";
            List<Param> @params = new List<Param>()
            {
                new Param{ SqlDbType = SqlDbType.NVarChar, ParamName="@USER_ID", SqlValue = userId},
            };

            var isApproved = false;
            using (var reader = await _iBaseDataAccess.ExecuteReaderAsync(query, @params))
            {
                while (reader.Read())
                {
                    isApproved = (!reader.IsDBNull(0) ? (reader.GetString(0) == "Approved" ? true : false) : false);
                }
            }
            return isApproved;
        }

        public async Task<(int total, int totalDisplay, IList<CommonConfigHistroyGetDto> records)> GetCommonConfigHistory(string userid, int page, int perPage, string searchText, string sortText)
        {
            string query = @"EXEC [SP_IA_GET_CMN_CNFG_HISTORY] @USER_ID, @PAGE_INDEX, @PAGE_SIZE, @SEARCH_TEXT, @ORDER_BY";

            List<Param> @params = new List<Param>()
            {
                new Param{ SqlDbType = SqlDbType.NVarChar, ParamName="@USER_ID", SqlValue = userid},
                new Param{ SqlDbType = SqlDbType.Int, ParamName="@PAGE_INDEX", SqlValue = page},
                new Param{ SqlDbType = SqlDbType.Int, ParamName="@PAGE_SIZE", SqlValue = perPage},
                new Param{ SqlDbType = SqlDbType.NVarChar, ParamName="@SEARCH_TEXT", SqlValue = searchText},
                new Param{ SqlDbType = SqlDbType.NVarChar, ParamName="@ORDER_BY", SqlValue = sortText}
            };

            IList<CommonConfigHistroyGetDto> vendorDashboardData = new List<CommonConfigHistroyGetDto>();

            int totalRecords = 0;
            int filteredRecords = 0;

            using (var reader = await _iBaseDataAccess.ExecuteReaderAsync(query, @params))
            {
                var countReadFlag = false;
                while (reader.Read())
                {
                    var rowInfo = new CommonConfigHistroyGetDto
                    {
                        RecordID=reader.GetInt32(0),
                        ApprovalDate = reader.GetDateTime(1).Date,
                        ApprovalStatus = reader.GetString(2),
                        Name = reader.GetString(3),
                        ApprovalComment = reader.GetString(4),
                        SignOffPdfData = (byte[])reader.GetValue(5),
                        TableRecordId=reader.GetInt32(6)
                    };
                    vendorDashboardData.Add(rowInfo);
                    if (!countReadFlag)
                    {
                        totalRecords = reader.GetInt32(7);
                        filteredRecords = reader.GetInt32(8);
                        countReadFlag = true;
                    }
                }
            }
            return (totalRecords, filteredRecords, vendorDashboardData);
        }
    }
}