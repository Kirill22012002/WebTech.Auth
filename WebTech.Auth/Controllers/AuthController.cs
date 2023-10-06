using Duende.IdentityServer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using WebTech.Auth.Controllers.Base;
using WebTech.Auth.Models.Dtos;
using WebTech.Auth.Models.Inputs;
using WebTech.Auth.Services.Interfaces.Auth;

namespace WebTech.Auth.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class AuthController : BaseController
{
    private readonly IAuthAppService _authService;

    public AuthController(
        IAuthAppService authService)
    {
        _authService = authService;
    }

    [HttpPost]
    public async Task<IActionResult> SignUp([FromBody] UserSignUpInput signUpRequest)
    {
        var result = await _authService.SignUpAsync(signUpRequest);

        if (!result.Success)
        {
            return BadRequest(result.ErrorMessage);
        }

        return Ok(result.UserId);
    }

    [HttpPut]
    [Authorize(IdentityServerConstants.LocalApi.AuthenticationScheme)]
    public async Task<IActionResult> ChangeUserInfo([FromBody] ChangeUserInfoInputByUser userInfoInput)
    {
        if (!ModelState.IsValid)
        {
            IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
            return BadRequest(allErrors.Select(x => x.ErrorMessage));
        }

        var userId = GetUserId();

        if (userId == null)
        {
            return BadRequest(new ErrorApiDto() { error_message = "no_userId" });
        }

        var updatingResult = await _authService.ChangeUserInfoAsync(userId, userInfoInput);
        if (updatingResult.Succeeded)
        {
            return Ok();
        }

        return BadRequest(new { error_message = updatingResult.Errors.Select(x => x.Code) });
    }
}