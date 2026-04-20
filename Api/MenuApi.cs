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

        group.MapPost("/", async (
       HttpRequest request,
       MenuService service,
       IWebHostEnvironment env) =>
        {
            var form = await request.ReadFormAsync();

            var name = form["Name"].ToString();
            var price = decimal.TryParse(form["Price"], out var p) ? p : 0;
            var typeId = int.TryParse(form["TypeId"], out var t) ? t : 0;
            var typeName = form["TypeName"].ToString();

            var file = form.Files.GetFile("Image");

            if (string.IsNullOrEmpty(name) || price <= 0)
                return Results.BadRequest("Nama menu harus diisi dan harga harus lebih dari 0");

            string? imagePath = null;

            if (file != null && file.Length > 0)
            {
                var uploads = Path.Combine(env.WebRootPath, "images");
                Directory.CreateDirectory(uploads);

                var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                var fullPath = Path.Combine(uploads, fileName);

                using var stream = new FileStream(fullPath, FileMode.Create);
                await file.CopyToAsync(stream);

                imagePath = $"/images/{fileName}";
            }

            var menu = new Menu
            {
                Name = name,
                Price = price,
                TypeId = typeId,
                TypeName = typeName,
                ImageUrl = imagePath
            };

            var success = await service.Create(menu);
            return success ? Results.Ok("Menu berhasil ditambahkan") : Results.BadRequest();
        });

        group.MapDelete("/{id}", async (int id, MenuService service) =>
            await service.Delete(id) ? Results.Ok("Menu dihapus") : Results.NotFound());
    }
}