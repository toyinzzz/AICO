using AICO.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AICO.Infrastructure.Data.Configurations
{
    public class MetricConfiguration : IEntityTypeConfiguration<Metric>
    {
        public void Configure(EntityTypeBuilder<Metric> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.Name)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(m => m.Category)
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(m => m.Value)
                .IsRequired();

            builder.Property(m => m.Unit)
                .HasMaxLength(50);

            builder.Property(m => m.Dimension)
                .HasMaxLength(100);

            builder.Property(m => m.DimensionValue)
                .HasMaxLength(200);

            builder.Property(m => m.RecordedAt)
                .IsRequired();

            // Configure relationship with Website
            builder.HasOne(m => m.Website)
                .WithMany()
                .HasForeignKey(m => m.WebsiteId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure BaseEntity properties
            builder.Property(m => m.CreatedAt)
                .IsRequired();

            builder.Property(m => m.UpdatedAt)
                .IsRequired();
        }
    }
}