using System.Collections;
using System.Data;
using System.Dynamic;

namespace LiteDAC
{
    internal static class DbCommandExtension
    {
        internal static void AddParameter(this IDbCommand dbCommand, string parameterName, 
            DbType parameterType, object value)
        {
            if (dbCommand == null) throw new ArgumentNullException(nameof(dbCommand));

            var parameter = dbCommand.CreateParameter();
            parameter.ParameterName = parameterName;
            parameter.DbType = parameterType;
            parameter.Value = value;
            parameter.Direction = ParameterDirection.Input;

            dbCommand.Parameters.Add(parameter);
        }

        internal static void AddOutParameter(this IDbCommand dbCommand, string parameterName,
           DbType parameterType)
        {
            if (dbCommand == null) throw new ArgumentNullException(nameof(dbCommand));

            var parameter = dbCommand.CreateParameter();
            parameter.ParameterName = parameterName;
            parameter.DbType = parameterType;
            parameter.Direction = ParameterDirection.Output;

            dbCommand.Parameters.Add(parameter);
        }
    }
}