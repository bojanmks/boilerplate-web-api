using FitLog.Application.Localization;
using FitLog.Application.UseCases;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitLog.Implementation.Validators.Abstraction
{
    public abstract class TranslatableFormattedValidator<TUseCase> : AbstractValidator<TUseCase> where TUseCase : IUseCase
    {
        private readonly ITranslator _translator;

        public TranslatableFormattedValidator(ITranslator translator)
        {
            _translator = translator;
        }

        protected string Translate(string messageKey)
        {
            return _translator.Translate(messageKey);
        }

        protected string TranslateAndFormat(string messageKey, params object[] values)
        {
            var translatedMessage = Translate(messageKey);
            var formattedMessage = String.Format(translatedMessage, values);

            return formattedMessage;
        }

        protected string IsRequired()
        {
            return Translate("isRequired");
        }

        protected string MinimumLength(int value)
        {
            return TranslateAndFormat("minimumLength", value);
        }
    }
}
