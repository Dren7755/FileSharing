using FileSharing.Models;
using Microsoft.EntityFrameworkCore;

namespace FileSharing.Infrastructure
{
    public class DataContext : DbContext
    {
        public DbSet<File> Files { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Link> Links { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
