using ClosedXML.Excel;
using System.Data;

namespace HBS.ImplementationAutomationConsole.Core.BLL.IServices
{
    public interface IBaseServices
    {
        public Task<(int startRowNumber, int columnCount)> GetWorkSheetRowAndColumnCount(IXLWorksheet worksheet);
        public Task InsertDatatableInWorksheet(IXLWorksheet worksheet, DataTable dataTable, int startRowNumber);
        public Task InsertHierarchyDataInExcelForOptions(IXLWorksheet worksheet, List<string> hierarchyNames,int rowNumber, int colNumber);
        public Task<string> GetExcelTemplateWithHierarchyOptionsData(byte[] templateByteArray, int worksheetNo, int rowNum, int colNum);
        public Task<string> GetExcelTemplateWithLeaveTypeCodeOptionsData(byte[] templateByteArray, int worksheetNo, int rowNum, int colNum);
    }
}