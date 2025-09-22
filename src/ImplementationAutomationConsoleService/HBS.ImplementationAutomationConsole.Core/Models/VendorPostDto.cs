namespace HBS.ImplementationAutomationConsole.Core.Models
{
    public class VendorPostDto
    {
        public string? VendorName { get; set; }
        public string? VendorApprovalStatus { get; set; }
        public string? VendorApprovalComment { get; set; }
        public DateTime VendorApprovalDate { get; set; }
        public int UserRecordId { get; set; }
        public string SubsectionName { get; set; }
    }
}
