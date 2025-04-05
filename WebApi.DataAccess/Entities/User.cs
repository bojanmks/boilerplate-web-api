using WebApi.Common.Enums.Auth;
using WebApi.DataAccess.Entities.Abstraction;

namespace WebApi.DataAccess.Entities
{
    public class User : SoftDeletableEntity
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public UserRole Role { get; set; }

        public virtual ICollection<JwtTokenRecord> JwtTokenRecords { get; set; } = new HashSet<JwtTokenRecord>();
    }
}
