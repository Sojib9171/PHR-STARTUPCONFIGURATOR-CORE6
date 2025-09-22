namespace HBS.ImplementationAutomationConsole.Core.Models
{
    public class AbsenceWizardDeleteRowDto
    {
        public int record_id { get; set; }
        public string leaveTypeCode { get; set; }
        public int vgt_id { get; set; }
        public int originalIndex { get; set; }
        public bool vgtSelected { get; set; }
    }

}