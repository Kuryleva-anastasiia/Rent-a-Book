namespace PoolOfBooks.Models
{
    public class Order_Rent_Books
    {
        public Order_Rent_Books() { }   
        public int id { get; set; }
        public int id_order { get; set; }
        public Order_Rent Order_Rent { get; set; } = null!;
        public int id_book { get; set; }
        public Books Books { get; set; } = null!;

        public Order_Rent_Books(int order, int book) { 
            id_order = order;
            id_book = book;
        }
    }
}
