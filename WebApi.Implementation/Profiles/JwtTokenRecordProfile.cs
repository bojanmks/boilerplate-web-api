using AutoMapper;
using WebApi.Common.DTO;
using WebApi.DataAccess.Entities;

namespace WebApi.Implementation.Profiles
{
    public class JwtTokenRecordProfile : Profile
    {
        public JwtTokenRecordProfile()
        {
            CreateMap<JwtTokenRecord, JwtTokenRecordDto>();
        }
    }
}
