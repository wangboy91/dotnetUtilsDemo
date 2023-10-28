using System;
using System.Reflection;

namespace Common.Utils
{
    public static class CloneUtils
    {
        /// <summary>
        /// 对象深度拷贝
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T DeepCopy<T>(this T obj)
        {
            //如果是字符串或值类型则直接返回
            if (obj == null || obj is string || obj.GetType().IsValueType) return obj;

            var instance = Activator.CreateInstance(obj.GetType());
            var fields = obj.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            foreach (var field in fields)
            {
                field.SetValue(instance, DeepCopy(field.GetValue(obj)));
            }
            return (T)instance;
        }
        public static T JsonDeepCopy<T>(this T t)
        {

            var jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(t);
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonStr);
            return obj;
        }
        /// <summary>
        /// 基类对象拷贝成一个子对象
        /// </summary>
        /// <typeparam name="TParent"></typeparam>
        /// <typeparam name="TChild"></typeparam>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static TChild AutoCopy<TParent, TChild>(this TParent parent) where TChild : TParent, new()
        {
            TChild child = new TChild();
            var parentType = typeof(TParent);
            var properties = parentType.GetProperties();
            foreach (var propertie in properties)
            {
                //循环遍历属性
                if (propertie.CanRead && propertie.CanWrite)
                {
                    //进行属性拷贝
                    propertie.SetValue(child, propertie.GetValue(parent, null), null);
                }
            }
            return child;
        }
        /// <summary>
        /// 子对象拷贝成基类对象
        /// </summary>
        /// <typeparam name="TParent"></typeparam>
        /// <typeparam name="TChild"></typeparam>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static TParent AutoCopyToParent<TParent, TChild>(this TChild child) where TChild : TParent where TParent : new()
        {
            TParent parent = new TParent();
            var parentType = typeof(TParent);
            var properties = parentType.GetProperties();
            foreach (var propertie in properties)
            {
                //循环遍历属性
                if (propertie.CanRead && propertie.CanWrite)
                {
                    //进行属性拷贝
                    propertie.SetValue(parent, propertie.GetValue(child, null), null);
                }
            }
            return parent;
        }
    }
}
