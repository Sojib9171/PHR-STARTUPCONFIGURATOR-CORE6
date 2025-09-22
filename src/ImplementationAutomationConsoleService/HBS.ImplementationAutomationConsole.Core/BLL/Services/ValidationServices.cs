using ClosedXML.Excel;
using HBS.ImplementationAutomationConsole.Core.BLL.IServices;
using HBS.ImplementationAutomationConsole.Core.DataAccess.IRepository;
using HBS.ImplementationAutomationConsole.Core.Models;
using System.Drawing;

namespace HBS.ImplementationAutomationConsole.Core.BLL.Services
{
    public class ValidationServices : IValidationServices
    {

        private readonly IValidationRepository _ivalidationRepository;
        private readonly ITemplateRepository _itemplateRepository;
        private readonly IExcelDataRepository _iexcelDataRepository;
        private readonly IBaseServices _ibaseServices;

        public ValidationServices(IValidationRepository ivalidationRepository, ITemplateRepository itemplateRepository, IExcelDataRepository excelDataRepository, IBaseServices baseServices)
        {
            _ivalidationRepository = ivalidationRepository;
            _itemplateRepository = itemplateRepository;
            _iexcelDataRepository = excelDataRepository;
            _ibaseServices = baseServices;
        }

        public async Task<List<int>> GetValidationCounts(string subsectionName)
        {
            var tableName = await _iexcelDataRepository.GetTableNameFromSubsectionName(subsectionName);
            var counts = await _ivalidationRepository.GetValidationCounts(tableName);

            return counts;
        }

        public async Task<List<string>> DownloadValidatedExcel(string subsectionName)
        {
            var templateId = await _itemplateRepository.GetTemplateIdFromSubsectionName(subsectionName);
            var templateData = await _itemplateRepository.GetTemplateByteArray(templateId);
            var templateByteArray = templateData.Item1;
            var templateName = templateData.Item2;

            if (subsectionName == "Roster Information")
            {
                var worksheetNo = 2;
                var rowNo = 2;
                var colNo = 4;
                var base64WithHierarchyOption = await _ibaseServices.GetExcelTemplateWithHierarchyOptionsData(templateByteArray, worksheetNo, rowNo, colNo);
                templateByteArray = Convert.FromBase64String(base64WithHierarchyOption);
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

                var models = await _ivalidationRepository.GetValidationResult(subsectionName);
                await MarkErrorsAndErrorSummaryInExcel(models, worksheet, dataStartRowNumber, lastColumnNumber);

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

        public async Task ValidateUploadedData(string subsectionName)
        {
            var templateId = await _itemplateRepository.GetTemplateIdFromSubsectionName(subsectionName);
            await _ivalidationRepository.ValidateUploadedData(templateId);
        }

        public async Task MarkErrorsAndErrorSummaryInExcel(List<ValidationDataModel> models, IXLWorksheet worksheet, int dataStartRowNumber, int lastColumnNumber)
        {
            if (models.Count == 0) return;

            Dictionary<int, string> dictionary = new Dictionary<int, string>();
            Color color = ColorTranslator.FromHtml("#FE4356");
            XLColor xlColor = XLColor.FromColor(color);
            foreach (var model in models)
            {
                var row = int.Parse(model.RowNumber);
                var col = int.Parse(model.ColumnNumber);
                worksheet.Cell(row + 5, col).Style.Fill.BackgroundColor = xlColor;

                if (dictionary.ContainsKey(row))
                {
                    dictionary[row] += $", {model.ErrorDetails}";
                }
                else
                {
                    dictionary[row] = $"{model.ErrorDetails}";
                }
            }

            Color color2 = ColorTranslator.FromHtml("#DDDDE1");
            XLColor xlColor2 = XLColor.FromColor(color2);

            worksheet.Cell(2, lastColumnNumber + 2).Style.Fill.BackgroundColor = xlColor2;
            worksheet.Cell(2, lastColumnNumber + 2).Value = "Error description";
            worksheet.Cell(2, lastColumnNumber + 2).Style.Font.Bold = true;
            worksheet.Cell(2, lastColumnNumber + 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            worksheet.Cell(2, lastColumnNumber + 2).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            worksheet.Column(lastColumnNumber + 2).AdjustToContents();

            foreach (KeyValuePair<int, string> kvp in dictionary)
            {
                var errorMessage = kvp.Value;
                var splitErrorMessage = errorMessage.Split(',');
                var messageHashset = new HashSet<string>();

                foreach (var error in splitErrorMessage)
                {
                    messageHashset.Add(error.Trim());
                }
                var uniqueMesagesLine = messageHashset.ToArray();
                var uniqueErrorMessage = string.Join(", ", uniqueMesagesLine);

                int errorMessageLength = uniqueErrorMessage.Length;
                var cellsNeeded = (int)Math.Ceiling((float)errorMessageLength / 32767); //Max character Limit for one excel cell in 32767
                if (cellsNeeded == 1)
                {
                    worksheet.Cell(kvp.Key + 5, lastColumnNumber + 2).Value = uniqueErrorMessage;
                }
            }
        }
    }
}