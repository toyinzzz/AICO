# AICO API Documentation

## 🚀 Quick Start for AI Assistants

**Base URL**: `https://localhost:7001/api`  
**Authentication**: JWT Bearer Token  
**Content-Type**: `application/json`  
**API Version**: v1

## 🔐 Authentication

### Login
```http
POST /api/auth/login
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "password123"
}
```

**Response:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "refresh_token_here",
  "expiresAt": "2024-01-01T12:00:00Z",
  "user": {
    "id": 1,
    "email": "user@example.com",
    "firstName": "John",
    "lastName": "Doe",
    "role": "Admin"
  }
}
```

### Using Authentication
```http
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

## 📊 MCP (Maximum Customer Profit) API

### Calculate MCP
```http
POST /api/mcp/calculate
Authorization: Bearer {token}
Content-Type: application/json

{
  "variantId": 1,
  "startDate": "2024-01-01T00:00:00Z",
  "endDate": "2024-01-31T23:59:59Z",
  "includeStatisticalAnalysis": true
}
```

**Response:**
```json
{
  "variantId": 1,
  "mcpValue": 125.50,
  "revenue": 10000.00,
  "cost": 3000.00,
  "conversions": 150,
  "visitors": 5000,
  "conversionRate": 0.03,
  "revenuePerVisitor": 2.00,
  "costPerVisitor": 0.60,
  "calculatedAt": "2024-01-01T12:00:00Z",
  "statisticalAnalysis": {
    "isSignificant": true,
    "pValue": 0.023,
    "confidenceLevel": 0.95,
    "marginOfError": 5.2
  }
}
```

### Compare MCP Between Variants
```http
POST /api/mcp/compare
Authorization: Bearer {token}
Content-Type: application/json

{
  "controlVariantId": 1,
  "testVariantId": 2,
  "startDate": "2024-01-01T00:00:00Z",
  "endDate": "2024-01-31T23:59:59Z"
}
```

**Response:**
```json
{
  "controlVariant": {
    "id": 1,
    "name": "Control",
    "mcpValue": 125.50,
    "conversions": 150,
    "visitors": 5000
  },
  "testVariant": {
    "id": 2,
    "name": "Test Variant A",
    "mcpValue": 142.30,
    "conversions": 180,
    "visitors": 5200
  },
  "comparison": {
    "mcpImprovement": 13.38,
    "mcpImprovementPercentage": 10.66,
    "isStatisticallySignificant": true,
    "pValue": 0.015,
    "confidenceLevel": 0.95,
    "winner": "testVariant"
  }
}
```

## 🧪 A/B Testing API

### Create A/B Test
```http
POST /api/abtests
Authorization: Bearer {token}
Content-Type: application/json

{
  "name": "Homepage CTA Test",
  "description": "Testing different CTA button colors",
  "hypothesis": "Red CTA button will increase conversions by 15%",
  "startDate": "2024-01-01T00:00:00Z",
  "endDate": "2024-01-31T23:59:59Z",
  "trafficSplit": 50,
  "variants": [
    {
      "name": "Control",
      "description": "Blue CTA button",
      "isControl": true
    },
    {
      "name": "Test A",
      "description": "Red CTA button",
      "isControl": false
    }
  ]
}
```

**Response:**
```json
{
  "id": 1,
  "name": "Homepage CTA Test",
  "description": "Testing different CTA button colors",
  "status": "Draft",
  "createdAt": "2024-01-01T10:00:00Z",
  "variants": [
    {
      "id": 1,
      "name": "Control",
      "description": "Blue CTA button",
      "isControl": true,
      "trafficAllocation": 50
    },
    {
      "id": 2,
      "name": "Test A",
      "description": "Red CTA button",
      "isControl": false,
      "trafficAllocation": 50
    }
  ]
}
```

### Get A/B Test Results
```http
GET /api/abtests/{id}/results
Authorization: Bearer {token}
```

**Response:**
```json
{
  "abTestId": 1,
  "name": "Homepage CTA Test",
  "status": "Completed",
  "duration": 30,
  "totalVisitors": 10000,
  "totalConversions": 320,
  "variants": [
    {
      "id": 1,
      "name": "Control",
      "visitors": 5000,
      "conversions": 150,
      "conversionRate": 0.03,
      "revenue": 15000.00,
      "mcpValue": 125.50
    },
    {
      "id": 2,
      "name": "Test A",
      "visitors": 5000,
      "conversions": 170,
      "conversionRate": 0.034,
      "revenue": 17000.00,
      "mcpValue": 142.30
    }
  ],
  "statisticalAnalysis": {
    "winner": "Test A",
    "isSignificant": true,
    "pValue": 0.023,
    "confidenceLevel": 0.95,
    "improvement": 13.38
  }
}
```

## 📈 Reporting API

### Generate MCP Analysis Report
```http
POST /api/reports/mcp-analysis
Authorization: Bearer {token}
Content-Type: application/json

{
  "abTestId": 1,
  "startDate": "2024-01-01T00:00:00Z",
  "endDate": "2024-01-31T23:59:59Z",
  "includeStatisticalInsights": true,
  "includeTrendAnalysis": true
}
```

**Response:**
```json
{
  "reportId": "rpt_123456",
  "abTestId": 1,
  "generatedAt": "2024-01-01T12:00:00Z",
  "summary": {
    "totalVisitors": 10000,
    "totalConversions": 320,
    "totalRevenue": 32000.00,
    "averageMCP": 133.90,
    "bestPerformingVariant": "Test A"
  },
  "variantAnalysis": [
    {
      "variantId": 1,
      "name": "Control",
      "mcpValue": 125.50,
      "performance": "baseline"
    },
    {
      "variantId": 2,
      "name": "Test A",
      "mcpValue": 142.30,
      "performance": "winner",
      "improvement": 13.38
    }
  ],
  "statisticalInsights": {
    "isSignificant": true,
    "pValue": 0.023,
    "confidenceLevel": 0.95,
    "recommendation": "Implement Test A variant"
  },
  "trendAnalysis": {
    "trend": "increasing",
    "weeklyGrowth": 2.5,
    "seasonality": "none_detected"
  }
}
```

### Get MCP Trends
```http
GET /api/reports/mcp-trends?variantId=1&startDate=2024-01-01&endDate=2024-01-31&granularity=daily
Authorization: Bearer {token}
```

**Response:**
```json
{
  "variantId": 1,
  "granularity": "daily",
  "dataPoints": [
    {
      "date": "2024-01-01T00:00:00Z",
      "mcpValue": 120.50,
      "visitors": 200,
      "conversions": 6,
      "revenue": 600.00
    },
    {
      "date": "2024-01-02T00:00:00Z",
      "mcpValue": 125.30,
      "visitors": 180,
      "conversions": 7,
      "revenue": 700.00
    }
  ],
  "summary": {
    "averageMCP": 125.50,
    "trend": "increasing",
    "volatility": "low"
  }
}
```

## 📊 Conversion Tracking API

### Record Conversion
```http
POST /api/conversions
Authorization: Bearer {token}
Content-Type: application/json

{
  "variantId": 1,
  "visitorId": "visitor_123",
  "sessionId": "session_456",
  "revenue": 99.99,
  "conversionType": "purchase",
  "metadata": {
    "productId": "prod_789",
    "category": "electronics",
    "source": "organic"
  }
}
```

**Response:**
```json
{
  "id": 1001,
  "variantId": 1,
  "visitorId": "visitor_123",
  "revenue": 99.99,
  "conversionType": "purchase",
  "timestamp": "2024-01-01T12:30:00Z",
  "processed": true
}
```

## 🔍 Analytics API

### Get Variant Performance
```http
GET /api/analytics/variants/{id}/performance?startDate=2024-01-01&endDate=2024-01-31
Authorization: Bearer {token}
```

**Response:**
```json
{
  "variantId": 1,
  "name": "Control",
  "period": {
    "startDate": "2024-01-01T00:00:00Z",
    "endDate": "2024-01-31T23:59:59Z"
  },
  "metrics": {
    "visitors": 5000,
    "conversions": 150,
    "conversionRate": 0.03,
    "revenue": 15000.00,
    "averageOrderValue": 100.00,
    "mcpValue": 125.50,
    "cost": 3000.00,
    "roi": 4.0
  },
  "dailyBreakdown": [
    {
      "date": "2024-01-01",
      "visitors": 200,
      "conversions": 6,
      "revenue": 600.00,
      "mcpValue": 120.50
    }
  ]
}
```

## 🚨 Error Responses

### Standard Error Format
```json
{
  "error": {
    "code": "VALIDATION_ERROR",
    "message": "Invalid request data",
    "details": [
      {
        "field": "email",
        "message": "Email is required"
      },
      {
        "field": "password",
        "message": "Password must be at least 8 characters"
      }
    ],
    "timestamp": "2024-01-01T12:00:00Z",
    "traceId": "trace_123456"
  }
}
```

### Common Error Codes

| Code | Status | Description |
|------|--------|-------------|
| `VALIDATION_ERROR` | 400 | Request validation failed |
| `UNAUTHORIZED` | 401 | Authentication required |
| `FORBIDDEN` | 403 | Insufficient permissions |
| `NOT_FOUND` | 404 | Resource not found |
| `CONFLICT` | 409 | Resource already exists |
| `RATE_LIMITED` | 429 | Too many requests |
| `INTERNAL_ERROR` | 500 | Server error |
| `SERVICE_UNAVAILABLE` | 503 | Service temporarily unavailable |

## 📝 Request/Response Patterns

### Pagination
```http
GET /api/abtests?page=1&pageSize=20&sortBy=createdAt&sortOrder=desc
```

**Response:**
```json
{
  "data": [...],
  "pagination": {
    "currentPage": 1,
    "pageSize": 20,
    "totalPages": 5,
    "totalItems": 100,
    "hasNext": true,
    "hasPrevious": false
  }
}
```

### Filtering
```http
GET /api/conversions?variantId=1&startDate=2024-01-01&endDate=2024-01-31&conversionType=purchase
```

### Bulk Operations
```http
POST /api/conversions/bulk
Content-Type: application/json

{
  "conversions": [
    {
      "variantId": 1,
      "visitorId": "visitor_1",
      "revenue": 99.99
    },
    {
      "variantId": 2,
      "visitorId": "visitor_2",
      "revenue": 149.99
    }
  ]
}
```

## 🔧 Development Guidelines for AI Assistants

### Controller Pattern
```csharp
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MCPController : ControllerBase
{
    [HttpPost("calculate")]
    [ProducesResponseType(typeof(MCPCalculationResponse), 200)]
    [ProducesResponseType(typeof(ErrorResponse), 400)]
    public async Task<ActionResult<MCPCalculationResponse>> Calculate(
        [FromBody] MCPCalculationRequest request)
    {
        // Implementation
    }
}
```

### Request/Response DTOs
```csharp
public class MCPCalculationRequest
{
    [Required]
    public int VariantId { get; set; }
    
    [Required]
    public DateTime StartDate { get; set; }
    
    [Required]
    public DateTime EndDate { get; set; }
    
    public bool IncludeStatisticalAnalysis { get; set; } = false;
}

public class MCPCalculationResponse
{
    public int VariantId { get; set; }
    public decimal MCPValue { get; set; }
    public decimal Revenue { get; set; }
    public decimal Cost { get; set; }
    public int Conversions { get; set; }
    public int Visitors { get; set; }
    public decimal ConversionRate { get; set; }
    public DateTime CalculatedAt { get; set; }
    public StatisticalAnalysisDto? StatisticalAnalysis { get; set; }
}
```

### Validation Attributes
```csharp
public class CreateABTestRequest
{
    [Required(ErrorMessage = "Test name is required")]
    [StringLength(200, ErrorMessage = "Name must not exceed 200 characters")]
    public string Name { get; set; } = string.Empty;
    
    [Range(1, 99, ErrorMessage = "Traffic split must be between 1 and 99")]
    public int TrafficSplit { get; set; }
    
    [FutureDate(ErrorMessage = "Start date must be in the future")]
    public DateTime StartDate { get; set; }
}
```

## 🧪 Testing Examples

### Integration Test
```csharp
[Fact]
public async Task CalculateMCP_ValidRequest_ReturnsCorrectCalculation()
{
    // Arrange
    var request = new MCPCalculationRequest
    {
        VariantId = 1,
        StartDate = DateTime.UtcNow.AddDays(-30),
        EndDate = DateTime.UtcNow
    };

    // Act
    var response = await _client.PostAsJsonAsync("/api/mcp/calculate", request);

    // Assert
    response.EnsureSuccessStatusCode();
    var result = await response.Content.ReadFromJsonAsync<MCPCalculationResponse>();
    Assert.NotNull(result);
    Assert.True(result.MCPValue > 0);
}
```

## 📚 Additional Resources

- **Swagger UI**: `https://localhost:7001/swagger`
- **Health Check**: `https://localhost:7001/health`
- **Metrics**: `https://localhost:7001/metrics`
- **OpenAPI Spec**: `https://localhost:7001/swagger/v1/swagger.json`

## 🔄 API Versioning

APIs are versioned using URL path versioning:
- Current: `/api/v1/...`
- Future: `/api/v2/...`

Backward compatibility is maintained for at least 2 major versions.