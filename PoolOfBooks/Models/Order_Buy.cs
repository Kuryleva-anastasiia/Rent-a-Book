namespace PoolOfBooks.Models
{
    public class Order_Buy
    {
        public int id { get; set; }
        public int id_client { get; set; }
        public DateTime date { get; set; }
        public float? sum { get; set; }
        public string? address { get; set; }

    }
}
