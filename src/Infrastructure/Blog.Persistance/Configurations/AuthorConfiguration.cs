using Blog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Persistance.Configurations;

public class AuthorConfiguration : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> entity)
    {
        entity.Property(u => u.About)
            .HasColumnType("nvarchar(max)");

        /* One-to-many between User and BlogPost */
        entity.HasMany(u => u.BlogPosts)
            .WithOne(bp => bp.Author)
            .HasForeignKey(bp => bp.AuthorId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);
        
        /* One-to-one between User and Image */
        entity.HasOne(u => u.ProfilePhoto)
            .WithOne(i => i.Author)
            .HasForeignKey<Author>(u => u.ProfilePhotoId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);
    }
}