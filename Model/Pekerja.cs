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
        public string Name { get; set; } = "";
        public Role Roles { get; set; }

        public int LokasiId { get; set; }
        public Lokasi Lokasi = null!;
    }
}
