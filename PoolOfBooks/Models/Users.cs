namespace PoolOfBooks.Models
{
    public class Users
    {

        public int id { get; set; }
        public string login { get; set; } = "";
        public string? password { get; set; }
        public string? role { get; set; } = "client";
        public string? last_name { get; set; }
        public string? first_name { get; set; }
        public string? third_name { get; set; }
        public string? address { get; set; } = "";
        public ICollection<Cart> Carts { get; } = new List<Cart>();
        public ICollection<Order_Rent> Order_Rent { get; } = new List<Order_Rent>();
        public ICollection<Order_Buy> Order_Buy { get; } = new List<Order_Buy>();
    }


}
