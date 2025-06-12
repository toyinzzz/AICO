using AICO.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AICO.Infrastructure.Data.Configurations
{
    public class ConversionConfiguration : IEntityTypeConfiguration<Conversion>
    {
        public void Configure(EntityTypeBuilder<Conversion> builder)
        {
            // Table configuration
            builder.ToTable("Conversions");

            // Primary key
            builder.HasKey(c => c.Id);

            // Properties
            builder.Property(c => c.ConversionType)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(c => c.GoalName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.Value)
                .HasPrecision(18, 2);

            builder.Property(c => c.Currency)
                .HasMaxLength(3);

            builder.Property(c => c.ConversionData)
                .HasColumnType("jsonb");

            builder.Property(c => c.ConvertedAt)
                .IsRequired();

            // Relationships
            builder.HasOne(c => c.Website)
                .WithMany()
                .HasForeignKey(c => c.WebsiteId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(c => c.Session)
                .WithMany()
                .HasForeignKey(c => c.SessionId)
                .OnDelete(DeleteBehavior.SetNull);

            // Indexes
            builder.HasIndex(c => c.WebsiteId);
            builder.HasIndex(c => c.SessionId);
            builder.HasIndex(c => c.ConversionType);
            builder.HasIndex(c => c.ConvertedAt);
        }
    }
}