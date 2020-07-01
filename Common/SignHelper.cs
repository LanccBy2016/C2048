using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;


namespace Common
{
    /// <summary>
    /// 签名认证
    /// </summary>
    public class SignHelper
    {

        #region 调用用户中心接口时，动态生成的签名
        /// <summary>
        /// 获取参数字符串，去掉参数名和参数意外的字符
        /// </summary>
        /// <param name="parms"></param>
        /// <returns></returns>
        public static string ParmeterWithString(Dictionary<string, string> parms)
        {
            string returnString = string.Empty;
            if (parms != null && parms.Count > 0)
            {
                parms = parms.OrderBy(k => k.Key).ToDictionary(k => k.Key, v => v.Value);
                foreach (KeyValuePair<string, string> kv in parms)
                {
                    returnString += kv.Key + HttpUtility.UrlEncode(kv.Value, Encoding.UTF8);
                }
            }
            return returnString;
        }

        /// <summary>
        /// MD5加密处理
        /// </summary>
        /// <param name="ConvertString"></param>
        /// <returns></returns>
        public static string Md5Hex32(string ConvertString)
        {
            byte[] b = System.Text.Encoding.Default.GetBytes(ConvertString);
            b = new System.Security.Cryptography.MD5CryptoServiceProvider().ComputeHash(b);
            string ret = "";
            for (int i = 0; i < b.Length; i++)
                ret += b[i].ToString("x").PadLeft(2, '0');
            return ret;
        } 
        #endregion


        #region  提供webapi接口签名认证

        /// <summary>
        /// 获取post/get集合
        /// </summary>
        /// <param name="ignoreCase">true 不区分大小写，统一返回小写;  false 区分大小写</param>
        /// <returns></returns>
        public static SortedDictionary<string, string> GetRequestSortDic(bool ignoreCase = true)
        {
            int i = 0;
            SortedDictionary<string, string> sArray = new SortedDictionary<string, string>();

            //var coll = HttpContext.Current.Request.Params;

            var coll = HttpContext.Current.Request.Unvalidated.Form;
            String[] requestItem = coll.AllKeys;
            for (i = 0; i < requestItem.Length; i++)
            {
                if (ignoreCase)
                    sArray.Add(requestItem[i].ToLower(), WebApiHelper.GetRequestParam(requestItem[i]));
                else
                    sArray.Add(requestItem[i], WebApiHelper.GetRequestParam(requestItem[i]));
            }

            coll = HttpContext.Current.Request.QueryString;
            requestItem = coll.AllKeys;
            for (i = 0; i < requestItem.Length; i++)
            {
                if (ignoreCase)
                    sArray.Add(requestItem[i].ToLower(), WebApiHelper.GetRequestParam(requestItem[i]));
                else
                    sArray.Add(requestItem[i], WebApiHelper.GetRequestParam(requestItem[i]));
            }

            return sArray;
        }

        /// <summary>
        /// 获取服务端Sign
        /// </summary>
        /// <param name="inputPara"></param>
        /// <param name="publicKey">MD5加密key</param>
        /// <returns></returns>
        public static string GetResponseMysign(SortedDictionary<string, string> inputPara, string publicKey)
        {
            string fullstring = FiterPostStrings(inputPara, "sign");
            if (!string.IsNullOrEmpty(publicKey))
            {
                fullstring += string.Format("&SecurityKey={0}", publicKey);
            }
            return MD5Helper.EncryptMD5(fullstring);
        }
        /// <summary>
        /// 过滤空值、sign与sign_type参数
        /// </summary>
        /// <param name="inputPara">参数键值对</param>
        /// <param name="excepted">过滤的值</param>
        /// <returns></returns>
        private static string FiterPostStrings(SortedDictionary<string, string> inputPara, string excepted)
        {
            Dictionary<string, string> sPara = new Dictionary<string, string>();

            //过滤空值、sign与sign_type参数
            foreach (KeyValuePair<string, string> temp in inputPara)
            {
                if (temp.Key.ToLower() != excepted && !string.IsNullOrEmpty(temp.Value))
                {
                    sPara.Add(temp.Key, temp.Value);
                }
            }
            //获得签名结果
            StringBuilder prestr = new StringBuilder();
            foreach (KeyValuePair<string, string> temp in sPara)
            {
                prestr.Append(temp.Key + "=" + temp.Value + "&");
            }
            //去掉最后一个&字符
            int nLen = prestr.Length;
            if (nLen > 1)
                prestr.Remove(nLen - 1, 1);
            return prestr.ToString();
        }


        #endregion
    }

}
