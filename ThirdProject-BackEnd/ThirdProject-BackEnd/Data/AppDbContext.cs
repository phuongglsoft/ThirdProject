using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ThirdProject_BackEnd.Models;

namespace ThirdProject_BackEnd.Data
{

    public class AppDbContext : DbContext
    {
        protected readonly IConfiguration _configuration;
        public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
            try
            {
                Database.EnsureCreated();
            }
            catch (Exception err)
            {
                Console.WriteLine($"Error connection to Db with connection string: {base.Database.GetConnectionString()}");
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(a => a.RefreshToken)
                .WithOne(b => b.user)
                .HasForeignKey<RefreshToken>(b => b.user_name);
        }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}
