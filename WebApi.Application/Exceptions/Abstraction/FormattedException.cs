namespace WebApi.Application.Exceptions.Abstraction
{
    public abstract class FormattedException : Exception
    {
        public FormattedException(string message, params object[] values) : base(String.Format(message, values))
        {
        }
    }
}
