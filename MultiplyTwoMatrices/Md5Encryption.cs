using System.Security.Cryptography;
using System.Text;

namespace some_csharp
{
    public static class Md5Encryption {
        public static string MD5Hash(string str) {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(str));

            for (int i = 0; i < bytes.Length; i++) {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }
    }
 }


