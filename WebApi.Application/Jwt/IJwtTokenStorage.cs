using WebApi.Common.DTO;
using WebApi.DataAccess.Entities;

namespace WebApi.Application.Jwt
{
    public interface IJwtTokenStorage
    {
        public JwtTokenRecord FindByRefreshToken(string token);
        public Tokens CreateRecord(User user);
    }
}
