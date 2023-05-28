using FitLog.Common.DTO;
using FitLog.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitLog.Application.Jwt
{
    public interface IJwtTokenStorage
    {
        public JwtTokenRecord FindByRefreshToken(string token);
        public Tokens CreateRecord(User user);
    }
}
