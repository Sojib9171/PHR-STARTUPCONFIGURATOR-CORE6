using HBS.ImplementationAutomationConsole.Core.DataAccess.Data;
using HBS.ImplementationAutomationConsole.Core.DataAccess.IRepository;
using System.Data;

namespace HBS.ImplementationAutomationConsole.Core.DataAccess.Repository
{
    public class ExcelDataRepository : IExcelDataRepository
    {
        private readonly IBaseDataAccess _iBaseDataAccess;
        public ExcelDataRepository(IBaseDataAccess iBaseDataAccess)
        {
            this._iBaseDataAccess = iBaseDataAccess;
        }

        public async Task<List<(string, string)>> GetColumnNames(string template_Id)
        {
            string query = @"EXEC [SP_IA_GET_DB_AND_EXCEL_COLUMNS] @TEMPLATE_ID";

            List<Param> @params = new List<Param>()
            {
                new Param{ SqlDbType = SqlDbType.VarChar, ParamName="@TEMPLATE_ID", SqlValue = template_Id}
            };
            using (var reader = await _iBaseDataAccess.ExecuteReaderAsync(query, @params))
            {
                List<(string, string)> columnMaps = new List<(string, string)>();

                while (reader.Read())
                {
                    (string, string) tuple = (reader.GetString(0), reader.GetString(1));

                    columnMaps.Add(tuple);
                }
                return columnMaps;
            }
        }

        public async Task<List<string>> GetColumnTypes(string template_Id)
        {
            string query = @"EXEC [SP_IA_GET_COLUMN_TYPES] @TEMPLATE_ID";

            List<Param> @params = new List<Param>()
            {
                new Param{ SqlDbType = SqlDbType.VarChar, ParamName="@TEMPLATE_ID", SqlValue = template_Id}
            };
            using (var reader = await _iBaseDataAccess.ExecuteReaderAsync(query, @params))
            {
                List<string> columnTypes = new List<string>();

                while (reader.Read())
                {
                    var value = reader.GetString(0);
                    columnTypes.Add(value);
                }
                return columnTypes;
            }
        }

        public async Task DeleteDataFromTable(string tableName)
        {
            string query = @"EXEC [SP_IA_DELETE_TABLE_DATA] @TABLE_NAME";

            List<Param> @params = new List<Param>()
            {
                new Param{ SqlDbType = SqlDbType.VarChar, ParamName="@TABLE_NAME", SqlValue = tableName}
            };
            
            await _iBaseDataAccess.ExecuteQueryAsync(query, @params);
        }

        public async Task<DataTable> GetAllDataFromTableAsDataTable(string subsectionName)
        {
            string tableName = await this.GetTableNameFromSubsectionName(subsectionName);
            string query = @"EXEC [SP_IA_GET_TABLE_DATA] @TABLE_NAME";

            List<Param> @params = new List<Param>()
            {
                new Param{ SqlDbType = SqlDbType.VarChar, ParamName="@TABLE_NAME", SqlValue = tableName}
            };

            var dataTable = await _iBaseDataAccess.GetDataTable(query, @params);
            return dataTable;
        }

        public async Task<string> GetTableNameFromSubsectionName(string subsectionName)
        {
            string query = @"EXEC [SP_IA_GET_TABLE_NAME] @SUBSECTION";

            List<Param> @params = new List<Param>()
            {
                new Param{ SqlDbType = SqlDbType.VarChar, ParamName="@SUBSECTION", SqlValue = subsectionName}
            };

            var tableName = string.Empty;

            using (var reader = await _iBaseDataAccess.ExecuteReaderAsync(query, @params))
            {
                while (reader.Read())
                {
                    tableName = reader.GetString(0);
                }
            }
            return tableName;
        }

        public async Task UpdateDependentColumns(string subsectionName)
        {
            string query = subsectionName switch
            {
                "Short Leave" => "EXEC [UPDATE_SHORT_LEAVE_COLUMNS]",
                "Statutory Leave" => "EXEC [UPDATE_STAT_LEAVE_COLUMNS]",
                _ => "Default",
            };

            await _iBaseDataAccess.ExecuteQueryAsync(query);
        }

        public async Task DeleteDataFromDependentTableForShortLeave()
        {
            string query = "EXEC [DELETE_SHORT_LEAVE_DEPN_DATA]";
            await _iBaseDataAccess.ExecuteQueryAsync(query);
        }
    }
}