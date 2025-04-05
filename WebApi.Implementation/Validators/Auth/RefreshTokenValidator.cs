using FluentValidation;
using WebApi.Application.Jwt;
using WebApi.Application.Localization;
using WebApi.Application.UseCases.Auth;

namespace WebApi.Implementation.Validators.Auth
{
    public class RefreshTokenValidator : TranslatableFormattedValidator<RefreshTokenUseCase>
    {
        public RefreshTokenValidator(ITranslator translator, IJwtTokenValidator jwtValidator) : base(translator)
        {
            RuleFor(x => x.Data.RefreshToken)
                .Must(jwtValidator.IsValid);
        }
    }
}
