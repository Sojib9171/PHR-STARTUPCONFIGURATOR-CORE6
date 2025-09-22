using ClosedXML.Excel;
using HBS.ImplementationAutomationConsole.Core.BLL.IServices;
using HBS.ImplementationAutomationConsole.Core.DataAccess.IRepository;
using System.Data;


namespace HBS.ImplementationAutomationConsole.Core.BLL.Services
{
    public class BaseServices : IBaseServices
    {
        private readonly ITemplateRepository _itemplateRepository;
        public BaseServices(ITemplateRepository itemplateRepository) { 
            _itemplateRepository = itemplateRepository;
        }
        public async Task<(int, int)> GetWorkSheetRowAndColumnCount(IXLWorksheet worksheet)
        {
            var headerRows = worksheet.RowsUsed().Where(row => row.CellsUsed().Any(cell => !string.IsNullOrEmpty(cell.Value.ToString()))); // Finding non empty rows
            int headerRowIndex = 1;

            foreach (var headerRow in headerRows)
            {
                if (headerRow != null && !headerRow.Cell(2).IsMerged()) //Checking if the cell in the 2nd column if merged. It should be merged to another cell if it is not the column name row.
                {
                    headerRowIndex = headerRow.RowNumber(); //This row will consist the column names.                                                          //This is found out on the logic that the first cell is not merged.
                    break;
                }
            }

            var firstRowUsed = worksheet.FirstRow();
            var rowskip = headerRowIndex + 2;

            var dataStartRow = firstRowUsed.RowBelow(rowskip).RowNumber();
            var lastheaderColumn = worksheet.Row(headerRowIndex).CellsUsed().LastOrDefault(cell => !string.IsNullOrEmpty(cell.Value.ToString()));
            var lastColumnNumber = lastheaderColumn.Address.ColumnNumber;

            return (dataStartRow, lastColumnNumber);
        }

        public async Task InsertDatatableInWorksheet(IXLWorksheet worksheet,DataTable dataTable, int startRowNumber)
        {
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow row = dataTable.Rows[i];

                for (int j = 0; j < dataTable.Columns.Count; j++)
                {
                    worksheet.Cell(startRowNumber + i, j + 2).Value = row[j].ToString();
                }
            }
        }

        public async Task<string> GetExcelTemplateWithHierarchyOptionsData(byte[] templateByteArray, int worksheetNo,int rowNum, int colNum)
        {
            IXLWorkbook workbook;
            var base64String = string.Empty;

            using (MemoryStream stream = new MemoryStream(templateByteArray))
            {
                workbook = new XLWorkbook(stream);
                var worksheet = workbook.Worksheet(worksheetNo);
                var hierarchyList = await _itemplateRepository.GetHierarchyNamesAsList();

                await InsertHierarchyDataInExcelForOptions(worksheet, hierarchyList,rowNum,colNum);

                MemoryStream ms = new MemoryStream();
                workbook.SaveAs(ms);
                base64String = Convert.ToBase64String(ms.ToArray());
            }
            return base64String;
        }

        public async Task InsertHierarchyDataInExcelForOptions(IXLWorksheet worksheet, List<string> hierarchyNames, int rowNumber, int colNumber)
        {
            foreach (var hierarchy in hierarchyNames)
            {
                worksheet.Cell(rowNumber, colNumber).Value = hierarchy;
                rowNumber++;
            }
        }

        public async Task<string> GetExcelTemplateWithLeaveTypeCodeOptionsData(byte[] templateByteArray, int worksheetNo, int rowNum, int colNum)
        {
            IXLWorkbook workbook;
            var base64String = string.Empty;

            using (MemoryStream stream = new MemoryStream(templateByteArray))
            {
                workbook = new XLWorkbook(stream);
                var worksheet = workbook.Worksheet(worksheetNo);
                var hierarchyList = await _itemplateRepository.GetLeaveTypeCodesAsList();

                await InsertLeaveTypeCodeInExcelForOptions(worksheet, hierarchyList, rowNum, colNum);

                MemoryStream ms = new MemoryStream();
                workbook.SaveAs(ms);
                base64String = Convert.ToBase64String(ms.ToArray());
            }
            return base64String;
        }

        public async Task InsertLeaveTypeCodeInExcelForOptions(IXLWorksheet worksheet, List<string> hierarchyNames, int rowNumber, int colNumber)
        {
            foreach (var hierarchy in hierarchyNames)
            {
                worksheet.Cell(rowNumber, colNumber).Value = hierarchy;
                rowNumber++;
            }
        }
    }
}