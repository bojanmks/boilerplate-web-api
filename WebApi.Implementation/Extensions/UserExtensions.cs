using WebApi.DataAccess.Entities;

namespace WebApi.Implementation.Extensions
{
    public static class UserExtensions
    {
        public static bool IsPasswordCorrect(this User user, string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, user.Password);
        }
    }
}
