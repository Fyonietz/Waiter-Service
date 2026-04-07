using WaiterBackend.Services.Endpoints;
using WaiterBackend.Models;

public static class LokasiEndpoints
{
    public static void MapLokasiEndpoints(this WebApplication app)
    {
        var path = app.MapGroup("/api/v2/lokasi");

        path.MapGet("/", (LokasiService service) =>
        {
            return Results.Ok(service.GetAll());
        });

        path.MapPost("/", (LokasiService service, Lokasi lokasi) =>
        {
            service.Post(lokasi);
            return Results.Json(new { status = "Success", message = "Meja Berhasil Ditambahkan" });
        });

        path.MapDelete("/{id:int}", (LokasiService service, int id) =>
        {
            return service.Delete(id) ? Results.Json(new { status = "Success", message = "Meja Berhasil Dihapus" }) : Results.NotFound();
        });
    }
}