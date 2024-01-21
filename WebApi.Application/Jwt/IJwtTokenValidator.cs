namespace WebApi.Application.Jwt
{
    public interface IJwtTokenValidator
    {
        bool IsValid(string token);
    }
}
