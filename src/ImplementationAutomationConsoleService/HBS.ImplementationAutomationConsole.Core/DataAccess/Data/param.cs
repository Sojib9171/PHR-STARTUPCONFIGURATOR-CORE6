using System.Data;

namespace HBS.ImplementationAutomationConsole.Core.DataAccess.Data
{
    public class Param
    {
        public SqlDbType SqlDbType { get; set; }
        public string? ParamName { get; set; }
        public dynamic? SqlValue { get; set; }
    }
}