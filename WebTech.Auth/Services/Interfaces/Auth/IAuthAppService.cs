using WebTech.Auth.Models.Dtos;
using WebTech.Auth.Models.Inputs;

namespace WebTech.Auth.Services.Interfaces.Auth;

public interface IAuthAppService
{
    public Task<AuthServiceDto> SignUp(UserSignUpInput signUpRequest);
}
