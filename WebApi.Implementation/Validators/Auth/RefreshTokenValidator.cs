using FluentValidation;
using WebApi.Application.Jwt;
using WebApi.Application.Localization;
using WebApi.Application.UseCases.Auth;
using WebApi.Implementation.Exceptions;

namespace WebApi.Implementation.Validators.Auth
{
    public class RefreshTokenValidator : TranslatableFormattedValidator<RefreshTokenUseCase>
    {
        public RefreshTokenValidator(ITranslator translator, IJwtTokenValidator jwtValidator) : base(translator)
        {
            RuleFor(x => x.Data.RefreshToken)
                .Must(refreshToken => jwtValidator.IsValid(refreshToken))
                .WithState(x => new ClientSideErrorException());
        }
    }
}
