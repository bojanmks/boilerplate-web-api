using System.Text.RegularExpressions;

namespace WebApi.Implementation.Extensions
{
    public static class StringExtensions
    {
        public static bool IsValidEmailAddress(this string value)
        {
            var emailRegex = new Regex(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*" + "@" + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$");

            return emailRegex.IsMatch(value);
        }
    }
}
