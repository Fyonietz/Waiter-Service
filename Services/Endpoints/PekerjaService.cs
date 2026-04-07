using WaiterBackend.Database;
using WaiterBackend.Models;
using Microsoft.Data.Sqlite;
using Dapper;

namespace WaiterBackend.Services.Endpoints;

public class PekerjaService
{
	private readonly Database.Database _db;
	public PekerjaService(Database.Database db) => _db = db;

	public List<Pekerja> GetAll()
	{
		using var connection = _db.GetConnection();
		var sql = "SELECT * FROM Pekerja";
		return connection.Query<Pekerja>(sql).ToList();
		
	}

	public void Post(Pekerja pekerja)
	{
		using var connection = _db.GetConnection();
		connection.Open();

		var sql = @"INSERT INTO Pekerja 
                (Name, Username, Password, Roles, LokasiId) 
                VALUES (@Name, @Username, @Password, @Roles, @LokasiId)";

		connection.Execute(sql, new
		{
			pekerja.Name,
			pekerja.Username,
			pekerja.Password,
			pekerja.Roles,
			pekerja.LokasiId
		});
	}
	public bool Update(Pekerja pekerja)
	{
		using var connection = _db.GetConnection();
		var sql = "INSERT INTO Pekerja (Name, Username, Password, Roles, LokasiId) VALUES (@Name, @Username, @Password, @Roles, @LokasiId)";
		var affectedRows = connection.Execute(sql, pekerja);
		return affectedRows > 0;
	}

	public bool Delete(Pekerja pekerja)
	{
		using var connection = _db.GetConnection();
		var sql = "DELETE FROM Pekerja WHERE Id = @Id";
		var affectedRows = connection.Execute(sql, new { Id = pekerja.Id });
		return affectedRows > 0;
	}
}