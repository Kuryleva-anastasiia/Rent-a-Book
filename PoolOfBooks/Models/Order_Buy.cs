using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PoolOfBooks.Models
{
    public class Order_Buy
    {
        public Order_Buy() { }
        [HiddenInput(DisplayValue = false)]
        public int id { get; set; }
        [DisplayName("Клиент")]
        [Required]
        public int id_client { get; set; }
        [DisplayName("Дата покупки")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy}")]
        [Required]
        public DateTime date { get; set; }
        [UIHint("Decimal")]
        [DisplayName("Сумма")]
        public decimal? sum { get; set; }
        [DataType(DataType.MultilineText)]
        [UIHint("MultilineText")]
        [DisplayName("Адрес доставки")]
        public string? address { get; set; }
        [DisplayName("Статус")]
        [Required]
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
