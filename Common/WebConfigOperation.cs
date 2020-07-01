using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class WebConfigOperation
    {

        public string GetConfig(string Key)
        {
            return System.Configuration.ConfigurationManager.AppSettings[Key] ?? Key;
        }


        #region Cookie配制信息

        /// <summary>
        /// 加密解密密匙
        /// </summary>
        public static string LocalSystemSecretKey
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["LocalSystemSecretKey"] ?? ""; }
        }

        /// <summary>
        /// 登录用户cookie
        /// </summary>
        public static string CookieName
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["CookieName"] ?? "CC";
            }
        }
        /// <summary>
        /// 游客cookie
        /// </summary>
        public static string YkCookieName
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["YkCookieName"] ?? "yk";
            }
        }
        /// <summary>
        /// CookieDomain
        /// </summary>
        public static string CookieDomain
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["CookieDomain"] ?? "";
            }
        }
        /// <summary>
        ///CookiePath
        /// </summary>
        public static string CookiePath
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["CookiePath"] ?? "";
            }
        }
        #endregion

        #region 关键配置

        /// <summary>
        /// 数据库类型
        /// </summary>
        public static int DBType
        {
            get { return Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["DBType"] ?? "1"); }
        }


        /// <summary>
        /// 缓存过期时间(分钟)
        /// </summary>
        public static int RuntimeCacheTime
        {
            get { return Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["RuntimeCacheTime"] ?? "0"); }
        }

        /// <summary>
        /// 公钥
        /// </summary>
        public static string APISignPublicKey
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["APISignPublicKey"] ?? ""; }
        }
        #endregion
    }
}
