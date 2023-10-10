using Duende.IdentityServer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WebTech.Auth.Controllers.Base;
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
    [SwaggerOperation(Summary = "User sign up")]
    [SwaggerResponse(200, "User id of the newly created user.")]
    [SwaggerResponse(400, "Bad request with error message")]
    public async Task<IActionResult> SignUp([FromBody] UserSignUpInput signUpRequest)
    {
        var result = await _authService.SignUpAsync(signUpRequest);

        if (!result.Success)
        {
            return BadRequest(new ErrorResponse { Message = result.ErrorMessage });
        }

        return Ok(result.UserId);
    }

    [HttpPut]
    [Authorize(IdentityServerConstants.LocalApi.AuthenticationScheme)]
    [SwaggerOperation(Summary = "Change user information")]
    [SwaggerResponse(200, "User information updated successfully.")]
    [SwaggerResponse(400, "Bad request with error messages.")]
    [SwaggerResponse(401, "Unauthorized - user not authenticated")]
    public async Task<IActionResult> ChangeUserInfo([FromBody] ChangeUserInfoInputByUser userInfoInput)
    {
        if (!ModelState.IsValid)
        {
            IEnumerable<string> errorMessages = ModelState.Values.SelectMany(v => v.Errors.Select(x => x.ErrorMessage));
            return BadRequest(new ErrorResponse { Message = "Invalid input data.", Errors = errorMessages });
        }

        var userId = GetUserId();

        if (userId == null)
        {
            return BadRequest(new ErrorResponse { Message = "no user id"});
        }

        var updatingResult = await _authService.ChangeUserInfoAsync(userId, userInfoInput);
        if (updatingResult.Succeeded)
        {
            return Ok();
        }

        return BadRequest(new ErrorResponse { Message = "Failed to change user information.", Errors = updatingResult.Errors.Select(x => x.Code) });
    }
}