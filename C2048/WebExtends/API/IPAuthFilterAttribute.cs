using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http.Filters;
using System.Xml;
using Newtonsoft.Json;

namespace C2048.WebExtends.API
{
    /// <summary>
    /// WebAPI白名单限制
    /// </summary>
    public class IPAuthFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            if (actionContext.ActionDescriptor.GetCustomAttributes<SkipIPAttribute>(false).Count == 0 &&
                actionContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes<SkipIPAttribute>(false).Count == 0)
            {
                var ipaddress = GetIpaddress(); //用户的ip
                object obj = null;
                obj = GetCache(ipaddress + "api"); //获取请求api的ip列表
                {
                    if (obj == null)
                    {
                        if (!IpConfig(ipaddress))
                        {
                            var data = new
                            {
                                Code = 1004,
                                Msg = "当前ip地址" + ipaddress + "无访问权限",
                                Success = false
                            };
                            actionContext.Response = new HttpResponseMessage(HttpStatusCode.Accepted)
                            {
                                Content =
                                    new StringContent(JsonConvert.SerializeObject(data),
                                        System.Text.Encoding.GetEncoding("UTF-8"), "application/json")
                            };
                            return;
                        }
                        else
                        {
                            SetCache(ipaddress + "api", 1, 5);
                        }
                    }
                }
            }
            base.OnActionExecuting(actionContext);
        }
        public static bool IpConfig(string ip)
        {
            string XmlPath = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/IPwhiteList.xml");
            XmlDocument doc = new XmlDocument();
            doc.Load(XmlPath);    //加载Xml文件 
            XmlElement root = doc.DocumentElement;   //获取根节点 
            XmlNodeList personNodes = root.GetElementsByTagName("item"); //获取item子节点集合 
            foreach (XmlNode node in personNodes)
            {
                string flag = ((XmlElement)node).GetAttribute("ip");   //获取Name属性值 
                if (flag == ip)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 获取当前应用程序指定CacheKey的Cache值
        /// </summary>
        /// <param name="CacheKey"></param>
        /// <returns></returns>
        public static object GetCache(string CacheKey)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            return objCache[CacheKey];
        }
        /// <summary>
        ///  设置缓存
        /// </summary>
        /// <param name="CacheKey"></param>
        /// <param name="objObject"></param>
        /// <param name="expires_in"></param>
        public static void SetCache(string CacheKey, object objObject, double expires_in)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;

            objCache.Insert(CacheKey, objObject, null, DateTime.Now.AddSeconds(expires_in), System.Web.Caching.Cache.NoSlidingExpiration);
        }
        /// <summary>
        ///   获取IP地址
        /// </summary>
        /// <returns></returns>
        public static string GetIpaddress()
        {
            string result = String.Empty;
            result = HttpContext.Current.Request.ServerVariables["HTTP_CDN_SRC_IP"];
            if (string.IsNullOrEmpty(result))
                result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

            if (string.IsNullOrEmpty(result))
                result = HttpContext.Current.Request.UserHostAddress;

            if (string.IsNullOrEmpty(result) || !IsIP(result))
                return "127.0.0.1";

            return result;
        }
        public static bool IsIP(string ip)
        {
            return Regex.IsMatch(ip, "^((2[0-4]\\d|25[0-5]|[01]?\\d\\d?)\\.){3}(2[0-4]\\d|25[0-5]|[01]?\\d\\d?)$");
        }
    }

    /// <summary>
    /// 用它标识的对象可以跳过IP验证
    /// </summary>
    public class SkipIPAttribute : Attribute
    {
    }
}