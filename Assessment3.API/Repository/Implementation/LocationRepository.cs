using Assessment3.API.Models;
using Assessment3.API.Services;
using Dapper;
using System.Net;

namespace Assessment3.API.Repository.Implementation
{
    public class LocationRepository : ILocationRepository
    {
        private readonly ISQLiteService dapper;
        public LocationRepository(ISQLiteService dapper)
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
                    "Street TEXT NOT NULL, Suburb TEXT NOT NULL,  City TEXT NOT NULL,  PostalCode TEXT NOT NULL,  Country TEXT NOT NULL,    " +
                    "DateCreated DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,    DateModified DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP)";
                await dapper.Execute(query);
            }

        }
        public async Task<int> CreateAsync(Location entity)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.AddDynamicParams(entity.City);
            parameters.AddDynamicParams(entity.Country);
            parameters.AddDynamicParams(entity.Street);
            parameters.AddDynamicParams(entity.Number);
            parameters.AddDynamicParams(entity.EventId);
            parameters.AddDynamicParams(entity.Suburb);
            parameters.AddDynamicParams(entity.PostalCode);
            parameters.AddDynamicParams(entity.DateCreated);
            parameters.AddDynamicParams(entity.DateModified);
            string query = "INSERT INTO Location " +
                "(City, Country, Street, Number, EventId, Suburb, PostalCode, DateCreated, DateModified) " +
                "VALUES (@City, @Country, @Street, @Number,@EventId,@Suburb,@PostalCode, @DateCreated, @DateModified)";

            return await dapper.Execute(query, parameters);
        }

        public async Task<bool> DeleteByIdAsync(int Id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.AddDynamicParams(Id);
            string query = "DELETE FROM Location WHERE EventId = @Id";
            return await dapper.Execute(query, parameters) > 0;
        }

        public async Task<List<Location>> GetAllAsync(int? Id = null)
        {
            string query = "SELECT * FROM Location";
            return await dapper.RequestAll<Location>(query);
        }

        public async Task<Location> GetById(int Id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.AddDynamicParams(Id);
            string query = "SELECT * FROM Location WHERE EventId = @Id";
            return await dapper.Request<Location>(query, parameters);
        }

        public async Task<bool> UpdateAsync(Location entity)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.AddDynamicParams(entity.Id);
            parameters.AddDynamicParams(entity.City);
            parameters.AddDynamicParams(entity.Country);
            parameters.AddDynamicParams(entity.Street);
            parameters.AddDynamicParams(entity.Number);
            parameters.AddDynamicParams(entity.EventId);
            parameters.AddDynamicParams(entity.Suburb);
            parameters.AddDynamicParams(entity.PostalCode);
            parameters.AddDynamicParams(entity.DateCreated);
            parameters.AddDynamicParams(entity.DateModified);
            string query = "UPDATE Location SET " +
                            "City = @City," +
                            "Country = @Country," +
                            "Street = @Street," +
                            "Number = @Number," +
                            "EventId = @EventId," +
                            "Suburb = @Suburb," +
                            "PostalCode = @PostalCode," +
                            "DateCreated = @DateCreated," +
                            "DateModified = @DateModified" +
                            " WHERE Id = @Id";
            return await dapper.Execute(query, parameters) > 0;
        }
    }
}
