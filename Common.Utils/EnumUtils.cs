using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace Common.Utils
{
    public static class EnumUtils
    {

        /// <summary>
        /// 获取枚举的描述信息
        /// </summary>
        public static string GetDescription(this Enum enumValue)
        {
            var value = enumValue.ToString();
            FieldInfo field = enumValue.GetType().GetField(value);
            var objs = field.GetCustomAttributes(typeof(DescriptionAttribute), false);    //获取描述属性
            if (objs.Length == 0)    //当描述属性没有时，直接返回名称
                return value;
            DescriptionAttribute descriptionAttribute = (DescriptionAttribute)objs[0];
            return descriptionAttribute.Description;
        }
        public static List<string> GetDescriptionList(Enum enumType)
        {
            var result = new List<string>();
            var curType = enumType.GetType();
            var fields = curType.GetFields();
            foreach (var field in fields)
            {
                if (field.FieldType != curType)
                {
                    continue;
                }

                var attr = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
                if (attr != null)
                {
                    result.Add((attr as DescriptionAttribute)?.Description);
                }
            }
            return result;
        }
    }
}