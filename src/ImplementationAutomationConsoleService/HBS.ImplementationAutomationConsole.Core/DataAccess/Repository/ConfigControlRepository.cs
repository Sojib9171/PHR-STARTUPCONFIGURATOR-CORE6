using HBS.ImplementationAutomationConsole.Core.DataAccess.Data;
using HBS.ImplementationAutomationConsole.Core.DataAccess.IRepository;
using System.Data;

namespace HBS.ImplementationAutomationConsole.Core.DataAccess.Repository
{
    public class ConfigControlRepository : IConfigControlRepository
    {
        private readonly IBaseDataAccess _iBaseDataAccess;
        public ConfigControlRepository(IBaseDataAccess iBaseDataAccess)
        {
            this._iBaseDataAccess = iBaseDataAccess;
        }

        public async Task<bool> UploadTemplate(string templateId, int moduleId, string subsectionId, string templateName, byte[] templateData, bool isActiveFlg)
        {
            string query = @"EXEC [SP_IA_INSERT_INTO_IA_TEMPLATES] @TEMPLATE_ID ,@MODULE_ID, @SUBSECTION_ID, @TEMPLATE_NAME, @TEMPLATE_DATA, @IS_ACTIVE_FLG";
            List<Param> @params = new List<Param>()
            {
                new Param{ SqlDbType = SqlDbType.NVarChar, ParamName="@TEMPLATE_ID", SqlValue = templateId},
                new Param{ SqlDbType = SqlDbType.Int, ParamName="@MODULE_ID", SqlValue = moduleId},
                new Param{ SqlDbType = SqlDbType.NVarChar, ParamName="@SUBSECTION_ID", SqlValue = subsectionId},
                new Param{ SqlDbType = SqlDbType.NVarChar, ParamName="@TEMPLATE_NAME", SqlValue = templateName},
                new Param{ SqlDbType = SqlDbType.VarBinary, ParamName="@TEMPLATE_DATA", SqlValue = templateData},
                new Param{ SqlDbType = SqlDbType.Bit, ParamName="@IS_ACTIVE_FLG", SqlValue = isActiveFlg},
            };
            return await _iBaseDataAccess.GetSingleInt(query, @params) == 1;
        }

        public async Task<List<string>> GetTemplateData(string template_Id)
        {
            string query = @"EXEC [SP_IA_GET_TEMPLATE_DATA] @TEMPLATE_ID";
            List<Param> @params = new List<Param>()
            {
                new Param{ SqlDbType = SqlDbType.NVarChar, ParamName="@TEMPLATE_ID", SqlValue = template_Id},
            };

            var list = new List<string>();
            using (var reader = await _iBaseDataAccess.ExecuteReaderAsync(query, @params))
            {
                while (reader.Read())
                {
                    var excelData = (byte[])reader["TEMPLATE_DATA"];
                    var templateName = (string)reader["TEMPLATE_NAME"];
                    var base64Data = Convert.ToBase64String(excelData);
                    list.Add(base64Data);
                    list.Add(templateName);
                }
            }
            return list;
        }

        public async Task<(byte[], string)> GetTemplateByteArray(string template_Id)
        {
            try
            {
                string query = @"EXEC [SP_IA_GET_TEMPLATE_DATA] @TEMPLATE_ID";
                List<Param> @params = new List<Param>()
            {
                new Param{ SqlDbType = SqlDbType.NVarChar, ParamName="@TEMPLATE_ID", SqlValue = template_Id},
            };
                var list = new List<string>();
                var excelData = new byte[] { };
                var templateName = string.Empty;
                using (var reader = await _iBaseDataAccess.ExecuteReaderAsync(query, @params))
                {
                    while (reader.Read())
                    {
                        excelData = (byte[])reader["TEMPLATE_DATA"];
                        templateName = (string)reader["TEMPLATE_NAME"];
                    }
                }
                return (excelData, templateName);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> GetTemplateIdFromSubsectionName(string subsectionName)
        {
            string query = @"EXEC [SP_IA_GET_TEMPLATE_ID] @SUBSECTION_NAME";

            List<Param> @params = new List<Param>()
            {
                new Param{ SqlDbType = SqlDbType.VarChar, ParamName="@SUBSECTION_NAME", SqlValue = subsectionName}
            };

            string templateID = string.Empty;
            using (var reader = await _iBaseDataAccess.ExecuteReaderAsync(query, @params))
            {
                while (reader.Read())
                {
                    templateID = reader.GetString(0);
                }
            }
            return templateID;
        }

        public async Task<List<string>> GetHierarchyNamesAsList()
        {
            string query = @"EXEC [SP_IA_GET_HIERARCHY_NAMES]";
            var hierarchyNames = new List<string>();

            using (var reader = await _iBaseDataAccess.ExecuteReaderAsync(query))
            {
                while (reader.Read())
                {
                    hierarchyNames.Add(reader.GetString(0));
                }
            }
            return hierarchyNames;
        }
    }
}