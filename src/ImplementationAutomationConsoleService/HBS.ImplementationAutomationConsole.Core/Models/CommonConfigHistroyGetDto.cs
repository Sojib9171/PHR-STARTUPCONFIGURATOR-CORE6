namespace HBS.ImplementationAutomationConsole.Core.Models
{
    public class ConfigControlHistroyGetDto
    {
        public int RecordID { get; set; }
        public string? Name { get; set; }
        public string? ApprovalStatus { get; set; }
        public string? ApprovalComment { get; set; }
        public DateTime ApprovalDate { get; set; }
        public byte[] SignOffPdfData { get; set; }
    }
}
