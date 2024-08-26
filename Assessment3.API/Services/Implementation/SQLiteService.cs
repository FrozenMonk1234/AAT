
using Assessment3.API.Constants;
using Dapper;
using System.Data.SQLite;

namespace Assessment3.API.Services.Implementation
{
    public class SQLiteService : ISQLiteService
    {
        public SQLiteService()
        {
            InitDatabase();
        }

        public void InitDatabase()
        {
            if (!File.Exists(AppSettings.ConnectionString))
            {
                File.Create(AppSettings.ConnectionString).Close();
            }
        }
        public async Task<int> Execute(string query, DynamicParameters? parameters = null)
        {
            using var connection = new SQLiteConnection($"Data Source={AppSettings.ConnectionString}");
            if (parameters != null)
                return await connection.ExecuteScalarAsync<int>(query, parameters);
            else
                return await connection.ExecuteScalarAsync<int>(query);
        }

        public async Task<T?> Request<T>(string query, DynamicParameters? parameters = null)
        {
            using var connection = new SQLiteConnection($"Data Source={AppSettings.ConnectionString}");
            if (parameters != null)
                return await connection.QueryFirstOrDefaultAsync<T>(query, parameters);
            else
                return await connection.QueryFirstOrDefaultAsync<T>(query);
        }

        public async Task<List<T>> RequestAll<T>(string query, DynamicParameters? parameters = null)
        {
            using var connection = new SQLiteConnection($"Data Source={AppSettings.ConnectionString}");
            if (parameters != null)
                return (List<T>)await connection.QueryAsync<T>(query, parameters);
            else
                return (List<T>)await connection.QueryAsync<T>(query);

        }
    }
}
