namespace HBS.ImplementationAutomationConsole.Core.Models
{
    public class CommonConfigUpdateDto
    {
        public int QuestionNo { get; set; }
        public int RecordId { get; set; }
        public object ResponseText { get; set; }
        public string QuestionType { get; set; }
        public string? LogoImageName { get; set; }
        public string? MobileLogoImageName { get; set; }
    }
}