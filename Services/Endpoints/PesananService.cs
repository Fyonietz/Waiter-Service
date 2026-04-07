using Dapper;
using WaiterBackend.Models;

namespace WaiterBackend.Services.Endpoints;

public class PesananService
{
    private readonly Database.Database _db;
    public PesananService(Database.Database db) => _db = db;

    public int CreateOrder(Pesanan header, List<DetailPesanan> details)
    {
        using var connection = _db.GetConnection();

        var sqlHeader = @"INSERT INTO Pesanan (LokasiId, ClientId, Tanggal, TotalHarga, Status, Note) 
                      VALUES (@LokasiId, @ClientId, DATETIME('now'), @TotalHarga, @Status, @Note);
                      SELECT last_insert_rowid();";

        int pesananId = connection.ExecuteScalar<int>(sqlHeader, header);

        foreach (var item in details)
        {
            var sqlDetail = @"INSERT INTO DetailPesanan (PesananId, ItemId, Qty, TotalHarga) 
                          VALUES (@PesananId, @ItemId, @Qty, @TotalHarga)";


            connection.Execute(sqlDetail, new
            {
                PesananId = pesananId,
                ItemId = item.ItemId,
                Qty = item.Qty,
                TotalHarga = item.TotalHarga
            });
        }

        return pesananId;
    }

    public List<dynamic> GetAllOrders()
    {
        using var connection = _db.GetConnection();

        var sql = @"SELECT p.*, l.name as NamaMeja, c.name as NamaClient 
                    FROM Pesanan p 
                    JOIN Lokasi l ON p.LokasiId = l.Id
                    JOIN Client c ON p.ClientId = c.Id";

        return connection.Query(sql).ToList();
    }
}