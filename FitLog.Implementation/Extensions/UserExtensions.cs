using FitLog.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitLog.Implementation.Extensions
{
    public static class UserExtensions
    {
        public static bool IsPasswordCorrect(this User user, string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, user.Password);
        }
    }
}
