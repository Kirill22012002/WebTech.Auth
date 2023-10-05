using Microsoft.AspNetCore.Identity;
using WebTech.Auth.Data.Models;
using WebTech.Auth.Models.Dtos;
using WebTech.Auth.Models.Inputs;
using WebTech.Auth.Services.Interfaces.Auth;

namespace WebTech.Auth.Services.Implementations.Auth;

public class AuthAppService : IAuthAppService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AuthAppService(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<AuthServiceDto> SignUp(UserSignUpInput signUpRequest)
    {
        var user = new ApplicationUser() { Email = signUpRequest.Email, UserName = signUpRequest.UserName };
        var registrationResult = await _userManager.CreateAsync(user, signUpRequest.Password);

        if (registrationResult.Succeeded)
        {
            var role = await _roleManager.FindByNameAsync("User");
            var roleAddingResult = await _userManager.AddToRoleAsync(user, role.Name);

            if (!roleAddingResult.Succeeded)
            {
                var roleAddingResultErrors = string.Join(", ", roleAddingResult.Errors.Select(error => error.Code));

                return new AuthServiceDto()
                {
                    Success = false,
                    ErrorMessage = roleAddingResultErrors
                };
            }

            return new AuthServiceDto()
            {
                UserId = user.Id
            };
        }

        var errorMessage = string.Join(", ", registrationResult.Errors.Select(error => error.Code));

        return new AuthServiceDto()
        {
            Success = false,
            ErrorMessage = errorMessage
        };
    }
}