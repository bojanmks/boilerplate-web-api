using FitLog.Common.DTO.Abstraction;
using FitLog.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitLog.Common.DTO
{
    public class UserDto : IdentifyableDto
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public UserRole Role { get; set; }
    }
}
