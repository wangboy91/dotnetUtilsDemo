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