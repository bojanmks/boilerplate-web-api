namespace WebApi.Application.Exceptions
{
    public class SortPropertyAlreadyExistsException : Exception
    {
        public SortPropertyAlreadyExistsException(string key) : base($"Sort property with the key {key} already exists.")
        {

        }
    }
}
