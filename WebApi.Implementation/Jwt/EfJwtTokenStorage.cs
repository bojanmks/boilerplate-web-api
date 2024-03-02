using AutoMapper;
using WebApi.Application.AppSettings;
using WebApi.Application.Jwt;
using WebApi.Common.DTO;
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

        public JwtTokenRecordDto FindByRefreshToken(string token)
        {
            var tokenHash = _tokenCryptor.GetSha256Hash(token);

            var jwtTokenRecord = _entityAccessor.Find<JwtTokenRecord>(x => x.RefreshToken == tokenHash);

            if (jwtTokenRecord is null)
            {
                return null;
            }

            return _mapper.Map<JwtTokenRecordDto>(jwtTokenRecord);
        }

        public Tokens CreateRecord(User user)
        {
            var accessToken = _jwtTokenGenerator.GenerateAccessToken(_mapper.Map<UserDto>(user));
            var refreshToken = _jwtTokenGenerator.GenerateRefreshToken();

            var accessTokenHash = _tokenCryptor.GetSha256Hash(accessToken.Token);
            var refreshTokenHash = _tokenCryptor.GetSha256Hash(refreshToken.Token);

            _entityAccessor.Add(new JwtTokenRecord
            {
                AccessToken = accessTokenHash,
                RefreshToken = refreshTokenHash,
                AccessTokenExpiry = accessToken.Expiry,
                RefreshTokenExpiry = refreshToken.Expiry,
                UserId = user.Id
            });

            _entityAccessor.SaveChanges();

            return new Tokens
            {
                AccessToken = accessToken.Token,
                RefreshToken = refreshToken.Token
            };
        }

        public void Delete(int id)
        {
            _entityAccessor.Delete<JwtTokenRecord>(id);
            _entityAccessor.SaveChanges();
        }

        public void DeleteExcessTokens(int userId)
        {
            int maxAllowedTokens = _appSettings.JwtSettings.MaxActiveRefreshTokens;

            var usersTokenRecords = _entityAccessor
                .FindAll<JwtTokenRecord>(x => x.UserId == userId)
                .OrderBy(x => x.Id);

            int tokensCount = usersTokenRecords.Count();

            if (tokensCount <= maxAllowedTokens)
            {
                return;
            }

            int numberOfTokensToRemove = tokensCount - maxAllowedTokens;

            var tokenRecordsToRemove = usersTokenRecords.Take(numberOfTokensToRemove);

            _entityAccessor.DeleteBatch(tokenRecordsToRemove);
            _entityAccessor.SaveChanges();
        }
    }
}
