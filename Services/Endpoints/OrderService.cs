using Dapper;
using WaiterBackend.Models;
using WaiterBackend.Services.Databases;

namespace WaiterBackend.Services.Endpoints;

public class OrderService
{
    private readonly Database _db;
    public OrderService(Database db) => _db = db;

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

            var sqlItem = @"INSERT INTO Order_Items (OrderId, MenuId, Quantity, PriceAtOrder) 
                        VALUES (@OrderId, @MenuId, @Quantity, @PriceAtOrder)";

            foreach (var item in items)
            {
                item.OrderId = orderId;
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
        JOIN Status s ON o.StatusId = s.Id
        JOIN Locations l ON o.LocationId = l.Id
        WHERE o.StatusId = @StatusId";

        return await conn.QueryAsync(sql, new { StatusId = statusId });
    }
}
