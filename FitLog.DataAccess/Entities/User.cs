using FitLog.Common.Enums;
using FitLog.DataAccess.Entities.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitLog.DataAccess.Entities
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
