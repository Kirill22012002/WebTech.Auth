using AutoMapper;
using Microsoft.AspNetCore.Identity;
using WebTech.Auth.Data.Models;
using WebTech.Auth.ErrorHandler.CustomExceptions;
using WebTech.Auth.Helpers;
using WebTech.Auth.Models.Dtos;
using WebTech.Auth.Models.Inputs;
using WebTech.Auth.Models.ViewModels;
using WebTech.Auth.Services.Interfaces.User;

namespace WebTech.Auth.Services.Implementations.User;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    private readonly IMapper _mapper;

    public UserService(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IMapper mapper)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _mapper = mapper;
    }

    public async Task<IEnumerable<UserViewModel>> GetUsersAsync(GetUsersDto usersDto)
    {
        var usersQueryable = await GetUsersAsQueryable();

        if (!string.IsNullOrEmpty(usersDto.Filter))
        {
            var filters = FilterHelper.GetFilters(usersDto.Filter);

            foreach (var filter in filters)
            {
                var filteredExpression = FilterHelper.GetFilterExpression<ApplicationUser>(filter);
                usersQueryable = usersQueryable.Where(filteredExpression);
            }
        }

        if (!string.IsNullOrEmpty(usersDto.Sorting))
        {
            usersQueryable = SortHelper.GetSorted(usersQueryable, usersDto.Sorting);
        }

        IEnumerable<ApplicationUser> users = usersQueryable
            .Skip(usersDto.SkipCount)
            .Take(usersDto.MaxResultCount)
            .ToList();

        return _mapper.Map<IEnumerable<UserViewModel>>(users);
    }

    public async Task<UserViewModel> GetUserByIdAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        return _mapper.Map<UserViewModel>(user);
    }

    public async Task<AuthServiceDto> CreateUserAsync(CreateUserInput userInput)
    {
        var user = _mapper.Map<ApplicationUser>(userInput);
        var registrationResult = await _userManager.CreateAsync(user, userInput.Password);

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

    public async Task<IdentityResult> ChangeUserInfoAsync(ChangeUserInfoInput userInfoInput)
    {
        var user = await _userManager.FindByIdAsync(userInfoInput.UserId);

        if (user == null)
        {
            throw new ClientException("user_not_found", 400);
        }

        if (!string.IsNullOrWhiteSpace(userInfoInput.Name) && userInfoInput.Name?.Trim() != user.Name)
        {
            user.Name = userInfoInput.Name;
        }
        if (userInfoInput.Email?.Trim() != user.Email)
        {
            user.Email = userInfoInput.Email;
        }
        if (userInfoInput.Age != user.Age)
        {
            user.Age = userInfoInput.Age;
        }

        return await _userManager.UpdateAsync(user);
    }

    private async Task<IQueryable<ApplicationUser>> GetUsersAsQueryable()
    {
        return await Task.Run(() =>
        {
            return _userManager.Users.AsQueryable();
        });
    }
}