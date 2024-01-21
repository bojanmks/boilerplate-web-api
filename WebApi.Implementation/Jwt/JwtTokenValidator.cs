using System.IdentityModel.Tokens.Jwt;
using WebApi.Application.Jwt;

namespace WebApi.Implementation.Jwt
{
    public class JwtTokenValidator : IJwtTokenValidator
    {
        public bool IsValid(string token)
        {
            JwtSecurityToken jwtSecurityToken;

            try
            {
                jwtSecurityToken = new JwtSecurityToken(token);
            }
            catch (Exception)
            {
                return false;
            }

            return jwtSecurityToken.ValidTo >= DateTime.UtcNow;
        }
    }
}
