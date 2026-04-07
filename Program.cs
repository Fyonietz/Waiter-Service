using WaiterBackend.Database;
using WaiterBackend.Services.Endpoints;
using WaiterBackend.Services;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddSingleton<Database>();
builder.Services.AddScoped<ItemService>();
builder.Services.AddScoped<PekerjaService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<LokasiService>();
builder.Services.AddScoped<PesananService>();

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
app.MapPekerjaEndpoints();
app.MapAuthEndpoints();
app.MapLokasiEndpoints();   
app.MapPesananEndpoints();

app.Run();



