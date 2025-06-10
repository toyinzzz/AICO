# Proper Entity Design with Encapsulation

## Key Principles

### 1. Encapsulation
- Private setters prevent external modification
- Properties can only be modified through controlled methods
- Ensures data integrity and valid state

### 2. Controlled Creation
- Static factory methods with validation
- Private constructors for EF Core compatibility
- Ensures entities are always created in a valid state

### 3. Immutability
- Once created, entities should not be modified directly
- Changes should be made through controlled methods
- Prevents invalid state transitions

### 4. Domain Logic Separation
- Entity validation in factory methods
- Business logic in domain services
- Entities remain focused on data representation

## Example Implementation

```csharp
public class AnalysisResult : BaseEntity
{
    // Properties with private setters
    public Guid WebsiteId { get; private set; }
    public string AnalysisType { get; private set; }
    public int Score { get; private set; }
    public string ResultData { get; private set; }
    public string Summary { get; private set; }
    public virtual ICollection<Recommendation> Recommendations { get; private set; }

    // Private constructor for EF Core
    private AnalysisResult()
    {
        Recommendations = new List<Recommendation>();
    }

    // Static factory method for controlled creation
    public static AnalysisResult Create(
        Guid websiteId,
        string analysisType,
        int score,
        string resultData,
        string summary = null)
    {
        // Validation logic
        if (string.IsNullOrWhiteSpace(analysisType))
            throw new ArgumentException("Analysis type cannot be null or empty", nameof(analysisType));
        
        if (string.IsNullOrWhiteSpace(resultData))
            throw new ArgumentException("Result data cannot be null or empty", nameof(resultData));
        
        if (score < 0 || score > 100)
            throw new ArgumentOutOfRangeException(nameof(score), "Score must be between 0 and 100");

        // Creation with valid state
        return new AnalysisResult
        {
            WebsiteId = websiteId,
            AnalysisType = analysisType,
            Score = score,
            ResultData = resultData,
            Summary = summary,
            Recommendations = new List<Recommendation>()
        };
    }

    // Internal methods for controlled modifications
    internal void AddRecommendation(Recommendation recommendation)
    {
        if (recommendation == null)
            throw new ArgumentNullException(nameof(recommendation));

        if (recommendation.AnalysisResultId != Id)
            throw new ArgumentException("Recommendation belongs to a different analysis result");

        Recommendations.Add(recommendation);
    }
}
```

## Domain Service Implementation

```csharp
public class AnalysisService : IAnalysisService
{
    private readonly IAuditService _auditService;

    public AnalysisService(IAuditService auditService)
    {
        _auditService = auditService ?? throw new ArgumentNullException(nameof(auditService));
    }

    public AnalysisResult CreateAnalysisEntity(Guid websiteId, string analysisType, int score, string resultData, string summary)
    {
        // Use the factory method to create a valid entity
        return AnalysisResult.Create(websiteId, analysisType, score, resultData, summary);
    }

    public async Task<AnalysisResult> CreateAnalysisAsync(Guid websiteId, string analysisType, int score, string resultData, string summary)
    {
        // Create the analysis result using the factory method
        var analysis = AnalysisResult.Create(websiteId, analysisType, score, resultData, summary);

        // Set audit information
        _auditService.SetCreationAudit(analysis);

        return analysis;
    }
}
```

## Benefits of This Approach

1. **Data Integrity**: Entities are always in a valid state
2. **Maintainability**: Clear separation of concerns
3. **Testability**: Easy to test domain logic in isolation
4. **Security**: Prevents unauthorized modifications
5. **EF Core Compatibility**: Works with ORM through private constructors

## Common Anti-Patterns to Avoid

1. **Anemic Domain Model**: Entities with public setters and no behavior
2. **Business Logic in Entities**: Overloading entities with complex operations
3. **Direct Entity Creation**: Using `new` instead of factory methods
4. **Public Setters**: Allowing uncontrolled state changes
5. **Missing Validation**: Not validating entity state during creation

By following these principles, we ensure that our domain model remains robust, maintainable, and aligned with domain-driven design best practices. 