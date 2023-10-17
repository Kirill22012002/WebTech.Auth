using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebTech.Auth.Data.Context;
using WebTech.Auth.Data.Models;

namespace WebTech.Auth.Extension;

public static class IdentityExtension
{
    public static void ConfigureIdentityServer(this WebApplicationBuilder builder)
    {
        var migrationsAssembly = typeof(IdentityExtension).Assembly.GetName().Name;

        string confStoreConnectionString;
        string operStoreConnectionString;

        if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
        {
            confStoreConnectionString = DbExtension.GetConnectionString("ConfigurationStore");
            operStoreConnectionString = DbExtension.GetConnectionString("OperationalStore");
        }
        else
        {
            confStoreConnectionString = builder.Configuration.GetConnectionString("ConfigurationStore");
            operStoreConnectionString = builder.Configuration.GetConnectionString("OperationalStore");
        }

        builder.Services.AddIdentityServer()
            .AddConfigurationStore(options =>
            {
                options.ConfigureDbContext = b =>
                    b.UseNpgsql(confStoreConnectionString,
                        sql => sql.MigrationsAssembly(migrationsAssembly));
            })
            .AddOperationalStore(options =>
            {
                options.ConfigureDbContext = b =>
                    b.UseNpgsql(operStoreConnectionString,
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
            config.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+/ абвгдеёжзийклмнопрстуфхцчшщъыьэюяАБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";
        })
            .AddEntityFrameworkStores<AccessDbContext>()
            .AddDefaultTokenProviders();
    }

}