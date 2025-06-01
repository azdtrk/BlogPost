using Blog.Domain.Entities;
using Blog.Persistance.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Blog.Persistance.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationUserRole, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options) 
        {
        }

        public DbSet<BlogPost>? BlogPosts { get; set; }
        public DbSet<User>? DomainUsers { get; set; }
        public DbSet<Author>? Authors { get; set; }
        public DbSet<Reader>? Readers { get; set; }
        public DbSet<Comment>? Comments { get; set; }
        public DbSet<Image>? Images { get; set; }
        public DbSet<Endpoint>? Endpoints { get; set; }
        public DbSet<ExceptionLog>? ExceptionLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply entity configurations
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new BlogPostConfiguration());
            modelBuilder.ApplyConfiguration(new CommentConfiguration());
            modelBuilder.ApplyConfiguration(new ExceptionLogConfiguration());
            modelBuilder.ApplyConfiguration(new ApplicationUserConfiguration());
            modelBuilder.ApplyConfiguration(new ApplicationUserRoleConfiguration());
            modelBuilder.ApplyConfiguration(new ImageConfiguration());
            modelBuilder.ApplyConfiguration(new AuthorConfiguration());
            modelBuilder.ApplyConfiguration(new ReaderConfiguration());
            
            // Ensure Author and Reader tables are properly configured
            modelBuilder.Entity<Author>().ToTable("Authors");
            modelBuilder.Entity<Reader>().ToTable("Readers");
            
            // Ignore unnecessary fields that comes with Identity Framework
            modelBuilder.Entity<IdentityUser>()
                        .Ignore(c => c.TwoFactorEnabled)
                        .Ignore(c => c.ConcurrencyStamp)
                        .Ignore(c => c.EmailConfirmed);
        }
    }
}
