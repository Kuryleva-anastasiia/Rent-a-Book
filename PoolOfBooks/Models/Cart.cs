using System.ComponentModel.DataAnnotations.Schema;

namespace PoolOfBooks.Models
{
    public class Cart
    {
        public Cart() { }
        public int id { get; set; }       
        public int id_client { get; set; }
        [ForeignKey("id_client")]
        public Users? Users { get; set; }
        public int id_book { get; set; }
        [ForeignKey("id_book")]
        public Books? Books { get; set; }
        public string? status { get; set; }

        public Cart(int client, int book, string st) { 
            id_client = client;
            id_book = book;
            status = st;
        }
    }
}
