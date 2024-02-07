using WebApi.Common.Enums;

namespace WebApi.Application.UseCases.Attributes
{
    public class AllowForRolesAttribute : Attribute
    {
        private readonly UserRole[] _roles;

        public AllowForRolesAttribute(params UserRole[] roles)
        {
            _roles = roles;
        }

        public UserRole[] Roles => _roles;
    }
}
