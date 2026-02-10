using WaiterBackend.Database;
using WaiterBackend.Services.Endpoints;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddSingleton<Database>();
builder.Services.AddScoped<ItemService>();

var app = builder.Build();

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

app.MapItemEndpoints();

app.Run();



