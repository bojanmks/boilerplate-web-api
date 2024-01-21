using AutoMapper;
using WebApi.Common.DTO;
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
