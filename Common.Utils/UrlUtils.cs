using System.Collections.Generic;

namespace Common.Utils
{
    public static class UrlUtils
    {
        
        /// <summary>
        /// 字典数据拼接成url类型的字符串
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="urlPrefix"></param>
        /// <returns></returns>
        public static string ToURLParams(this Dictionary<string, string> dictionary, string urlPrefix = "")
        {
            var stringList = new List<string>();
            foreach (var item in dictionary)
            {
                stringList.Add($"{item.Key}={item.Value}");
            }

            if (string.IsNullOrEmpty(urlPrefix))
            {
                return string.Join("&", stringList.ToArray());
            }

            return $"{urlPrefix}?{string.Join("&", stringList.ToArray())}";
        }

        /// <summary>
        /// Url转换成参数字典
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static Dictionary<string, string> ToURLParamsDictionary(this string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                var paramsUrl = url;
                if (url.IndexOf('?') >= 0)
                {
                    paramsUrl = url.Substring(url.IndexOf('?'), url.Length - url.IndexOf('?'));
                }

                string[] array = paramsUrl.Split('&');
                if (array.Length != 0)
                {
                    Dictionary<string, string> dictionary = new Dictionary<string, string>();
                    string[] array2 = array;
                    foreach (string text2 in array2)
                    {
                        if (string.IsNullOrEmpty(text2))
                        {
                            continue;
                        }

                        string[] array3 = text2.Split('=');
                        if (array3.Length == 2)
                        {
                            string key = array3[0];
                            if (dictionary.ContainsKey(key))
                            {
                                dictionary[key] = $"{dictionary[key]},{array3[1]}";
                            }
                            else
                            {
                                dictionary[key] = array3[1];
                            }
                        }
                    }

                    return dictionary;
                }
            }

            return new Dictionary<string, string>();
        }
    }
}