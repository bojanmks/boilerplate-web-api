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
        private readonly IJwtTokenStorage _jwtTokenStorage;
        private readonly ITranslator _translator;

        public LoginUseCaseHandler(
            EntityAccessor accessor,
            IJwtTokenStorage jwtTokenStorage,
            ITranslator translator
        ) : base(accessor)
        {
            _jwtTokenStorage = jwtTokenStorage;
            _translator = translator;
        }

        public override async Task<Result<Tokens>> HandleAsync(LoginUseCase useCase, CancellationToken cancellationToken = default)
        {
            var user = await _accessor.FindAsync<User>(x => x.Email == useCase.Data.Email);

            if (user is null || !user.IsPasswordCorrect(useCase.Data.Password))
            {
                return Result<Tokens>.ValidationError(new[] { _translator.Translate("invalidCredentials") });
            }

            var tokens = await _jwtTokenStorage.CreateRecordAsync(user);

            await _jwtTokenStorage.DeleteExcessTokensAsync(user.Id);

            return Result<Tokens>.Success(tokens);
        }
    }
}
