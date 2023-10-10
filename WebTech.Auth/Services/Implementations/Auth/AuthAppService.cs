using Microsoft.AspNetCore.Identity;
using WebTech.Auth.Data.Models;
using WebTech.Auth.ErrorHandler.CustomExceptions;
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

    public async Task<AuthServiceDto> SignUpAsync(UserSignUpInput signUpRequest)
    {
        if (string.IsNullOrWhiteSpace(signUpRequest.Name) || string.IsNullOrWhiteSpace(signUpRequest.Email))
            throw new ClientException("Name and Email are required fields.", 400);

        var existingUser = await _userManager.FindByEmailAsync(signUpRequest.Email);
        if (existingUser != null)
            throw new ClientException("Email is already in use.", 400);

        var user = new ApplicationUser() { Email = signUpRequest.Email, Name = signUpRequest.Name };
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

    public async Task<IdentityResult> ChangeUserInfoAsync(string userId, ChangeUserInfoInputByUser userInfoInput)
    {
        if (string.IsNullOrWhiteSpace(userInfoInput.Name) || string.IsNullOrWhiteSpace(userInfoInput.Email) || userInfoInput.Age <= 0)
        {
            throw new ClientException("Name, Email, and Age are required fields.", 400);
        }
        var existingUser = await _userManager.FindByEmailAsync(userInfoInput.Email);
        if (existingUser != null)
        {
            throw new ClientException("Email is already in use.", 400);
        }
        if (userInfoInput.Age <= 0)
        {
            throw new ClientException("Age must be a positive number.", 400);
        }

        var user = await _userManager.FindByIdAsync(userId);

        if(user == null)
        {
            throw new ClientException("User not found", 404);
        }

        if(!string.IsNullOrWhiteSpace(userInfoInput.Name) && userInfoInput.Name?.Trim() != user.Name)
        {
            user.Name = userInfoInput.Name;
        }
        if(userInfoInput.Email?.Trim() != user.Email)
        {
            user.Email = userInfoInput.Email;
        }
        if(userInfoInput.Age != user.Age)
        {
            user.Age = userInfoInput.Age;
        }

        return await _userManager.UpdateAsync(user);
    }
}