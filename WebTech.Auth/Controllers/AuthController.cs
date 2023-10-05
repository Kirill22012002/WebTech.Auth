using Microsoft.AspNetCore.Mvc;
using WebTech.Auth.Models.Inputs;
using WebTech.Auth.Services.Interfaces.Auth;

namespace WebTech.Auth.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class AuthController : ControllerBase
{
    private readonly IAuthAppService _authService;

    public AuthController(
        IAuthAppService authService)
    {
        _authService = authService;
    }

    [HttpPost]
    public async Task<IActionResult> SignUp(UserSignUpInput signUpRequest)
    {
        var result = await _authService.SignUp(signUpRequest);

        if (!result.Success)
        {
            return BadRequest(result.ErrorMessage);
        }

        return Ok(result.UserId);
    }

}