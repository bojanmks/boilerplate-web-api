using WebApi.Application.ApplicationUsers;
using WebApi.Common.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Implementation.ApplicationUsers
{
    public class ApplicationUser : IApplicationUser
    {
        public virtual int? Id { get; set; }

        public virtual string Email { get; set; }

        public virtual UserRole Role { get; set; }

        public CultureInfo Locale { get; set; }
        public List<string> AllowedUseCases { get; set; } = new List<string>();
    }
}
