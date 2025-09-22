using LiteDAC;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Text.RegularExpressions;

namespace HBS.ImplementationAutomationConsole.Core.DataAccess.Data
{
    public class BaseDataAccess : IBaseDataAccess
    {
        private readonly IDbConnection _connection;
        private readonly IConfiguration _configuration;
        public BaseDataAccess(IConfiguration configuration, LiteDb liteDb)
        {
            _connection = liteDb.Connection;
            _configuration = configuration;
        }

        public async Task<string> GetDbUserName()
        {
            var userName = _configuration.GetConnectionString("DefaultConnection")?.Split(';')
                .FirstOrDefault(part => part.Trim().StartsWith("user id", StringComparison.OrdinalIgnoreCase))?
                .Split('=')[1]?.Trim();
            return userName;
        }

        public async Task ExecuteQueryAsync(string query, List<Param>? @params = null, int commandtimeout = 30, string typeName = null)
        {
            using (DbCommand command = (DbCommand)_connection.CreateCommand())
            {
                command.CommandText = query;
                if (@params != null)
                {
                    foreach (Param param in @params)
                    {
                        if (param.SqlDbType == SqlDbType.Structured)
                        {
                            command.Parameters.Add(new SqlParameter(param.ParamName, param.SqlDbType) { Value = param.SqlValue, TypeName = typeName });
                            continue;
                        }

                        if (String.IsNullOrEmpty(Convert.ToString(param.SqlValue)))
                        {
                            param.SqlValue = DBNull.Value;
                        }
                        command.Parameters.Add(new SqlParameter(param.ParamName, param.SqlDbType) { Value = param.SqlValue });
                    }
                }
                command.CommandTimeout = commandtimeout;
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<DataTable> GetDataTable(string query, List<Param>? @params)
        {
            DataTable data = new DataTable();
            using (DbCommand command = (DbCommand)_connection.CreateCommand())
            {
                command.CommandText = query;
                if (@params != null)
                {
                    foreach (Param param in @params)
                    {
                        if (String.IsNullOrEmpty(Convert.ToString(param.SqlValue)))
                        {
                            param.SqlValue = DBNull.Value;
                        }
                        command.Parameters.Add(new SqlParameter(param.ParamName, param.SqlDbType) { Value = param.SqlValue });
                    }
                }
                using (DataAdapter dataAdapter = CreateDataAdapter(command))
                using (DataSet dataset = new DataSet())
                {
                    dataAdapter.Fill(dataset);
                    data = dataset.Tables[0];
                }
            }
            return data;
        }
        public async Task<int> GetSingleInt(string query, List<Param>? @params = null)
        {
            int result = 0;
            using (DbCommand command = (DbCommand)_connection.CreateCommand())
            {
                command.CommandText = query;
                if (@params != null)
                {
                    foreach (Param param in @params)
                    {
                        if (String.IsNullOrEmpty(Convert.ToString(param.SqlValue)))
                        {
                            param.SqlValue = DBNull.Value;
                        }
                        command.Parameters.Add(new SqlParameter(param.ParamName, param.SqlDbType) { Value = param.SqlValue });
                    }
                }
                var data = await command.ExecuteScalarAsync();
                if (data != null)
                {
                    result = Convert.ToInt32(data);
                }
            }
            return result;
        }

        public async Task BulkInsert(IDataReader dataReader, string destinationTableName, List<(string, string)> columnMaps)
        {
            using (var connection = (SqlConnection)_connection)
            {
                using (var bulkCopy = new SqlBulkCopy(connection))
                {
                    try
                    {
                        bulkCopy.DestinationTableName = destinationTableName;
                        bulkCopy.BatchSize = 100000;
                        bulkCopy.BulkCopyTimeout = 120;
                        foreach (var columnMap in columnMaps)
                        {
                            bulkCopy.ColumnMappings.Add(columnMap.Item2.ToString(), columnMap.Item1.ToString());
                        }

                        bulkCopy.WriteToServer(dataReader);
                    }
                    catch (SqlException ex)
                    {
                        var errorMessage = string.Empty;
                        if (ex.Message.Contains("Received an invalid column length from the bcp client for colid"))
                        {
                            string pattern = @"\d+";
                            Match match = Regex.Match(ex.Message.ToString(), pattern);
                            var index = Convert.ToInt32(match.Value) - 1;

                            FieldInfo fi = typeof(SqlBulkCopy).GetField("_sortedColumnMappings", BindingFlags.NonPublic | BindingFlags.Instance);
                            var sortedColumns = fi.GetValue(bulkCopy);
                            var items = (Object[])sortedColumns.GetType().GetField("_items", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(sortedColumns);

                            FieldInfo itemdata = items[index].GetType().GetField("_metadata", BindingFlags.NonPublic | BindingFlags.Instance);
                            var metadata = itemdata.GetValue(items[index]);

                            var column = metadata.GetType().GetField("column", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).GetValue(metadata);
                            var length = metadata.GetType().GetField("length", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).GetValue(metadata);
                            errorMessage = String.Format("Column: {0} contains data with a length greater than: {1}", column, length);
                        }
                        throw new Exception(errorMessage);
                    }
                }
            }
        }

        public async Task<DbDataReader> ExecuteReaderAsync(string query, List<Param>? @params = null, string outputParamName = null, string typeName = null)
        {
            using (DbCommand command = (DbCommand)_connection.CreateCommand())
            {
                command.CommandText = query;
                if (@params != null)
                {
                    foreach (Param param in @params)
                    {
                        if (param.SqlDbType == SqlDbType.Structured)
                        {
                            command.Parameters.Add(new SqlParameter(param.ParamName, param.SqlDbType) { Value = param.SqlValue, TypeName = typeName });
                            continue;
                        }

                        if (String.IsNullOrEmpty(Convert.ToString(param.SqlValue)))
                        {
                            param.SqlValue = DBNull.Value;
                        }
                        command.Parameters.Add(new SqlParameter(param.ParamName, param.SqlDbType) { Value = param.SqlValue });
                    }
                }
                if (!String.IsNullOrEmpty(outputParamName))
                {
                    SqlParameter outputParam = new SqlParameter(outputParamName, SqlDbType.Int);
                    outputParam.Direction = ParameterDirection.Output;
                    command.Parameters.Add(outputParam);
                }
                return await command.ExecuteReaderAsync();
            }
        }

        public async Task<DbDataReader> ExecuteReaderWithUserDefinedDataTypeAsync(string query, string typeName, List<Param>? @params = null)
        {
            using (DbCommand command = (DbCommand)_connection.CreateCommand())
            {
                command.CommandText = query;
                if (@params != null)
                {
                    foreach (Param param in @params)
                    {
                        if (String.IsNullOrEmpty(Convert.ToString(param.SqlValue)))
                        {
                            param.SqlValue = DBNull.Value;
                        }
                        command.Parameters.Add(new SqlParameter(param.ParamName, param.SqlDbType) { Value = param.SqlValue, TypeName = typeName });
                    }
                }
                return await command.ExecuteReaderAsync();
            }
        }

        public DbDataAdapter CreateDataAdapter(DbCommand command)
        {
            string? _dbType = _configuration["Db.Provider"];

            switch (_dbType)
            {
                case "Sql":
                    return new SqlDataAdapter((SqlCommand)command);
                case "Oracle":
                    return new OracleDataAdapter((OracleCommand)command);
                default:
                    throw new InvalidOperationException("Unsupported database configuration.");
            }
        }
    }
}