using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Common
{
    public class CharHelper
    {
        /// <summary>
        /// 过滤javascript脚步注入
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static String ClearJavascript(String html)
        {
            if (!string.IsNullOrEmpty(html))
            {
                string filterString = HttpContext.Current.Server.HtmlDecode(html).Trim();
                Regex reg = new Regex(@"<script[^>]*>(.|\n)*?(?=<\/script>)<\/script>");
                string result = reg.Replace(filterString, "");
                return result;
            }
            else
            {
                return html;
            }

        }

        /// <summary>
        /// 屏蔽关键字
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static String ClearKeyWord(string html)
        {
            var strChar = string.Empty;
            if (!string.IsNullOrEmpty(html))
            {
                String[] arrFiltrateChar = { "0.5", "佣金", "搜房", "58", "赶集", "安居客", "链家" };
                for (Int32 iFiltrateChar = 0; iFiltrateChar < arrFiltrateChar.Length; iFiltrateChar++)
                {
                    strChar = html.Replace(arrFiltrateChar[iFiltrateChar], "");
                }
                return strChar;
            }
            else
            {
                return "";
            }
        }


        /// <summary>
        /// 过滤非本站链接
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string ClearNoSite(string html)
        {
            if (string.IsNullOrEmpty(html))
            {
                return "";
            }
            string filterString = HttpContext.Current.Server.HtmlDecode(html).Trim();
            Regex reg = new Regex(@"[\s]href=([""'])(http?(?:(?!(?:360fdc.com))[^\1])*?)\1");
            string result = reg.Replace(filterString, "$1");
            return HttpContext.Current.Server.HtmlEncode(result);
        }



        /// <summary>
        /// 完全过滤富文本编辑框的非法字符，脚步注入，和非本站链接
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string CompleteClear(string html)
        {

            var result = "";
            if (!string.IsNullOrEmpty(html))
            {
                String[] arrFiltrateChar = { "0.5", "佣金", "搜房", "58", "赶集", "安居客", "链家" };
                for (Int32 iFiltrateChar = 0; iFiltrateChar < arrFiltrateChar.Length; iFiltrateChar++)
                {
                    result = html.Replace(arrFiltrateChar[iFiltrateChar], "");
                }
                string filterString = HttpContext.Current.Server.HtmlDecode(result).Trim();
                Regex reg = new Regex(@"<script[^>]*>(.|\n)*?(?=<\/script>)<\/script>");
                result = reg.Replace(filterString, "");
                Regex reg2 = new Regex(@"[\s]href=([""'])(http?(?:(?!(?:360fdc.com))[^\1])*?)\1");
                result = reg2.Replace(result, "$1");
                return HttpContext.Current.Server.HtmlEncode(result);
            }
            else
            {
                return "";
            }
        }
        /// <summary>
        /// 过滤特殊字符,全面过滤，用于登录和传值过滤
        /// </summary>
        /// <param name="strChar"></param>
        /// <returns></returns>
        public static String ClearSpecial(String strChar)
        {
            if (string.IsNullOrEmpty(strChar))
            {
                return "";
            }
            String[] arrFiltrateChar = { "~", "`", "!", "@", "#", "$", "%", "^", "&", "*", "(", ")", "-", "_", "+", "=", "{", "[", "}", "]", "|", "\\", ":", ";", "\"", "'", "<", ",", ">", ".", "?", "/", "！", "（", "）", "《", "》", "—", "【", "】", "￥", "；", "：", "？", "、", "…", "。", "‘", "’" };
            for (Int32 iFiltrateChar = 0; iFiltrateChar < arrFiltrateChar.Length; iFiltrateChar++)
            {
                strChar = strChar.Replace(arrFiltrateChar[iFiltrateChar], "");
            }
            return strChar;
        }

        /// <summary>
        /// 去掉html标签分成数组
        /// </summary>
        /// <param name="htmlstring"></param>
        /// <returns></returns>
        public static string[] RemoveHTML(string htmlstring)//将字符串中的HTML标签包含的内容移除
        {
            if (!string.IsNullOrEmpty(htmlstring))
            {
                #region
                //删除js脚本
                htmlstring = System.Text.RegularExpressions.Regex.Replace(htmlstring, @"<script[^>]*?>.*?</script>", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                //删除HTML标签
                htmlstring = System.Text.RegularExpressions.Regex.Replace(htmlstring, @"<(.[^>]*)>", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                //htmlstring = System.Text.RegularExpressions.Regex.Replace(htmlstring, @"([\r\n])[\s]+", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                htmlstring = System.Text.RegularExpressions.Regex.Replace(htmlstring, @"-->", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                htmlstring = System.Text.RegularExpressions.Regex.Replace(htmlstring, @"<!--.*", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                htmlstring = System.Text.RegularExpressions.Regex.Replace(htmlstring, @"&(quot|#34);", "\"", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                htmlstring = System.Text.RegularExpressions.Regex.Replace(htmlstring, @"&(amp|#38);", "&", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                htmlstring = htmlstring.Replace("&mdash;", "");
                htmlstring = System.Text.RegularExpressions.Regex.Replace(htmlstring, @"&(nbsp|#160);", " ", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                htmlstring = System.Text.RegularExpressions.Regex.Replace(htmlstring, @"&(iexcl|#161);", "\xa1", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                htmlstring = System.Text.RegularExpressions.Regex.Replace(htmlstring, @"&(cent|#162);", "\xa2", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                htmlstring = System.Text.RegularExpressions.Regex.Replace(htmlstring, @"&(pound|#163);", "\xa3", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                htmlstring = System.Text.RegularExpressions.Regex.Replace(htmlstring, @"&(copy|#169);", "\xa9", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                htmlstring = System.Text.RegularExpressions.Regex.Replace(htmlstring, @"&#(\d+);", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                htmlstring.Replace("<", "");
                htmlstring.Replace(">", "");
                htmlstring.Replace("\r\n", "");
                #endregion
                string[] strs = Regex.Split(htmlstring, "&gt;", RegexOptions.IgnoreCase);
                return strs;
            }
            else
            {
                return null;
            }

        }

        /// <summary>
        /// 获取html字符串文本
        /// </summary>
        /// <param name="htmlstring"></param>
        /// <returns></returns>
        public static string GetText(string[] htmlstring)
        {
            string str = "";
            if (htmlstring != null)
            {
                for (int i = 0, j = htmlstring.Length; i < j; i++)
                {
                    if (htmlstring[i].IndexOf("&lt;") > -1)
                    {
                        str += htmlstring[i].Substring(0, htmlstring[i].IndexOf("&lt;"));
                    }
                    else
                    {
                        str += htmlstring[i];
                    }
                }
            }
            return str;
        }


        /// <summary>
        /// 阿拉伯数字转换为汉子数字
        /// </summary>
        /// <returns></returns>
        public static string ShuZi(int? num)
        {
            Hashtable dic = new Hashtable();
            dic.Add(0, "零");
            dic.Add(1, "一");
            dic.Add(2, "二");
            dic.Add(3, "三");
            dic.Add(4, "四");
            dic.Add(5, "五");
            dic.Add(6, "六");
            dic.Add(7, "七");
            dic.Add(8, "八");
            dic.Add(9, "九");
            dic.Add(10, "十");
            dic.Add(11, "十一");
            dic.Add(12, "十二");
            dic.Add(13, "十三");
            dic.Add(14, "十四");
            dic.Add(15, "十五");
            dic.Add(16, "十六");
            dic.Add(17, "十七");
            dic.Add(18, "十八");
            dic.Add(19, "十九");
            dic.Add(20, "二十");
            if (num != null && num < 21 && num > 0)
            {
                return dic[num].ToString();
            }
            else
            {
                return "";
            }
        }
    }
}
