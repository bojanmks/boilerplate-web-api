using WebApi.Application.Exceptions.Abstraction;
using WebApi.Application.Localization;

namespace WebApi.Application.Exceptions
{
    public class PropertyNotFoundException : TranslatableFormattedException
    {
        public PropertyNotFoundException(ITranslator translator, string propName) : base(translator, "propertyNotFound", propName)
        {
        }
    }
}
