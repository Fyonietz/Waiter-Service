using WaiterBackend.Services.Endpoints;
using WaiterBackend.Models;

namespace WaiterBackend.Api;

public static class OrderApi
{
    public record OrderRequest(OrderOnWaiter Order, List<OrderItem> Items);

    public static void MapOrderApi(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/order");

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

        group.MapGet("/status/{id}", async (int id, OrderService service) =>
        {
            try
            {
                return Results.Ok(await service.GetOrdersByStatus(id));
            }
            catch (Exception ex)
            {
                return Results.InternalServerError(ex.Message);
            }
        });

        group.MapPatch("/{id}/status", async (int id, int statusId, OrderService service) =>
        {
            try
            {
                var success = await service.UpdateStatus(id, statusId);
                return success
                    ? Results.Ok("Status diperbarui")
                    : Results.NotFound();
            }
            catch (Exception ex)
            {
                return Results.InternalServerError(ex.Message);
            }
        });

        group.MapGet("/{id}", async (int id, OrderService service) =>
        {
            try
            {
                var detail = await service.GetOrderDetails(id);
                return detail != null
                    ? Results.Ok(detail)
                    : Results.NotFound();
            }
            catch (Exception ex)
            {
                return Results.InternalServerError(ex.Message);
            }
        });
    }
}