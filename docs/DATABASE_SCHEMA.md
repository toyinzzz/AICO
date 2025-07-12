# AICO Database Schema Documentation

## 🗄️ Database Overview

**Database**: PostgreSQL 15  
**ORM**: Entity Framework Core 8  
**Migration Strategy**: Code-First with EF Migrations  
**Connection String**: Configured in `appsettings.json`

## 📊 Core Entities

### ABTest
Primary entity for A/B testing experiments.

```sql
CREATE TABLE "ABTests" (
    "Id" SERIAL PRIMARY KEY,
    "Name" VARCHAR(200) NOT NULL,
    "Description" TEXT,
    "Hypothesis" TEXT,
    "Status" VARCHAR(50) NOT NULL DEFAULT 'Draft',
    "StartDate" TIMESTAMP WITH TIME ZONE,
    "EndDate" TIMESTAMP WITH TIME ZONE,
    "TrafficSplit" INTEGER NOT NULL DEFAULT 50,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    "UpdatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    "CreatedBy" INTEGER,
    "IsDeleted" BOOLEAN NOT NULL DEFAULT FALSE
);

CREATE UNIQUE INDEX "IX_ABTests_Name" ON "ABTests" ("Name") WHERE "IsDeleted" = FALSE;
CREATE INDEX "IX_ABTests_Status_StartDate" ON "ABTests" ("Status", "StartDate");
CREATE INDEX "IX_ABTests_CreatedAt" ON "ABTests" ("CreatedAt");
```

**Entity Configuration:**
```csharp
public class ABTest
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Hypothesis { get; set; }
    public ABTestStatus Status { get; set; } = ABTestStatus.Draft;
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int TrafficSplit { get; set; } = 50;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int? CreatedBy { get; set; }
    public bool IsDeleted { get; set; }
    
    // Navigation Properties
    public virtual ICollection<Variant> Variants { get; set; } = new List<Variant>();
    public virtual User? Creator { get; set; }
}

public enum ABTestStatus
{
    Draft,
    Running,
    Paused,
    Completed,
    Cancelled
}
```

### Variant
Represents different versions being tested in an A/B test.

```sql
CREATE TABLE "Variants" (
    "Id" SERIAL PRIMARY KEY,
    "ABTestId" INTEGER NOT NULL,
    "Name" VARCHAR(200) NOT NULL,
    "Description" TEXT,
    "IsControl" BOOLEAN NOT NULL DEFAULT FALSE,
    "TrafficAllocation" INTEGER NOT NULL DEFAULT 50,
    "Configuration" JSONB,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    "UpdatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    "IsDeleted" BOOLEAN NOT NULL DEFAULT FALSE,
    
    CONSTRAINT "FK_Variants_ABTests" FOREIGN KEY ("ABTestId") 
        REFERENCES "ABTests" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_Variants_ABTestId" ON "Variants" ("ABTestId");
CREATE INDEX "IX_Variants_IsControl" ON "Variants" ("IsControl");
CREATE UNIQUE INDEX "IX_Variants_ABTestId_Name" ON "Variants" ("ABTestId", "Name") WHERE "IsDeleted" = FALSE;
```

**Entity Configuration:**
```csharp
public class Variant
{
    public int Id { get; set; }
    public int ABTestId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsControl { get; set; }
    public int TrafficAllocation { get; set; } = 50;
    public string? Configuration { get; set; } // JSON configuration
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
    
    // Navigation Properties
    public virtual ABTest ABTest { get; set; } = null!;
    public virtual ICollection<Conversion> Conversions { get; set; } = new List<Conversion>();
    public virtual ICollection<VisitorSession> VisitorSessions { get; set; } = new List<VisitorSession>();
}
```

### Conversion
Tracks conversion events and revenue data.

```sql
CREATE TABLE "Conversions" (
    "Id" SERIAL PRIMARY KEY,
    "VariantId" INTEGER NOT NULL,
    "VisitorId" VARCHAR(100) NOT NULL,
    "SessionId" VARCHAR(100),
    "ConversionType" VARCHAR(50) NOT NULL DEFAULT 'purchase',
    "Revenue" DECIMAL(18,2) NOT NULL DEFAULT 0,
    "Cost" DECIMAL(18,2) NOT NULL DEFAULT 0,
    "Timestamp" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    "Metadata" JSONB,
    "IsValid" BOOLEAN NOT NULL DEFAULT TRUE,
    "ProcessedAt" TIMESTAMP WITH TIME ZONE,
    
    CONSTRAINT "FK_Conversions_Variants" FOREIGN KEY ("VariantId") 
        REFERENCES "Variants" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_Conversions_VariantId" ON "Conversions" ("VariantId");
CREATE INDEX "IX_Conversions_VisitorId" ON "Conversions" ("VisitorId");
CREATE INDEX "IX_Conversions_Timestamp" ON "Conversions" ("Timestamp");
CREATE INDEX "IX_Conversions_ConversionType" ON "Conversions" ("ConversionType");
CREATE INDEX "IX_Conversions_VariantId_Timestamp" ON "Conversions" ("VariantId", "Timestamp");
```

**Entity Configuration:**
```csharp
public class Conversion
{
    public int Id { get; set; }
    public int VariantId { get; set; }
    public string VisitorId { get; set; } = string.Empty;
    public string? SessionId { get; set; }
    public string ConversionType { get; set; } = "purchase";
    public decimal Revenue { get; set; }
    public decimal Cost { get; set; }
    public DateTime Timestamp { get; set; }
    public string? Metadata { get; set; } // JSON metadata
    public bool IsValid { get; set; } = true;
    public DateTime? ProcessedAt { get; set; }
    
    // Navigation Properties
    public virtual Variant Variant { get; set; } = null!;
}
```

### VisitorSession
Tracks visitor sessions and traffic allocation.

```sql
CREATE TABLE "VisitorSessions" (
    "Id" SERIAL PRIMARY KEY,
    "VariantId" INTEGER NOT NULL,
    "VisitorId" VARCHAR(100) NOT NULL,
    "SessionId" VARCHAR(100) NOT NULL,
    "StartTime" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    "EndTime" TIMESTAMP WITH TIME ZONE,
    "PageViews" INTEGER NOT NULL DEFAULT 1,
    "UserAgent" TEXT,
    "IpAddress" VARCHAR(45),
    "ReferrerUrl" TEXT,
    "LandingPage" TEXT,
    "IsConverted" BOOLEAN NOT NULL DEFAULT FALSE,
    "Metadata" JSONB,
    
    CONSTRAINT "FK_VisitorSessions_Variants" FOREIGN KEY ("VariantId") 
        REFERENCES "Variants" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_VisitorSessions_VariantId" ON "VisitorSessions" ("VariantId");
CREATE INDEX "IX_VisitorSessions_VisitorId" ON "VisitorSessions" ("VisitorId");
CREATE INDEX "IX_VisitorSessions_SessionId" ON "VisitorSessions" ("SessionId");
CREATE INDEX "IX_VisitorSessions_StartTime" ON "VisitorSessions" ("StartTime");
CREATE UNIQUE INDEX "IX_VisitorSessions_SessionId_VariantId" ON "VisitorSessions" ("SessionId", "VariantId");
```

**Entity Configuration:**
```csharp
public class VisitorSession
{
    public int Id { get; set; }
    public int VariantId { get; set; }
    public string VisitorId { get; set; } = string.Empty;
    public string SessionId { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public int PageViews { get; set; } = 1;
    public string? UserAgent { get; set; }
    public string? IpAddress { get; set; }
    public string? ReferrerUrl { get; set; }
    public string? LandingPage { get; set; }
    public bool IsConverted { get; set; }
    public string? Metadata { get; set; }
    
    // Navigation Properties
    public virtual Variant Variant { get; set; } = null!;
}
```

### MCPData
Stores calculated Maximum Customer Profit data.

```sql
CREATE TABLE "MCPData" (
    "Id" SERIAL PRIMARY KEY,
    "VariantId" INTEGER NOT NULL,
    "CalculationDate" DATE NOT NULL,
    "MCPValue" DECIMAL(18,4) NOT NULL,
    "Revenue" DECIMAL(18,2) NOT NULL,
    "Cost" DECIMAL(18,2) NOT NULL,
    "Conversions" INTEGER NOT NULL,
    "Visitors" INTEGER NOT NULL,
    "ConversionRate" DECIMAL(8,6) NOT NULL,
    "RevenuePerVisitor" DECIMAL(18,4) NOT NULL,
    "CostPerVisitor" DECIMAL(18,4) NOT NULL,
    "CalculatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    "IsValid" BOOLEAN NOT NULL DEFAULT TRUE,
    
    CONSTRAINT "FK_MCPData_Variants" FOREIGN KEY ("VariantId") 
        REFERENCES "Variants" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_MCPData_VariantId" ON "MCPData" ("VariantId");
CREATE INDEX "IX_MCPData_CalculationDate" ON "MCPData" ("CalculationDate");
CREATE UNIQUE INDEX "IX_MCPData_VariantId_CalculationDate" ON "MCPData" ("VariantId", "CalculationDate");
```

**Entity Configuration:**
```csharp
public class MCPData
{
    public int Id { get; set; }
    public int VariantId { get; set; }
    public DateTime CalculationDate { get; set; }
    public decimal MCPValue { get; set; }
    public decimal Revenue { get; set; }
    public decimal Cost { get; set; }
    public int Conversions { get; set; }
    public int Visitors { get; set; }
    public decimal ConversionRate { get; set; }
    public decimal RevenuePerVisitor { get; set; }
    public decimal CostPerVisitor { get; set; }
    public DateTime CalculatedAt { get; set; }
    public bool IsValid { get; set; } = true;
    
    // Navigation Properties
    public virtual Variant Variant { get; set; } = null!;
}
```

## 👥 User Management

### User
```sql
CREATE TABLE "Users" (
    "Id" SERIAL PRIMARY KEY,
    "Email" VARCHAR(255) NOT NULL,
    "FirstName" VARCHAR(100) NOT NULL,
    "LastName" VARCHAR(100) NOT NULL,
    "PasswordHash" VARCHAR(255) NOT NULL,
    "Role" VARCHAR(50) NOT NULL DEFAULT 'User',
    "IsActive" BOOLEAN NOT NULL DEFAULT TRUE,
    "EmailConfirmed" BOOLEAN NOT NULL DEFAULT FALSE,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    "UpdatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    "LastLoginAt" TIMESTAMP WITH TIME ZONE
);

CREATE UNIQUE INDEX "IX_Users_Email" ON "Users" ("Email");
CREATE INDEX "IX_Users_Role" ON "Users" ("Role");
```

**Entity Configuration:**
```csharp
public class User
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public UserRole Role { get; set; } = UserRole.User;
    public bool IsActive { get; set; } = true;
    public bool EmailConfirmed { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? LastLoginAt { get; set; }
    
    // Navigation Properties
    public virtual ICollection<ABTest> CreatedABTests { get; set; } = new List<ABTest>();
}

public enum UserRole
{
    User,
    Admin,
    SuperAdmin
}
```

## 📊 Statistical Analysis

### StatisticalAnalysis
```sql
CREATE TABLE "StatisticalAnalyses" (
    "Id" SERIAL PRIMARY KEY,
    "ABTestId" INTEGER NOT NULL,
    "ControlVariantId" INTEGER NOT NULL,
    "TestVariantId" INTEGER NOT NULL,
    "AnalysisDate" DATE NOT NULL,
    "PValue" DECIMAL(10,8) NOT NULL,
    "ConfidenceLevel" DECIMAL(4,3) NOT NULL DEFAULT 0.95,
    "IsSignificant" BOOLEAN NOT NULL,
    "MCPImprovement" DECIMAL(18,4) NOT NULL,
    "MCPImprovementPercentage" DECIMAL(8,4) NOT NULL,
    "SampleSize" INTEGER NOT NULL,
    "PowerAnalysis" DECIMAL(4,3),
    "EffectSize" DECIMAL(10,6),
    "StandardError" DECIMAL(18,6),
    "CalculatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    
    CONSTRAINT "FK_StatisticalAnalyses_ABTests" FOREIGN KEY ("ABTestId") 
        REFERENCES "ABTests" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_StatisticalAnalyses_ControlVariants" FOREIGN KEY ("ControlVariantId") 
        REFERENCES "Variants" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_StatisticalAnalyses_TestVariants" FOREIGN KEY ("TestVariantId") 
        REFERENCES "Variants" ("Id") ON DELETE RESTRICT
);

CREATE INDEX "IX_StatisticalAnalyses_ABTestId" ON "StatisticalAnalyses" ("ABTestId");
CREATE INDEX "IX_StatisticalAnalyses_AnalysisDate" ON "StatisticalAnalyses" ("AnalysisDate");
CREATE UNIQUE INDEX "IX_StatisticalAnalyses_ABTestId_AnalysisDate" ON "StatisticalAnalyses" ("ABTestId", "AnalysisDate");
```

## 🔍 Common Queries for AI Assistants

### MCP Calculation Query
```sql
-- Calculate MCP for a variant within a date range
SELECT 
    v.Id as VariantId,
    v.Name as VariantName,
    COUNT(DISTINCT vs.VisitorId) as Visitors,
    COUNT(c.Id) as Conversions,
    COALESCE(SUM(c.Revenue), 0) as TotalRevenue,
    COALESCE(SUM(c.Cost), 0) as TotalCost,
    CASE 
        WHEN COUNT(DISTINCT vs.VisitorId) > 0 
        THEN CAST(COUNT(c.Id) AS DECIMAL) / COUNT(DISTINCT vs.VisitorId)
        ELSE 0 
    END as ConversionRate,
    CASE 
        WHEN COUNT(DISTINCT vs.VisitorId) > 0 
        THEN (COALESCE(SUM(c.Revenue), 0) - COALESCE(SUM(c.Cost), 0)) / COUNT(DISTINCT vs.VisitorId)
        ELSE 0 
    END as MCPValue
FROM "Variants" v
LEFT JOIN "VisitorSessions" vs ON v.Id = vs.VariantId 
    AND vs.StartTime >= @StartDate 
    AND vs.StartTime <= @EndDate
LEFT JOIN "Conversions" c ON v.Id = c.VariantId 
    AND c.Timestamp >= @StartDate 
    AND c.Timestamp <= @EndDate 
    AND c.IsValid = true
WHERE v.Id = @VariantId
GROUP BY v.Id, v.Name;
```

### A/B Test Performance Comparison
```sql
-- Compare performance between control and test variants
WITH VariantMetrics AS (
    SELECT 
        v.Id,
        v.Name,
        v.IsControl,
        COUNT(DISTINCT vs.VisitorId) as Visitors,
        COUNT(c.Id) as Conversions,
        COALESCE(SUM(c.Revenue), 0) as Revenue,
        COALESCE(SUM(c.Cost), 0) as Cost
    FROM "Variants" v
    LEFT JOIN "VisitorSessions" vs ON v.Id = vs.VariantId
    LEFT JOIN "Conversions" c ON v.Id = c.VariantId AND c.IsValid = true
    WHERE v.ABTestId = @ABTestId
    GROUP BY v.Id, v.Name, v.IsControl
)
SELECT 
    *,
    CASE WHEN Visitors > 0 THEN CAST(Conversions AS DECIMAL) / Visitors ELSE 0 END as ConversionRate,
    CASE WHEN Visitors > 0 THEN (Revenue - Cost) / Visitors ELSE 0 END as MCPValue
FROM VariantMetrics
ORDER BY IsControl DESC, MCPValue DESC;
```

### Daily MCP Trends
```sql
-- Get daily MCP trends for a variant
SELECT 
    DATE(vs.StartTime) as Date,
    COUNT(DISTINCT vs.VisitorId) as Visitors,
    COUNT(c.Id) as Conversions,
    COALESCE(SUM(c.Revenue), 0) as Revenue,
    COALESCE(SUM(c.Cost), 0) as Cost,
    CASE 
        WHEN COUNT(DISTINCT vs.VisitorId) > 0 
        THEN (COALESCE(SUM(c.Revenue), 0) - COALESCE(SUM(c.Cost), 0)) / COUNT(DISTINCT vs.VisitorId)
        ELSE 0 
    END as MCPValue
FROM "VisitorSessions" vs
LEFT JOIN "Conversions" c ON vs.VariantId = c.VariantId 
    AND DATE(vs.StartTime) = DATE(c.Timestamp)
    AND c.IsValid = true
WHERE vs.VariantId = @VariantId
    AND vs.StartTime >= @StartDate
    AND vs.StartTime <= @EndDate
GROUP BY DATE(vs.StartTime)
ORDER BY Date;
```

## 🏗️ Entity Framework Configuration

### DbContext Configuration
```csharp
public class AicoDbContext : DbContext
{
    public AicoDbContext(DbContextOptions<AicoDbContext> options) : base(options) { }

    public DbSet<ABTest> ABTests { get; set; }
    public DbSet<Variant> Variants { get; set; }
    public DbSet<Conversion> Conversions { get; set; }
    public DbSet<VisitorSession> VisitorSessions { get; set; }
    public DbSet<MCPData> MCPData { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<StatisticalAnalysis> StatisticalAnalyses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Apply all configurations
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AicoDbContext).Assembly);
        
        // Global query filters
        modelBuilder.Entity<ABTest>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<Variant>().HasQueryFilter(x => !x.IsDeleted);
        
        // Seed data
        SeedData(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Update timestamps
        var entries = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

        foreach (var entry in entries)
        {
            if (entry.Entity is IAuditable auditable)
            {
                if (entry.State == EntityState.Added)
                {
                    auditable.CreatedAt = DateTime.UtcNow;
                }
                auditable.UpdatedAt = DateTime.UtcNow;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
```

### Migration Commands
```bash
# Add new migration
dotnet ef migrations add MigrationName --project AICO.Infrastructure --startup-project AICO.API

# Update database
dotnet ef database update --project AICO.Infrastructure --startup-project AICO.API

# Generate SQL script
dotnet ef migrations script --project AICO.Infrastructure --startup-project AICO.API

# Remove last migration
dotnet ef migrations remove --project AICO.Infrastructure --startup-project AICO.API
```

## 🔧 Repository Pattern Implementation

### Generic Repository
```csharp
public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
    Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null);
    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
}

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly AicoDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(AicoDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public virtual async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    // Additional implementation...
}
```

### Specific Repository Example
```csharp
public interface IConversionRepository : IRepository<Conversion>
{
    Task<IEnumerable<Conversion>> GetByVariantIdAsync(int variantId);
    Task<decimal> CalculateTotalRevenueAsync(int variantId, DateTime startDate, DateTime endDate);
    Task<int> GetConversionCountAsync(int variantId, DateTime startDate, DateTime endDate);
    Task<IEnumerable<Conversion>> GetConversionsByDateRangeAsync(int variantId, DateTime startDate, DateTime endDate);
}

public class ConversionRepository : Repository<Conversion>, IConversionRepository
{
    public ConversionRepository(AicoDbContext context) : base(context) { }

    public async Task<IEnumerable<Conversion>> GetByVariantIdAsync(int variantId)
    {
        return await _dbSet
            .Where(c => c.VariantId == variantId && c.IsValid)
            .Include(c => c.Variant)
            .OrderByDescending(c => c.Timestamp)
            .ToListAsync();
    }

    public async Task<decimal> CalculateTotalRevenueAsync(int variantId, DateTime startDate, DateTime endDate)
    {
        return await _dbSet
            .Where(c => c.VariantId == variantId && 
                       c.IsValid && 
                       c.Timestamp >= startDate && 
                       c.Timestamp <= endDate)
            .SumAsync(c => c.Revenue);
    }
}
```

## 📈 Performance Optimization

### Indexes for Common Queries
```sql
-- Composite indexes for MCP calculations
CREATE INDEX "IX_Conversions_VariantId_Timestamp_IsValid" 
ON "Conversions" ("VariantId", "Timestamp", "IsValid") 
INCLUDE ("Revenue", "Cost");

CREATE INDEX "IX_VisitorSessions_VariantId_StartTime" 
ON "VisitorSessions" ("VariantId", "StartTime") 
INCLUDE ("VisitorId", "IsConverted");

-- Partial indexes for active tests
CREATE INDEX "IX_ABTests_Active" 
ON "ABTests" ("Status", "StartDate", "EndDate") 
WHERE "Status" IN ('Running', 'Paused') AND "IsDeleted" = FALSE;
```

### Query Optimization Tips
1. **Use appropriate indexes** for date range queries
2. **Include columns** in indexes for covering queries
3. **Partition large tables** by date if needed
4. **Use materialized views** for complex aggregations
5. **Implement caching** for frequently accessed data

## 🧪 Test Data Setup

### Seed Data for Development
```csharp
private static void SeedData(ModelBuilder modelBuilder)
{
    // Seed Users
    modelBuilder.Entity<User>().HasData(
        new User
        {
            Id = 1,
            Email = "admin@aico.com",
            FirstName = "Admin",
            LastName = "User",
            PasswordHash = "hashed_password",
            Role = UserRole.Admin,
            IsActive = true,
            EmailConfirmed = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        }
    );

    // Seed ABTest
    modelBuilder.Entity<ABTest>().HasData(
        new ABTest
        {
            Id = 1,
            Name = "Homepage CTA Test",
            Description = "Testing different CTA button colors",
            Status = ABTestStatus.Running,
            StartDate = DateTime.UtcNow.AddDays(-7),
            EndDate = DateTime.UtcNow.AddDays(23),
            TrafficSplit = 50,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            CreatedBy = 1
        }
    );

    // Seed Variants
    modelBuilder.Entity<Variant>().HasData(
        new Variant
        {
            Id = 1,
            ABTestId = 1,
            Name = "Control",
            Description = "Blue CTA button",
            IsControl = true,
            TrafficAllocation = 50,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        },
        new Variant
        {
            Id = 2,
            ABTestId = 1,
            Name = "Test A",
            Description = "Red CTA button",
            IsControl = false,
            TrafficAllocation = 50,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        }
    );
}
```

This schema provides a solid foundation for the AICO platform's MCP optimization and A/B testing functionality. The design emphasizes performance, data integrity, and scalability while maintaining clear relationships between entities.