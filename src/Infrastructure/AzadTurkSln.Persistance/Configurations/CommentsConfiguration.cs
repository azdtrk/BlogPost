using AzadTurkSln.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace AzadTurkSln.Persistance.Configurations
{
    public class CommentsConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> entity)
        {
            entity.HasKey(c => c.Id);

            entity.Property(c => c.Content)
                .IsRequired()
                .HasColumnType("nvarchar(750)");

            entity.Property(c => c.DateCreated)
                .HasColumnType("datetime2");

            entity.Property(c => c.IsApproved)
                .HasColumnType("bit");

            /* One-to-many between Comments and User*/
            entity.HasOne(u => u.User)
                .WithMany(c => c.Comments)
                .HasForeignKey(bp => bp.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            /* One-to-many between Comments and BlogPost*/
            entity.HasOne(bp => bp.BlogPost)
                .WithMany(c => c.Comments)
                .HasForeignKey(c => c.BlogPostId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

        }

    }
}
