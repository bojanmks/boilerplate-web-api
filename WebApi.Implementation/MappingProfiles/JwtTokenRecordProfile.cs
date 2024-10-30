using AutoMapper;
using WebApi.Common.DTO.Auth;
using WebApi.DataAccess.Entities;

namespace WebApi.Implementation.MappingProfiles
{
    public class JwtTokenRecordProfile : Profile
    {
        public JwtTokenRecordProfile()
        {
            CreateMap<JwtTokenRecord, JwtTokenRecordDto>();
        }
    }
}
