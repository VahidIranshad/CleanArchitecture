using CA.Api.Authorization;
using CA.Api.Extensions;
using CA.Api.Middleware;
using CA.Api.Services;
using CA.Application;
using CA.Application.Contracts.Identity;
using CA.Domain.Constants.Permission;
using CA.Identity;
using CA.Identity.DbContexts;
using CA.Infrastructure;
using CA.Infrastructure.DbContexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddCors();
builder.Services.AddControllers();
builder.Services.AddResponseCaching();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddLocalization(options =>
//{
//    options.ResourcesPath = "Resources";
//});

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.RegisterSwagger();
builder.Services.ConfigureApplicationServices();
builder.Services.ConfigurePersistenceServices(configuration);
builder.Services.ConfigureIdentityServices(configuration);
builder.Services.AddJwtAuthentication(builder.Services.GetApplicationSettings(configuration));


builder.Services.AddTransient<IAuthorizationHandler, PermissionHandler>();
builder.Services.AddAuthorization(options =>
{
    // Here I stored necessary permissions/roles in a constant
    foreach (var prop in typeof(Permissions).GetNestedTypes().SelectMany(c => c.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)))
    {
        var propertyValue = prop.GetValue(null);
        if (propertyValue is not null)
        {
            options.AddPolicy(propertyValue.ToString(), policy => policy.RequireClaim(ApplicationClaimTypes.Permission, propertyValue.ToString()));
        }
    }
});
builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

//builder.Services.AddControllers(options =>
//{
//    options.OutputFormatters.RemoveType<SystemTextJsonOutputFormatter>();
//    options.OutputFormatters.Add(new SystemTextJsonOutputFormatter(new JsonSerializerOptions(JsonSerializerDefaults.Web)
//    {
//        ReferenceHandler = ReferenceHandler.Preserve,
//    }));
//});
var app = builder.Build();

app.UseForwarding(configuration);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.ConfigureSwagger();
}

app.UseHttpsRedirection();
var origins = configuration.GetSection("Origins").Value.Split(",");

app.UseCors(x => x
            .AllowAnyMethod()
            .AllowAnyHeader()
            .WithOrigins(origins)
            );

app.UseResponseCaching();
app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");

//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;

//    var contextSDA = services.GetRequiredService<CustomDbContext>();
//    contextSDA.Database.SetConnectionString(configuration.GetConnectionString("SDAConnectionString"));
//    if (contextSDA.Database.GetPendingMigrations().Any())
//    {
//        contextSDA.Database.Migrate();
//    }
//    var context = services.GetRequiredService<IdentityDbContext>();
//    context.Database.SetConnectionString(configuration.GetConnectionString("IdentityConnectionString"));
//    if (context.Database.GetPendingMigrations().Any())
//    {
//        context.Database.Migrate();
//    }
//}

app.Run();

public partial class Program { }
