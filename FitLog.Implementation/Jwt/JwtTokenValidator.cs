using FitLog.Application.Jwt;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitLog.Implementation.Jwt
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
