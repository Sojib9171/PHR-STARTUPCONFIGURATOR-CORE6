namespace HBS.ImplementationAutomationConsole.Core.Models
{
    public class ValidationDataModel
    {
        public string Identifier { get; set; }
        public string TemplateID { get; set; }
        public string ErrorDetails { get; set; }
        public string ColumnNumber { get; set; }
        public string ColumnName { get; set; }
        public string RowNumber { get; set; }
        public int ErrorID { get; set; }
    }
}