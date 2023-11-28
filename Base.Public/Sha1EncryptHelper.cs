using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Base.Public
{
    /// <summary>
    /// Sha1加密
    /// </summary>
    public static class Sha1EncryptHelper
    {
        /// <summary>
        /// 用SHA1加密字符串
        /// </summary>
        /// <param name="source">要扩展的对象</param>
        /// <returns></returns>
        public static string Encrypt(string source)
        {
            var sha1 = SHA1.Create();
            var hash = sha1.ComputeHash(Encoding.Default.GetBytes(source));
            string byte2String = null;
            for (int i = 0; i < hash.Length; i++)
            {
                byte2String += hash[i].ToString("x2");
            }
            return byte2String;
        }
    }
}
