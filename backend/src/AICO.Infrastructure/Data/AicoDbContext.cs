using AICO.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AICO.Infrastructure.Data;

public class AicoDbContext : DbContext
{
    public AicoDbContext(DbContextOptions<AicoDbContext> options) : base(options)
    {
    }

    public DbSet<AbTest> AbTests { get; set; }
    public DbSet<Campaign> Campaigns { get; set; }
    public DbSet<Variant> Variants { get; set; }
    public DbSet<Website> Websites { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Session> Sessions { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<Conversion> Conversions { get; set; }
    public DbSet<Revenue> Revenues { get; set; }
    public DbSet<Metric> Metrics { get; set; }
    public DbSet<Snippet> Snippets { get; set; }
    public DbSet<Recommendation> Recommendations { get; set; }
    public DbSet<AnalysisResult> AnalysisResults { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply configurations
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AicoDbContext).Assembly);

        // Configure string properties
        modelBuilder.Entity<User>()
            .Property(u => u.Email)
            .HasMaxLength(256)
            .IsRequired();

        modelBuilder.Entity<Website>()
            .Property(w => w.Url)
            .HasMaxLength(2048)
            .IsRequired();

        modelBuilder.Entity<Website>()
            .Property(w => w.Name)
            .HasMaxLength(200)
            .IsRequired();

        // Configure enums
        modelBuilder.Entity<Event>()
            .Property(e => e.EventType)
            .HasConversion<string>();

        modelBuilder.Entity<Conversion>()
            .Property(c => c.ConversionType)
            .HasConversion<string>();

        modelBuilder.Entity<AbTest>()
            .Property(a => a.TestType)
            .HasConversion<string>();

        modelBuilder.Entity<Metric>()
            .Property(m => m.Category)
            .HasConversion<string>();
    }
}