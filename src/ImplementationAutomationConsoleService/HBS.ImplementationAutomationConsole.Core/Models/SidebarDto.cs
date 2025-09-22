namespace HBS.ImplementationAutomationConsole.Core.Models
{
    public class SidebarDto
    {
        public string Subsection_Id { get; set; }
        public string Subsection_Name { get; set; }
        public int Module_Id { get; set; }
        public string Approval_Status { get; set;}
        public string Vendor_ApprovalStatus { get; set; }
        public bool Enabled { get; set;}
    }
}