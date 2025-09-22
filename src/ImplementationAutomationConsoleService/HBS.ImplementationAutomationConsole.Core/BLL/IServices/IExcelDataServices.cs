using ClosedXML.Excel;
using System.Data;

namespace HBS.ImplementationAutomationConsole.Core.BLL.IServices
{
    public interface IExcelDataServices
    {
        public Task UploadData(MemoryStream stream, string subsectionName);
        public Task<DataTable> ExcelToDataTable(IXLWorksheet worksheet, List<string> columnTypes);
        public Task DeleteAllFromTable(string subsectionName);
        public Task<List<string>> DownloadExcel(string subsectionName);
        public Task UpdateDependentColumns(string subsectionName);
    }
}