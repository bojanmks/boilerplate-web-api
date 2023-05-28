using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitLog.Implementation.Jwt
{
    public static class FitLogClaims
    {
        public static string UserId => "UserId";
        public static string Email => "Email";
        public static string FirstName => "FirstName";
        public static string LastName => "LastName";
        public static string Role => "Role";
    }
}
