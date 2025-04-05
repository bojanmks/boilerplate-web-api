using WebApi.Application.Jwt;
using WebApi.Application.Localization;
using WebApi.Application.UseCases.Auth;
using WebApi.Common.DTO.Auth;
using WebApi.Common.DTO.Result;
using WebApi.DataAccess.Entities;
using WebApi.Implementation.Core;
using WebApi.Implementation.Extensions;
using WebApi.Implementation.UseCaseHandlers.Abstraction;

namespace WebApi.Implementation.UseCaseHandlers.Auth
{
    public class LoginUseCaseHandler : EfUseCaseHandler<LoginUseCase, LoginData, Tokens>
    {
        private readonly ITranslator _translator;
        private readonly IJwtTokenStorage _jwtTokenStorage;

        public LoginUseCaseHandler(EntityAccessor accessor, ITranslator translator, IJwtTokenStorage jwtTokenStorage) : base(accessor)
        {
            _translator = translator;
            _jwtTokenStorage = jwtTokenStorage;
        }

        public override async Task<Result<Tokens>> HandleAsync(LoginUseCase useCase, CancellationToken cancellationToken = default)
        {
            var user = await _accessor.FindAsync<User>(x => x.Email == useCase.Data.Email);

            if (user is null || !user.IsPasswordCorrect(useCase.Data.Password))
            {
                return Result<Tokens>.Error(Enumerable.Empty<string>());
            }

            var tokens = await _jwtTokenStorage.CreateRecordAsync(user);

            await _jwtTokenStorage.DeleteExcessTokensAsync(user.Id);

            return Result<Tokens>.Success(tokens);
        }
    }
}
