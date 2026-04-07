using WaiterBackend.Models;
using Microsoft.Data.Sqlite;

namespace WaiterBackend.Services;

public class AuthService
{
    private readonly Database.Database _db;
    public AuthService(Database.Database db) => _db = db;

    public Pekerja? Login(string username, string password)
    {
        using var connection = _db.GetConnection();
        var command = connection.CreateCommand();

     
        command.CommandText = "SELECT Id, Name, Roles FROM Pekerja WHERE Username = @u AND Password = @p";
        command.Parameters.AddWithValue("@u", username);
        command.Parameters.AddWithValue("@p", password);

        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
            return new Pekerja
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Roles = (Role)reader.GetInt32(2)
            };
        }
        return null;
    }
}