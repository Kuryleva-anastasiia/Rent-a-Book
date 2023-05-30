using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Office.Interop.Word;
using Windows.System;

namespace PoolOfBooks.Models
{
    public class BooksFilters
    {
        public IEnumerable<Books> Books { get; set; } = new List<Books>();
        public SelectList Categories { get; set; } = new SelectList(new List<Categories>(), "id", "name");
        public string? name { get; set; }


    }
}
