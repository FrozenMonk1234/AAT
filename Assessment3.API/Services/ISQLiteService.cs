using Dapper;

namespace Assessment3.API.Services
{
    public interface ISQLiteService
    {
        Task<List<T>> RequestAll<T>(string query, DynamicParameters? parameters = null);
        Task<T> Request<T>(string query, DynamicParameters? parameters = null);
        Task<int> Execute(string query, DynamicParameters? parameters = null);
    }
}
