using WebApi.Common.DTO.Auth;
using WebApi.Common.Enums.Auth;

namespace WebApi.Application.UseCases.Auth
{
    [AllowForRoles(UserRole.Anonymous)]
    public class RefreshTokenUseCase : UseCase<Tokens, Tokens>
    {
        public RefreshTokenUseCase(Tokens data) : base(data)
        {
        }

        public override string Id => "RefreshToken";
    }
}
