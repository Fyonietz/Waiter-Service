using WaiterBackend.Services.Endpoints;
using WaiterBackend.Models;

namespace WaiterBackend.Api;

public static class MenuApi
{
    public static void MapMenuApi(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/menu");

        group.MapGet("/", async (MenuService service) =>
            Results.Ok(await service.GetAll()));

        group.MapPost("/", async (Menu menu, MenuService service) =>
        {
            if (string.IsNullOrEmpty(menu.Name) || menu.Price <= 0)
            {
                return Results.BadRequest("Nama menu harus diisi dan harga harus lebih dari 0");
            }

            var success = await service.Create(menu);
            return success ? Results.Ok("Menu berhasil ditambahkan") : Results.BadRequest();
        });

        group.MapDelete("/{id}", async (int id, MenuService service) =>
            await service.Delete(id) ? Results.Ok("Menu dihapus") : Results.NotFound());
    }
}