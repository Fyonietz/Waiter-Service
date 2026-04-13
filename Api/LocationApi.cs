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

        group.MapGet("/", async (LocationService service) => Results.Ok(await service.GetAll()));

        group.MapPost("/", async (Location location, LocationService service) =>
            await service.Create(location) ? Results.Ok() : Results.BadRequest());

        group.MapDelete("/{id}", async (int id, LocationService service) =>
            await service.Delete(id) ? Results.Ok() : Results.NotFound());
        }
    }
    }