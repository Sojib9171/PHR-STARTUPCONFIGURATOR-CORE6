using HBS.ImplementationAutomationConsole.Core.Models;

namespace HBS.ImplementationAutomationConsole.Core.DataAccess.IRepository
{
    public interface IValidationRepository
    {
        Task<List<int>> GetValidationCounts(string tableName);
        Task<List<ValidationDataModel>> GetValidationResult(string subsectionName);
        Task ValidateUploadedData(string templateId);
        //Task<List<ValidationDataModel>> GetValidationResultAbsence();
        //Task<List<ValidationDataModel>> GetValidationResultRoster();
    }
}