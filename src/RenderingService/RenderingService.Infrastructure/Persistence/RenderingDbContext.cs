namespace RenderingService.Infrastructure.Persistence
{
    public class RenderingDbContext(DbContextOptions<RenderingDbContext> options) : DbContext(options)
    {
        public DbSet<RenderedForm> RenderedForms => Set<RenderedForm>();

        protected override void OnModelCreating(ModelBuilder b)
        {
            b.Entity<RenderedForm>(e =>
            {
                e.HasKey(x => x.Id);
                e.HasIndex(x => new { x.TenantId, x.Name });
            });
        }
    }
}
