namespace FormsService.Infrastructure.Persistence
{
    public class FormsDbContext : DbContext
    {
        public FormsDbContext(DbContextOptions<FormsDbContext> options): base(options) { }

        public DbSet<Form> Forms => Set<Form>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Form>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.TenantId).HasMaxLength(100).IsRequired();
                e.Property(x => x.Name).HasMaxLength(100).IsRequired();
                e.Property(x => x.Description).HasMaxLength(500);
                e.Property(x => x.EntityId).HasMaxLength(100);
                e.Property(x => x.Version).HasDefaultValue(1);
                e.HasIndex(x => new { x.TenantId, x.Name });
            });
        }
    }
}
