namespace HBS.ImplementationAutomationConsole.Core.Models
{
    public class DashboardPostDto
    {
        public string SubsectionName { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
        public int ModuleID { get; set; }
        public string SubsectionID { get; set; }
        public string ApprovalStatus { get; set; }
        public string ApprovalSignature { get; set; }
        public DateTime ApprovalDate { get; set; }
        public byte[] ApprovalData { get; set;}
        public byte[] SignOffPdfData { get; set; }
    }
}
