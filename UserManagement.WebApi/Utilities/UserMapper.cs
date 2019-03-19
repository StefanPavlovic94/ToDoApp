using AutoMapper;
using UserManagement.Core.Model;
using UserManagement.WebApi.ViewModels;

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