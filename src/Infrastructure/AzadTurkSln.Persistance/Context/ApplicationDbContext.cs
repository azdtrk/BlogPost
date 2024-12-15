using AzadTurkSln.Domain.Entities;
using AzadTurkSln.Persistance.Configurations;
using Microsoft.EntityFrameworkCore;

namespace AzadTurkSln.Persistance.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UsersConfiguration());
            modelBuilder.ApplyConfiguration(new BlogPostsConfiguration());
            modelBuilder.ApplyConfiguration(new CommentsConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
