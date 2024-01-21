using WebApi.Common.DTO;

namespace WebApi.Application.UseCases.Auth
{
    public class RefreshTokenUseCase : UseCase<Tokens, Tokens>
    {
        public RefreshTokenUseCase(Tokens data) : base(data)
        {
        }

        public override string Id => "RefreshToken";
    }
}
