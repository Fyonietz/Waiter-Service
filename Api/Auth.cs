using WaiterBackend.Services;
using WaiterBackend.Models;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/v2/auth");

        group.MapPost("/login", (AuthService service, LoginRequest req) =>
        {
            var user = service.Login(req.Username, req.Password);

            if (user == null)
                return Results.Json(new { message = "Username atau Password Salah" }, statusCode: 401);

            return Results.Ok(new
            {
                status = "Login Berhasil",
                data = new { user.Name, user.Roles }
            });
        });
    }
}

public record LoginRequest(string Username, string Password);