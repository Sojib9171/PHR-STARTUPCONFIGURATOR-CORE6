namespace HBS.ImplementationAutomationConsole.Core.Models
{
    public class AdminApprovalGetDto
    {
        public int RecordID { get; set; }
        public string? MainsectionName { get; set; }
        public string? SubsectionName { get; set; }
        public string? Name { get; set; }
        public string? ApprovalStatus { get; set; }
        public string? ApprovalComment { get; set; }
        public DateTime ApprovalDate { get; set; }
        public byte[] ApprovalData { get; set; }
    }
}
