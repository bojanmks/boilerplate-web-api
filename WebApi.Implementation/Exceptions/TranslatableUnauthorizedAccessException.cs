using WebApi.Application.Exceptions.Abstraction;
using WebApi.Application.Localization;

namespace WebApi.Implementation.Exceptions
{
    public class TranslatableUnauthorizedAccessException : TranslatableException
    {
        public TranslatableUnauthorizedAccessException(ITranslator translator, string messageKey) : base(translator, messageKey)
        {
        }
    }
}
