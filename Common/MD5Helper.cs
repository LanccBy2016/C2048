using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class MD5Helper
    {

        public static bool VerifyMd5Hash(string input, string hash, string securityKey = null)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                if (!string.IsNullOrEmpty(securityKey))
                {
                    input += string.Format("&SecurityKey={0}", securityKey);
                }
                string hashOfInput = GetMd5Hash(md5Hash, input );
                StringComparer comparer = StringComparer.OrdinalIgnoreCase;
                if (0 == comparer.Compare(hashOfInput, hash))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string EncryptMD5(string input)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                return GetMd5Hash(md5Hash,  input );
            }
        }
       
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="md5Hash"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        private static string GetMd5Hash(MD5 md5Hash, string input)
        {

            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString().ToLower();
        }
    }
}
