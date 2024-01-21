using WebApi.Application.Exceptions.Abstraction;
using WebApi.Application.Localization;

namespace WebApi.Application.Exceptions
{
    public class InvalidSortDirectionException : TranslatableException
    {
        public InvalidSortDirectionException(ITranslator translator) : base(translator, "invalidSortDirection")
        {
        }
    }
}
