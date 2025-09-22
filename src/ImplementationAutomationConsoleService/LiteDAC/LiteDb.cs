using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;

namespace LiteDAC;

public class LiteDb : IDisposable
{
    private readonly IDbConnection? _metaConnection;
    private readonly IDbConnection? _clientConnection;

    private const string CDbType = "Db.Provider";
    private const string CDbSettingsKey = "Db.Connection";

    public LiteDb(IConfiguration configuration, string subDomain)
    {
        var dbType = configuration[CDbType];
        var metaDbConnection = configuration[CDbSettingsKey];
        var dbSettings = configuration.GetConnectionString(metaDbConnection ??
                                                           throw new InvalidOperationException(
                                                               "Datasource has not been configured."));

        _metaConnection = dbType switch
        {
            "Sql" => new SqlConnection(dbSettings),
            "Oracle" => new OracleConnection(dbSettings),
            _ => throw new InvalidOperationException("Unsupported database configuration.")
        };

        _metaConnection.Open();
        _clientConnection = _metaConnection.ResolveClientConnection(subDomain);
    }

    public IDbConnection? Connection => _clientConnection;

    public void Dispose()
    {
        _metaConnection?.Close();
        _metaConnection?.Dispose();

        if (_clientConnection == null) return;
        _clientConnection.Close();
        _clientConnection.Dispose();
    }
}