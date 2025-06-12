using AICO.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AICO.Infrastructure.Data.Configurations
{
    public class MetricConfiguration : IEntityTypeConfiguration<Metric>
    {
        public void Configure(EntityTypeBuilder<Metric> builder)
        {
            // Table configuration
            builder.ToTable("Metrics");

            // Primary key
            builder.HasKey(m => m.Id);

            // Properties
            builder.Property(m => m.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(m => m.Category)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(m => m.Value)
                .IsRequired()
                .HasPrecision(18, 6);

            builder.Property(m => m.Unit)
                .HasMaxLength(20);

            builder.Property(m => m.Dimension)
                .HasMaxLength(50);

            builder.Property(m => m.DimensionValue)
                .HasMaxLength(100);

            builder.Property(m => m.RecordedAt)
                .IsRequired();

            // Relationships
            builder.HasOne(m => m.Website)
                .WithMany()
                .HasForeignKey(m => m.WebsiteId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(m => m.WebsiteId);
            builder.HasIndex(m => m.Name);
            builder.HasIndex(m => m.Category);
            builder.HasIndex(m => m.RecordedAt);
            builder.HasIndex(m => new { m.WebsiteId, m.Name, m.RecordedAt });
        }
    }
}