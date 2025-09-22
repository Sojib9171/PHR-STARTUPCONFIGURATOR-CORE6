namespace HBS.ImplementationAutomationConsole.Core.BLL.IServices
{
    public interface ISignOffService
    {
        public Task<string> GetModuleNameFromSubsection(string subsectionName);
        public Task<String> GetSignOffHtmlString(string name, DateTime dateOfApproval, string approvalStatus,
            string moduleName, string subsectionName, string comment, string userName);
        public Task<String> GetSignOffHtmlStringForConfiguration(string name, DateTime dateOfApproval, string approvalStatus,
     string comment, string userName, string sectionName);
        public Task<byte[]> GetSignOffPdfAsByteArray(int recordId);
    }
}