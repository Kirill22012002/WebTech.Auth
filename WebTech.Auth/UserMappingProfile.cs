﻿using AutoMapper;
using Microsoft.AspNetCore.Identity;
using WebTech.Auth.Data.Models;
using WebTech.Auth.Models.Inputs;
using WebTech.Auth.Models.ViewModels;

namespace WebTech.Auth;
public class UserMappingProfile : Profile
{
    public UserMappingProfile() 
    {
        // User
        CreateMap<ApplicationUser, UserViewModel>().ReverseMap();
        CreateMap<ApplicationUser, CreateUserInput>().ReverseMap();
        CreateMap<ApplicationUser, ChangeUserInfoInput>().ReverseMap();
        CreateMap<ApplicationUser, ChangeUserInfoInputByUser>().ReverseMap();

        // Role
        CreateMap<IdentityRole, RoleViewModel>().ReverseMap();
    }
}