using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PoolOfBooks.Models
{
    public class Books
    {
        [ScaffoldColumn(false)]
        public int id { get; set; }
        [DisplayName("Название")]
        public string? name { get; set; }
        [DisplayName("Автор")]
        public string? author { get; set; }
        [DisplayName("Цикл")]
        public string? cycle { get; set; }
        [DisplayName("Описание")]
        public string? description { get; set; }
        [DisplayName("Жанр")]
        public int? id_category { get; set; }
        [DisplayName("Обложка")]
        public string cover { get; set; } = "твердая";
        [DisplayName("Количество страниц")]
        public int pages { get; set; } = 300;
        [DisplayName("Количество на складе")]
        public int? in_stock { get; set; }
        [DisplayName("Цена")]
        public decimal? price { get; set; }
        [DisplayName("Статус")]
        public string status { get; set; } = "аренда";
        [DisplayName("Количество прочтений")]
        public int? count_was_read { get; set; }
        public ICollection<Cart> Carts { get; } = new List<Cart>();
        public ICollection<Order_Rent_Books> RentBooks { get; } = new List<Order_Rent_Books>();
        public ICollection<Order_Buy_Books> BuyBooks { get; } = new List<Order_Buy_Books>();
        public Category Category { get; set; } = null!;


    }
}
