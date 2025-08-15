using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace RenderingService.Infrastructure.Persistence
{
    public class RenderingDbContext : DbContext
    {
        public RenderingDbContext(DbContextOptions<RenderingDbContext> options) : base(options) { }
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
