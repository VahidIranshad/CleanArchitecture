
using CA.Application.Contracts.Ent;
using CA.Application.Contracts.Generic;
using CA.Infrastructure.DbContexts;
using CA.Infrastructure.Repositories.Ent;
using CA.Infrastructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CA.Infrastructure
{

    public static class InfrastructureServicesRegistration
    {
        public static IServiceCollection ConfigurePersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CustomDbContext>(options =>
               options.UseSqlServer(configuration.GetConnectionString("SDAConnectionString")),
               ServiceLifetime.Transient
               );


            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
            services.AddTransient(typeof(ISelectionRepository), typeof(SelectionRepository));

            return services;
        }

    }
    public class SDALeitnerBoxDbContextFactor : IDesignTimeDbContextFactory<CustomDbContext>
    {
        public CustomDbContext CreateDbContext(string[] args)
        {

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var builder = new DbContextOptionsBuilder<CustomDbContext>();
            var connectionString = configuration.GetConnectionString("SDAConnectionString");
            builder.UseSqlServer(connectionString);
            return new CustomDbContext(builder.Options);

        }
    }
}
