using FormsService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FormsService.Infrastructure.Persistence
{
    public class FormsDbContext(DbContextOptions<FormsDbContext> options) : DbContext(options)
    {
        public DbSet<Form> Forms => Set<Form>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Form>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.TenantId).IsRequired();
                e.Property(x => x.Name).HasMaxLength(100).IsRequired();
                e.Property(x => x.Version).HasDefaultValue(1);
                e.HasIndex(x => new { x.TenantId, x.Name });
            });
        }
    }
}
