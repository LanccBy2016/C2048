using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
       
    /// <summary>
    /// 加解密方式。
    /// </summary>
    public class EncryptAndDecrypt
    {
        //密钥,大于8位

        private static string key = "CC&20190920";

        /// <summary>
        /// DES加密字符串
        /// </summary>
        /// <param name="encryptString">待加密的字符串</param>
        /// <param name="encryptKey">加密密钥,要求为8位</param>
        /// <returns>加密成功返回加密后的字符串，失败返回源串</returns>
        public static string Encrypt(string str)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(key.Substring(0, 8));
                byte[] rgbIV = Encoding.UTF8.GetBytes(key);
                byte[] inputByteArray = Encoding.UTF8.GetBytes(str); //用指定的密钥和初始化向量创建CBC模式的DES加密标准
                DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();

                dCSP.Mode = CipherMode.CBC;

                dCSP.Padding = PaddingMode.PKCS7;

                MemoryStream mStream = new MemoryStream();

                CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);

                cStream.Write(inputByteArray, 0, inputByteArray.Length);//写入内存流

                cStream.FlushFinalBlock();//将缓冲区中的数据写入内存流，并清除缓冲区

                var msg = ByteArrayToHexString(mStream.ToArray()); //将内存流转写入字节数组并转换为string字符
                return msg;
            }
            catch (Exception ex)
            {
                return str;
            }
        }

        /// <summary>
        /// DES解密字符串
        /// </summary>
        /// <param name="decryptString">待解密的字符串</param>
        /// <param name="decryptKey">解密密钥,要求为8位,和加密密钥相同</param>
        /// <returns>解密成功返回解密后的字符串，失败返源串</returns>
        public static string Decrypt(string str)
        {
            try
            {
                byte[] keyArray = UTF8Encoding.UTF8.GetBytes(key.Substring(0, 8));
                byte[] ivArray = UTF8Encoding.UTF8.GetBytes(key);
                byte[] toEncryptArray = HexStringToByteArray(str);

                DESCryptoServiceProvider rDel = new DESCryptoServiceProvider();
                //rDel.Key = keyArray;
                //rDel.IV = ivArray;
                rDel.Mode = CipherMode.CBC;
                rDel.Padding = PaddingMode.PKCS7;

                MemoryStream mStream = new MemoryStream();

                CryptoStream cStream = new CryptoStream(mStream, rDel.CreateDecryptor(keyArray, ivArray), CryptoStreamMode.Write);

                cStream.Write(toEncryptArray, 0, toEncryptArray.Length);

                cStream.FlushFinalBlock();
                var msg = Encoding.UTF8.GetString(mStream.ToArray());
                return msg;
            }
            catch (Exception ex)
            {
                return str;
            }
        }
        /// <summary>
        /// 将指定的16进制字符串转换为byte数组
        /// </summary>
        /// <param name="s">16进制字符串(如：“7F 2C 4A”或“7F2C4A”都可以)</param>
        /// <returns>16进制字符串对应的byte数组</returns>
        public static byte[] HexStringToByteArray(string s)
        {
            s = s.Replace(" ", "");
            byte[] buffer = new byte[s.Length / 2];
            for (int i = 0; i < s.Length; i += 2)
                buffer[i / 2] = (byte)Convert.ToByte(s.Substring(i, 2), 16);
            return buffer;
        }

        /// <summary>
        /// 将一个byte数组转换成一个格式化的16进制字符串
        /// </summary>
        /// <param name="data">byte数组</param>
        /// <returns>格式化的16进制字符串</returns>
        public static string ByteArrayToHexString(byte[] data)
        {
            StringBuilder sb = new StringBuilder(data.Length * 3);
            foreach (byte b in data)
            {
                //16进制数字
                sb.Append(Convert.ToString(b, 16).PadLeft(2, '0'));
                //16进制数字之间以空格隔开
                //sb.Append(Convert.ToString(b, 16).PadLeft(2, '0').PadRight(3, ' '));
            }
            return sb.ToString().ToLower();
        }

    }

}
