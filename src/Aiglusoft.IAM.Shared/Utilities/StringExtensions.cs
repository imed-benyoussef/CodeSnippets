using System.Text.RegularExpressions;

namespace Aiglusoft.IAM.Shared.Utilities
{
    public static class StringExtensions
    {
        public static string ToSnakeCase(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return str;

            var startUnderscores = Regex.Match(str, @"^_+");
            return startUnderscores + Regex.Replace(str, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
        }
    }
}
