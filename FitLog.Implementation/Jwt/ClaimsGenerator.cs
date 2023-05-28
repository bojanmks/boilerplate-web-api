﻿using FitLog.Application.AppSettings;
using FitLog.Common.DTO;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FitLog.Implementation.Jwt
{
    public class ClaimsGenerator
    {
        private readonly TokenCryptor _tokenCryptor;
        private readonly JwtSettings _jwtSettings;

        public ClaimsGenerator(TokenCryptor tokenCryptor, JwtSettings jwtSettings)
        {
            _tokenCryptor = tokenCryptor;
            _jwtSettings = jwtSettings;
        }

        public List<Claim> GenerateAccessTokenClaims(UserDto user)
        {
            var claims = GenerateBaseClaims();

            claims.Add(new Claim(FitLogClaims.UserId, user.Id.ToString(), ClaimValueTypes.String, _jwtSettings.Issuer));
            claims.Add(new Claim(FitLogClaims.Email, user.Email, ClaimValueTypes.String, _jwtSettings.Issuer));
            claims.Add(new Claim(FitLogClaims.FirstName, user.FirstName.ToString(), ClaimValueTypes.String, _jwtSettings.Issuer));
            claims.Add(new Claim(FitLogClaims.LastName, user.LastName, ClaimValueTypes.String, _jwtSettings.Issuer));
            claims.Add(new Claim(FitLogClaims.Role, user.Role.ToString(), ClaimValueTypes.String, _jwtSettings.Issuer));

            return claims;
        }

        public List<Claim> GenerateRefreshTokenClaims()
        {
            return GenerateBaseClaims();
        }

        private List<Claim> GenerateBaseClaims()
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, _tokenCryptor.CreateCryptographicallySecureGuid().ToString(), ClaimValueTypes.String, _jwtSettings.Issuer),
                new Claim(JwtRegisteredClaimNames.Iss, _jwtSettings.Issuer, ClaimValueTypes.String, _jwtSettings.Issuer),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64, _jwtSettings.Issuer)
            };

            return claims;
        }
    }
}
