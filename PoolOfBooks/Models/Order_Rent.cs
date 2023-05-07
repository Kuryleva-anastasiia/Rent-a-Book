using System.ComponentModel.DataAnnotations.Schema;

namespace PoolOfBooks.Models
{
    public class Order_Rent
    {

        public int id { get; set; }

        public int id_client { get; set; }
        public DateTime date_begin { get; set; }
        public DateTime date_end { get; set; }
        public float? sum { get; set; }
        public string? address { get; set; }
        public string? status { get; set; }
        [ForeignKey("id_client")]
        public Users? Users { get; set; }
    }
}
