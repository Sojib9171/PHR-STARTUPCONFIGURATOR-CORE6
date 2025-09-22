using ClosedXML.Excel;
using System.Data;
using System.Data.Common;

namespace HBS.ImplementationAutomationConsole.Core.DataAccess.Data
{
    public interface IBaseDataAccess
    {
        public Task ExecuteQueryAsync(string query, List<Param>? @params = null,int commandtimeout=30, string typeName = null);
        public Task<DataTable> GetDataTable(string query, List<Param>? @params = null);
        public Task<int> GetSingleInt(string query, List<Param>? @params = null);
        public Task<DbDataReader> ExecuteReaderAsync(string query, List<Param>? @params = null, string outputParamName = null, string typeName=null);
        public Task<DbDataReader> ExecuteReaderWithUserDefinedDataTypeAsync(string query, string typeName, List<Param>? @params = null);
        public Task BulkInsert(IDataReader dataReader, string destinationTableName, List<(string, string)> columnMaps);
        public Task<string> GetDbUserName();
    }
}