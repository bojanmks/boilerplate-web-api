using WebApi.Application.Jwt;
using WebApi.Application.UseCases.Auth;
using WebApi.Common.DTO;
using WebApi.DataAccess.Entities;
using WebApi.Implementation.Core;
using WebApi.Implementation.UseCaseHandlers.Abstraction;

namespace WebApi.Implementation.UseCaseHandlers.Auth
{
    public class RefreshTokenUseCaseHandler : EfUseCaseHandler<RefreshTokenUseCase, Tokens, Tokens>
    {
        private readonly IJwtTokenStorage _jwtTokenStorage;

        public RefreshTokenUseCaseHandler(EntityAccessor accessor, IJwtTokenStorage jwtTokenStorage) : base(accessor)
        {
            _jwtTokenStorage = jwtTokenStorage;
        }

        public override Tokens Handle(RefreshTokenUseCase useCase)
        {
            var tokenRecord = _jwtTokenStorage.FindByRefreshToken(useCase.Data.RefreshToken);

            if (tokenRecord is null)
            {
                throw new UnauthorizedAccessException();
            }

            var user = _accessor.Find<User>(tokenRecord.UserId);

            if (user is null)
            {
                throw new UnauthorizedAccessException();
            }

            var tokens = _jwtTokenStorage.CreateRecord(user);

            _jwtTokenStorage.Delete(tokenRecord.Id);
            _jwtTokenStorage.DeleteExcessTokens(user.Id);

            return tokens;
        }
    }
}
