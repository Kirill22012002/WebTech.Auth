using Microsoft.AspNetCore.Identity;
using WebTech.Auth.Models.Dtos;
using WebTech.Auth.Models.Inputs;

namespace WebTech.Auth.Services.Interfaces.Auth;

public interface IAuthAppService
{
    public Task<AuthServiceDto> SignUpAsync(UserSignUpInput signUpRequest);
    public Task<IdentityResult> ChangeUserInfoAsync(string userId, ChangeUserInfoInputByUser userInfoInput);
}