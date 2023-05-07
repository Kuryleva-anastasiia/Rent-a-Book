namespace PoolOfBooks.Models
{
    public class RentOrdersMid
    {
        public int order_id { get; set; }
        public Order_Rent? Order_Rent { get; set; }
        public int user_id { get; set; }
        public Users? Users { get; set; }
    }
}
