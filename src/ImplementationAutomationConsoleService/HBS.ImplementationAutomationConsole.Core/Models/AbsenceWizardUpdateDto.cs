namespace HBS.ImplementationAutomationConsole.Core.Models
{
    public class AbsenceWizardUpdateDto
    {
        public int QuestionNo { get; set; }
        public int RecordId { get; set; }
        public object? ResponseText { get; set; }
        public string QuestionType { get; set; }
        public string? SubsectionName { get; set; }
    }
}