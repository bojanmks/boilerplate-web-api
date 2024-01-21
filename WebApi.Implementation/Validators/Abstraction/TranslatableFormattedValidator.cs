using FluentValidation;
using WebApi.Application.Localization;
using WebApi.Application.UseCases;

namespace WebApi.Implementation.Validators.Abstraction
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
