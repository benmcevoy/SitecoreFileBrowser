using System.Text;

namespace SitecoreFileBrowser.Browse
{
    public static class StringExtensions
    {

        public static string RelativePath(this string absolutePath, string rootPath)
        {
            return absolutePath.Substring(rootPath.Length);
        }

        public static string ToBase64(this string value)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(value);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string FromBase64(this string value)
        {
            var x = System.Convert.FromBase64String(value);
            return Encoding.UTF8.GetString(x);
        }
    }
}
