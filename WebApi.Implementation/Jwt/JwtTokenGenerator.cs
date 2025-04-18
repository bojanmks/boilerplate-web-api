﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using WebApi.Application.AppSettings;
using WebApi.Application.Jwt;
using WebApi.Common.DTO.Auth;
using WebApi.Common.DTO.Users;

namespace WebApi.Implementation.Jwt
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly JwtSettings _jwtSettings;
        private readonly ClaimsGenerator _claimsGenerator;

        public JwtTokenGenerator(JwtSettings jwtSettings, ClaimsGenerator claimsGenerator)
        {
            _jwtSettings = jwtSettings;
            _claimsGenerator = claimsGenerator;
        }

        public TokenData GenerateAccessToken(UserDto user)
        {
            var claims = _claimsGenerator.GenerateAccessTokenClaims(user);

            var token = GenerateToken(new TokenGenerationData
            {
                Claims = claims,
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.TokenValidityInMinutes)
            });

            return token;
        }

        public TokenData GenerateRefreshToken()
        {
            var randomNumber = new byte[32];

            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);

            string refreshToken = Convert.ToBase64String(randomNumber);

            var token = new TokenData
            {
                Token = refreshToken,
                Expiry = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenValidityInDays)
            };

            return token;
        }

        private TokenData GenerateToken(TokenGenerationData generationData)
        {
            var credentials = GenerateCredentials();

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                expires: generationData.Expires,
                claims: generationData.Claims,
                signingCredentials: credentials
            );

            return new TokenData
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiry = generationData.Expires
            };
        }

        private SigningCredentials? GenerateCredentials()
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            return credentials;
        }

        private class TokenGenerationData
        {
            public List<Claim> Claims { get; set; }
            public DateTime Expires { get; set; }
        }
    }
}
