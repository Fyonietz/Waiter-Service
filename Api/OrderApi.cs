using WaiterBackend.Services.Endpoints;
using WaiterBackend.Models;

namespace WaiterBackend.Api;

public static class OrderApi
{
    public record OrderRequest(OrderOnWaiter Order, List<OrderItem> Items);

    public static void MapOrderApi(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/order");

        // ✅ CREATE ORDER
        group.MapPost("/", async (OrderRequest request, OrderService service) =>
        {
            try
            {
                if (request.Items == null || request.Items.Count == 0)
                    return Results.BadRequest("Pesanan tidak boleh kosong");

                var success = await service.CreateOrder(request.Order, request.Items);

                return success
                    ? Results.Ok("Pesanan berhasil dibuat")
                    : Results.BadRequest("Gagal memproses pesanan");
            }
            catch (Exception ex)
            {
                return Results.InternalServerError(ex.Message);
            }
        });

        // ✅ GET ALL ORDERS
        group.MapGet("/", async (OrderService service) =>
        {
            try
            {
                var orders = await service.GetAllOrders();
                return Results.Ok(orders);
            }
            catch (Exception ex)
            {
                return Results.InternalServerError(ex.Message);
            }
        });

        // ✅ GET ORDERS BY STATUS
        group.MapGet("/status/{statusId:int}", async (int statusId, OrderService service) =>
        {
            try
            {
                var orders = await service.GetOrdersByStatus(statusId);
                return Results.Ok(orders);
            }
            catch (Exception ex)
            {
                return Results.InternalServerError(ex.Message);
            }
        });

        // ✅ UPDATE STATUS
        group.MapPatch("/{id:int}/status/{statusId:int}", async (int id, int statusId, OrderService service) =>
        {
            try
            {
                var success = await service.UpdateStatus(id, statusId);

                return success
                    ? Results.Ok("Status diperbarui")
                    : Results.NotFound($"Order dengan id {id} tidak ditemukan");
            }
            catch (Exception ex)
            {
                return Results.InternalServerError(ex.Message);
            }
        });

        // ✅ GET DETAIL ORDER
        group.MapGet("/{id:int}", async (int id, OrderService service) =>
        {
            try
            {
                var detail = await service.GetOrderDetails(id);

                return detail != null
                    ? Results.Ok(detail)
                    : Results.NotFound($"Order dengan id {id} tidak ditemukan");
            }
            catch (Exception ex)
            {
                return Results.InternalServerError(ex.Message);
            }
        });
    }
}