using Duende.IdentityServer.EntityFramework.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Models;
using WebTech.Auth;
using WebTech.Auth.Data.Context;
using WebTech.Auth.Extension;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDependencies();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDatabase(builder.Configuration);

builder.ConfigureAspIdentity();
builder.ConfigureIdentityServer();

builder.Services.AddLocalApiAuthentication();


builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"Enter 'Bearer' [space] and your token",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                },
                Scheme="oauth2",
                Name="Bearer",
                In=ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

IdentityModelEventSource.ShowPII = true;

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

ApplyMigrations(app);

/*app.ApplyMigrations();
*/
app.UseCors(options =>
{
    options.AllowAnyOrigin();
    options.AllowAnyHeader();
    options.AllowAnyMethod();
});

app.UseHttpsRedirection();

app.UseIdentityServer();
app.UseAuthorization();

app.MapControllers();

app.Run();

static void ApplyMigrations(IApplicationBuilder app)
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