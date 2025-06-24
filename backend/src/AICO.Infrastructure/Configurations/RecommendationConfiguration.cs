using AICO.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AICO.Infrastructure.Data.Configurations
{
    public class RecommendationConfiguration : IEntityTypeConfiguration<Recommendation>
    {
        public void Configure(EntityTypeBuilder<Recommendation> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.RecommendationType)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(r => r.Title)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(r => r.Description)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(r => r.Priority)
                .IsRequired();

            builder.Property(r => r.ExpectedImpact)
                .HasMaxLength(500);

            builder.Property(r => r.ImplementationEffort)
                .HasMaxLength(50);

            builder.Property(r => r.Status)
                .IsRequired()
                .HasConversion<string>() // Store enum as string
                .HasMaxLength(50);

            builder.Property(r => r.GeneratedAt)
                .IsRequired();

            builder.Property(r => r.ImplementedAt);

            builder.Property(r => r.Metadata)
                .HasColumnType("jsonb"); // Assuming PostgreSQL for JSONB, adjust if different DB

            // Relationships
            builder.HasOne(r => r.Website)
                .WithMany()
                .HasForeignKey(r => r.WebsiteId)
                .OnDelete(DeleteBehavior.Cascade); // Or Restrict, SetNull depending on requirements

            builder.HasOne(r => r.AnalysisResult)
                .WithMany(ar => ar.Recommendations)
                .HasForeignKey(r => r.AnalysisResultId)
                .OnDelete(DeleteBehavior.Cascade); // Or Restrict, SetNull

            // BaseEntity properties are typically configured in a base configuration or directly in DbContext if not complex
            builder.Property(r => r.CreatedAt).IsRequired();
            builder.Property(r => r.ModifiedAt).IsRequired();
            builder.Property(r => r.RowVersion).IsRowVersion();
        }
    }
}