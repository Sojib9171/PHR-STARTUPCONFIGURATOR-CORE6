using System.Data;
using Microsoft.Data.SqlClient;

namespace LiteDAC;

internal static class DbConnectionExtension
{
    internal static IDbConnection ResolveClientConnection(this IDbConnection metaConnection, string clientDomain)
    {
        using var cmd = metaConnection.CreateCommand();
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandText = "SP_IA_GET_CLIENT_CONNCTN_META";
        cmd.AddParameter("@domain", DbType.String, clientDomain);
        cmd.CommandTimeout = 180000;

        using var reader = cmd.ExecuteReader();

        if (!reader.Read()) return null!;
        var credentials = new DatabaseCredentials()
        {
            Database = reader["Database"].ToString(),
            UserName = reader["UserName"].ToString(),
            Password = reader["Password"].ToString(),
            Server = reader["Servername"].ToString(),
            Schema = reader["Schema"].ToString()
        };
            
        var tenantConnection = new SqlConnection()
        {
            ConnectionString = credentials.BuildConnectionString()
        };

        tenantConnection.Open();

        return tenantConnection;
    }
}