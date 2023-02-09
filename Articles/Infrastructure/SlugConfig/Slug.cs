using System.Text.RegularExpressions;

namespace Articles.Infrastructure.SlugConfig
{
    public static class Slug
    {
        public static string GenerateSlug(this string phrase)
        {
            if (phrase is null)
            {
                return null;
            }

            string str = phrase.ToLowerInvariant();
            // Invalid Character
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            //convert multiple space into one space
            str = Regex.Replace(str, @"\s+", " ").Trim();
            // cut and trim
            str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();
            //hyphens
            str = Regex.Replace(str, @"\s", "-");
            return str;
        }
    }
}
