namespace HBS.ImplementationAutomationConsole.Core.Models
{
    public class AbsenceServerParamsDto
    {
        public string SortField { get; set; }
        public string SortType { get; set; }
        public int Page { get; set; }
        public int PerPage { get; set; }
        public string SearchText { get; set; }
        public string Subsection { get; set; }
        public int[] RowIds { get; set; }
    }
}