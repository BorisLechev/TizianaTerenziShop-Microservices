namespace TizianaTerenzi.Services
{
    using System.Text.RegularExpressions;

    public class UrlGenerator : IUrlGenerator
    {
        public string GenerateUrl(int id, string name)
        {
            return $"/product/{this.ToUrl(name)}/{id}";
        }

        public string ToUrl(string name)
        {
            // Replace spaces with dashes
            name = name.Replace(" ", "-").Replace("--", "-").Replace("--", "-");

            // Remove non-letter characters
            name = Regex.Replace(name, "[^a-zA-Z0-9_-]+", string.Empty, RegexOptions.Compiled);

            return name.Trim('-').ToLower();
        }
    }
}
