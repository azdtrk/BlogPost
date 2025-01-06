using AzadTurkSln.Domain.Entities;
using AzadTurkSln.Persistance.Configurations;
using Microsoft.EntityFrameworkCore;

namespace AzadTurkSln.Persistance.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<BlogPost>? BlogPosts { get; set; }
        public DbSet<User>? Users { get; set; }
        public DbSet<Comment>? Comments { get; set; }
        public DbSet<Image>? Images { get; set; }
        public DbSet<ExceptionLog>? ExceptionLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UsersConfiguration());
            modelBuilder.ApplyConfiguration(new BlogPostsConfiguration());
            modelBuilder.ApplyConfiguration(new CommentsConfiguration());
            modelBuilder.ApplyConfiguration(new ExceptionLogConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
