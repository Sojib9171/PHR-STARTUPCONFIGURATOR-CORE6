namespace HBS.ImplementationAutomationConsole.Core.DataAccess.IRepository
{
    public interface ITemplateRepository
    {
        public Task<bool> UploadTemplate(string templateId, int moduleId, string subsectionId, string templateName, byte[] templateData, bool isActiveFlg);
        public Task<List<string>> GetTemplateData(string template_Id);
        public Task<string> GetTemplateIdFromSubsectionName(string subsectionName);
        public Task<(byte[], string)> GetTemplateByteArray(string template_Id);
        public Task<List<string>> GetHierarchyNamesAsList();
        public Task<List<string>> GetLeaveTypeCodesAsList();
    }
}