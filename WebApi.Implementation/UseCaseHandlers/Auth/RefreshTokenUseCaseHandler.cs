using WebApi.Application.Jwt;
using WebApi.Application.UseCases.Auth;
using WebApi.Common.DTO.Auth;
using WebApi.Common.DTO.Result;
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

        public override async Task<Result<Tokens>> HandleAsync(RefreshTokenUseCase useCase, CancellationToken cancellationToken = default)
        {
            var tokenRecord = await _jwtTokenStorage.FindByRefreshTokenAsync(useCase.Data.RefreshToken, cancellationToken);

            if (tokenRecord is null)
            {
                return Result<Tokens>.ValidationError(Enumerable.Empty<string>());
            }

            var user = await _accessor.FindByIdAsync<User>(tokenRecord.UserId);

            if (user is null)
            {
                return Result<Tokens>.ValidationError(Enumerable.Empty<string>());
            }

            var tokens = await _jwtTokenStorage.CreateRecordAsync(user, cancellationToken);

            await _jwtTokenStorage.DeleteAsync(tokenRecord.Id, cancellationToken);

            return Result<Tokens>.Success(tokens);
        }
    }
}
