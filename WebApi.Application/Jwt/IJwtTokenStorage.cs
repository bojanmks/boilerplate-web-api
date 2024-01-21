using WebApi.Common.DTO;
using WebApi.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Application.Jwt
{
    public interface IJwtTokenStorage
    {
        public JwtTokenRecord FindByRefreshToken(string token);
        public Tokens CreateRecord(User user);
    }
}
