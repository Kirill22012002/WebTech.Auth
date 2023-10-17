using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.Extensions;
using Microsoft.EntityFrameworkCore;
using WebTech.Auth.Data.Context;
using WebTech.Auth.Data.DataSeeder;
using WebTech.Auth.ErrorHandler.CustomExceptions;

namespace WebTech.Auth.Extension;

public static class DbExtension
{
    public static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        if (services == null) throw new CriticalServerException(nameof(services));

        string connectionString;

        if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
        {
            connectionString = GetConnectionString();
        }
        else
        {
            connectionString = configuration.GetConnectionString("LocalDatabase");
        }

        services.AddDbContext<AccessDbContext>(options =>
            options.UseNpgsql(connectionString));
    }

    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var services = scope.ServiceProvider;

        var context = services.GetRequiredService<AccessDbContext>();
        context.Database.Migrate();

        var configurationDbContext = services.GetRequiredService<ConfigurationDbContext>();
        configurationDbContext.Database.Migrate();

        var persistedGrantDbContext = services.GetRequiredService<PersistedGrantDbContext>();
        persistedGrantDbContext.Database.Migrate();
    }

    public static void SeedDatabase(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var dbSeed = scope.ServiceProvider.GetService<IDataSeeder>();
        dbSeed?.SeedAsync().Wait();
    }

    public static string GetConnectionString()
    {
        /*var connectionUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

        connectionUrl = connectionUrl.Replace("postgres://", string.Empty);
        var userPassSide = connectionUrl.Split("@")[0];
        var hostSide = connectionUrl.Split("@")[1];

        var user = userPassSide.Split(":")[0];
        var password = userPassSide.Split(":")[1];
        var host = hostSide.Split("/")[0];
        var database = hostSide.Split("/")[1].Split("?")[0];*/

        //return $"Host={host};Database={database};Username={user};Password={password};SSL Mode=Require;Trust Server Certificate=true";
        return $"Host=ec2-34-251-233-253.eu-west-1.compute.amazonaws.com;Database=dc8kvfoqolibka;Username=zjusaeiisuipmt;Password=2f1496e657b1ff16ccb2b2351d80f6750c4245569137e30e0b2752ebd5e6d957;SSL Mode=Require;Trust Server Certificate=true";

    }
}