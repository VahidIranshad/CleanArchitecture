using CA.Identity.DbContexts;
using CA.Infrastructure.DbContexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SharedDatabaseSetup;
using System.Data.Common;

namespace FunctionalTests
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        public static object o = new object();
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {

                builder.ConfigureServices(services =>
                {
                // Remove the app's StoreContext registration.
                    var descriptorSDADbContext = services.SingleOrDefault(
                        d => d.ServiceType ==
                            typeof(DbContextOptions<CustomDbContext>));

                    if (descriptorSDADbContext != null)
                    {
                        services.Remove(descriptorSDADbContext);
                    }

                    var descriptorIdentityDbContext = services.SingleOrDefault(
                        d => d.ServiceType ==
                            typeof(DbContextOptions<IdentityDbContext>));

                    if (descriptorIdentityDbContext != null)
                    {
                        services.Remove(descriptorIdentityDbContext);
                    }

                // Add StoreContext using an in-memory database for testing.
                    string dbNameSDADbContext = "DataSource=myshareddb;mode=memory;cache=shared";
                    DbConnection ConnectionSDADbContext = new SqliteConnection($"Filename={dbNameSDADbContext}");
                    ConnectionSDADbContext.Open();
                    services.AddDbContext<CustomDbContext>(options =>
                    {
                        options.UseSqlite(ConnectionSDADbContext);
                    //options.UseInMemoryDatabase("InMemoryDbForFunctionalTesting");
                    });
                    services.AddDbContext<IdentityDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("InMemoryDbForFunctionalTestingIdentity");
                    });

                // Get service provider.
                    var serviceProvider = services.BuildServiceProvider();

                    using (var scope = serviceProvider.CreateScope())
                    {
                        var scopedServices = scope.ServiceProvider;

                        var logger = scopedServices.GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

                        var storeDbContextSDADbContext = scopedServices.GetRequiredService<CustomDbContext>();
                        storeDbContextSDADbContext.Database.EnsureCreated();

                        try
                        {
                            DatabaseSetup.SeedDataCustomDbContext(storeDbContextSDADbContext);
                        }
                        catch (Exception ex)
                        {
                            logger.LogError(ex, $"An error occurred seeding the Store database with test messages. Error: {ex.Message}");
                        }


                        var storeDbContextIdentityDbContext = scopedServices.GetRequiredService<IdentityDbContext>();
                        storeDbContextIdentityDbContext.Database.EnsureCreated();

                        try
                        {
                            DatabaseSetup.SeedDataIdentityDbContext(storeDbContextIdentityDbContext);
                        }
                        catch (Exception ex)
                        {
                            logger.LogError(ex, $"An error occurred seeding the Store database with test messages. Error: {ex.Message}");
                        }
                    }
                });

            
        }

        public void CustomConfigureServices(IWebHostBuilder builder)
        {
            //builder.ConfigureServices(services =>
            //{
            //    // Get service provider.
            //    var serviceProvider = services.BuildServiceProvider();

            //    using (var scope = serviceProvider.CreateScope())
            //    {
            //        var scopedServices = scope.ServiceProvider;

            //        //var logger = scopedServices.GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

            //        //var storeDbContextSDADbContext = scopedServices.GetRequiredService<CustomDbContext>();
            //        //var storeDbContextIdentityDbContext = scopedServices.GetRequiredService<IdentityDbContext>();

            //        try
            //        {
            //            //DatabaseSetup.SeedDataCustomDbContext(storeDbContextSDADbContext);
            //            //DatabaseSetup.SeedDataIdentityDbContext(storeDbContextIdentityDbContext);
            //        }
            //        catch (Exception ex)
            //        {
            //            //logger.LogError(ex, $"An error occurred seeding the Store database with test messages. Error: {ex.Message}");
            //        }
            //    }
            //});
        }
    }
}
