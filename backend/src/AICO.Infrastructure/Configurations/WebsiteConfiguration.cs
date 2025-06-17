using AICO.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AICO.Infrastructure.Data.Configurations
{
    public class WebsiteConfiguration : IEntityTypeConfiguration<Website>
    {
        public void Configure(EntityTypeBuilder<Website> builder)
        {
            builder.HasKey(w => w.Id);

            builder.Property(w => w.Url)
                .HasMaxLength(2048)
                .IsRequired();

            builder.Property(w => w.Name)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(w => w.Description).HasMaxLength(1000);
            builder.Property(w => w.Industry).HasMaxLength(100);
            builder.Property(w => w.Domain).HasMaxLength(255); // Assuming a standard domain length

            // Relationships
            builder.HasOne(w => w.User)
                .WithMany() // Assuming User does not have a direct collection of Websites, or it's configured elsewhere
                .HasForeignKey(w => w.UserId)
                .IsRequired();

            builder.HasMany(w => w.AnalysisResults)
                .WithOne(ar => ar.Website)
                .HasForeignKey(ar => ar.WebsiteId)
                .OnDelete(DeleteBehavior.Cascade); // If an AnalysisResult cannot exist without a Website

            // BaseEntity properties
            builder.Property(w => w.CreatedAt).IsRequired();
            builder.Property(w => w.ModifiedAt).IsRequired();
            builder.Property(w => w.RowVersion).IsRowVersion();
        }
    }
}