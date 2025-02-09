using Microsoft.AspNetCore.WebUtilities;
using System.Security.Cryptography;
using System.Text;

namespace Blog.Application.Helpers
{
    static public class Encoders
    {
        public static string UrlEncode(this string value)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(value);
            return WebEncoders.Base64UrlEncode(bytes);
        }

        public static string UrlDecode(this string value)
        {
            byte[] bytes = WebEncoders.Base64UrlDecode(value);
            return Encoding.UTF8.GetString(bytes);
        }

        public static string EncryptPassword(string password)
        {
            using var md5 = MD5.Create();

            byte[] inputBytes = Encoding.ASCII.GetBytes(password);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            return Convert.ToHexString(hashBytes);
        }

    }
}
