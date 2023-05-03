namespace PoolOfBooks.Models
{
    public class Books
    {
        public int id { get; set; }
        public string? name { get; set; }
        public string? author { get; set; }
        public string? cycle { get; set; }
        public string? description { get; set; }
        public int? id_category { get; set; }
        public string cover { get; set; } = "твердая";
        public int? in_stock { get; set; }
        public decimal? price { get; set; }
        public string status { get; set; } = "аренда";
        public int? count_was_read { get; set; }




    }
}
