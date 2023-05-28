using FitLog.Application.Localization;
using FitLog.Application.UseCases.Auth;
using FitLog.Implementation.Extensions;
using FitLog.Implementation.Validators.Abstraction;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitLog.Implementation.Validators.Auth
{
    public class LoginValidator : TranslatableFormattedValidator<LoginUseCase>
    {
        public LoginValidator(ITranslator translator) : base(translator)
        {
            RuleFor(x => x.Data.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage(IsRequired())
                .Must(email => email.IsValidEmailAddress())
                .WithMessage(Translate("invalidEmailAddressFormat"));

            RuleFor(x => x.Data.Password)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage(IsRequired())
                .MinimumLength(8)
                .WithMessage(MinimumLength(8));
        }
    }
}
