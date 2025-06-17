using AICO.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using AICO.Domain.ValueObjects; // Assuming this is where your Value Objects like Money are
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.Storage; // Added for IDbContextTransaction

namespace AICO.Infrastructure.Data
{
    public class AicoDbContext : DbContext, IUnitOfWork
    {
        private IDbContextTransaction _currentTransaction;

        public AicoDbContext(DbContextOptions<AicoDbContext> options) : base(options)
        {
        }

        // Entities
        public DbSet<User> Users { get; set; }
        public DbSet<Website> Websites { get; set; }
        public DbSet<AnalysisResult> AnalysisResults { get; set; }
        public DbSet<Recommendation> Recommendations { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Conversion> Conversions { get; set; }
        public DbSet<ConversionEvent> ConversionEvents { get; set; } // Added this line
        public DbSet<AbTest> AbTests { get; set; }
        public DbSet<Variant> Variants { get; set; }
        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<Metric> Metrics { get; set; }
        public DbSet<Revenue> Revenues { get; set; } // Added this line
        public DbSet<Snippet> Snippets { get; set; } // Added this line
        public DbSet<Report> Reports { get; set; } // Uncommented
        public DbSet<Integration> Integrations { get; set; } // Uncommented
        public DbSet<Notification> Notifications { get; set; } // Uncommented
        public DbSet<AuditLog> AuditLogs { get; set; } // Uncommented
        public DbSet<Setting> Settings { get; set; } // Uncommented
        public DbSet<Subscription> Subscriptions { get; set; } // Uncommented
        public DbSet<Payment> Payments { get; set; } // Uncommented
        public DbSet<Invoice> Invoices { get; set; } // Uncommented
        public DbSet<FeatureFlag> FeatureFlags { get; set; } // Uncommented
        public DbSet<SupportTicket> SupportTickets { get; set; } // Uncommented
        public DbSet<KnowledgeBaseArticle> KnowledgeBaseArticles { get; set; } = null!; // Uncommented
        public DbSet<UserActivity> UserActivities { get; set; } = null!; // Uncommented
        public DbSet<Role> Roles { get; set; } = null!; // Uncommented
        public DbSet<Permission> Permissions { get; set; } = null!; // Uncommented
        public DbSet<UserRole> UserRoles { get; set; } // Uncommented
        public DbSet<RolePermission> RolePermissions { get; set; } // Uncommented

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // Configure all string properties to have a max length of 255 by default
            // and to be non-unicode if not specified otherwise
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(string) && property.GetMaxLength() == null)
                    {
                        property.SetMaxLength(255);
                    }

                    if (property.ClrType == typeof(string) && property.IsUnicode() == null)
                    {
                        property.SetIsUnicode(false);
                    }

                    // Configure enums to be stored as strings
                    if (property.ClrType.IsEnum)
                    {
                        var converterType = typeof(EnumToStringConverter<>).MakeGenericType(property.ClrType);
                        var converterInstance = (ValueConverter)Activator.CreateInstance(converterType, new ConverterMappingHints());
                        property.SetValueConverter(converterInstance);
                        property.SetMaxLength(50); // Default max length for enum strings
                    }

                    // Configure Value Objects like Money
                    if (property.ClrType == typeof(Money))
                    {
                        modelBuilder.Entity(entityType.ClrType).OwnsOne(typeof(Money), property.Name, ownedNavigationBuilder =>
                        {
                            ownedNavigationBuilder.Property(nameof(Money.Amount)).HasColumnType("decimal(18,2)");
                            ownedNavigationBuilder.Property(nameof(Money.Currency)).HasMaxLength(3);
                        });
                    }
                }
            }
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Dispatch Domain Events collection. 
            // Choices: 
            // A) Right BEFORE committing data (EF SaveChanges) into the DB will make a single transaction including 
            // side effects from the domain event handlers which are using the same DbContext with Scope lifetime
            // B) Right AFTER committing data (EF SaveChanges) into the DB will make multiple transactions. 
            // You will need to handle eventual consistency and compensatory actions in case of failures in any of the Handlers.
            // await _mediator.DispatchDomainEventsAsync(this);

            return await base.SaveChangesAsync(cancellationToken);
        }

        public async Task BeginTransactionAsync()
        {
            if (_currentTransaction != null)
            {
                return;
            }

            _currentTransaction = await Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await SaveChangesAsync();
                _currentTransaction?.Commit();
            }
            catch
            {
                await RollbackTransactionAsync();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public async Task RollbackTransactionAsync()
        {
            try
            {
                _currentTransaction?.Rollback();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }
    }
}