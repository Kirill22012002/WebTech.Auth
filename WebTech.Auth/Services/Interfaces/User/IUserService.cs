using WebTech.Auth.Models.ViewModels;

namespace WebTech.Auth.Services.Interfaces.User;

public interface IUserService
{
    public Task<IEnumerable<UserViewModel>> GetUsersAsync();
}