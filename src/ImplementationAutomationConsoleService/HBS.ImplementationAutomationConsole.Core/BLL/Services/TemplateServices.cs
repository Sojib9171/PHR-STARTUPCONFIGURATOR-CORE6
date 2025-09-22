using ClosedXML.Excel;
using HBS.ImplementationAutomationConsole.Core.BLL.IServices;
using HBS.ImplementationAutomationConsole.Core.DataAccess.IRepository;


namespace HBS.ImplementationAutomationConsole.Core.BLL.Services
{
    public class TemplateServices : ITemplateServices
    {

        private readonly ITemplateRepository _itemplateRepository;
        private readonly IBaseServices _ibaseServices;

        public TemplateServices(ITemplateRepository itemplateRepository,IBaseServices ibaseServices)
        {
            _itemplateRepository = itemplateRepository;
            _ibaseServices = ibaseServices;
        }
        public async Task<bool> UploadTemplate(string templateID, int moduleID, string subsectionID, string templateName, byte[] fileBytes, bool isActiveFlag)
        {
            try
            {
                return await _itemplateRepository.UploadTemplate(templateID, moduleID, subsectionID, templateName, fileBytes, isActiveFlag);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<string>> DownloadTemplate(string subsectionName)
        {
            var templateId = await _itemplateRepository.GetTemplateIdFromSubsectionName(subsectionName);
            var response = await _itemplateRepository.GetTemplateData(templateId);

            return response;
        }

        public async Task<List<string>> DownloadTemplateWithDynamicData(string subsectionName)
        {
            var templateId = await _itemplateRepository.GetTemplateIdFromSubsectionName(subsectionName);
            var templateData = await _itemplateRepository.GetTemplateByteArray(templateId);
            var templateByteArray = templateData.Item1;
            var templateName = templateData.Item2;

            var worksheetNo = 2;
            var rowNo = 2;
            var colNo = 4;
            var templateBase64String = await _ibaseServices.GetExcelTemplateWithHierarchyOptionsData(templateByteArray, worksheetNo, rowNo, colNo);
            List<string> response = new List<string>
                {
                    templateBase64String,
                    templateName
                };

            return response;
        }

        public async Task<List<string>> DownloadTemplateWithDynamicDataForStatutoryLeave(string subsectionName)
        {
            var templateId = await _itemplateRepository.GetTemplateIdFromSubsectionName(subsectionName);
            var templateData = await _itemplateRepository.GetTemplateByteArray(templateId);
            var templateByteArray = templateData.Item1;
            var templateName = templateData.Item2;

            var worksheetNo = 3;
            var rowNo = 2;
            var colNo = 1;
            var templateBase64String = await _ibaseServices.GetExcelTemplateWithLeaveTypeCodeOptionsData(templateByteArray, worksheetNo, rowNo, colNo);
            List<string> response = new List<string>
                {
                    templateBase64String,
                    templateName
                };

            return response;
        }
    }
}