using WaiterBackend.Services;
using WaiterBackend.Models;
using WaiterBackend.Services.Endpoints;

namespace WaiterBackend.Api
{
    public static class AuthApi
    {
        public static void MapAuthApi(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("api/auth");

            group.MapPost("/login", async (User loginData, AuthService service) =>
            {
                try
                {
                    var user = await service.Login(
                        loginData.Name ?? "",
                        loginData.Password ?? "");

                    if (user == null)
                        return Results.Json(
                            new { message = "Gagal Login" },
                            statusCode: 401);

                    return Results.Ok(user);
                }
                catch (Exception ex)
                {
                    return Results.InternalServerError(ex.Message);
                }
            });
        }
    }
}