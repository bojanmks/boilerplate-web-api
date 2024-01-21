﻿using AutoMapper;
using WebApi.Application.Jwt;
using WebApi.Common.DTO;
using WebApi.DataAccess;
using WebApi.DataAccess.Entities;

namespace WebApi.Implementation.Jwt
{
    public class JwtTokenStorage : IJwtTokenStorage
    {
        private readonly DatabaseContext _context;
        private readonly TokenCryptor _tokenCryptor;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IMapper _mapper;

        public JwtTokenStorage(DatabaseContext context, TokenCryptor tokenCryptor, IJwtTokenGenerator jwtTokenGenerator, IMapper mapper)
        {
            _context = context;
            _tokenCryptor = tokenCryptor;
            _jwtTokenGenerator = jwtTokenGenerator;
            _mapper = mapper;
        }

        public JwtTokenRecord FindByRefreshToken(string token)
        {
            var tokenHash = _tokenCryptor.GetSha256Hash(token);
            return _context.JwtTokenRecords.FirstOrDefault(x => x.RefreshToken == tokenHash);
        }

        public Tokens CreateRecord(User user)
        {
            var accessToken = _jwtTokenGenerator.GenerateAccessToken(_mapper.Map<UserDto>(user));
            var refreshToken = _jwtTokenGenerator.GenerateRefreshToken();

            var accessTokenHash = _tokenCryptor.GetSha256Hash(accessToken.Token);
            var refreshTokenHash = _tokenCryptor.GetSha256Hash(refreshToken.Token);

            _context.JwtTokenRecords.Add(new JwtTokenRecord
            {
                AccessToken = accessTokenHash,
                RefreshToken = refreshTokenHash,
                AccessTokenExpiry = accessToken.Expiry,
                RefreshTokenExpiry = refreshToken.Expiry,
                UserId = user.Id
            });

            _context.SaveChanges();

            return new Tokens
            {
                AccessToken = accessToken.Token,
                RefreshToken = refreshToken.Token
            };
        }
    }
}