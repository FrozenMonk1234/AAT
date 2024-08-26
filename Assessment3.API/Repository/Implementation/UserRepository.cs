using Assessment3.API.Models;
using Assessment3.API.Services;
using Dapper;

namespace Assessment3.API.Repository.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly ISQLiteService dapper;
        public UserRepository(ISQLiteService dapper)
        {
            this.dapper = dapper;
            InitTable();
        }
        private async void InitTable()
        {
            // Execute a query to retrieve the table names
            var tableNames = await dapper.RequestAll<string>("SELECT name FROM sqlite_master WHERE type='table'");

            // Check if the desired table exists
            bool tableExists = tableNames.Any(t => t == "Location");

            if (!tableExists)
            {
                string query = "CREATE TABLE Location (  Id INTEGER PRIMARY KEY AUTOINCREMENT,  EventId INTEGER NOT NULL,  Number INTEGER,  " +
                    "Password TEXT NOT NULL,Street TEXT NOT NULL, Suburb TEXT NOT NULL,  City TEXT NOT NULL,  PostalCode TEXT NOT NULL,  Country TEXT NOT NULL,    " +
                    "DateCreated DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,    DateModified DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP)";
                await dapper.Execute(query);
            }

        }
        public async Task<int> CreateAsync(User entity)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.AddDynamicParams(entity.Name);
            parameters.AddDynamicParams(entity.Surname);
            parameters.AddDynamicParams(entity.DateCreated);
            parameters.AddDynamicParams(entity.DateModified);
            string query = "INSERT INTO User " +
                "(Name, Surname, DateCreated, DateModified) " +
                "VALUES (@Name, @Surname, @DateCreated, @DateModified)";

            return await dapper.Execute(query, parameters);
        }

        public async Task<bool> DeleteByIdAsync(int Id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.AddDynamicParams(Id);
            string query = "DELETE FROM User WHERE Id = @Id";
            return await dapper.Execute(query, parameters) > 0;
        }

        public async Task<List<User>> GetAllAsync(int? Id = null)
        {
            string query = "SELECT * FROM User";
            return await dapper.RequestAll<User>(query);
        }

        public async Task<User> GetById(int Id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.AddDynamicParams(Id);
            string query = "SELECT * FROM User WHERE Id = @Id";
            return await dapper.Request<User>(query, parameters);
        }

        public async Task<bool> UpdateAsync(User entity)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.AddDynamicParams(entity.Id);
            parameters.AddDynamicParams(entity.Name);
            parameters.AddDynamicParams(entity.Surname);
            parameters.AddDynamicParams(entity.DateCreated);
            parameters.AddDynamicParams(entity.DateModified);
            string query = "UPDATE User SET " +
                "Name = @Name," +
                "Surname = @Surname," +
                "DateCreated = @DateCreated," +
                "DateModified = @DateModified" +
                " WHERE Id = @Id";
            return await dapper.Execute(query, parameters) > 0;
        }

        public async Task<User> Login(User user)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.AddDynamicParams(user.Name);
            parameters.AddDynamicParams(user.Password);
            string query = "SELECT * FROM User WHERE Name = @Name AND Password =@Password";
            return await dapper.Request<User>(query, parameters);
        }
    }
}
