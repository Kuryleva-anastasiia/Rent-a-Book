namespace PoolOfBooks.Models
{
    public class Order_Buy
    {
        public Order_Buy() { }
        public int id { get; set; }
        public int id_client { get; set; }
        public DateTime date { get; set; }
        public decimal? sum { get; set; }
        public string? address { get; set; }
        public string? status { get; set; }
        public Users Users { get; set; } = null!;
        public ICollection<Order_Buy_Books> BuyBooks { get; } = new List<Order_Buy_Books>();
        public Order_Buy( int id_client, DateTime date, decimal? sum, string? address, string? status)
        {
            this.id_client = id_client;
            this.date = date;
            this.sum = sum;
            this.address = address;
            this.status = status;
        }
    }
}
