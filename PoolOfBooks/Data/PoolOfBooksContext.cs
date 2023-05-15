using System;
using Microsoft.EntityFrameworkCore;
using PoolOfBooks.Models;

namespace PoolOfBooks.Data
{
    public class PoolOfBooksContext : Microsoft.EntityFrameworkCore.DbContext
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



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>()
            .HasMany(e => e.Carts)
            .WithOne(e => e.Users)
            .HasForeignKey(e => e.userId)
            .IsRequired();

           

            modelBuilder.Entity<Books>()
            .HasMany(e => e.Carts)
            .WithOne(e => e.Books)
            .HasForeignKey(e => e.bookId)
            .IsRequired();

            

            modelBuilder.Entity<Users>()
            .HasMany(e => e.Order_Rent)
            .WithOne(e => e.Users)
            .HasForeignKey(e => e.id_client)
            .IsRequired();

            modelBuilder.Entity<Order_Rent>()
            .HasMany(e => e.RentBooks)
            .WithOne(e => e.Order_Rent)
            .HasForeignKey(e => e.id_order)
            .IsRequired();

            modelBuilder.Entity<Books>()
            .HasMany(e => e.RentBooks)
            .WithOne(e => e.Books)
            .HasForeignKey(e => e.id_book)
            .IsRequired();

            modelBuilder.Entity<Users>()
           .HasMany(e => e.Order_Buy)
           .WithOne(e => e.Users)
           .HasForeignKey(e => e.id_client)
           .IsRequired();

            modelBuilder.Entity<Order_Buy>()
            .HasMany(e => e.BuyBooks)
            .WithOne(e => e.Order_Buy)
            .HasForeignKey(e => e.id_order)
            .IsRequired();

            modelBuilder.Entity<Books>()
            .HasMany(e => e.BuyBooks)
            .WithOne(e => e.Books)
            .HasForeignKey(e => e.id_book)
            .IsRequired();

            modelBuilder.Entity<Category>()
            .HasMany(e => e.Books)
            .WithOne(e => e.Category)
            .HasForeignKey(e => e.id_category)
            .IsRequired();

            modelBuilder.Entity<Cart>()
           .HasOne(e => e.Users)
           .WithMany(e => e.Carts)
           .HasForeignKey(e => e.userId)
           .IsRequired();

            modelBuilder.Entity<Cart>()
            .HasOne(e => e.Books)
            .WithMany(e => e.Carts)
            .HasForeignKey(e => e.bookId)
            .IsRequired();

            modelBuilder.Entity<Order_Rent>()
            .HasOne(e => e.Users)
            .WithMany(e => e.Order_Rent)
            .HasForeignKey(e => e.id_client)
            .IsRequired();

            modelBuilder.Entity<Order_Buy>()
            .HasOne(e => e.Users)
            .WithMany(e => e.Order_Buy)
            .HasForeignKey(e => e.id_client)
            .IsRequired();
        }
        //#region Required
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Blog>()
        //        .Property(b => b.Url)
        //        .IsRequired();
        //}
        //#endregion

    }
}
