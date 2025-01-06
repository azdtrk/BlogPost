using AzadTurkSln.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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

            /* One-to-many between Comment and CommentReply */
            entity.HasOne(c => c.ParentComment)
                .WithMany(r => r.Replies)
                .HasForeignKey(r => r.ParentCommentId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

        }

    }
}
