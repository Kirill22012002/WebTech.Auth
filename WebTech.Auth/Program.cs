using Microsoft.Extensions.Logging.Console;
using Microsoft.IdentityModel.Logging;
using WebTech.Auth;
using WebTech.Auth.ErrorHandler;
using WebTech.Auth.Extension;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging(builder =>
{
    builder.AddConsole();
});

using var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddSimpleConsole(i => i.ColorBehavior = LoggerColorBehavior.Disabled);
});

var logger = loggerFactory.CreateLogger<Program>();

logger.LogInformation("------------------------------------here we conect controllers-------------------------------------------");

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDependencies();
builder.Services.AddHttpContextAccessor();

if(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
{
    logger.LogInformation(DbExtension.GetConnectionString()); ;
}

builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddAutoMapper();

builder.ConfigureAspIdentity();
builder.ConfigureIdentityServer();

builder.Services.AddLocalApiAuthentication();

builder.AddSwaggerGenConfiguration();

IdentityModelEventSource.ShowPII = true;

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebTech.Auth v1");
    c.RoutePrefix = "swagger";
});

app.ApplyMigrations();
app.SeedDatabase();

app.UseCors(options =>
{
    options.AllowAnyOrigin();
    options.AllowAnyHeader();
    options.AllowAnyMethod();
});

//app.UseHttpsRedirection();

app.UseIdentityServer();
app.UseAuthorization();

app.MapControllers();
app.UseErrorHandler();

app.Run();