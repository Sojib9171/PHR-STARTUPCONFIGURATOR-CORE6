using HBS.ImplementationAutomationConsole.Core.DataAccess.Data;
using HBS.ImplementationAutomationConsole.Core.DataAccess.IRepository;
using System.Data;

namespace HBS.ImplementationAutomationConsole.Core.DataAccess.Repository
{
    public class SignOffRepository : ISignOffRepository
    {
        private readonly IBaseDataAccess _iBaseDataAccess;
        public SignOffRepository(IBaseDataAccess iBaseDataAccess)
        {
            this._iBaseDataAccess = iBaseDataAccess;
        }

        public async Task<byte[]> GetPDFByteArray(int recordID)
        {
            string query = @"EXEC [SP_IA_GET_PDF_WITH_RECORD_ID] @RECORD_ID";
            List<Param> @params = new List<Param>()
            {
                new Param{ SqlDbType = SqlDbType.Int, ParamName="@RECORD_ID", SqlValue = recordID},
            };
            var pdfData = new byte[] { };
            using (var reader = await _iBaseDataAccess.ExecuteReaderAsync(query, @params))
            {
                while (reader.Read())
                {
                    pdfData = (byte[])reader.GetValue(0);
                }
            }
            return pdfData;
        }
    }
}