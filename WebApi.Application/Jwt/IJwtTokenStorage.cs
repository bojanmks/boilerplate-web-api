using WebApi.Common.DTO;
using WebApi.DataAccess.Entities;

namespace WebApi.Application.Jwt
{
    public interface IJwtTokenStorage
    {
        JwtTokenRecordDto FindByRefreshToken(string token);
        Tokens CreateRecord(User user);
        void Delete(int id);
        void DeleteExcessTokens(int userId);
    }
}
