using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Enums
    {
        /// <summary>
        /// 获取枚举描述信息
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <param name="enumValue">枚举值</param>
        /// <returns></returns>
        public static string GetDescriptionFromEnumValue(Type enumType, object enumValue)
        {
            try
            {
                object o = Enum.Parse(enumType, enumValue.ToString());

                string name = o.ToString();
                System.ComponentModel.DescriptionAttribute[] customAttributes = (System.ComponentModel.DescriptionAttribute[])enumType.GetField(name).GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
                if ((customAttributes.Length == 1))
                {
                    return customAttributes[0].Description;
                }
                return name;
            }
            catch
            {
                return "";
            }
        }
    }
    /// <summary>
    /// 排序类型
    /// </summary>
    public enum OrderType
    {
        [Description("正序")]
        Asc,
        [Description("倒序")]
        Desc
    }
}
