namespace FarmHeroes.Web.Extensions
{
    using System.Text.RegularExpressions;

    public static class StringExtensions
    {
        public static string ToFriendlyCase(this string pascalString)
        {
            return Regex.Replace(pascalString, "(?!^)([A-Z])", " $1");
        }
    }
}
