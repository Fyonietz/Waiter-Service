using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using WaiterBackend.Services.Endpoints;
using WaiterBackend.Models;

public static class PekerjaEndpoints
{
	public static void MapPekerjaEndpoints(this WebApplication app)
	{
		var path = app.MapGroup("/api/v2/pekerja");

		path.MapGet("/", (PekerjaService service) => Results.Ok(service.GetAll()));

		path.MapPost("/", (PekerjaService service, Pekerja pekerja) => {
			service.Post(pekerja);
			return Results.Json(new { status = "Success Tambah Pekerja" });
		});

		path.MapGet("/role/{roleId}", (PekerjaService service, int roleId) => {
			var data = service.GetAll().Where(p => (int)p.Roles == roleId);
			return Results.Ok(data);
		});

		

	}
}