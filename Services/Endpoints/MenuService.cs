using Dapper;
using WaiterBackend.Models;
using WaiterBackend.Services.Databases;

namespace WaiterBackend.Services.Endpoints
{

    public class MenuService
    {
        private readonly Database _db;
        public MenuService(Database db) => _db = db;

        public async Task<IEnumerable<Menu>> GetAll()
        {
            using var conn = _db.GetConnection();
            return await conn.QueryAsync<Menu>(@"
            SELECT m.*, t.Name as TypeName 
            FROM Menus m 
            LEFT JOIN Types t ON m.TypeId = t.Id");
        }

        public async Task<bool> Create(Menu menu)
        {
            using var conn = _db.GetConnection();
            var sql = "INSERT INTO Menus (Name, Price, TypeId, TypeName) VALUES (@Name, @Price, @TypeId, @TypeName)";
            return await conn.ExecuteAsync(sql, menu) > 0;
        }

        public async Task<bool> Delete(int id)
        {
            using var conn = _db.GetConnection();
            return await conn.ExecuteAsync("DELETE FROM Menus WHERE Id = @Id", new { Id = id }) > 0;
        }
    }
}