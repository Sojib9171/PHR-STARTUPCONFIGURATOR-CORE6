namespace HBS.ImplementationAutomationConsole.Core.Models
{
    public class CommonConfigHistoryServerParamsDto
    {
        public string userid { get; set; }
        public string SortField { get; set; }
        public string SortType { get; set; }
        public int Page { get; set; }
        public int PerPage { get; set; }
        public string searchText { get; set; }
    }
}