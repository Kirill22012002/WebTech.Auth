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

        var connection = conStrBuilder.ConnectionString;

        services.AddDbContext<AccessDbContext>(options =>
            options.UseNpgsql(connection));
    }
}