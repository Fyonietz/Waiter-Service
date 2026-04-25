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
            {
                try
                {
                    return Results.Ok(await service.GetByRole(roleId));
                }
                catch (Exception ex)
                {
                    return Results.InternalServerError(ex.Message);
                }
            });

            group.MapPost("/", async (User user, UserService service) =>
            {
                try
                {
                    var success = await service.Create(user);
                    return success
                        ? Results.Created($"/api/user/{user.Id}", user)
                        : Results.BadRequest();
                }
                catch (Exception ex)
                {
                    return Results.InternalServerError(ex.Message);
                }
            });

            group.MapGet("/", async (UserService service) =>
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

            group.MapDelete("/{id}", async (int id, UserService service) =>
            {
                try
                {
                    var success = await service.Delete(id);
                    return success
                        ? Results.NoContent()
                        : Results.NotFound();
                }
                catch (Exception ex)
                {
                    return Results.InternalServerError(ex.Message);
                }
            });

            group.MapPut("/{id}", async (int id, User updatedUser, UserService service) =>
            {
                try
                {
                    var result = await service.Update(id, updatedUser);
                    return result
                        ? Results.Ok(updatedUser)
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