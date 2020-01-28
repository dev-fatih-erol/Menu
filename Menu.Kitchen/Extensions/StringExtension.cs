using System.Security.Cryptography;
using System.Text;

namespace Menu.Kitchen.Extensions
{
    public static class StringExtension
    {
        public static string ToMD5(this string source)
        {
            StringBuilder hash = new StringBuilder();

            using var md5provider = new MD5CryptoServiceProvider();

            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(source));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }

            return hash.ToString();
        }
    }
}