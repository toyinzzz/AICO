# Entity Design Principles

This document outlines the design principles for entities in our AICO domain model following Domain-Driven Design (DDD) principles.

## Core Principles

### 1. Encapsulation

- **Private Setters**: All entity properties should have private setters to prevent direct manipulation from outside the entity.
- **Factory Methods**: Use static factory methods to create entities with proper validation.
- **Internal Methods**: Use internal methods for operations that modify the entity state.

### 2. Immutability

- Entities should be immutable wherever possible.
- Changes to entity state should be done through well-defined methods.
- Each method that changes state should validate inputs.

### 3. Validation

- Validate all inputs in factory methods and update methods.
- Use data annotations to define constraints on properties.
- Throw appropriate exceptions with meaningful messages for validation errors.

### 4. Entity Creation

- Use private constructors for EF Core compatibility.
- Provide static factory methods for entity creation.
- Factory methods should validate all required parameters.
- Initialize collections in the constructor.

## Example Pattern

```csharp
public class User : BaseEntity
{
    // Properties with private setters
    public string Email { get; private set; }
    public string Username { get; private set; }
    
    // Private constructor for EF Core
    private User()
    {
        // Initialize collections
    }
    
    // Static factory method with validation
    public static User Create(string email, string username)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required", nameof(email));
            
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Username is required", nameof(username));
            
        return new User
        {
            Email = email.Trim(),
            Username = username.Trim()
        };
    }
    
    // Internal methods for state changes
    internal void UpdateUsername(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Username is required", nameof(username));
            
        Username = username.Trim();
    }
}
```

## Working with Entity Framework Core

Entity Framework Core requires:

1. A parameterless constructor (can be private)
2. Properties that can be set during materialization

Our pattern satisfies these requirements while maintaining proper encapsulation:

- Private parameterless constructor for EF Core
- Private setters allow EF Core to set properties during materialization
- Public getters allow reading property values

## Domain Services

Operations that involve multiple entities or complex domain logic should be implemented in domain services rather than in the entities themselves. This follows the Single Responsibility Principle.

Examples:
- `AuditService`: Handles audit-related operations
- `AnalysisService`: Manages analysis operations
- `RecommendationService`: Handles recommendation creation and updates

## Audit Trail

All entities inherit from `BaseEntity` which implements `IAuditableEntity` interface, providing:

- Creation date tracking
- Modification date tracking
- User tracking for creation and modification

The `AuditService` is responsible for managing these audit properties.

## Security Considerations

- Sensitive properties like passwords should have additional protection.
- Role changes should be controlled through dedicated methods.
- Verification status changes should be controlled through dedicated methods.

## Testing

- Each entity should have comprehensive unit tests.
- Tests should verify:
  - Factory methods validate inputs correctly
  - Update methods validate inputs and change state correctly
  - Invalid operations throw appropriate exceptions 