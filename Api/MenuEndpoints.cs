using WaiterBackend.Database;
using WaiterBackend.Services.Endpoints;
using WaiterBackend.Models;
public static class ItemEndpoints
{
    public static void MapItemEndpoints(this WebApplication app)
    {
        var path = app.MapGroup("/api/Menu");

        path.MapGet("/", (ItemService service) =>
        {
            return Results.Ok(service.Get());
        });

        path.MapPost("/", (ItemService service, Item item) => {
            var res = service.Post(item);
            return Results.Json(new
            {
                status = "Success",
            });
        });

        path.MapPost("/delete", (ItemService service, Item item) => {
            return service.Delete(item) ? Results.NoContent() : Results.NotFound();
        });

        path.MapPut("/", (ItemService service, Item item) => {

            return service.Update(item) ? Results.Json(new
            {
                status = "Success"
            }) : Results.NotFound();
        });
    }
}
