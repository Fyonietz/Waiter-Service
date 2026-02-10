using System.Collections.Generic;

namespace WaiterBackend.Models
{
    public class Client : Base
    {
        public string name { get; set; } = "";

        public int LokasiId { get; set; }
        public Lokasi Lokasi { get; set; } = null!;

        public List<Pesanan> Pesanan { get; set; } = new();
    }
}
