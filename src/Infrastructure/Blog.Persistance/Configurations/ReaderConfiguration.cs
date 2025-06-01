using Blog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Persistance.Configurations;

public class ReaderConfiguration : IEntityTypeConfiguration<Reader>
{
    public void Configure(EntityTypeBuilder<Reader> entity)
    {
        entity.Property(r => r.Preferences)
            .HasColumnType("nvarchar(max)")
            .IsRequired(false);
            
        entity.Property(r => r.ReceiveNotifications)
            .HasDefaultValue(true);
            
        entity.Property(r => r.LastLoginDate)
            .HasDefaultValueSql("GETDATE()");
        
        /* Many-to-many between Reader and BlogPost for saved posts */
        entity.HasMany(r => r.SavedPosts)
            .WithMany()
            .UsingEntity(j => j.ToTable("ReaderSavedPosts"));
    }
} 