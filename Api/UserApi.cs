using WaiterBackend.Services;
using WaiterBackend.Models;
using WaiterBackend.Services.Endpoints;
namespace WaiterBackend.Api
{

    public static class UserApi
    {
        public static void MapUserApi(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("api/user");

            // Contoh: api/user/role/2 untuk mendapatkan semua Waiter
            group.MapGet("/role/{roleId}", async (int roleId, UserService service) =>
                Results.Ok(await service.GetByRole(roleId)));

            group.MapPost("/", async (User user, UserService service) =>
                await service.Create(user) ? Results.Created($"/api/user/{user.Id}", user) : Results.BadRequest());
        }
    }
}