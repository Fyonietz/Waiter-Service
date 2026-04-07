using System.Collections.Generic;

namespace WaiterBackend.Models
{
    public enum Status
    {
        Cooking,
        Ready
    }

    public class Pesanan : Base
    {   


        public int LokasiId { get; set; }
        public Lokasi Lokasi { get; set; } = null!;

        public int ClientId { get; set; }
        public Client Client { get; set; } = null!;

        public Status Status { get; set; }

        public string? Note { get; set; }
        public decimal TotalHarga { get; set; }

        public List<DetailPesanan> Items { get; set; } = new();
    }
}
