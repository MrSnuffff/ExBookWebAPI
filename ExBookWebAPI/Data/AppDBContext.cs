using Microsoft.EntityFrameworkCore;
using ExBookWebAPI.Models;

namespace ExBookWebAPI.Data
{
    public class AppDbContext : DbContext
    {       
        public AppDbContext(DbContextOptions options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Userbook> Userbooks { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Message> Messages { get; set; }    
    }
}
