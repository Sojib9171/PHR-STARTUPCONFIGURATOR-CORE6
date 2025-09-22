namespace HBS.ImplementationAutomationConsole.Core.Models
{
    public class CommonConfigPostDto
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
        public string ApprovalStatus { get; set; }
        public DateTime ApprovalDate { get; set; }
        public byte[] SignOffPdfData { get; set; }
        public int TableRowId { get; set; }
    }
}
