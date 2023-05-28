namespace PoolOfBooks.Models
{
    public enum SortState
    {
        NameAsc,    // по имени по возрастанию
        NameDesc,   // по имени по убыванию
        AuthorAsc, // по автору по возрастанию
        AuthorDesc,    // по автору по убыванию
        PriceAsc, // по цене по возрастанию
        PriceDesc, // по цене по убыванию
        StatusRent, // по статусу по возрастанию
        StatusBuy // по статусу по убыванию
    }
}
