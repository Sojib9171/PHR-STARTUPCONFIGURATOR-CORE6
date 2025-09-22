using System.Data;

namespace HBS.ImplementationAutomationConsole.Core.DataAccess.IRepository
{
    public interface IExcelDataRepository
    {
        public Task<List<(string, string)>> GetColumnNames(string template_Id);
        public Task<List<string>> GetColumnTypes(string template_Id);
        public Task<DataTable> GetAllDataFromTableAsDataTable(string subsectionName);
        public Task<string> GetTableNameFromSubsectionName(string subsectionName);
        public Task DeleteDataFromTable(string tableName);
        public Task UpdateDependentColumns(string subsectionName);
        Task DeleteDataFromDependentTableForShortLeave();
    }
}