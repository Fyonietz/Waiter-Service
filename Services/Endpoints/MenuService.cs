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
            var sql = @"INSERT INTO Menus (Name, Price, TypeId, TypeName, ImageUrl) 
           VALUES(@Name, @Price, @TypeId, @TypeName, @ImageUrl)";
            return await conn.ExecuteAsync(sql, menu) > 0;
        }

        public async Task<bool> Delete(int id)
        {
            using var conn = _db.GetConnection();
            return await conn.ExecuteAsync("DELETE FROM Menus WHERE Id = @Id", new { Id = id }) > 0;
        }

        public async Task<bool> UpdateName(int id, Menu updatedMenu)
        {
            using var conn = _db.GetConnection();
            var sql = @"
            UPDATE Menus
            SET Name = @Name
            WHERE Id = @Id";
            return await conn.ExecuteAsync(sql, new
            {
                Id = id,
                updatedMenu.Name,
            }) > 0;
        }

        public async Task<bool> UpdatePrice(int id, Menu updatedMenu)
        {
            using var conn = _db.GetConnection();
            var sql = @"
            UPDATE Menus
            SET Price = @Price
            WHERE Id = @Id";
            return await conn.ExecuteAsync(sql, new
            {
                Id = id,
                updatedMenu.Price,
            }) > 0;
        }
    }
}