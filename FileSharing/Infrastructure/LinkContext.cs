using FileSharing.Models;
using Microsoft.EntityFrameworkCore;

namespace FileSharing.Infrastructure
{
    public class LinkContext : DbContext
    {
        public DbSet<Link> Files { get; set; }

        public LinkContext(DbContextOptions<LinkContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
