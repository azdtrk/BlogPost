using Blog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Persistance.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> entity)
        {

            entity.HasKey(u => u.Id);

            entity.Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(128)
                .HasColumnType("nvarchar(128)");

            entity.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(128)
                .HasColumnType("nvarchar(128)");

            entity.Property(u => u.PasswordHash)
                .IsRequired()
                .HasMaxLength(512)
                .HasColumnType("nvarchar(512)");
            
            /* One-to-many between User and Comment */
            entity.HasMany(u => u.Comments)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            /* One-to-one between DomainUser and ApplicationUser */
            entity.HasOne(u => u.ApplicationUser)
                .WithOne(au => au.DomainUser)
                .HasForeignKey<User>(u => u.ApplicationUserId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);

            /* One-to-one between User and ApplicationUserRole */
            entity.HasOne(u => u.ApplicationUserRole)
                .WithOne(au => au.DomainUser)
                .HasForeignKey<User>(u => u.ApplicationUserRoleId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
