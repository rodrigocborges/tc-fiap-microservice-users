using FIAPCloudGames.Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace FIAPCloudGames.Infrastructure.DatabaseContext
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }
    }
}
