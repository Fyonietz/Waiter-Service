using WaiterBackend.Services.Databases;
using WaiterBackend.Services.Endpoints;
using WaiterBackend.Services;
using System.Diagnostics;
using WaiterBackend.Api;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<Database>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<LocationService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<MenuService>();
builder.Services.AddScoped<OrderService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAndroid", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Waiter API v1");
    options.RoutePrefix = string.Empty;
});

app.UseStaticFiles();
app.UseCors("AllowAndroid");

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

app.MapAuthApi();
app.MapLocationApi();
app.MapUserApi();
app.MapMenuApi();
app.MapOrderApi();

app.Run();