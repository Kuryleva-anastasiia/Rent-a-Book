using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PoolOfBooks.Models;

namespace PoolOfBooks.Data
{
    public class PoolOfBooksContext : DbContext
    {
        public PoolOfBooksContext (DbContextOptions<PoolOfBooksContext> options)
            : base(options)
        {
        }

        public DbSet<PoolOfBooks.Models.Users> Users { get; set; } = default!;

        public DbSet<PoolOfBooks.Models.Book_Queue> Book_Queue { get; set; } = default!;

        public DbSet<PoolOfBooks.Models.Books> Books { get; set; } = default!;

        public DbSet<PoolOfBooks.Models.Cart> Cart { get; set; } = default!;

        public DbSet<PoolOfBooks.Models.Category> Category { get; set; } = default!;

        public DbSet<PoolOfBooks.Models.Order_Buy> Order_Buy { get; set; } = default!;

        public DbSet<PoolOfBooks.Models.Order_Buy_Books> Order_Buy_Books { get; set; } = default!;

        public DbSet<PoolOfBooks.Models.Order_Rent> Order_Rent { get; set; } = default!;

        public DbSet<PoolOfBooks.Models.Order_Rent_Books> Order_Rent_Books { get; set; } = default!;
    }
}
