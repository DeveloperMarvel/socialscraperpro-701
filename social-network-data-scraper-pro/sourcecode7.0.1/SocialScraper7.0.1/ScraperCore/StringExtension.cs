using System.Text;
using System.Text.RegularExpressions;

namespace ScraperCore
{
    public static class StringExtension
    {
        public static string ExtractEmail(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }
            var builder = new StringBuilder();
            const string MatchEmailPattern =
           @"(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
           + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
             + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
           + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})";
            var rx = new Regex(MatchEmailPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var matches = rx.Matches(input);
            foreach (Match match in matches)
            {
                builder.Append(match.Value).Append(";");
            }
            return builder.ToString().TrimEnd(';');
        }


        public static string ExtractTel(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }
            input = input.Replace(" ", string.Empty).Replace("(", string.Empty).Replace(")", string.Empty);
            var builder = new StringBuilder();
            var regex = new Regex(@"\+\d{6,16}");
            var matches = regex.Matches(input);
            foreach (Match item in matches)
            {
                builder.Append(item.Value).Append(";");
            }
            return builder.ToString().TrimEnd(';');
        }
    }
}
