using AICO.Domain.Entities;
using AICO.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AICO.Infrastructure.Configurations
{
    /// <summary>
    /// Entity Framework configuration for Event entity
    /// </summary>
    public class EventConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.ToTable("Events");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .ValueGeneratedNever();

            builder.Property(e => e.WebsiteId)
                .IsRequired();

            builder.Property(e => e.EventType)
                .HasConversion(
                    v => v.Value,
                    v => EventType.Create(v))
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(e => e.EventData)
                .IsRequired();

            builder.Property(e => e.UserAgent)
                .HasMaxLength(500);

            builder.Property(e => e.IpAddress)
                .HasMaxLength(45);

            builder.Property(e => e.Timestamp)
                .IsRequired();

            builder.Property(e => e.CreatedAt)
                .IsRequired();

            builder.Property(e => e.RowVersion)
                .IsRowVersion();

            builder.HasOne(e => e.Website)
                .WithMany()
                .HasForeignKey(e => e.WebsiteId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Session)
                .WithMany(s => s.Events)
                .HasForeignKey(e => e.SessionId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasQueryFilter(e => !e.IsDeleted);
        }
    }
}