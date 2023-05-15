namespace PoolOfBooks.Models
{
    public class Category
    {
        public int id { get; set; }
        public string? name { get; set; }
        public ICollection<Books> Books { get; } = new List<Books>();
    }
}
