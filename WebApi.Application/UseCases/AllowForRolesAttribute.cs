using WebApi.Common.Enums.Auth;

namespace WebApi.Application.UseCases
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
