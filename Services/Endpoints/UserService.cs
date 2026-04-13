using Dapper;
using WaiterBackend.Models;
using WaiterBackend.Services.Databases;

namespace WaiterBackend.Services.Endpoints
{

    public class UserService
    {
        private readonly Database _db;
        public UserService(Database db) => _db = db;

        public async Task<IEnumerable<User>> GetByRole(int roleId)
        {
            using var conn = _db.GetConnection();
            string sql = @"SELECT u.*, r.Name as RoleName FROM Users u JOIN Roles r ON u.RoleId = r.Id WHERE u.RoleId = @RoleId";
            return await conn.QueryAsync<User>(sql, new { RoleId = roleId });
        }

        public async Task<bool> Create(User user)
        {
            using var conn = _db.GetConnection();
            string sql = "INSERT INTO Users (Name, Password, RoleId) VALUES (@Name, @Password, @RoleId)";
            return await conn.ExecuteAsync(sql, user) > 0;
        }

        public async Task<bool> Delete(int id)
        {
            using var conn = _db.GetConnection();
            return await conn.ExecuteAsync("DELETE FROM Users WHERE Id = @Id", new { Id = id }) > 0;
        }
    }
}