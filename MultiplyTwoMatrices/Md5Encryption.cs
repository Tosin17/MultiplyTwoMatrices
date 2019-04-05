﻿using System.Security.Cryptography;
using System.Text;

namespace MultiplyTwoMatrices
{
    public static class Md5Encryption {
        public static string GetMd5Hash(string input) {
            using (MD5 md5 = MD5.Create()) {
                byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder sBuilder = new StringBuilder();

                for (int i = 0; i < data.Length; i++)
                    sBuilder.Append(data[i].ToString("x2"));

                return sBuilder.ToString();
            }
        }
    }
 }


