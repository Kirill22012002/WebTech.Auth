using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using WebTech.Auth.Data.Context;
using WebTech.Auth.Data.Models;

namespace WebTech.Auth.Extension;

public static class IdentityExtension
{
    public static void ConfigureIdentityServer(this WebApplicationBuilder builder)
    {
        var migrationsAssembly = typeof(IdentityExtension).Assembly.GetName().Name;

        builder.Services.AddIdentityServer()
            .AddConfigurationStore(options =>
            {
                options.ConfigureDbContext = b =>
                    b.UseNpgsql(builder.GetConnectionString("ConfigurationStore"),
                        sql => sql.MigrationsAssembly(migrationsAssembly));
            })
            .AddOperationalStore(options =>
            {
                options.ConfigureDbContext = b =>
                    b.UseNpgsql(builder.GetConnectionString("OperationalStore"),
                        sql => sql.MigrationsAssembly(migrationsAssembly));
            })
            .AddAspNetIdentity<ApplicationUser>()
            .AddResourceOwnerValidator<ROPCValidator>()
            .AddDeveloperSigningCredential()
            .AddJwtBearerClientAuthentication();
    }

    public static void ConfigureAspIdentity(this WebApplicationBuilder builder)
    {
        builder.Services.AddIdentity<ApplicationUser, IdentityRole>(config =>
        {
            config.Password.RequiredLength = 8;
            config.Password.RequireDigit = true;
            config.Password.RequireNonAlphanumeric = true;
            config.Password.RequireUppercase = true;
            config.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+/ абвгдеёжзийклмнопрстуфхцчшщъыьэюяАБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";
            config.Password.RequireLowercase = false;
            config.User.RequireUniqueEmail = true;
        })
            .AddEntityFrameworkStores<AccessDbContext>()
            .AddDefaultTokenProviders();
    }

    private static string GetConnectionString(this WebApplicationBuilder builder, string connStringName)
    {
        if (builder.Services == null) throw /*new CriticalServerException(nameof(builder.Services));*/ new Exception();

        var connStrBuilder = new NpgsqlConnectionStringBuilder(
            builder.Configuration.GetConnectionString(connStringName));

        return connStrBuilder.ConnectionString;
    }
}