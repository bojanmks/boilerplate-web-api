using WebApi.Common.Enums;

namespace WebApi.Common.Attributes
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
