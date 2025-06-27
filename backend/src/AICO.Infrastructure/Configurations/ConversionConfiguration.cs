using AICO.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AICO.Infrastructure.Data.Configurations
{
    public class ConversionConfiguration : IEntityTypeConfiguration<Conversion>
    {
        public void Configure(EntityTypeBuilder<Conversion> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.ConversionType)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50); // Max length for the string representation of the enum

            builder.Property(c => c.GoalName)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(c => c.Value)
                .HasColumnType("decimal(18,2)"); // Precision and scale for monetary value

            builder.Property(c => c.Currency)
                .HasMaxLength(3); // Standard currency code length (e.g., USD, EUR)

            builder.Property(c => c.ConversionData)
                .HasColumnType("jsonb"); // Assuming PostgreSQL, adjust if different DB

            builder.Property(c => c.ConvertedAt).IsRequired();

            // Relationships
            builder.HasOne(c => c.Website)
                .WithMany() // Assuming Website doesn't have a direct collection of Conversions, or configured elsewhere
                .HasForeignKey(c => c.WebsiteId)
                .IsRequired();

            builder.HasOne(c => c.Session)
                .WithMany(s => s.Conversions) // Assuming Session has a collection of Conversions
                .HasForeignKey(c => c.SessionId)
                .IsRequired(false); // SessionId is nullable

            // BaseEntity properties
            builder.Property(c => c.CreatedAt).IsRequired();
            builder.Property(c => c.ModifiedAt).IsRequired();
            builder.Property(c => c.RowVersion).IsRowVersion();
        }
    }
}