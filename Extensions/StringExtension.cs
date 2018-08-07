using System.Security.Cryptography;
using System.Text;

namespace HappyMeal_v3.Extensions
{
    public static class StringExtension
    {
        public static string ToSha1(this string str)
        {
            using (var sha1 = new SHA1Managed())
            {
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(str));
                var sb = new StringBuilder(hash.Length * 2);
                foreach (var b in hash)
                    sb.Append(b.ToString("X2"));
                return sb.ToString();
            }
        }
    }
}
