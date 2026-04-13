using Dapper;
using WaiterBackend.Models;
using WaiterBackend.Services.Databases;

namespace WaiterBackend.Services.Endpoints
{

    public class AuthService
    {
        private readonly Database _db;

        public AuthService(Database db) => _db = db;

        public async Task<User?> Login(string name, string password)
        {
            using var conn = _db.GetConnection();
            string sql = @"
            SELECT u.*, r.Name as RoleName 
            FROM Users u 
            LEFT JOIN Roles r ON u.RoleId = r.Id 
            WHERE u.Name = @Name AND u.Password = @Password";

            return await conn.QueryFirstOrDefaultAsync<User>(sql, new { Name = name, Password = password });
        }
    }
}