using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebTech.Auth.Data.Models;
using WebTech.Auth.Services.Interfaces.User;

namespace WebTech.Auth.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public readonly UserManager<ApplicationUser> _userManager;
    public readonly RoleManager<IdentityRole> _roleManager;

    public UserController(
        IUserService userService,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _userService = userService;
        _userManager = userManager;
        _roleManager = roleManager;
    }


    [HttpGet]
    public async Task<IActionResult> GetAllUsers(string conditions)
    {
        var users = await _userService.GetUsersAsync(conditions);

        if(users == null)
        {
            return NotFound();
        }

        return Ok(users);
    }
}