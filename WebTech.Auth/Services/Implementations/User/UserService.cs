using AutoMapper;
using Microsoft.AspNetCore.Identity;
using WebTech.Auth.Data.Models;
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

    public async Task<IEnumerable<UserViewModel>> GetUsersAsync()
    {
        var usersQueryable = await GetUsersAsQueryable();
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