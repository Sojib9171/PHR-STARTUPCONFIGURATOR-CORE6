namespace HBS.ImplementationAutomationConsole.Core.Models
{
    public class AdminPostDto
    {
        public string? AdminName { get; set; }
        public string? AdminApprovalStatus { get; set; }
        public string? AdminApprovalComment { get; set; }
        public DateTime AdminApprovalDate { get; set; }
        public int UserRecordId { get; set; }
    }
}
