using AICO.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AICO.Infrastructure.Data.Configurations
{
    public class AbTestConfiguration : IEntityTypeConfiguration<AbTest>
    {
        public void Configure(EntityTypeBuilder<AbTest> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(t => t.Description).HasMaxLength(1000);
            builder.Property(t => t.Hypothesis).HasMaxLength(500);
            builder.Property(t => t.ExpectedOutcome).HasMaxLength(500);

            builder.Property(t => t.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(t => t.TestType)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(t => t.TargetSelector)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(t => t.OriginalContent).IsRequired();

            builder.Property(t => t.PrimaryMetric)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(t => t.TrafficPercentage);
            builder.Property(t => t.MinimumSampleSize);
            builder.Property(t => t.SignificanceLevel).HasColumnType("decimal(5,4)");
            builder.Property(t => t.StatisticalPower).HasColumnType("decimal(5,4)");
            builder.Property(t => t.BaselineConversionRate).HasColumnType("decimal(5,4)");
            builder.Property(t => t.ExpectedLift).HasColumnType("decimal(5,4)");
            builder.Property(t => t.MinimumDurationDays);
            builder.Property(t => t.AudienceFilter).HasMaxLength(1000);

            // Relationships
            builder.HasOne(t => t.Campaign)
                .WithMany(c => c.AbTests)
                .HasForeignKey(t => t.CampaignId)
                .IsRequired();

            builder.HasMany(t => t.Variants)
                .WithOne(v => v.AbTest)
                .HasForeignKey(v => v.AbTestId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.Conversions)
                .WithOne() // Assuming Conversion doesn't have a direct navigation back to AbTest, or it's not its primary relation
                .HasForeignKey("AbTestId") // Shadow property if no direct navigation
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull); // Or Cascade, depending on requirements

            // BaseEntity properties
            builder.Property(t => t.CreatedAt).IsRequired();
            builder.Property(t => t.ModifiedAt).IsRequired();
            builder.Property(t => t.RowVersion).IsRowVersion();
        }
    }
}