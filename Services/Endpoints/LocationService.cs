using Dapper;
using WaiterBackend.Models;
using WaiterBackend.Services.Databases;

namespace WaiterBackend.Services.Endpoints
{

    public class LocationService
    {
        private readonly Database _db;
        public LocationService(Database db) => _db = db;

        public async Task<IEnumerable<Location>> GetAll()
        {
            using var conn = _db.GetConnection();
            return await conn.QueryAsync<Location>("SELECT * FROM Locations");
        }

        public async Task<bool> Create(Location loc)
        {
            using var conn = _db.GetConnection();
            var sql = "INSERT INTO Locations (Name) VALUES (@Name)";
            return await conn.ExecuteAsync(sql, loc) > 0;
        }

        public async Task<bool> Update(Location loc)
        {
            using var conn = _db.GetConnection();
            var sql = "UPDATE Locations SET Name = @Name WHERE Id = @Id";
            return await conn.ExecuteAsync(sql, loc) > 0;
        }

        public async Task<bool> Delete(int id)
        {
            using var conn = _db.GetConnection();
            return await conn.ExecuteAsync("DELETE FROM Locations WHERE Id = @Id", new { Id = id }) > 0;
        }
    }
}