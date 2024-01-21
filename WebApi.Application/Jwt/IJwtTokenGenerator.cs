using WebApi.Common.DTO;

namespace WebApi.Application.Jwt
{
    public interface IJwtTokenGenerator
    {
        TokenData GenerateAccessToken(UserDto user);
        TokenData GenerateRefreshToken();
    }
}
