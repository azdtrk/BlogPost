using Blog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Persistance.Configurations
{
    public class ExceptionLogConfiguration : IEntityTypeConfiguration<ExceptionLog>
    {
        public void Configure(EntityTypeBuilder<ExceptionLog> entity)
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Message).IsRequired();
            entity.Property(e => e.Details).IsRequired();
        }

    }
}
