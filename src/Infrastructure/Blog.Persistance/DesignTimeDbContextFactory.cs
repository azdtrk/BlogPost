using Blog.Persistance.Configurations;
using Blog.Persistance.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Blog.Persistance
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {

            DbContextOptionsBuilder<ApplicationDbContext> dbContextOptionsBuilder = new();

            dbContextOptionsBuilder.UseSqlServer(ConnectionStringConfiguration.ConnectionString);

            return new ApplicationDbContext(dbContextOptionsBuilder.Options);
        }
    }
}