using AspNetCoreHero.ToastNotification.Abstractions;
using PoolOfBooks.Controllers;
using System.ComponentModel.DataAnnotations.Schema;

namespace PoolOfBooks.Models
{

    public class Cart
    {
        public Cart() { }
        public int id { get; set; }    
        public int userId { get; set; }
        public Users? Users { get; set; } = null!;
        public int bookId { get; set; }
        public Books? Books { get; set; } = null!;
        public string? status { get; set; }

        public Cart(int user, int book, string status)
        {
            userId = user;
            bookId = book;
            this.status = status;
        }
    }
}
