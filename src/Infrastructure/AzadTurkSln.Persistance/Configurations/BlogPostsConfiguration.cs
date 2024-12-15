using AzadTurkSln.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AzadTurkSln.Persistance.Configurations
{
    public class BlogPostsConfiguration : IEntityTypeConfiguration<BlogPost>
    {
        public void Configure(EntityTypeBuilder<BlogPost> entity)
        {
            entity.HasKey(bp => bp.Id);

            entity.Property(bp => bp.DateCreated)
                .HasColumnType("datetime2");

            entity.Property(c => c.Content)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnType("nvarchar(100)");

            entity.Property(c => c.Title)
                .IsRequired()
                .HasColumnType("nvarchar(max)");

            entity.Property(bp => bp.CanBePublished)
                .HasDefaultValue(false)
                .HasColumnType("int");

            entity.Property(bp => bp.DateUpdated)
                .HasColumnType("datetime2");



        }
    }
}
