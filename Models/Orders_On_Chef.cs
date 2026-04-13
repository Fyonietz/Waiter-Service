namespace WaiterBackend.Models;

public class OrderOnChef
{
    public int Id { get; set; }
    public int? OrderId { get; set; }
    public int? UserId { get; set; }

    public string? UserName { get; set; }
}