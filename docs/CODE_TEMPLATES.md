# Code Templates for AI Assistants

This document provides standardized code templates for consistent development patterns in the AICO project.

## 🎯 Quick Reference

- **Namespace Pattern**: `AICO.{Layer}.{Feature}`
- **Service Registration**: Always register interfaces in `Program.cs`
- **Validation**: Use FluentValidation for all DTOs
- **Logging**: Inject `ILogger<T>` in all services
- **Error Handling**: Use custom exceptions with global handler

## 🏗️ Backend Templates

### API Controller Template

```csharp
using AICO.Application.Features.{Feature}.Commands;
using AICO.Application.Features.{Feature}.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AICO.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class {Feature}Controller : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<{Feature}Controller> _logger;

        public {Feature}Controller(IMediator mediator, ILogger<{Feature}Controller> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<{Feature}Response>> GetById(int id)
        {
            try
            {
                var query = new Get{Feature}ByIdQuery { Id = id };
                var result = await _mediator.Send(query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving {feature} with ID {Id}", nameof({Feature}), id);
                throw;
            }
        }

        [HttpPost]
        public async Task<ActionResult<{Feature}Response>> Create([FromBody] Create{Feature}Command command)
        {
            try
            {
                var result = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating {feature}", nameof({Feature}));
                throw;
            }
        }
    }
}
```

### Service Implementation Template

```csharp
using AICO.Application.Interfaces;
using AICO.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace AICO.Application.Services
{
    public class {Feature}Service : I{Feature}Service
    {
        private readonly I{Feature}Repository _repository;
        private readonly ILogger<{Feature}Service> _logger;

        public {Feature}Service(
            I{Feature}Repository repository,
            ILogger<{Feature}Service> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<{Feature}Result> Process{Feature}Async({Feature}Request request)
        {
            try
            {
                _logger.LogInformation("Processing {feature} request for {Property}", 
                    nameof({Feature}), request.PropertyName);

                // Business logic here
                var entity = new {Feature}
                {
                    // Map properties
                };

                var result = await _repository.AddAsync(entity);
                
                _logger.LogInformation("Successfully processed {feature} with ID {Id}", 
                    nameof({Feature}), result.Id);

                return new {Feature}Result
                {
                    Id = result.Id,
                    // Map other properties
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing {feature} request", nameof({Feature}));
                throw;
            }
        }
    }
}
```

### Repository Implementation Template

```csharp
using AICO.Domain.Entities;
using AICO.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AICO.Infrastructure.Repositories
{
    public class {Feature}Repository : Repository<{Feature}>, I{Feature}Repository
    {
        private readonly AicoDbContext _context;
        private readonly ILogger<{Feature}Repository> _logger;

        public {Feature}Repository(
            AicoDbContext context,
            ILogger<{Feature}Repository> logger) : base(context)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<{Feature}>> GetBy{Property}Async(int {property}Id)
        {
            try
            {
                return await _context.{Feature}s
                    .Where(x => x.{Property}Id == {property}Id)
                    .Include(x => x.RelatedEntity)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving {feature}s by {property} ID {Id}", 
                    nameof({Feature}), nameof({Property}), {property}Id);
                throw;
            }
        }

        public async Task<decimal> Calculate{Metric}Async(int {feature}Id, DateTime startDate, DateTime endDate)
        {
            try
            {
                return await _context.{Feature}s
                    .Where(x => x.Id == {feature}Id && 
                               x.CreatedAt >= startDate && 
                               x.CreatedAt <= endDate)
                    .SumAsync(x => x.{Metric}Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating {metric} for {feature} ID {Id}", 
                    nameof({Metric}), nameof({Feature}), {feature}Id);
                throw;
            }
        }
    }
}
```

### Entity Configuration Template

```csharp
using AICO.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AICO.Infrastructure.Data.Configurations
{
    public class {Feature}Configuration : IEntityTypeConfiguration<{Feature}>
    {
        public void Configure(EntityTypeBuilder<{Feature}> builder)
        {
            builder.ToTable("{Feature}s");
            
            builder.HasKey(x => x.Id);
            
            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();
                
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);
                
            builder.Property(x => x.Description)
                .HasMaxLength(1000);
                
            builder.Property(x => x.CreatedAt)
                .IsRequired();
                
            builder.Property(x => x.UpdatedAt)
                .IsRequired();

            // Relationships
            builder.HasOne(x => x.RelatedEntity)
                .WithMany(x => x.{Feature}s)
                .HasForeignKey(x => x.RelatedEntityId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(x => x.Name)
                .IsUnique();
                
            builder.HasIndex(x => new { x.RelatedEntityId, x.CreatedAt });
        }
    }
}
```

### CQRS Command Template

```csharp
using AICO.Application.Common.Interfaces;
using AICO.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AICO.Application.Features.{Feature}.Commands
{
    public class Create{Feature}Command : IRequest<{Feature}Response>
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        // Add other properties
    }

    public class Create{Feature}CommandHandler : IRequestHandler<Create{Feature}Command, {Feature}Response>
    {
        private readonly I{Feature}Repository _repository;
        private readonly ILogger<Create{Feature}CommandHandler> _logger;

        public Create{Feature}CommandHandler(
            I{Feature}Repository repository,
            ILogger<Create{Feature}CommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<{Feature}Response> Handle(Create{Feature}Command request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Creating new {feature} with name {Name}", 
                    nameof({Feature}), request.Name);

                var entity = new {Feature}
                {
                    Name = request.Name,
                    Description = request.Description,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var result = await _repository.AddAsync(entity);
                
                _logger.LogInformation("Successfully created {feature} with ID {Id}", 
                    nameof({Feature}), result.Id);

                return new {Feature}Response
                {
                    Id = result.Id,
                    Name = result.Name,
                    Description = result.Description,
                    CreatedAt = result.CreatedAt
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating {feature}", nameof({Feature}));
                throw;
            }
        }
    }
}
```

### FluentValidation Template

```csharp
using AICO.Application.Features.{Feature}.Commands;
using FluentValidation;

namespace AICO.Application.Features.{Feature}.Validators
{
    public class Create{Feature}CommandValidator : AbstractValidator<Create{Feature}Command>
    {
        public Create{Feature}CommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("{Feature} name is required")
                .MaximumLength(200).WithMessage("{Feature} name must not exceed 200 characters")
                .Matches(@"^[a-zA-Z0-9\s-_]+$").WithMessage("{Feature} name contains invalid characters");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters");

            // Add custom business rules
            RuleFor(x => x)
                .MustAsync(BeUniqueAsync)
                .WithMessage("{Feature} with this name already exists");
        }

        private async Task<bool> BeUniqueAsync(Create{Feature}Command command, CancellationToken cancellationToken)
        {
            // Implement uniqueness check
            return true;
        }
    }
}
```

## 🎨 Frontend Templates

### Vue Component Template

```vue
<template>
  <div class="{feature}-container">
    <div class="header">
      <h2>{{ title }}</h2>
      <button @click="create{Feature}" class="btn-primary">
        Create {Feature}
      </button>
    </div>
    
    <div class="content">
      <div v-if="loading" class="loading">
        Loading...
      </div>
      
      <div v-else-if="error" class="error">
        {{ error }}
      </div>
      
      <div v-else class="{feature}-list">
        <{Feature}Card 
          v-for="item in {feature}s" 
          :key="item.id"
          :item="item"
          @edit="edit{Feature}"
          @delete="delete{Feature}"
        />
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { use{Feature}Store } from '@/stores/{feature}Store'
import {Feature}Card from '@/components/{Feature}Card.vue'
import type { {Feature} } from '@/types/{feature}'

interface Props {
  title?: string
}

const props = withDefaults(defineProps<Props>(), {
  title: '{Feature} Management'
})

const {feature}Store = use{Feature}Store()
const loading = ref(false)
const error = ref<string | null>(null)
const {feature}s = ref<{Feature}[]>([])

onMounted(async () => {
  await load{Feature}s()
})

const load{Feature}s = async () => {
  try {
    loading.value = true
    error.value = null
    {feature}s.value = await {feature}Store.getAll()
  } catch (err) {
    error.value = 'Failed to load {feature}s'
    console.error('Error loading {feature}s:', err)
  } finally {
    loading.value = false
  }
}

const create{Feature} = () => {
  // Navigate to create form or open modal
}

const edit{Feature} = (item: {Feature}) => {
  // Navigate to edit form or open modal
}

const delete{Feature} = async (item: {Feature}) => {
  if (confirm(`Are you sure you want to delete ${item.name}?`)) {
    try {
      await {feature}Store.delete(item.id)
      await load{Feature}s()
    } catch (err) {
      error.value = 'Failed to delete {feature}'
      console.error('Error deleting {feature}:', err)
    }
  }
}
</script>

<style scoped>
.{feature}-container {
  padding: 1rem;
}

.header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 1rem;
}

.btn-primary {
  background-color: #007bff;
  color: white;
  border: none;
  padding: 0.5rem 1rem;
  border-radius: 0.25rem;
  cursor: pointer;
}

.btn-primary:hover {
  background-color: #0056b3;
}

.loading, .error {
  text-align: center;
  padding: 2rem;
}

.error {
  color: #dc3545;
}

.{feature}-list {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
  gap: 1rem;
}
</style>
```

### Pinia Store Template

```typescript
import { defineStore } from 'pinia'
import { ref } from 'vue'
import { {feature}Api } from '@/services/{feature}Api'
import type { {Feature}, Create{Feature}Request, Update{Feature}Request } from '@/types/{feature}'

export const use{Feature}Store = defineStore('{feature}', () => {
  const {feature}s = ref<{Feature}[]>([])
  const current{Feature} = ref<{Feature} | null>(null)
  const loading = ref(false)
  const error = ref<string | null>(null)

  const getAll = async (): Promise<{Feature}[]> => {
    try {
      loading.value = true
      error.value = null
      const response = await {feature}Api.getAll()
      {feature}s.value = response.data
      return response.data
    } catch (err) {
      error.value = 'Failed to fetch {feature}s'
      throw err
    } finally {
      loading.value = false
    }
  }

  const getById = async (id: number): Promise<{Feature}> => {
    try {
      loading.value = true
      error.value = null
      const response = await {feature}Api.getById(id)
      current{Feature}.value = response.data
      return response.data
    } catch (err) {
      error.value = 'Failed to fetch {feature}'
      throw err
    } finally {
      loading.value = false
    }
  }

  const create = async (request: Create{Feature}Request): Promise<{Feature}> => {
    try {
      loading.value = true
      error.value = null
      const response = await {feature}Api.create(request)
      {feature}s.value.push(response.data)
      return response.data
    } catch (err) {
      error.value = 'Failed to create {feature}'
      throw err
    } finally {
      loading.value = false
    }
  }

  const update = async (id: number, request: Update{Feature}Request): Promise<{Feature}> => {
    try {
      loading.value = true
      error.value = null
      const response = await {feature}Api.update(id, request)
      const index = {feature}s.value.findIndex(item => item.id === id)
      if (index !== -1) {
        {feature}s.value[index] = response.data
      }
      return response.data
    } catch (err) {
      error.value = 'Failed to update {feature}'
      throw err
    } finally {
      loading.value = false
    }
  }

  const remove = async (id: number): Promise<void> => {
    try {
      loading.value = true
      error.value = null
      await {feature}Api.delete(id)
      {feature}s.value = {feature}s.value.filter(item => item.id !== id)
    } catch (err) {
      error.value = 'Failed to delete {feature}'
      throw err
    } finally {
      loading.value = false
    }
  }

  const clearError = () => {
    error.value = null
  }

  return {
    // State
    {feature}s,
    current{Feature},
    loading,
    error,
    
    // Actions
    getAll,
    getById,
    create,
    update,
    delete: remove,
    clearError
  }
})
```

## 🧪 Testing Templates

### Unit Test Template

```csharp
using AICO.Application.Services;
using AICO.Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace AICO.Tests.Unit.Services
{
    public class {Feature}ServiceTests
    {
        private readonly Mock<I{Feature}Repository> _mockRepository;
        private readonly Mock<ILogger<{Feature}Service>> _mockLogger;
        private readonly {Feature}Service _service;

        public {Feature}ServiceTests()
        {
            _mockRepository = new Mock<I{Feature}Repository>();
            _mockLogger = new Mock<ILogger<{Feature}Service>>();
            _service = new {Feature}Service(_mockRepository.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task Process{Feature}Async_ValidRequest_ReturnsSuccess()
        {
            // Arrange
            var request = new {Feature}Request
            {
                Name = "Test {Feature}",
                Description = "Test Description"
            };

            var expectedEntity = new {Feature}
            {
                Id = 1,
                Name = request.Name,
                Description = request.Description,
                CreatedAt = DateTime.UtcNow
            };

            _mockRepository.Setup(x => x.AddAsync(It.IsAny<{Feature}>()))
                .ReturnsAsync(expectedEntity);

            // Act
            var result = await _service.Process{Feature}Async(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedEntity.Id, result.Id);
            Assert.Equal(expectedEntity.Name, result.Name);
            _mockRepository.Verify(x => x.AddAsync(It.IsAny<{Feature}>()), Times.Once);
        }

        [Fact]
        public async Task Process{Feature}Async_RepositoryThrows_ThrowsException()
        {
            // Arrange
            var request = new {Feature}Request { Name = "Test" };
            _mockRepository.Setup(x => x.AddAsync(It.IsAny<{Feature}>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _service.Process{Feature}Async(request));
        }
    }
}
```

## 📝 Usage Instructions

1. **Replace Placeholders**: Replace `{Feature}`, `{Property}`, `{Metric}` with actual names
2. **Follow Naming Conventions**: Use PascalCase for classes, camelCase for variables
3. **Add Logging**: Always include structured logging with relevant context
4. **Handle Errors**: Implement proper exception handling and logging
5. **Validate Input**: Use FluentValidation for all DTOs and commands
6. **Test Coverage**: Write unit tests for all business logic

## 🔧 Service Registration Pattern

Always register services in `Program.cs`:

```csharp
// Repository registration
builder.Services.AddScoped<I{Feature}Repository, {Feature}Repository>();

// Service registration
builder.Services.AddScoped<I{Feature}Service, {Feature}Service>();

// Validator registration
builder.Services.AddScoped<IValidator<Create{Feature}Command>, Create{Feature}CommandValidator>();
```