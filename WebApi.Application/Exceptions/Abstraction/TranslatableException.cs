using WebApi.Application.Localization;

namespace WebApi.Application.Exceptions.Abstraction
{
    public abstract class TranslatableException : Exception
    {
        public TranslatableException(ITranslator translator, string messageKey) : base(translator.Translate(messageKey))
        {
        }
    }
}
