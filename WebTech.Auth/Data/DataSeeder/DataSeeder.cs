using Microsoft.AspNetCore.Identity;

namespace WebTech.Auth.Data.DataSeeder;

public class DataSeeder : IDataSeeder
{
    private readonly RoleManager<IdentityRole> _roleManager;

    public DataSeeder(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task SeedAsync()
    {
        await SeedAccessDbContext();
    }

    private async Task SeedAccessDbContext()
    {
        if (await _roleManager.FindByNameAsync(IdentityConfig.SuperAdmin) == null)
        {
            await _roleManager.CreateAsync(new IdentityRole(IdentityConfig.User));
            await _roleManager.CreateAsync(new IdentityRole(IdentityConfig.Admin));
            await _roleManager.CreateAsync(new IdentityRole(IdentityConfig.Support));
            await _roleManager.CreateAsync(new IdentityRole(IdentityConfig.SuperAdmin));
        }
    }
}