using FitLog.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitLog.Common.Attributes
{
    public class InheritUseCasesAttribute : Attribute
    {
        private readonly UserRole[] _roles;

        public InheritUseCasesAttribute(params UserRole[] roles)
        {
            _roles = roles;
        }

        public UserRole[] Roles => _roles;
    }
}
