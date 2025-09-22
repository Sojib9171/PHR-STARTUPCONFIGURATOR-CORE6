namespace HBS.ImplementationAutomationConsole.Core.Models
{
    public class DashboardGetDto
    {
        public int RecordID { get; set; }
        public string? MainsectionName { get; set; }
        public string? SubsectionName { get; set; }
        public string? Name { get; set; }
        public string? ApprovalStatus { get; set; }
        public string? ApprovalComment { get; set; }
        public DateTime ApprovalDate { get; set; }
        public string? VendorName { get; set; }
        public string? VendorApprovalStatus { get; set; }
        public string? VendorApprovalComment { get; set; }
        public DateTime VendorApprovalDate { get; set; }
        public byte[] ApprovalData { get; set; }
        public byte[] SignOffPdfData { get; set; }
    }
}
