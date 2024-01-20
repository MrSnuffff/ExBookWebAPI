using Microsoft.EntityFrameworkCore;
using Authorization.Models;
using Authorization.Models.User;

namespace Authorization.Data
{
    public class AppDbContext : DbContext
    {       
        public AppDbContext(DbContextOptions options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<VerificationCodes> VerificationCodes { get; set; }

    }
}
