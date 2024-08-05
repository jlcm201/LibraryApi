using LibraryApi.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Api.Data
{
    public class LibraryContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<LibraryLoan> LibraryLoans { get; set; }

        public LibraryContext(DbContextOptions<LibraryContext> options)
        : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("LibraryDB");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                .HasOne(l => l.Author);

            modelBuilder.Entity<Book>()
                .HasOne(l => l.Category);

            modelBuilder.Entity<LibraryLoan>()
                .HasOne(l => l.User);

            modelBuilder.Entity<LibraryLoan>()
                .HasOne(l => l.Book);
        }
    }
}
