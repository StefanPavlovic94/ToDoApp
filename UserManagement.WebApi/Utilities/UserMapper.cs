using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UserManagement.Core.Model;
using UserManagement.WebApi.Models;

namespace UserManagement.WebApi.Utilities
{
    public class UserMapperProfile : Profile
    {
        public UserMapperProfile()
        {
            CreateMap<User, UserViewModel>().ReverseMap();
            CreateMap<User, CreateUserViewModel>().ReverseMap();
        }
    }
}