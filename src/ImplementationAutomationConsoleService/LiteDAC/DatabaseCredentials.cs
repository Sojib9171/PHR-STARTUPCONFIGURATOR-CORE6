using Microsoft.Data.SqlClient;

namespace LiteDAC
{
    internal class DatabaseCredentials
    {
        public string? Server { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? Database { get; set; }
        public string? Schema { get; set; }

        public string BuildConnectionString()
        {
            var dbConnectionStringBuilder = new SqlConnectionStringBuilder();
            dbConnectionStringBuilder[MsSqlKeys.Server] = this.Server;
            dbConnectionStringBuilder[MsSqlKeys.Database] = this.Database;
            dbConnectionStringBuilder[MsSqlKeys.UserName] = this.UserName;
            dbConnectionStringBuilder[MsSqlKeys.Password] = this.Password;
            dbConnectionStringBuilder["Trusted_Connection"] = "no";
            dbConnectionStringBuilder["Encrypt"] = "True";
            dbConnectionStringBuilder["TrustServerCertificate"] = "True";
            
            return dbConnectionStringBuilder.ConnectionString;
        }
    }
}