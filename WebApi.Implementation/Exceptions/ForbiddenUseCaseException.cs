namespace WebApi.Implementation.Exceptions
{
    public class ForbiddenUseCaseException : Exception
    {
        public ForbiddenUseCaseException(string useCaseId, string userId) : base($"User with an Id of {(string.IsNullOrEmpty(userId) ? "NULL" : userId)} does not have permission to execute the {useCaseId} use case.")
        {

        }
    }
}
