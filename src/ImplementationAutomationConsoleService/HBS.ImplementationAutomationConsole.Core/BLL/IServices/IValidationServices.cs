using ClosedXML.Excel;
using HBS.ImplementationAutomationConsole.Core.Models;

namespace HBS.ImplementationAutomationConsole.Core.BLL.IServices
{
    public interface IValidationServices
    {
        public Task<List<int>> GetValidationCounts(string subsectionName);
        public Task<List<string>> DownloadValidatedExcel(string subsectionName);
        public Task MarkErrorsAndErrorSummaryInExcel(List<ValidationDataModel> models, IXLWorksheet worksheet, int dataStartRowNumber, int lastColumnNumber);
        public Task ValidateUploadedData(string subsectionName);
    }
}