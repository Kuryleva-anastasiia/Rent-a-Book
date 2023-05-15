using System.ComponentModel.DataAnnotations.Schema;

namespace PoolOfBooks.Models
{
    public class Order_Rent
    {

        public int id { get; set; }

        public int id_client { get; set; }
        public DateTime date_begin { get; set; }
        public DateTime date_end { get; set; }
        public decimal? sum { get; set; }
        public string? address { get; set; }
        public string? status { get; set; }
        public Users Users { get; set; } = null!;
        public ICollection<Order_Rent_Books> RentBooks { get; } = new List<Order_Rent_Books>();

        public Order_Rent(int id_client, DateTime date_begin, DateTime date_end, decimal? sum, string? address, string? status)
        {
            this.id_client = id_client;
            this.date_begin = date_begin;
            this.date_end = date_end;
            this.sum = sum;
            this.address = address;
            this.status = status;
        }
    }
}
