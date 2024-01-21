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
    public class AnonymousUser : ApplicationUser
    {
        public override int? Id => null;

        public override string Email => null;

        public override UserRole Role => UserRole.Anonymous;
    }
}
