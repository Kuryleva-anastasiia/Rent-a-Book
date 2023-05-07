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
        public List<Order_Rent>? Order_Rents { get; set; }
        public List<Books>? Books { get; set; }
    }


}
