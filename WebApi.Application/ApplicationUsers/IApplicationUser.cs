using WebApi.Common.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Application.ApplicationUsers
{
    public interface IApplicationUser
    {
        public int? Id { get; set; }
        public string Email { get; set; }
        public UserRole Role { get; set; }
        public CultureInfo Locale { get; set; }
        public List<string> AllowedUseCases { get; set; }
    }
}
