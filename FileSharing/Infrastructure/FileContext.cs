using FileSharing.Models;
using Microsoft.EntityFrameworkCore;

namespace FileSharing.Infrastructure
{
    public class FileContext : DbContext
    {
        public DbSet<File> Files { get; set; }

        public FileContext(DbContextOptions<FileContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
