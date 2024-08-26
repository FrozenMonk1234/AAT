using Assessment3.API.Constants;
using Assessment3.API.Models;
using Assessment3.API.Services;
using Dapper;
using System.Data.SQLite;
using static Dapper.SqlMapper;

namespace Assessment3.API.Repository.Implementation
{
    public class EventRepository : IEventRepository
    {
        private readonly ISQLiteService dapper;
        public EventRepository(ISQLiteService dapper)
        {
            this.dapper = dapper;
            InitTable();
        }

        private async void InitTable()
        {
            // Execute a query to retrieve the table names
            var tableNames = await dapper.RequestAll<string>("SELECT name FROM sqlite_master WHERE type='table'");

            // Check if the desired table exists
            bool tableExists = tableNames.Any(t => t == "Event");

            if (!tableExists)
            {
                string query = "CREATE TABLE Events (Id INTEGER PRIMARY KEY AUTOINCREMENT, Name TEXT NOT NULL, Description TEXT, TotalSeats INTEGER NOT NULL," +
                    " SeatsTaken INTEGER NOT NULL, Date DATETIME NOT NULL, LocationId INTEGER, DateCreated DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,  DateModified DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP)";
                await dapper.Execute(query);
            }

        }
        public async Task<int> CreateAsync(Event entity)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.AddDynamicParams(entity.Name);
            parameters.AddDynamicParams(entity.Description);
            parameters.AddDynamicParams(entity.TotalSeats);
            parameters.AddDynamicParams(entity.SeatsTaken);
            parameters.AddDynamicParams(entity.Date);
            parameters.AddDynamicParams(entity.DateCreated);
            parameters.AddDynamicParams(entity.DateModified);
            string query = "INSERT INTO Event " +
                "(Name, Description, TotalSeats, SeatsTaken, Date, DateCreated, DateModified) " +
                "VALUES (@Name,@Description, @Total, @SeatsTaken, @Date, @DateCreated, @DateModified)";

            return await dapper.Execute(query, parameters);
        }

        public async Task<bool> DeleteByIdAsync(int Id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.AddDynamicParams(Id);
            string query = "DELETE FROM Event WHERE Id = @Id";
            return await dapper.Execute(query, parameters) > 0;
        }

        public async Task<List<Event>> GetAllAsync(int? Id = null)
        {
            string query = "SELECT * FROM Event";
            return await dapper.RequestAll<Event>(query);
        }

        public async Task<Event> GetById(int Id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.AddDynamicParams(Id);
            string query = "SELECT * FROM Event WHERE Id = @Id";
            return await dapper.Request<Event>(query, parameters);
        }

        public async Task<bool> UpdateAsync(Event entity)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.AddDynamicParams(entity.Id);
            parameters.AddDynamicParams(entity.Name);
            parameters.AddDynamicParams(entity.Description);
            parameters.AddDynamicParams(entity.TotalSeats);
            parameters.AddDynamicParams(entity.SeatsTaken);
            parameters.AddDynamicParams(entity.Date);
            parameters.AddDynamicParams(entity.DateCreated);
            parameters.AddDynamicParams(entity.DateModified);
            string query = "UPDATE Event SET " +
                            "Name = @Name," +
                            "Description = @Description," +
                            "TotalSeats = @TotalSeats," +
                            "SeatsTaken = @SeatsTaken," +
                            "Date = @Date," +
                            "DateCreated = @DateCreated," +
                            "DateModified = @DateModified" +
                            " WHERE Id = @Id";
            return await dapper.Execute(query, parameters) > 0;
        }
    }
}
