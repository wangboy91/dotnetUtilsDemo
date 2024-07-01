﻿using System;
using System.Collections.Generic;

namespace Common.Utils
{
    public static class DictionaryUtils
    {
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
                else
                {
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
    }
}