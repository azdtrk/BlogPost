using Blog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Persistance.Configurations
{
    public class ApplicationUserRoleConfiguration : IEntityTypeConfiguration<ApplicationUserRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationUserRole> entity)
        {
            // Ensure DomainUserId is nullable
            entity.Property(p => p.DomainUserId).IsRequired(false);

            // Configure the relationship from ApplicationUserRole to DomainUser
            entity.HasOne(aur => aur.DomainUser)
                .WithOne(u => u.ApplicationUserRole)
                .HasForeignKey<ApplicationUserRole>(aur => aur.DomainUserId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
} 