using WebApi.Common.DTO.Auth;
using WebApi.DataAccess.Entities;

namespace WebApi.Application.Jwt
{
    public interface IJwtTokenStorage
    {
        Task<JwtTokenRecordDto> FindByRefreshTokenAsync(string token, CancellationToken cancellationToken = default);
        Task<Tokens> CreateRecordAsync(User user, CancellationToken cancellationToken = default);
        Task DeleteAsync(int id, CancellationToken cancellationToken = default);
        Task DeleteExcessTokensAsync(int userId, CancellationToken cancellationToken = default);
    }
}
