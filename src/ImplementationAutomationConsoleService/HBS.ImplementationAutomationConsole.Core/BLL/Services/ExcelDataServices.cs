using ClosedXML.Excel;
using HBS.ImplementationAutomationConsole.Core.BLL.IServices;
using HBS.ImplementationAutomationConsole.Core.DataAccess.Data;
using HBS.ImplementationAutomationConsole.Core.DataAccess.IRepository;
using System.Data;
using System.Globalization;

namespace HBS.ImplementationAutomationConsole.Core.BLL.Services
{
    public class ExcelDataServices : IExcelDataServices
    {

        private readonly IExcelDataRepository _iexcelDataRepository;
        private readonly IBaseDataAccess _ibaseDataAccess;
        private readonly ITemplateRepository _itemplateRepository;
        private readonly IBaseServices _ibaseServices;

        public ExcelDataServices(IExcelDataRepository iexcelDataRepository, IBaseDataAccess ibaseDataAccess, ITemplateRepository itemplateRepository, IBaseServices baseServices)
        {
            _iexcelDataRepository = iexcelDataRepository;
            _ibaseDataAccess = ibaseDataAccess;
            _itemplateRepository = itemplateRepository;
            _ibaseServices = baseServices;
        }

        public async Task UploadData(MemoryStream stream, string subsectionName)
        {
            using (var workbook = new XLWorkbook(stream))
            {
                var worksheet = workbook.Worksheets.Last();
                var templateId = await _itemplateRepository.GetTemplateIdFromSubsectionName(subsectionName);
                var columnTypes = await _iexcelDataRepository.GetColumnTypes(templateId);
                var dataTable = new DataTable();
                if (subsectionName == "Roster Information")
                {
                    dataTable = await ExcelToDataTableForRoster(worksheet, columnTypes);
                }
                else if (subsectionName == "Shift Information")
                {
                    dataTable = await ExcelToDataTableForShift(worksheet, columnTypes);
                }
                else
                {
                     dataTable = await ExcelToDataTable(worksheet, columnTypes);
                }
                var tableName = await _iexcelDataRepository.GetTableNameFromSubsectionName(subsectionName);
                var columnMaps = await _iexcelDataRepository.GetColumnNames(templateId);

                await _iexcelDataRepository.DeleteDataFromTable(tableName);

                await _ibaseDataAccess.BulkInsert(dataTable.CreateDataReader(), tableName, columnMaps);
            }
        }


        public async Task<DataTable> ExcelToDataTable(IXLWorksheet worksheet, List<string> columnTypes)
        {
            var dataTable = new DataTable();
            var headerRows = worksheet.RowsUsed().Where(row => row.CellsUsed().Any(cell => !string.IsNullOrEmpty(cell.Value.ToString()))); // Finding non empty rows  
            int headerRowIndex = 1;

            foreach (var headerRow in headerRows)
            {
                if (headerRow != null && !headerRow.Cell(2).IsMerged()) //Checking if the cell in the 2nd column if merged. It should be merged to another cell if it is not the column name row.
                {
                    headerRowIndex = headerRow.RowNumber(); //This row will consist the column names.                                                           //This is found out on the logic that the first cell is not merged.
                    break;
                }
            }

            foreach (var cell in worksheet.Row(headerRowIndex).CellsUsed()) //Row(headerRowIndex) contains the headers in excel
            {
                dataTable.Columns.Add(cell.Value.ToString().Trim()); //Adding column names to the datatable
            }

            var firstRowUsed = worksheet.FirstRow();
            var rowskip = headerRowIndex + 2;
            var row = firstRowUsed.RowBelow(rowskip); //We should follow a template format that the row after the column names will be an details row explaining input details. The input data will be read from two rows after the column row.

            try
                {

                while (!row.IsEmpty())
                {
                    var colCount = dataTable.Columns.Count;
                    var dataRow = dataTable.NewRow();

                    for (int i = 2; i < colCount + 2; i++)
                    {
                        switch (columnTypes[i - 2])
                        {
                            case "datetime":
                            case "date":
                                var colValue = row.Cell(i).Value.ToString();
                                if (string.IsNullOrEmpty(colValue))
                                {
                                    dataRow[i - 2] = null;
                                    continue;
                                }
                                var format = "d/M/yyyy";
                                dataRow[i - 2] = DateTime.ParseExact(colValue, format, CultureInfo.InvariantCulture);
                                break;

                            case "decimal":
                            case "numeric":
                                colValue = row.Cell(i).Value.ToString();
                                if (string.IsNullOrEmpty(colValue))
                                {
                                    dataRow[i - 2] = null;
                                    continue;
                                }
                                dataRow[i - 2] = Decimal.Parse(colValue);
                                break;

                            case "smallint":
                            case "bigint":
                            case "int":
                                colValue = row.Cell(i).Value.ToString();
                                if (string.IsNullOrEmpty(colValue))
                                {
                                    dataRow[i - 2] = null;
                                    continue;
                                }
                                dataRow[i - 2] = int.Parse(colValue);
                                break;

                            case "nvarchar":
                            case "varchar":
                                colValue = row.Cell(i).Value.ToString();
                                if (string.IsNullOrEmpty(colValue))
                                {
                                    dataRow[i - 2] = null;
                                    continue;
                                }
                                dataRow[i - 2] = colValue.Trim();
                                break;
                        }
                    }
                    dataTable.Rows.Add(dataRow);
                    row = row.RowBelow();
                }
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DataTable> ExcelToDataTableForRoster(IXLWorksheet worksheet, List<string> columnTypes)
        {
            var dataTable = await ExcelToDataTable(worksheet, columnTypes);
            dataTable.Columns.Add("Time Zone");

            TimeZoneInfo timeZone = TimeZoneInfo.Local;

            TimeSpan offset = timeZone.BaseUtcOffset;
            string numericOffset = string.Format("{0}{1}.{2:00}", offset.TotalHours >= 0 ? string.Empty : "-", Math.Abs(offset.Hours), Math.Abs(offset.Minutes));
            var timezoneNumeric = double.Parse(numericOffset);

            foreach (DataRow row in dataTable.Rows)
            {
                DataColumn lastColumn = dataTable.Columns[dataTable.Columns.Count - 1];
                row[lastColumn] = timezoneNumeric;
            }
            return dataTable;
        }

        public async Task<DataTable> ExcelToDataTableForShift(IXLWorksheet worksheet, List<string> columnTypes)
        {
            var dataTable = await ExcelToDataTable(worksheet, columnTypes);
            dataTable.Columns.Add("Shift Code");

            foreach (DataRow row in dataTable.Rows)
            {
                DataColumn lastColumn = dataTable.Columns[dataTable.Columns.Count - 1];
                row[lastColumn] = null;
            }
            return dataTable;
        }

        public async Task DeleteAllFromTable(string subsectionName)
        {
            var tableName = await _iexcelDataRepository.GetTableNameFromSubsectionName(subsectionName);
            if(subsectionName=="Short Leave")
            {
                await _iexcelDataRepository.DeleteDataFromDependentTableForShortLeave();
            }
            await _iexcelDataRepository.DeleteDataFromTable(tableName);
        }

        public async Task<List<string>> DownloadExcel(string subsectionName)
        {
            var templateId = await _itemplateRepository.GetTemplateIdFromSubsectionName(subsectionName);
            var templateData = await _itemplateRepository.GetTemplateByteArray(templateId);
            var templateByteArray = templateData.Item1;
            var templateName = templateData.Item2;
            
            if(subsectionName=="Roster Information")
            {
                var worksheetNo = 2;
                var rowNo = 2;
                var colNo = 4;
                var templateBase64 = await _ibaseServices.GetExcelTemplateWithHierarchyOptionsData(templateByteArray,worksheetNo,rowNo,colNo);
                templateByteArray=Convert.FromBase64String(templateBase64);
            }

            IXLWorkbook workbook;
            var base64String = string.Empty;

            using (MemoryStream stream = new MemoryStream(templateByteArray))
            {
                workbook = new XLWorkbook(stream);
                var worksheet = workbook.Worksheets.Last();
                (int dataStartRowNumber, int lastColumnNumber) = await _ibaseServices.GetWorkSheetRowAndColumnCount(worksheet);
                var dataTable = await _iexcelDataRepository.GetAllDataFromTableAsDataTable(subsectionName);
                await _ibaseServices.InsertDatatableInWorksheet(worksheet, dataTable, dataStartRowNumber);

                MemoryStream ms = new MemoryStream();
                workbook.SaveAs(ms);
                base64String = Convert.ToBase64String(ms.ToArray());
            }

            List<string> response = new List<string>
                {
                    base64String,
                    templateName
                };

            return response;
        }

        public async Task UpdateDependentColumns(string subsectionName)
        {
            await _iexcelDataRepository.UpdateDependentColumns(subsectionName);
        }
    }
}