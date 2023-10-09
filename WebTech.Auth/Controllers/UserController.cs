using Microsoft.AspNetCore.Mvc;
using WebTech.Auth.Models.Inputs;
using WebTech.Auth.Services.Interfaces.User;
using WebTech.Auth.Controllers.Base;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using WebTech.Auth.Models.Dtos;

namespace WebTech.Auth.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class UserController : BaseController
{
    private readonly IUserService _userService;

    public UserController(
        IUserService userService)
    {
        _userService = userService;
    }


    [HttpGet]
    public async Task<IActionResult> GetUsers([FromQuery] GetUsersDto usersDto)
    {
        var users = await _userService.GetUsersAsync(usersDto);

        if (users == null)
        {
            return NotFound();
        }

        return Ok(users);
    }

    [HttpGet]
    public async Task<IActionResult> GetById([FromQuery] string userId)
    {
        var user = await _userService.GetUserByIdAsync(userId);
        
        if(user == null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserInput userInput)
    {
        var result = await _userService.CreateUserAsync(userInput);

        if (!result.Success)
        {
            return BadRequest(result.ErrorMessage);
        }

        return Ok(result.UserId);
    }

    [HttpPut]
    public async Task<IActionResult> ChangeUserInfo([FromBody] ChangeUserInfoInput userInfoInput)
    {
        if (!ModelState.IsValid)
        {
            IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
            return BadRequest(allErrors.Select(x => x.ErrorMessage));
        }

        var updatingResult = await _userService.ChangeUserInfoAsync(userInfoInput);
        if (updatingResult.Succeeded)
        {
            return Ok();
        }

        return BadRequest(new { error_message = updatingResult.Errors.Select(x => x.Code) });
    }
}