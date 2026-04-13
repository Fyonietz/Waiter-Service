using WaiterBackend.Services;
using WaiterBackend.Models;
using Microsoft.AspNetCore.Mvc;
using WaiterBackend.Services.Endpoints;

namespace WaiterBackend.Api
{

    public static class MenuApi
    {
        public static void MapMenuApi(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("api/menu");

            group.MapGet("/", async (MenuService service) => Results.Ok(await service.GetAll()));

            group.MapPost("/", async (HttpContext context, MenuService service) =>
            {
                var form = await context.Request.ReadFormAsync();

                var name = form["name"];
                var price = decimal.Parse(form["price"]!);
                var typeId = int.Parse(form["typeId"]!);
                var file = form.Files.GetFile("image");

                string fileName = "";
                if (file != null)
                {
                    fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/menu", fileName);

                    using var stream = new FileStream(path, FileMode.Create);
                    await file.CopyToAsync(stream);
                }

                var newMenu = new Menu
                {
                    Name = name,
                    Price = price,
                    TypeId = typeId,
                    TypeName = fileName
                };

                return await service.Create(newMenu) ? Results.Ok("Menu Created") : Results.BadRequest();
            }).DisableAntiforgery();
        }
    }
}