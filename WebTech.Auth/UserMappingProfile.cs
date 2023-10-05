using AutoMapper;
using WebTech.Auth.Data.Models;
using WebTech.Auth.Models.ViewModels;

namespace WebTech.Auth;
public class UserMappingProfile : Profile
{
    public UserMappingProfile() 
    {
        CreateMap<ApplicationUser, UserViewModel>().ReverseMap();
    }
}