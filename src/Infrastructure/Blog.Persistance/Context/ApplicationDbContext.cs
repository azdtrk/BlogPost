using Blog.Domain.Entities;
using Blog.Persistance.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Blog.Persistance.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationUserRole, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<BlogPost>? BlogPosts { get; set; }
        public DbSet<User>? DomainUsers { get; set; }
        public DbSet<Comment>? Comments { get; set; }
        public DbSet<Image>? Images { get; set; }
        public DbSet<ExceptionLog>? ExceptionLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UsersConfiguration());
            modelBuilder.ApplyConfiguration(new BlogPostsConfiguration());
            modelBuilder.ApplyConfiguration(new CommentsConfiguration());
            modelBuilder.ApplyConfiguration(new ExceptionLogConfiguration());

            modelBuilder.Entity<IdentityUser>()
                        .Ignore(c => c.TwoFactorEnabled)
                        .Ignore(c => c.ConcurrencyStamp)
                        .Ignore(c => c.EmailConfirmed);

            base.OnModelCreating(modelBuilder);
        }
    }
}
