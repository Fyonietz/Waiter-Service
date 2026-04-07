namespace WaiterBackend.Models
{
    public enum Role
    {
        Waiter,
        Chef,
        Kasir
    }

    public class Pekerja : Base
    {
        public string Name { get; set; } = String.Empty;
        public string Username { get; set; } = String.Empty; 
        public string Password { get; set; } = String.Empty; 
        public Role Roles { get; set; }
        public int LokasiId { get; set; }
    }
}
