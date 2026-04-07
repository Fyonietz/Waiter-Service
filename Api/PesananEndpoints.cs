using WaiterBackend.Services.Endpoints;
using WaiterBackend.Models;

public static class PesananEndpoints
{
    public static void MapPesananEndpoints(this WebApplication app)
    {
        var path = app.MapGroup("/api/v2/pesanan");

    
        path.MapPost("/checkout", (PesananService service, OrderRequest req) =>
        {
            var id = service.CreateOrder(req.Header, req.Cart);
            return Results.Ok(new { status = "Success", orderId = id });
        });

        path.MapGet("/", (PesananService service) => Results.Ok(service.GetAllOrders()));
    }
}

public record OrderRequest(Pesanan Header, List<DetailPesanan> Cart);