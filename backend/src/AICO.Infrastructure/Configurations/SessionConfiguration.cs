using AICO.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AICO.Infrastructure.Data.Configurations
{
    public class SessionConfiguration : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            // Table configuration
            builder.ToTable("Sessions");

            // Primary key
            builder.HasKey(s => s.Id);

            // Properties
            builder.Property(s => s.VisitorId)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(s => s.UserAgent)
                .HasMaxLength(500);

            builder.Property(s => s.IpAddress)
                .HasMaxLength(45);

            builder.Property(s => s.Referrer)
                .HasMaxLength(2048);

            builder.Property(s => s.EntryPage)
                .HasMaxLength(2048);

            builder.Property(s => s.StartedAt)
                .IsRequired();

            builder.Property(s => s.HasConverted)
                .HasDefaultValue(false);

            // Relationships
            builder.HasOne(s => s.Website)
                .WithMany()
                .HasForeignKey(s => s.WebsiteId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(s => s.Events)
                .WithOne(e => e.Session)
                .HasForeignKey(e => e.SessionId);

            // Indexes
            builder.HasIndex(s => s.WebsiteId);
            builder.HasIndex(s => s.VisitorId);
            builder.HasIndex(s => s.StartedAt);
            builder.HasIndex(s => new { s.WebsiteId, s.VisitorId });
        }
    }
}