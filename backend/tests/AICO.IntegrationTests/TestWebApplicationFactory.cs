using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using AICO.Infrastructure.Data; // Added this line

namespace AICO.IntegrationTests
{
    public class TestWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Remove the app's ApplicationDbContext registration
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<AicoDbContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Add AicoDbContext using an in-memory database for testing
                services.AddDbContext<AicoDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                });

                // Build the service provider
                var sp = services.BuildServiceProvider();

                // Create a scope to obtain a reference to the database context
                using var scope = sp.CreateScope();
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<AicoDbContext>();
                var logger = scopedServices.GetRequiredService<ILogger<TestWebApplicationFactory>>();

                // Ensure the database is created
                db.Database.EnsureCreated();

                try
                {
                    // Seed the database with test data if needed
                    SeedTestData(db);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred seeding the database with test data. Error: {Message}", ex.Message);
                }
            });
        }

        private static void SeedTestData(AicoDbContext context)
        {
            // Add any test data seeding logic here
        }
    }
}