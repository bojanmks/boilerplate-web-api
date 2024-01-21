using WebApi.Common.Enums;
using WebApi.DataAccess.Entities.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
