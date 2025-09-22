using HBS.ImplementationAutomationConsole.Core.Models;

namespace HBS.ImplementationAutomationConsole.Core.DataAccess.IRepository
{
    public interface ISignOffRepository
    {
        public Task<byte[]> GetPDFByteArray(int recordID);
    }
}