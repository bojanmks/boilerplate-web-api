using WebApi.Application.Localization;

namespace WebApi.Application.Exceptions.Abstraction
{
    public abstract class TranslatableFormattedException : FormattedException
    {
        public TranslatableFormattedException(ITranslator translator, string messageKey, params object[] values) : base(translator.Translate(messageKey), values)
        {
        }
    }
}
