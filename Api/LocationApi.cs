using WaiterBackend.Services;
using WaiterBackend.Models;
using WaiterBackend.Services.Endpoints;

namespace WaiterBackend.Api
{
    public static class LocationApi
    {
        public static void MapLocationApi(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("api/location");

            group.MapGet("/", async (LocationService service) =>
            {
                try
                {
                    return Results.Ok(await service.GetAll());
                }
                catch (Exception ex)
                {
                    return Results.InternalServerError(ex.Message);
                }
            });

            group.MapPost("/", async (Location location, LocationService service) =>
            {
                try
                {
                    var success = await service.Create(location);
                    return success
                        ? Results.Ok()
                        : Results.BadRequest();
                }
                catch (Exception ex)
                {
                    return Results.InternalServerError(ex.Message);
                }
            });

            group.MapDelete("/{id}", async (int id, LocationService service) =>
            {
                try
                {
                    var success = await service.Delete(id);
                    return success
                        ? Results.Ok()
                        : Results.NotFound();
                }
                catch (Exception ex)
                {
                    return Results.InternalServerError(ex.Message);
                }
            });
        }
    }
}