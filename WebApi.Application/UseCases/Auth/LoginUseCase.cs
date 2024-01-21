using WebApi.Common.DTO;

namespace WebApi.Application.UseCases.Auth
{
    public class LoginUseCase : UseCase<LoginData, Tokens>
    {
        public LoginUseCase(LoginData data) : base(data)
        {
        }

        public override string Id => "Login";
    }
}
