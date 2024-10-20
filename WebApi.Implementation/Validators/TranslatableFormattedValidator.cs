using FluentValidation;
using WebApi.Application.Localization;

namespace WebApi.Implementation.Validators
{
    public abstract class TranslatableFormattedValidator<TUseCase> : AbstractValidator<TUseCase>
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
            var formattedMessage = string.Format(translatedMessage, values);

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
