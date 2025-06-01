using Blog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Persistance.Configurations
{
    public class ImageConfiguration : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> entity)
        {
            entity.HasKey(i => i.Id);
            
            entity.Property(i => i.Width);
            entity.Property(i => i.Height);
            entity.Property(i => i.IsThumbnail).HasDefaultValue(false);
            
            // Configure relationship with BlogPost
            entity.HasOne(i => i.BlogPost)
                .WithMany(bp => bp.Images)
                .HasForeignKey(i => i.BlogPostId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);
                
            // Configure relationship with BlogPost for ThumbnailImage
            entity.HasOne(i => i.ThumbnailForBlogPost)
                .WithOne(bp => bp.ThumbnailImage)
                .HasForeignKey<Image>(i => i.ThumbnailForBlogPostId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);
                
            // Configure relationship with Author
            entity.HasOne(i => i.Author)
                .WithOne(a => a.ProfilePhoto)
                .HasForeignKey<Image>(i => i.AuthorId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
} 