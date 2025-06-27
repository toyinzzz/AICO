using AICO.Infrastructure.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory; // For UseInMemoryDatabase
using Npgsql.EntityFrameworkCore.PostgreSQL; // Add this line for UseNpgsql
using AICO.Infrastructure.Data;
using AICO.Application.Interfaces.Services;
using AICO.Application.Commands;
using AICO.Application.Interfaces.Commands;
using AICO.Application.Interfaces.Queries;
using AICO.Application.Queries;
using AICO.Infrastructure.DependencyInjection;


var builder = WebApplication.CreateBuilder(args);

// Configure logging
builder.Logging.AddConsole();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add HTTP Context Accessor for getting user context
builder.Services.AddHttpContextAccessor();

// Add Entity Framework
builder.Services.AddDbContext<AicoDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    
    if (builder.Environment.IsEnvironment("Testing"))
    {
        // Use in-memory database for testing
        options.UseInMemoryDatabase(databaseName: "TestDb");
        options.EnableSensitiveDataLogging();
        options.EnableDetailedErrors();
    }
    else
    {
        options.UseNpgsql(connectionString);
    }
});

// Add MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

// Register services using extension methods
builder.Services.AddDomainServices();
builder.Services.AddRepositories();
builder.Services.AddApplicationServices();
builder.Services.AddExternalServices();

// Command and Query handlers are registered in AddApplicationServices()


// Add HTTP Client for external services
builder.Services.AddHttpClient();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

// Ensure database is created
// Auto-migrate database in testing environment
if (app.Environment.IsEnvironment("Testing"))
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<AicoDbContext>();
    context.Database.EnsureCreated();
}

app.Run();

public partial class Program { } // For testing
