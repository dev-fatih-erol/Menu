using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Menu.Api.Extensions
{
    public static class StringExtension
    {
        public static string FirstCharToUpper(this string source)
        {
            return source switch
            {
                null => throw new ArgumentNullException(nameof(source)),
                "" => throw new ArgumentException(nameof(source)),
                _ => source.First().ToString().ToUpper() + source.Substring(1),
            };
        }

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