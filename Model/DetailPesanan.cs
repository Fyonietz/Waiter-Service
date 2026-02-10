namespace WaiterBackend.Models
{
    public class DetailPesanan : Base
    {
        public int PesananId { get; set; }
        public Pesanan Pesanan { get; set; } = null!;

        public int ItemId { get; set; }
        public Item Item { get; set; } = null!;

        public int Qty { get; set; }
        public decimal Harga { get; set; }
    }
}
