using AutoMapper;
using Microsoft.AspNetCore.Identity;
using WebTech.Auth.Data.Models;
using WebTech.Auth.Helpers;
using WebTech.Auth.Models.ViewModels;
using WebTech.Auth.Services.Interfaces.User;

namespace WebTech.Auth.Services.Implementations.User;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;

    private readonly IMapper _mapper;

    public UserService(
        UserManager<ApplicationUser> userManager,
        IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<IEnumerable<UserViewModel>> GetUsersAsync(string conditions)
    {
        var usersQueryable = await GetUsersAsQueryable();

        if (!string.IsNullOrEmpty(conditions))
        {
            var filters = FilterHelper.GetFilters(conditions);

            foreach (var filter in filters)
            {
                var filteredExpression = FilterHelper.GetFilterExpression<ApplicationUser>(filter);
                usersQueryable = usersQueryable.Where(filteredExpression);
            }
        }

        IEnumerable<ApplicationUser> users = usersQueryable.ToList();

        return _mapper.Map<IEnumerable<UserViewModel>>(users);
    }

    private async Task<IQueryable<ApplicationUser>> GetUsersAsQueryable()
    {
        return await Task.Run(() =>
        {
            return _userManager.Users.AsQueryable();
        });
    }
}