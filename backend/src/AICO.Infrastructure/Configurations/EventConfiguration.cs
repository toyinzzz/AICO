using AICO.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AICO.Infrastructure.Data.Configurations
{
    public class EventConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.EventType)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50); // Max length for the string representation of the enum

            builder.Property(e => e.EventData)
                .HasColumnType("jsonb"); // Assuming PostgreSQL, adjust if different DB

            builder.Property(e => e.UserAgent).HasMaxLength(500);
            builder.Property(e => e.IpAddress).HasMaxLength(45); // Max length for IPv6

            builder.Property(e => e.Timestamp).IsRequired();

            // Relationships
            builder.HasOne(e => e.Website)
                .WithMany() // Assuming Website doesn't have a direct collection of Events, or configured elsewhere
                .HasForeignKey(e => e.WebsiteId)
                .IsRequired();

            builder.HasOne(e => e.Session)
                .WithMany(s => s.Events) // Assuming Session has a collection of Events
                .HasForeignKey(e => e.SessionId)
                .IsRequired(false); // SessionId is nullable

            // BaseEntity properties
            builder.Property(e => e.CreatedAt).IsRequired();
            builder.Property(e => e.ModifiedAt).IsRequired();
            builder.Property(e => e.RowVersion).IsRowVersion();
        }
    }
}