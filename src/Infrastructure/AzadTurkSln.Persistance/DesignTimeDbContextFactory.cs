using AzadTurkSln.Persistance.Configurations;
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

            dbContextOptionsBuilder.UseSqlServer(ConnectionStringConfiguration.ConnectionString);

            return new ApplicationDbContext(dbContextOptionsBuilder.Options);
        }
    }
}