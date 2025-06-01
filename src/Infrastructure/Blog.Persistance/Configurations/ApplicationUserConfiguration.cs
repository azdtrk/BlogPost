using Blog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Persistance.Configurations
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> entity)
        {
            // Configure the one-to-one relationship from ApplicationUser side
            entity.HasOne(au => au.DomainUser)
                .WithOne(u => u.ApplicationUser)
                .HasForeignKey<ApplicationUser>(au => au.DomainUserId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
} 