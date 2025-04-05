using System.Globalization;
using WebApi.Application.ApplicationUsers;
using WebApi.Common.Enums.Auth;

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
