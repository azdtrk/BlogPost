using Blog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Persistance.Configurations
{
    public class BlogPostConfiguration : IEntityTypeConfiguration<BlogPost>
    {
        public void Configure(EntityTypeBuilder<BlogPost> entity)
        {
            entity.HasKey(bp => bp.Id);

            entity.Property(bp => bp.DateCreated)
                .HasColumnType("datetime2");

            entity.Property(c => c.Content)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnType("nvarchar(max)");

            entity.Property(c => c.Title)
                .IsRequired()
                .HasColumnType("nvarchar(200)");

            entity.Property(bp => bp.CanBePublished)
                .HasDefaultValue(false)
                .HasColumnType("int");

            entity.Property(bp => bp.DateUpdated)
                .HasColumnType("datetime2");

            /* One-to-many between BlogPost and Image */
            entity.HasMany(bp => bp.Images)
                .WithOne(i => i.BlogPost)
                .HasForeignKey(i => i.BlogPostId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);

            /* One-to-one between BlogPost and Image for ThumbnailImage */
            entity.HasOne(bp => bp.ThumbnailImage)
                .WithOne(i => i.ThumbnailForBlogPost)
                .HasForeignKey<BlogPost>(bp => bp.ThumbnailImageId)
                .IsRequired(false)  // Making it optional
                .OnDelete(DeleteBehavior.NoAction);

            /* One-to-many between BlogPost and Comment */
            entity.HasMany(bp => bp.Comments)
                .WithOne(c => c.BlogPost)
                .HasForeignKey(c => c.BlogPostId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
