namespace PoolOfBooks.Models
{
    public class Users
    {
        public int id { get; set; }
        public string login { get; set; } = "";
        public byte[]? password { get; set; }
        public string? role { get; set; }
        public string? last_name { get; set; }
        public string? first_name { get; set; }
        public string? third_name { get; set; }
        public string? address { get; set; }
    }
}
