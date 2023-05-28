using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RequiredAttribute = Microsoft.Build.Framework.RequiredAttribute;

namespace PoolOfBooks.Models
{
    public class Order_Rent
    {
        [HiddenInput(DisplayValue = false)]
        public int id { get; set; }

        [DisplayName("Клиент")]
        [Required]
        public int id_client { get; set; }

        [DisplayName("Дата начала аренды")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [Required]
        public DateTime date_begin { get; set; }

        [DisplayName("Дата конца аренды")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [Required]
        public DateTime date_end { get; set; }

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
