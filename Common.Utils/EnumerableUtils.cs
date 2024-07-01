using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Common.Utils
{
    public static class EnumerableUtils
    {
                /// <summary>
        /// 移除无效GUID
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static List<string> ToRemoveErrorGuid(this List<string> ids)
        {
            var tempGuidIds = new List<string>();
            ids.ForEach(x =>
            {
                Guid tempId;
                if (!string.IsNullOrEmpty(x)&&Guid.TryParse(x, out tempId))
                {
                    tempGuidIds.Add(tempId.ToString());
                }
            });
            return tempGuidIds;
        }
        /// <summary>
        /// 字符串转guidList
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static List<Guid> ToGuidList(this List<string> ids)
        {
            var guidIds = new List<Guid>();
            ids.ForEach(x =>
            {
                Guid tempId;
                if (Guid.TryParse(x, out tempId))
                {
                    guidIds.Add(tempId);
                }
            });
            return guidIds;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static List<string> ToStringList(this List<Guid> ids)
        {
            return ids.Select(x=>x.ToString()).ToList();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="keyGetter"></param>
        /// <param name="valueGetter"></param>
        /// <typeparam name="TElement"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public static Dictionary<TKey, TValue> ToDictionaryEx<TElement, TKey, TValue>(this IEnumerable<TElement> source, Func<TElement, TKey> keyGetter, Func<TElement, TValue> valueGetter)
        {
            Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();
            foreach (TElement item in source)
            {
                TKey key = keyGetter(item);
                if (!dictionary.ContainsKey(key))
                {
                    dictionary.Add(key, valueGetter(item));
                }
            }

            return dictionary;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="keyGetter"></param>
        /// <param name="valueGetter"></param>
        /// <typeparam name="TElement"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public static Dictionary<TKey, List<TValue>> ToDictionaryExList<TElement, TKey, TValue>(this IEnumerable<TElement> source, Func<TElement, TKey> keyGetter, Func<TElement, TValue> valueGetter)
        {
            Dictionary<TKey, List<TValue>> dictionary = new Dictionary<TKey, List<TValue>>();
            foreach (TElement item in source)
            {
                TKey key = keyGetter(item);
                if (!dictionary.ContainsKey(key))
                {
                    dictionary.Add(key, new List<TValue> { { valueGetter(item) } });
                }
                else {
                    dictionary[key].Add(valueGetter(item));
                }
            }

            return dictionary;
        }
        /// <summary>
        /// 字典集合中添加字典集合
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="source"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static IDictionary<TKey, TValue> AddRange<TKey, TValue>(this IDictionary<TKey, TValue> source, IEnumerable<KeyValuePair<TKey, TValue>> values)
        {
            if (values == null)
                return source;
            foreach (KeyValuePair<TKey, TValue> keyValuePair in values)
                source[keyValuePair.Key] = keyValuePair.Value;
            return source;
        }
        /// <summary>
        /// 字典集合中新增或更新key-value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dic"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void AddOrUpdateValue<T>(this IDictionary<string, T> dic, string key, T value)
        {
            if (dic.ContainsKey(key))
            {
                dic[key] = value;
            }
            else
            {
                dic.Add(key, value);
            }
        }
        /// <summary>
        /// 判断迭代器是否为空
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> list)
        {
            return list == null || !list.Any();
        }
        /// <summary>
        /// 对象根据特定筛选器排重
        /// </summary>
        /// <typeparam name="TResult">返回类型</typeparam>
        /// <typeparam name="TSource">输入类型</typeparam>
        /// <param name="values">需要筛选的对象</param>
        /// <param name="selector">筛选器</param>
        /// <returns>筛选结果</returns>
        public static IEnumerable<TResult> SelectAndDistinct<TResult, TSource>(this IEnumerable<TSource> values, Func<TSource, TResult> selector)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }
            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            HashSet<TResult> set = new HashSet<TResult>();
            foreach (var item in values)
            {
                var result = selector(item);
                set.Add(result);
            }
            return set.AsEnumerable();
        }
        /// <summary>
        /// 根据字段进行排重
        /// 判断优先级：
        /// 引用地址是否相同
        /// 如果任意一个对象为空，返回空
        /// 判断Type类别是否一致
        /// 判断查询委托条件
        /// </summary>
        /// <typeparam name="T">处理的对象类型</typeparam>
        /// <param name="value">处理的集合</param>
        /// <param name="distinctFunc">处理的表达式</param>
        /// <returns></returns>
        public static IEnumerable<T> Distinct<T>(this IEnumerable<T> value, Func<T, object> distinctFunc) where T : class
        {
            return value.Distinct(new CustomComparer<T>(distinctFunc));
        }
        
        /// <summary>
        /// 将集合进行分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        /// <param name="sliceCount"></param>
        /// <returns></returns>
        public static List<List<T>> GetExecuteMaxRangeSlices<T>(this IEnumerable<T> entities, int sliceCount = 300)
        {
            var list = entities.ToList();
            List<List<T>> result = new List<List<T>>();
            if(list.Count==0)
                return result;
            var total = list.Count;
            var form = 0;
            do
            {
                var tempItem = list.Skip(form).Take(sliceCount).ToList();
                result.Add(tempItem);
                form += sliceCount;

            } while (form<total);
            return result;
        }
        /// <summary>
        /// 分批执行
        /// </summary>
        /// <param name="items">数据源</param>
        /// <param name="action">执行方法,arg1:批次号,arg2:分批数据</param>
        /// <param name="size">分批执行大小</param>
        /// <typeparam name="T"></typeparam>
        public static void SplitDoSync<T>(this IEnumerable<T> items, Action<IEnumerable<T>> action, int size = 300)
        {
            var list = items.ToList();
            if(list.Count==0)
                return;
            var total = list.Count;
            var form = 0;
            do
            {
                var tempItem = list.Skip(form).Take(size).ToList();
                action(tempItem);
                form += size;

            } while (form<total);
        }
        /// <summary>
        /// 分批执行器
        /// </summary>
        /// <param name="items"></param>
        /// <param name="func"></param>
        /// <param name="size"></param>
        /// <typeparam name="T"></typeparam>
        public static async Task SplitDoAsync<T>(this IEnumerable<T> items,Func<IEnumerable<T>,Task> func,int size = 300)
        {
            var list = items.ToList();
            if(list.Count==0)
                return;
            var total = list.Count;
            var form = 0;
            do
            {
                var tempItem = list.Skip(form).Take(size).ToList();
                await func(tempItem);
                form += size;

            } while (form<total);
        }
    }
}