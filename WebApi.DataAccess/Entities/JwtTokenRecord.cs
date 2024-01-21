using WebApi.DataAccess.Entities.Abstraction;

namespace WebApi.DataAccess.Entities
{
    public class JwtTokenRecord : Entity
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime AccessTokenExpiry { get; set; }
        public DateTime RefreshTokenExpiry { get; set; }
        public int UserId { get; set; }

        public virtual User User { get; set; }
    }
}
