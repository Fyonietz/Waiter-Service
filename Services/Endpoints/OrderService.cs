using Dapper;
using WaiterBackend.Models;
using WaiterBackend.Services.Databases;

namespace WaiterBackend.Services.Endpoints;

public class OrderService
{
    private readonly Database _db;
    public OrderService(Database db) => _db = db;

    private class MenuPrice
    {
        public int Id { get; set; } = 0;
        public decimal Price { get; set; } = 0;
    }
    public async Task<bool> CreateOrder(OrderOnWaiter order, List<OrderItem> items)
    {
        using var conn = _db.GetConnection();
        using var transaction = conn.BeginTransaction();

        try
        {
            var sqlOrder = @"INSERT INTO Orders_On_Waiter (UserId, CustomerName, Date, StatusId, LocationId) 
                         VALUES (@UserId, @CustomerName, @Date, @StatusId, @LocationId)";

            await conn.ExecuteAsync(sqlOrder, order, transaction);

            var orderId = await conn.QueryFirstOrDefaultAsync<int>("SELECT last_insert_rowid()", null, transaction);

            // Ambil semua MenuId yang dibutuhkan sekaligus
            var menuIds = items.Select(i => i.MenuId).Distinct();
            var prices = (await conn.QueryAsync<MenuPrice>(
     "SELECT Id, Price FROM Menus WHERE Id IN @Ids",
     new { Ids = menuIds },
     transaction
 )).ToDictionary(m => (int)m.Id, m => (decimal)m.Price);

            var sqlItem = @"INSERT INTO Order_Items (OrderId, MenuId, Quantity, PriceAtOrder) 
                        VALUES (@OrderId, @MenuId, @Quantity, @PriceAtOrder)";

            foreach (var item in items)
            {
                item.OrderId = orderId;

                // Kalau MenuId tidak ditemukan di DB, batalkan transaksi
                if (!prices.TryGetValue(item.MenuId ?? 0, out var unitPrice) || item.MenuId == null)
                    throw new Exception($"Menu ID {item.MenuId} tidak ditemukan.");

                item.PriceAtOrder = item.Quantity * unitPrice; // otomatis dihitung
                await conn.ExecuteAsync(sqlItem, item, transaction);
            }

            transaction.Commit();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR ORDER: {ex.Message}");
            transaction.Rollback();
            return false;
        }
    }

    public async Task<IEnumerable<dynamic>> GetOrdersByStatus(int statusId)
    {
        using var conn = _db.GetConnection();

        var sql = @"
    SELECT o.*, u.Name as WaiterName, s.Name as StatusName, l.Name as TableName
    FROM Orders_On_Waiter o
    JOIN Users u ON o.UserId = u.Id
    JOIN Statuses s ON o.StatusId = s.Id
    JOIN Locations l ON o.LocationId = l.Id
    WHERE o.StatusId = @StatusId";

        return await conn.QueryAsync(sql, new { StatusId = statusId });
    }

    public async Task<bool> UpdateStatus(int orderId, int statusId)
    {
        using var conn = _db.GetConnection();
        string sql = "UPDATE Orders_On_Waiter SET StatusId = @StatusId WHERE Id = @Id";
        return await conn.ExecuteAsync(sql, new { StatusId = statusId, Id = orderId }) > 0;
    }

    public async Task<dynamic?> GetOrderDetails(int orderId)
    {
        using var conn = _db.GetConnection();
        var header = await conn.QueryFirstOrDefaultAsync(@"
        SELECT o.*, u.Name as WaiterName, l.Name as LocationName, s.Name as StatusName
        FROM Orders_On_Waiter o
        JOIN Users u ON o.UserId = u.Id
        JOIN Locations l ON o.LocationId = l.Id
        JOIN Statuses s ON o.StatusId = s.Id
        WHERE o.Id = @Id", new { Id = orderId });

        if (header == null) return null;

        var items = await conn.QueryAsync(@"
        SELECT i.*, m.Name as MenuName
        FROM Order_Items i
        JOIN Menus m ON i.MenuId = m.Id
        WHERE i.OrderId = @OrderId", new { OrderId = orderId });

        return new { Order = header, Items = items };
    }

    public async Task<IEnumerable<dynamic>> GetAllOrders()
    {
        using var conn = _db.GetConnection();
        var sql = @"
        SELECT o.*, u.Name as WaiterName, s.Name as StatusName, l.Name as TableName
        FROM Orders_On_Waiter o
        JOIN Users u ON o.UserId = u.Id
        JOIN Statuses s ON o.StatusId = s.Id
        JOIN Locations l ON o.LocationId = l.Id";
        return await conn.QueryAsync(sql);
    }
}