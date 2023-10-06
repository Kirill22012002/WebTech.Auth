using Microsoft.AspNetCore.Identity;
using WebTech.Auth.Models.Dtos;
using WebTech.Auth.Models.Inputs;
using WebTech.Auth.Models.ViewModels;

namespace WebTech.Auth.Services.Interfaces.User;

public interface IUserService
{
    public Task<IEnumerable<UserViewModel>> GetUsersAsync(GetUsersDto usersDto);
    public Task<UserViewModel> GetUserByIdAsync(string userId);
    public Task<AuthServiceDto> CreateUserAsync(CreateUserInput userInput);
    public Task<IdentityResult> ChangeUserInfoAsync(ChangeUserInfoInput userInfoInput);
}