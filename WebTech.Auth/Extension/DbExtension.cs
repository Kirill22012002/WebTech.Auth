using Duende.IdentityServer.EntityFramework.DbContexts;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using WebTech.Auth.Data.Context;

namespace WebTech.Auth.Extension;

public static class DbExtension
{
    public static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        if (services == null) /*throw new CriticalServerException(nameof(services));*/ throw new Exception();

        var conStrBuilder = new NpgsqlConnectionStringBuilder(
                    configuration.GetConnectionString("UsersDatabase"));

        conStrBuilder.Password = "password";

        var connection = conStrBuilder.ConnectionString;

        services.AddDbContext<AccessDbContext>(options =>
            options.UseNpgsql(connection));
    }

    /*public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var services = scope.ServiceProvider;

        var context = services.GetRequiredService<AccessDbContext>();
        context.Database.Migrate();

        var configurationDbContext = services.GetRequiredService<ConfigurationDbContext>();
        configurationDbContext.Database.Migrate();

        var persistedGrantDbContext = services.GetRequiredService<PersistedGrantDbContext>();
        persistedGrantDbContext.Database.Migrate();
    }*/
}