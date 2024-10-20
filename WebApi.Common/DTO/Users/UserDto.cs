using WebApi.Common.Enums;

namespace WebApi.Common.DTO.Users
{
    public class UserDto : IdentifyableDto
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public UserRole Role { get; set; }
    }
}
