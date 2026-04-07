using Dapper;
using WaiterBackend.Models;

namespace WaiterBackend.Services.Endpoints;

public class LokasiService
{
    private readonly Database.Database _db;
    public LokasiService(Database.Database db) => _db = db;

    
    public List<Lokasi> GetAll()
    {
        using var connection = _db.GetConnection();
        return connection.Query<Lokasi>("SELECT * FROM Lokasi").ToList();
    }

    public void Post(Lokasi lokasi)
    {
        using var connection = _db.GetConnection();
        var sql = "INSERT INTO Lokasi (name) VALUES (@name)";
        connection.Execute(sql, lokasi);
    }

    public bool Delete(int id)
    {
        using var connection = _db.GetConnection();
        var sql = "DELETE FROM Lokasi WHERE Id = @Id";
        return connection.Execute(sql, new { Id = id }) > 0;
    }
}