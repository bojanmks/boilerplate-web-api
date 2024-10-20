using AutoMapper;
using WebApi.Common.DTO.Users;
using WebApi.DataAccess.Entities;

namespace WebApi.Implementation.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
        }
    }
}
