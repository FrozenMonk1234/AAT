using Assessment3.API.Enums;
using Assessment3.API.Models;
using Assessment3.API.Services;
using Dapper;
using static Assessment3.API.Enums.RegisterEnums;

namespace Assessment3.API.Repository.Implementation
{
    public class RegisterRepository : IRegisterRepository
    {
        private readonly ISQLiteService dapper;
        public RegisterRepository(ISQLiteService dapper)
        {
            this.dapper = dapper;
            InitTable();
        }
        private async void InitTable()
        {
            // Execute a query to retrieve the table names
            var tableNames = await dapper.RequestAll<string>("SELECT name FROM sqlite_master WHERE type='table'");

            // Check if the desired table exists
            bool tableExists = tableNames.Any(t => t == "Register");

            if (!tableExists)
            {
                string query = "CREATE TABLE Register ( Id INTEGER PRIMARY KEY AUTOINCREMENT, EventId INTEGER NOT NULL, " +
                    "UserId INTEGER NOT NULL, DateCreated DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP, DateModified DATETIME " +
                    "NOT NULL DEFAULT CURRENT_TIMESTAMP)";
                await dapper.Execute(query);
            }

        }
        public async Task<int> CreateAsync(Register entity)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.AddDynamicParams(entity.EventId);
            parameters.AddDynamicParams(entity.UserId);
            parameters.AddDynamicParams(entity.DateCreated);
            parameters.AddDynamicParams(entity.DateModified);
            string query = "INSERT INTO Register " +
                "(EventId, UserId, DateCreated, DateModified) " +
                "VALUES (@EventId, @UserId, @DateCreated, @DateModified)";

            return await dapper.Execute(query, parameters);
        }

        public async Task<bool> DeleteByIdAsync(int Id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.AddDynamicParams(Id);
            string query = "DELETE FROM Register WHERE Id = @Id";
            return await dapper.Execute(query, parameters) > 0;
        }

        public async Task<List<Register>> GetAllAsync(int? Id = null)
        {
            string query = "SELECT * FROM Register";
            return await dapper.RequestAll<Register>(query);
        }

        public async Task<List<Register>> GetAllAsync(int? Id, RegisterIdType registerIdType)
        {
            string query = "";
            DynamicParameters parameters = new DynamicParameters();

            switch (registerIdType)
            {
                case RegisterIdType.User:
                    parameters.AddDynamicParams(Id);
                    query = "SELECT * FROM Register WHERE UserId = @Id";
                    break;
                case RegisterIdType.Event:
                    query = "SELECT * FROM Register  WHERE EventId = @Id";
                    break;
            }

            return await dapper.RequestAll<Register>(query, parameters);
        }

        public async Task<Register> GetById(int Id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.AddDynamicParams(Id);
            string query = "SELECT * FROM Register WHERE Id = @Id";
            return await dapper.Request<Register>(query, parameters);
        }

        public async Task<bool> UpdateAsync(Register entity)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.AddDynamicParams(entity.Id);
            parameters.AddDynamicParams(entity.EventId);
            parameters.AddDynamicParams(entity.UserId);
            parameters.AddDynamicParams(entity.DateCreated);
            parameters.AddDynamicParams(entity.DateModified);
            string query = "UPDATE Register SET " +
                "EventId = @EventId," +
                "UserId = @UserId," +
                "DateCreated = @DateCreated," +
                "DateModified = @DateModified" +
                " WHERE Id = @Id";
            return await dapper.Execute(query, parameters) > 0;
        }
    }
}
