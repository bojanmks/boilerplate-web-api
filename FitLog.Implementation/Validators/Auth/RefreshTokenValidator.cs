using FitLog.Application.Jwt;
using FitLog.Application.Localization;
using FitLog.Application.UseCases.Auth;
using FitLog.Implementation.Validators.Abstraction;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitLog.Implementation.Validators.Auth
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
