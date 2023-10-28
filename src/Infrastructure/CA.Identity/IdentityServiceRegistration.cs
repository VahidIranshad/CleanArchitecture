using CA.Application.Contracts.Identity;
using CA.Identity.DbContexts;
using CA.Identity.Models;
using CA.Identity.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CA.Identity
{

    public static class IdentityServiceRegistration
    {
        public static IServiceCollection ConfigureIdentityServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<IdentityDbContext>(options =>
               options.UseSqlServer(
                   configuration.GetConnectionString("IdentityConnectionString")));



            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<IdentityDbContext>().AddDefaultTokenProviders();

            //services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IRoleClaimService, RoleClaimService>();
            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IUserService, UserService>();

            //services.AddTransient<IAuditService, AuditService>();
            return services;
        }

    }

    public class IdentityDbContextFactor : IDesignTimeDbContextFactory<IdentityDbContext>
    {
        public IdentityDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory()))
                .AddJsonFile("appsettings.json")
                .Build();
            var builder = new DbContextOptionsBuilder<IdentityDbContext>();
            var connectionString = configuration.GetConnectionString("IdentityConnectionString");
            builder.UseSqlServer(connectionString);
            //builder.UseSqlServer("Data Source=.;Initial Catalog=Fuzzy;User ID=sa;Password=1;Persist Security Info=True");
            return new IdentityDbContext(builder.Options);

        }
    }
}
