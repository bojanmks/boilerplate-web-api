using WebApi.Application.UseCases.Attributes;
using WebApi.Common.DTO;
using WebApi.Common.Enums;

namespace WebApi.Application.UseCases.Auth
{
    [AllowForRoles(UserRole.Anonymous)]
    public class RefreshTokenUseCase : UseCase<Tokens, Tokens>
    {
        public RefreshTokenUseCase() : base(null) { }

        public RefreshTokenUseCase(Tokens data) : base(data)
        {
        }

        public override string Id => "RefreshToken";
    }
}
