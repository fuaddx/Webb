using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WebApplication1.Models;

namespace WebApplication1.Contexts
{
    public class DataDbContext : IdentityDbContext
	{
        public DataDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Blog> Blogs { get; set; }
        public  DbSet<AppUser>AppUsers { get; set; }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            IEnumerable<EntityEntry<Blog>> entries = ChangeTracker.Entries<Blog>();

            foreach (EntityEntry<Blog> entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    DateTime utcTime = DateTime.UtcNow;
                    DateTime azTime = utcTime.AddHours(4);
                    entry.Entity.CreatedTime = azTime;
                    entry.Entity.UpdatedTime = null; 
                }
                else if (entry.State == EntityState.Modified)
                {

                    DateTime utcTime = DateTime.UtcNow;
                    DateTime azTime = utcTime.AddHours(4);
                    entry.Entity.UpdatedTime = azTime;

                    // Check if any properties were modified
                    var modifiedProperties = entry.Properties
                        .Where(property => property.IsModified && !property.Metadata.IsPrimaryKey());

                    if (!modifiedProperties.Any())
                    {
                        entry.Entity.UpdatedTime = null;
                    }
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }

    }
}
