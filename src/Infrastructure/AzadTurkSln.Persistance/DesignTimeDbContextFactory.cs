using AzadTurkSln.Persistance.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AzadTurkSln.Persistance
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<ApplicationDbContext> dbContextOptionsBuilder = new();

            dbContextOptionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=MyBlogPostDb;Trusted_connection=true;TrustServerCertificate=true;");

            return new ApplicationDbContext(dbContextOptionsBuilder.Options);
        }
    }
}