namespace WaiterBackend.Models;

public class Menu
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public decimal? Price { get; set; }
    public int? TypeId { get; set; }

    public string? TypeName { get; set; }

    public string? ImageUrl { get; set; }
}