namespace PoolOfBooks.Models
{
    public class Order_Buy_Books
    {
        public Order_Buy_Books() { }
        public int id { get; set; }
        public int id_order { get; set; }
        public Order_Buy Order_Buy { get; set; } = null!;
        public int id_book { get; set; }
        public Books Books { get; set; } = null!;
        public Order_Buy_Books(int order, int book)
        {
            id_order = order;
            id_book = book;
        }
    }
}
