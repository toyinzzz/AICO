using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using AICO.Infrastructure.Data;
using AICO.Domain.Entities;
using AICO.Domain.ValueObjects;

namespace AICO.IntegrationTests
{
    public class TestWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");
            
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
                    options.EnableSensitiveDataLogging();
                    options.EnableDetailedErrors();
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
            // Clear existing data
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            // Seed test users
            var testUser = User.Create(
                "test@example.com", 
                "testuser", 
                "Test", 
                "User", 
                "hashedpassword", 
                "salt");
            testUser.Id = Guid.Parse("11111111-1111-1111-1111-111111111111");
            context.Users.Add(testUser);

            // Seed test website
            var testWebsite = Website.Create(
                "https://test.example.com", 
                "Test Website", 
                testUser.Id);
            testWebsite.Id = Guid.Parse("22222222-2222-2222-2222-222222222222");
            context.Websites.Add(testWebsite);

            // Seed test campaign
            var testCampaign = new Campaign("Test Campaign", "Test campaign for MVP validation", testWebsite.Id, testUser.Id)
            {
                Id = Guid.Parse("33333333-3333-3333-3333-333333333333")
            };
            context.Campaigns.Add(testCampaign);

            // Seed test A/B test
            var testType = TestType.Headline;
            var abTest = AbTest.Create(
                "Test A/B Test",
                "Test Description",
                Guid.NewGuid(), // campaignId - we'll create a dummy one
                testType,
                ".cta-button",
                "<button class='cta-button blue'>Click Me</button>",
                "conversion_rate"
            );
            abTest.Id = Guid.Parse("55555555-5555-5555-5555-555555555555");
            context.AbTests.Add(abTest);

            // Create Variants
            var controlVariant = new AbTestVariant(
                abTest.Id,
                "Control",
                "<button class='cta-button'>Click Me</button>",
                50.0m,
                isControl: true
            );
            controlVariant.Id = Guid.Parse("66666666-6666-6666-6666-111111111111");

            var testVariant = new AbTestVariant(
                abTest.Id,
                "Test Variant",
                "<button class='cta-button red'>Click Me</button>",
                50.0m,
                isControl: false
            );
            testVariant.Id = Guid.Parse("77777777-7777-7777-7777-222222222222");

            context.AbTestVariants.AddRange(controlVariant, testVariant);

            // Save all changes
            context.SaveChanges();
        }
    }
}