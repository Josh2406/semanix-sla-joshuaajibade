using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormsService.Infrastructure.Persistence
{
    public class FormsDbContextFactory : IDesignTimeDbContextFactory<FormsDbContext>
    {
        public FormsDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<FormsDbContext>();

            // Local SQL Server Express connection string
            optionsBuilder.UseSqlServer(
                "Server=.\\SQLEXPRESS;Database=FormsServiceDb;Trusted_Connection=True;TrustServerCertificate=True;");

            return new FormsDbContext(optionsBuilder.Options);
        }
    }
}
