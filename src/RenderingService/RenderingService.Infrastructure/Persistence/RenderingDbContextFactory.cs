namespace RenderingService.Infrastructure.Persistence
{
    internal class RenderingDbContextFactory: IDesignTimeDbContextFactory<RenderingDbContext>
    {
        public RenderingDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<RenderingDbContext>();

            // Local SQL Server Express connection string
            optionsBuilder.UseSqlServer(
                "Server=.\\SQLEXPRESS;Database=RenderingServiceDb;Trusted_Connection=True;TrustServerCertificate=True;");

            return new RenderingDbContext(optionsBuilder.Options);
        }
    }
}
