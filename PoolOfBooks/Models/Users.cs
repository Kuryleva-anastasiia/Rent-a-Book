using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PoolOfBooks.Models
{
    public class Users
    {
        [HiddenInput(DisplayValue = false)]
        public int id { get; set; }
        [DisplayName("Email")]
        [DataType(DataType.EmailAddress)]
        [Required]
        public string login { get; set; } = "";
        [DisplayName("Телефон")]
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string phone { get; set; } = "";
        [Required]
        [DisplayName("Пароль")]
        [DataType(DataType.Password)]
        public string? password { get; set; }
        [DisplayName("Роль")]
        public string? role { get; set; } = "client";
        [DisplayName("Фамилия")]
        public string? last_name { get; set; }
        [DisplayName("Имя")]
        public string? first_name { get; set; }
        [DisplayName("Отчество")]
        public string? third_name { get; set; } = "";
        [DisplayName("Адрес доставки")]
        [DataType(DataType.MultilineText)]
        public string? address { get; set; } = "";
        public ICollection<Cart> Carts { get; } = new List<Cart>();
        public ICollection<Order_Rent> Order_Rent { get; } = new List<Order_Rent>();
        public ICollection<Order_Buy> Order_Buy { get; } = new List<Order_Buy>();
    }


}
