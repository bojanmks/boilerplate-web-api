using WebApi.Common.Attributes;

namespace WebApi.Common.Enums
{
    public enum UserRole
    {
        Anonymous = 1,

        Regular = 2,

        [InheritUseCases(Regular)]
        Admin = 3
    }
}
