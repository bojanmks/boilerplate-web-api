using WebApi.Common.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Common.Enums
{
    public enum UserRole
    {
        Anonymous = 1,
        Regular = 2,
        [InheritUseCases(UserRole.Regular)]
        Admin = 3
    }
}
