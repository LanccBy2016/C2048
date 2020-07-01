using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Common
{
    public class WebApiHelper
    {
        
        /// <summary>
        /// 从当前环境中获取
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="unvalidated">跳过验证</param>
        /// <returns></returns>
        public static string GetRequestParam(string name, bool unvalidated = false)
        {
            string res = "";
            if (unvalidated)
            {
               var v = HttpContext.Current.Request.Unvalidated[name];
                if (v != null)
                {
                    res = v;
                }
            }
            else
            {
                var v = HttpContext.Current.Request[name];
                if (v != null)
                {
                    res = v;
                }
            }
            return res;
        }

        /// <summary>
        /// 获取请求参数信息
        /// </summary>
        /// <param name="request"></param>
        ///  <param name="unvalidated">跳过验证</param>
        /// <returns></returns>
        public static string GetRequestParam(HttpRequest request, bool unvalidated = false)
        {
            string param = string.Empty;
            if (request != null)
            {
                NameValueCollection collF = null;
                NameValueCollection collQ = null;

                if (unvalidated)
                {
                    collF = request.Unvalidated.Form;
                    collQ = request.Unvalidated.QueryString;
                }
                else
                {
                    collF = request.Form;
                    collQ = request.QueryString;
                }

                if (collF != null && !collF.ToString().Equals(""))
                {
                    param = collF.ToString();
                }
                else if (collQ != null && !collQ.ToString().Equals(""))
                {
                    param = collQ.ToString();
                }
                else
                {
                  param= new StreamReader(request.InputStream, Encoding.UTF8).ReadToEnd();
                }
            }
            return param;
        }







        /// <summary>
        /// 将对象属性转换为key-value对
        /// </summary>
        /// <param name="o"></param>
        /// <param name="ignoreCase">true 不区分大小写，统一返回小写 ; false 区分大小写</param>
        /// <returns></returns>
        public static Dictionary<string, object> ToMap(object o, bool ignoreCase = true)
        {
            var map = new Dictionary<string, object>();
            Type t = o.GetType();
            PropertyInfo[] pi = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo p in pi)
            {
                var n = p.Name;
                var v = p.GetValue(n);
                if (v != null)
                {
                    if (ignoreCase)
                        map.Add(n.ToLower(), v);
                    else
                        map.Add(n, v);
                }
            }
            return map;
        }


    }


    #region Newtonsoft.Json进行序列化返回数据时间格式处理
    /// <summary>
    /// 序列化日期
    /// </summary>
    public class DateConverter : DateTimeConverterBase
    {
        private static IsoDateTimeConverter dtConverter = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd" };

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return dtConverter.ReadJson(reader, objectType, existingValue, serializer);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            dtConverter.WriteJson(writer, value, serializer);
        }
    }
    /// <summary>
    /// 序列化时间
    /// </summary>
    public class TimeConverter : DateTimeConverterBase
    {
        private static IsoDateTimeConverter dtConverter = new IsoDateTimeConverter { DateTimeFormat = "HH:mm:ss" };

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return dtConverter.ReadJson(reader, objectType, existingValue, serializer);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            dtConverter.WriteJson(writer, value, serializer);
        }
    }
    /// <summary>
    /// 序列化日期时间
    /// </summary>
    public class DateTimeConverter : DateTimeConverterBase
    {
        private static IsoDateTimeConverter dtConverter = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" };

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return dtConverter.ReadJson(reader, objectType, existingValue, serializer);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            dtConverter.WriteJson(writer, value, serializer);
        }
    }

    /// <summary>
    /// 动态决定属性是否序列化
    /// </summary>
    public class LimitPropsContractResolver : DefaultContractResolver
    {
        private string[] props = null;

        private bool retain;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="props">传入的属性数组</param>
        /// <param name="retain">true:表示props是需要保留的字段  false：表示props是要排除的字段</param>
        public LimitPropsContractResolver(string[] props, bool retain = true)
        {
            //指定要序列化属性的清单
            this.props = props;

            this.retain = retain;
        }

        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            IList<JsonProperty> list =
                base.CreateProperties(type, memberSerialization);
            //只保留清单有列出的属性
            return list.Where(p =>
            {
                if (retain)
                {
                    return props.Contains(p.PropertyName);
                }
                else
                {
                    return !props.Contains(p.PropertyName);
                }
            }).ToList();
        }
    } 
    #endregion





}
