using WaiterBackend.Services.Databases;
using WaiterBackend.Services.Endpoints;
using WaiterBackend.Services;
using System.Diagnostics;
using WaiterBackend.Api;


var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddSingleton<Database>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<LocationService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<MenuService>();
builder.Services.AddScoped<OrderService>();


var app = builder.Build();


app.UseStaticFiles();

// 🔹 FastAPI / Vite-style request logging
app.Use(async (context, next) =>
{
    var sw = Stopwatch.StartNew();

    await next();

    sw.Stop();

    var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";

    Console.WriteLine(
        $"INFO: {ip} - \"{context.Request.Method} {context.Request.Path} " +
        $"{context.Response.StatusCode}\" {sw.ElapsedMilliseconds}ms"
    );
});

// Routes
app.MapGet("/", () => "Hello World!");

app.MapGet("/fadli", () =>
{
    return Results.Json(new
    {
        nama = "Fadli-Mizan",
        umur = 18
    });
});

app.MapAuthApi();
app.MapLocationApi();
app.MapUserApi();
app.MapMenuApi();
app.MapOrderApi();

app.Run();