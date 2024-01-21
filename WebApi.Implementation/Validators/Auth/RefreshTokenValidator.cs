using FluentValidation;
using WebApi.Application.Jwt;
using WebApi.Application.Localization;
using WebApi.Application.UseCases.Auth;
using WebApi.Implementation.Validators.Abstraction;

namespace WebApi.Implementation.Validators.Auth
{
    public class RefreshTokenValidator : TranslatableFormattedValidator<RefreshTokenUseCase>
    {
        public RefreshTokenValidator(ITranslator translator, IJwtTokenStorage jwtStorage, IJwtTokenValidator jwtValidator) : base(translator)
        {
            RuleFor(x => x.Data.RefreshToken)
                .Cascade(CascadeMode.Stop)
                .Must(refreshToken => jwtStorage.FindByRefreshToken(refreshToken) is not null)
                .WithState(x => new UnauthorizedAccessException())
                .Must(refreshToken => jwtValidator.IsValid(refreshToken))
                .WithState(x => new UnauthorizedAccessException());
        }
    }
}
