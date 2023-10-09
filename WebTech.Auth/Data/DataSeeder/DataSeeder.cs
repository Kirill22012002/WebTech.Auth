using IdentityModel;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using WebTech.Auth.Data.Models;

namespace WebTech.Auth.Data.DataSeeder;

public class DataSeeder : IDataSeeder
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public DataSeeder(
        RoleManager<IdentityRole> roleManager,
        UserManager<ApplicationUser> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
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

        var userData = new[]
        {
            new
            {
                Name = "Bob James",
                Email = "bob@gmail.com",
                Age = 34,
                Role = IdentityConfig.SuperAdmin,
            },
            new
            {
                Name = "Kirill Perepechkin",
                Email = "perepechkin.kirill@gmail.com",
                Age = 21,
                Role = IdentityConfig.Support
            },
            new
            {
                Name = "Denis Borisov",
                Email = "admin_denis@yandex.ru",
                Age = 21,
                Role = IdentityConfig.Admin
            },
            new
            {
                Name = "Jane Doe",
                Email = "jane@example.com",
                Age = 34,
                Role = IdentityConfig.Support
            },
            new
            {
                Name = "Alice Smith",
                Email = "alice@example.com",
                Age = 56,
                Role = IdentityConfig.Admin
            },
            new
            {
                Name = "John Smith",
                Email = "john@example.com",
                Age = 14,
                Role = IdentityConfig.User
            },
            new
            {
                Name = "Susan Johnson",
                Email = "susan@example.com",
                Age = 14,
                Role = IdentityConfig.User
            },
            new
            {
                Name = "User1",
                Email = "user1@example.com",
                Age = 25,
                Role = IdentityConfig.User
            },
            new
            {
                Name = "User2",
                Email = "user2@example.com",
                Age = 30,
                Role = IdentityConfig.User
            },
            new
            {
                Name = "User3",
                Email = "user3@example.com",
                Age = 40,
                Role = IdentityConfig.User
            },
            new
            {
                Name = "User4",
                Email = "user4@example.com",
                Age = 35,
                Role = IdentityConfig.User
            },
            new
            {
                Name = "User5",
                Email = "user5@example.com",
                Age = 28,
                Role = IdentityConfig.User
            },
            new
            {
                Name = "User6",
                Email = "user6@example.com",
                Age = 22,
                Role = IdentityConfig.User
            },
            new
            {
                Name = "User7",
                Email = "user7@example.com",
                Age = 33,
                Role = IdentityConfig.User
            },
            new
            {
                Name = "User8",
                Email = "user8@example.com",
                Age = 31,
                Role = IdentityConfig.User
            },
            new
            {
                Name = "User9",
                Email = "user9@example.com",
                Age = 27,
                Role = IdentityConfig.User
            },
            new
            {
                Name = "User10",
                Email = "user10@example.com",
                Age = 29,
                Role = IdentityConfig.User
            }
        };

        foreach (var userItem in userData)
        {
            if (await _userManager.FindByEmailAsync(userItem.Email) == null)
            {
                var user = new ApplicationUser()
                {
                    Name = userItem.Name,
                    Email = userItem.Email,
                    Age = userItem.Age,
                    EmailConfirmed = true
                };

                var userResult = await _userManager.CreateAsync(user, "Password123*");
                if (userResult.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, userItem.Role);
                    await _userManager.AddClaimsAsync(user, new Claim[]
                    {
                        new Claim(JwtClaimTypes.Name, userItem.Name),
                        new Claim(JwtClaimTypes.Role, userItem.Role),
                        new Claim(JwtClaimTypes.Email, userItem.Email)
                    });
                }
            }
        }

    }
}