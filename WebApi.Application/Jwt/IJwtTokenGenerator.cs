using WebApi.Common.DTO.Auth;
using WebApi.Common.DTO.Users;

namespace WebApi.Application.Jwt
{
    public interface IJwtTokenGenerator
    {
        TokenData GenerateAccessToken(UserDto user);
        TokenData GenerateRefreshToken();
    }
}
