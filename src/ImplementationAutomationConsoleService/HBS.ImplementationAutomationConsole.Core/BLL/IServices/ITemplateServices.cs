using ClosedXML.Excel;

namespace HBS.ImplementationAutomationConsole.Core.BLL.IServices
{
    public interface ITemplateServices
    {
        public Task<bool> UploadTemplate(string templateID, int moduleID, string subsectionID, string templateName, byte[] fileBytes, bool isActiveFlag);
        public Task<List<string>> DownloadTemplate(string subsectionName);
        public Task<List<string>> DownloadTemplateWithDynamicData(string subsectionName);
        public Task<List<string>> DownloadTemplateWithDynamicDataForStatutoryLeave(string subsectionName);
    }
}