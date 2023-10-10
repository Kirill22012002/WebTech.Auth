using Microsoft.AspNetCore.Mvc;
using WebTech.Auth.Models.Inputs;
using WebTech.Auth.Services.Interfaces.User;
using WebTech.Auth.Controllers.Base;
using WebTech.Auth.Models.Dtos;
using Swashbuckle.AspNetCore.Annotations;
using WebTech.Auth.Models.ViewModels;
using WebTech.Auth.Controllers.Responses;

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
    [SwaggerOperation(
    Summary = "Get a list of users with filtering, sorting, and pagination",
    Description = "Retrieve a list of users with optional filtering and sorting.\n\n" +
                  "### Filtering:\n" +
                  "To filter the results, use the `Filter` query parameter with conditions separated by '~and~' or '~or~'. " +
                  "Each condition should be in the format `PropertyName~Operator~Value`, where:\n\n" +
                  "- `PropertyName` is the name of the property to filter on.\n" +
                  "- `Operator` is one of the supported comparison operators (`eq`, `ne`, `lt`, `gt`, `le`, `ge`, `sw`, `ew`, `con`).\n" +
                  "- `Value` is the value to compare against.\n\n" +
                  "Example: `Filter=Name~eq~Kirill~and~Email~ew~mail.ru`\n\n" +
                  "### Sorting:\n" +
                  "To sort the results, use the `Sorting` query parameter with sort instructions separated by '~'. " +
                  "Each instruction should specify the property name followed by '~Desc' for descending order, if needed. " +
                  "Multiple sort instructions can be provided, separated by '~'.\n\n" +
                  "Example: `Sorting=Asc~Age~Name`\n\n" +
                  "Example: `Sorting=Desc~Email`\n\n" +
                  "### Pagination:\n" +
                  "To paginate the results, use the `SkipCount` and `MaxResultCount` query parameters. " +
                  "Use `SkipCount` to skip a certain number of records and `MaxResultCount` to limit the number of records per page.\n\n" +
                  "Example: `SkipCount=0&MaxResultCount=10` to retrieve the first 10 records."
    )]
    [SwaggerResponse(200, "List of users", typeof(IEnumerable<UserViewModel>))]
    [SwaggerResponse(404, "Users not found")]
    public async Task<IActionResult> GetUsers([FromQuery] GetUsersDto usersDto)
    {
        var users = await _userService.GetUsersAsync(usersDto);

        if (users == null)
        {
            return NotFound("No users found.");
        }

        return Ok(users);
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Get user by id")]
    [SwaggerResponse(200, "User details retrieved successfully.", typeof(UserViewModel))]
    [SwaggerResponse(404, "User not found.")]
    public async Task<IActionResult> GetById([FromQuery] string userId)
    {
        var user = await _userService.GetUserByIdAsync(userId);

        if (user == null)
        {
            return NotFound("User not found.");
        }

        return Ok(user);
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Create user")]
    [SwaggerResponse(200, "User created successfully.", typeof(AuthServiceDto))]
    [SwaggerResponse(400, "Bad request with error message.")]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserInput userInput)
    {
        var result = await _userService.CreateUserAsync(userInput);

        if (!result.Success)
        {
            return BadRequest(new ErrorResponse { Message = result.ErrorMessage });
        }

        return Ok(result);
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Get all roles")]
    [SwaggerResponse(200, "List of all roles retrieved successfully.", typeof(IEnumerable<RoleViewModel>))]
    public async Task<IActionResult> GetAllRoles()
    {
        return Ok(await _userService.GetAllRolesAsync());
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Add role to user")]
    [SwaggerResponse(200, "Role added successfully.")]
    [SwaggerResponse(400, "Bad request with error message.")]
    public async Task<IActionResult> AddRoleToUser([FromQuery] AddRoleToUserDto dto)
    {
        var result = await _userService.AddRoleToUserAsync(dto);

        if (result.Succeeded)
        {
            return Ok();
        }

        return BadRequest(new ErrorResponse { Message = "Failed to add role to user.", Errors = result.Errors.Select(x => x.Code) });
    }

    [HttpPut]
    [SwaggerOperation(Summary = "Change user info")]
    [SwaggerResponse(200, "User info changed successfully.")]
    [SwaggerResponse(400, "Bad request with error message.")]
    public async Task<IActionResult> ChangeUserInfo([FromBody] ChangeUserInfoInput userInfoInput)
    {
        if (!ModelState.IsValid)
        {
            IEnumerable<string> errorMessages = ModelState.Values.SelectMany(v => v.Errors.Select(x => x.ErrorMessage));
            return BadRequest(new ErrorResponse { Message = "Invalid input data.", Errors = errorMessages });
        }

        var updatingResult = await _userService.ChangeUserInfoAsync(userInfoInput);
        if (updatingResult.Succeeded)
        {
            return Ok();
        }

        return BadRequest(new ErrorResponse { Message = "Failed to change user information.", Errors = updatingResult.Errors.Select(x => x.Code) });
    }

    [HttpDelete]
    [SwaggerOperation(Summary = "Remove user")]
    [SwaggerResponse(204, "User removed successfully.")]
    [SwaggerResponse(400, "Bad request with error message.")]
    public async Task<IActionResult> RemoveUser([FromQuery] string userId)
    {
        var result = await _userService.RemoveUserAsync(userId);

        if (result.Succeeded)
        {
            return NoContent();
        }

        return BadRequest(new ErrorResponse { Message = "Failed to remove user.", Errors = result.Errors.Select(x => x.Code) });
    }
}