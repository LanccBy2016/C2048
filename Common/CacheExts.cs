using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;

namespace Common
{
    public class CacheExts<T>
    {
        private static int Minutes
        {
            get
            {
                return Convert.ToInt32( WebConfigOperation.RuntimeCacheTime);
            }
        }

        /// <summary>
        /// 系统统一设置runtimeCache
        /// </summary>
        /// <param name="key"></param>
        /// <param name="fun"></param>
        /// <param name="mins">缓存时间，单位分钟</param>
        /// <param name="noSlidingExpiration">true为绝对缓存，false滑动缓存</param>
        /// <returns></returns>
        public static T GetValue(string key, Func<T> fun, int mins = 0, bool noSlidingExpiration=true)
        {
            try
            {
                Cache mCache = HttpRuntime.Cache;
                if (mCache[key] == null)
                {
                    T catchValue = fun();
                    if (catchValue != null)
                    {
                        int minuts = mins > 0 ? mins : Minutes;
                        if (noSlidingExpiration)
                            //绝对缓存失效
                            mCache.Insert(key, catchValue, null, DateTime.Now.AddMinutes(minuts), Cache.NoSlidingExpiration);
                        //mCache.Insert(key, catchValue, null, DateTime.Now.AddMinutes(minuts), TimeSpan.Zero);
                        else
                            //滑动缓存失效
                            mCache.Insert(key, catchValue, null, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(mins));
                        
                        return catchValue;
                    }
                }
                else
                {
                    return (T)mCache[key];
                }
                return default(T);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
     
        public static T GetValue(string key)
        {
            try
            {
                Cache mCache = HttpRuntime.Cache;
                if (mCache[key] == null)
                {
                    return default(T);
                }
                else
                {
                    return (T)mCache[key];
                }
            }
            catch (Exception ex)
            {
               throw ex;
            }
        }

        /// <summary>
        /// 系统统一设置runtimeCache
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="mins">缓存时间，单位分钟</param>
        /// <param name="noSlidingExpiration">true为绝对缓存，false滑动缓存</param>
        /// <returns></returns>
        public static bool SetValue(string key, object obj, int mins = 0, bool noSlidingExpiration = true)
        {
            if (obj == null)
                return false;
            try
            {
                Cache mCache = HttpRuntime.Cache;
                 int minuts = mins > 0 ? mins : Minutes;
                if (noSlidingExpiration)
                    //绝对缓存失效
                    mCache.Insert(key, obj, null, DateTime.Now.AddMinutes(minuts), Cache.NoSlidingExpiration);
                else
                    //滑动缓存失效
                    mCache.Insert(key, obj, null, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(mins));
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
    
}
}
