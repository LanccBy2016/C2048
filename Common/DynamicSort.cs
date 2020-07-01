using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// 给List动态排序
    /// By CC
    /// 2019年7月12日15:46:21
    /// </summary>
    public static class DynamicSort
    {

        /// <summary>
        /// 动态排序
        /// </summary>
        /// <typeparam name="T">排序对象</typeparam>
        /// <param name="list">所需排序的List</param>
        /// <param name="OrderField">所需排序的字段</param>
        /// <param name="orderType">排序类型</param>
        /// <returns></returns>
        public static List<T> Sort<T>(List<T> list,string OrderField, OrderType orderType)
        {
            if (OrderField != null && OrderField!="")
            {
                return list;
            }
            if (orderType == OrderType.Desc)
            {
                var query = from p in list
                            orderby GetPropertyValue(p, OrderField) descending
                            select p;
                return query.ToList<T>();
            }
            else
            {
                var query = from p in list
                            orderby GetPropertyValue(p, OrderField)
                            select p;
                return query.ToList<T>();
            }
            
        }

        private static object GetPropertyValue(object obj, string property)
        {
            System.Reflection.PropertyInfo propertyInfo = obj.GetType().GetProperty(property);
            return propertyInfo.GetValue(obj, null);
        }

    }
}
