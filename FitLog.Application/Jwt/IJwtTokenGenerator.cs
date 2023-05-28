﻿using FitLog.Common.DTO;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FitLog.Application.Jwt
{
    public interface IJwtTokenGenerator
    {
        TokenData GenerateAccessToken(UserDto user);
        TokenData GenerateRefreshToken();
    }
}
