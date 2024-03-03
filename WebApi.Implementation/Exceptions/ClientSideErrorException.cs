using WebApi.Application.Localization;

namespace WebApi.Implementation.Exceptions
{
    public class ClientSideErrorException : Exception
    {
        public ClientSideErrorException() : base()
        {
        }

        public ClientSideErrorException(ITranslator translator, string messageKey) : base(translator.Translate(messageKey))
        {
        }
    }
}
