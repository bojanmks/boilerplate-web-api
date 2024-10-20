using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.AppSettings;
using WebApi.Application.Jwt;
using WebApi.Common.DTO.Auth;
using WebApi.Common.DTO.Users;
using WebApi.DataAccess.Entities;
using WebApi.Implementation.Core;

namespace WebApi.Implementation.Jwt
{
    public class EfJwtTokenStorage : IJwtTokenStorage
    {
        private readonly EntityAccessor _entityAccessor;
        private readonly TokenCryptor _tokenCryptor;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;

        public EfJwtTokenStorage(
            EntityAccessor entityAccessor,
            TokenCryptor tokenCryptor,
            IJwtTokenGenerator jwtTokenGenerator,
            IMapper mapper,
            AppSettings appSettings)
        {
            _entityAccessor = entityAccessor;
            _tokenCryptor = tokenCryptor;
            _jwtTokenGenerator = jwtTokenGenerator;
            _mapper = mapper;
            _appSettings = appSettings;
        }

        public async Task<JwtTokenRecordDto> FindByRefreshTokenAsync(string token, CancellationToken cancellationToken = default)
        {
            var tokenHash = _tokenCryptor.GetSha256Hash(token);

            var jwtTokenRecord = await _entityAccessor.FindAsync<JwtTokenRecord>(x => x.RefreshToken == tokenHash, cancellationToken: cancellationToken);

            if (jwtTokenRecord is null)
            {
                return null;
            }

            return _mapper.Map<JwtTokenRecordDto>(jwtTokenRecord);
        }

        public async Task<Tokens> CreateRecordAsync(User user, CancellationToken cancellationToken = default)
        {
            var accessToken = _jwtTokenGenerator.GenerateAccessToken(_mapper.Map<UserDto>(user));
            var refreshToken = _jwtTokenGenerator.GenerateRefreshToken();

            var accessTokenHash = _tokenCryptor.GetSha256Hash(accessToken.Token);
            var refreshTokenHash = _tokenCryptor.GetSha256Hash(refreshToken.Token);

            await _entityAccessor.AddAsync(new JwtTokenRecord
            {
                AccessToken = accessTokenHash,
                RefreshToken = refreshTokenHash,
                AccessTokenExpiry = accessToken.Expiry,
                RefreshTokenExpiry = refreshToken.Expiry,
                UserId = user.Id
            }, cancellationToken);

            await _entityAccessor.SaveChangesAsync(cancellationToken);

            return new Tokens
            {
                AccessToken = accessToken.Token,
                RefreshToken = refreshToken.Token
            };
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            await _entityAccessor.DeleteByIdAsync<JwtTokenRecord>(id, cancellationToken: cancellationToken);
            await _entityAccessor.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteExcessTokensAsync(int userId, CancellationToken cancellationToken = default)
        {
            int maxAllowedTokens = _appSettings.JwtSettings.MaxActiveRefreshTokens;

            var usersTokenRecords = await _entityAccessor
                .FindAll<JwtTokenRecord>(x => x.UserId == userId)
                .OrderBy(x => x.Id)
                .ToListAsync(cancellationToken);

            int tokensCount = usersTokenRecords.Count();

            if (tokensCount <= maxAllowedTokens)
            {
                return;
            }

            int numberOfTokensToRemove = tokensCount - maxAllowedTokens;

            var tokenRecordsToRemove = usersTokenRecords.Take(numberOfTokensToRemove);

            _entityAccessor.DeleteBatch(tokenRecordsToRemove);
            await _entityAccessor.SaveChangesAsync(cancellationToken);
        }
    }
}
