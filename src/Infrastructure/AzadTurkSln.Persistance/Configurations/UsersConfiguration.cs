using AzadTurkSln.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AzadTurkSln.Persistance.Configurations
{
    public class UsersConfiguration : IEntityTypeConfiguration<User>
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

            entity.Property(u => u.Password)
                .IsRequired()
                .HasMaxLength(128)
                .HasColumnType("nvarchar(128)");

            entity.Property(u => u.About)
                .HasColumnType("nvarchar(max)");

            entity.Property(u => u.Role)
                .IsRequired()
                .HasColumnType("int");

            /* One-to-many between User and BlogPost */
            entity.HasMany(u => u.BlogPosts)
                .WithOne(bp => bp.Author)
                .HasForeignKey(bp => bp.AuthorId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            /* One-to-many between User and Comment */
            entity.HasMany(u => u.Comments)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            /* One-to-one between User and Image */
            entity.HasOne(u => u.ProfilePhoto)
                .WithOne(i => i.User)
                .HasForeignKey<User>(u => u.ProfilePhotoId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
