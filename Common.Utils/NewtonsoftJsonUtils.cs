using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Common.Utils
{
    /// <summary>
    /// 序列化排序
    /// </summary>
    public class JsonPropertySortResolver : CamelCasePropertyNamesContractResolver
    {
        //public JsonPropertySortResolver() : base(true)
        //{
        //}

        /// <summary>
        /// 属性名称按照字典顺序排序输出
        /// </summary>
        /// <param name="type"></param>
        /// <param name="memberSerialization"></param>
        /// <returns></returns>
        protected override IList<JsonProperty> CreateProperties(Type type,
            MemberSerialization memberSerialization)
        {
            IList<JsonProperty> list = base.CreateProperties(type, memberSerialization);
            return list.OrderBy(a => a.PropertyName).ToList();
        }

    }
    
    public static class NewtonsoftJsonUtils
    {
        /// <summary>
        /// 字段对象转换成json
        /// </summary>
        /// <param name="data"></param>
        /// <param name="noConvertKeys"></param>
        /// <returns></returns>
        public static string ToJsonString(this Dictionary<string, object> data, params string[] noConvertKeys)
        {
            var jData = new Dictionary<string, string>();
            foreach (var field in data)
            {
                if (noConvertKeys.Contains(field.Key))
                {
                    continue;
                }
                if (!jData.ContainsKey(field.Key))
                {
                    jData.Add(field.Key, field.Value?.ToString());
                }

            }
            return JsonConvert.SerializeObject(jData);
        }
        
        /// <summary>
        /// 获取json中的值 转字符串
        /// </summary>
        /// <param name="jt"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetString(this JObject jt, string path)
        {
            return jt.SelectTokenToString(path);
        }
        /// <summary>
        /// 获取json中的值 转字符串
        /// </summary>
        /// <param name="jt"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetString(this JToken jt, string path)
        {
            return jt.SelectTokenToString(path);
        }
        /// <summary>
        /// 将对象序列化成Json
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="camelCasePropertyName">是否驼峰命名</param>
        /// <returns></returns>
        public static string ToJsonNewtonsoft(this object obj, bool camelCasePropertyName=true)
        {
            if (camelCasePropertyName)
            {
                JsonSerializerSettings setting = new JsonSerializerSettings()
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(obj, setting);
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }
        /// <summary>
        /// 将对象序列化成Json 并排序
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static string ToJsonNewtonsoftSort(this object obj)
        {

            JsonSerializerSettings setting = new JsonSerializerSettings()
            {
                //ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new JsonPropertySortResolver(),

            };
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj, setting);
        }
        /// <summary>
        /// 将json反序列化成对象
        /// </summary>
        /// <typeparam name="TType">反序列化成的类型</typeparam>
        /// <param name="json">json</param>
        /// <returns>TType</returns>
        public static TType ToObjectNewtonsoft<TType>(this string json)
        {
            var type = typeof(TType);
            return (TType)Newtonsoft.Json.JsonConvert.DeserializeObject(json, type);
        }
        private static JsonSerializerSettings JsonSerializerSettings(Formatting formatting) => new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Formatting =formatting
        };
        /// <summary>
        /// 转换为JSON
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="formatting"></param>
        /// <returns></returns>
        public static string ToJsonFormatting(this object obj, Formatting formatting = default(Formatting)) =>
            JsonConvert.SerializeObject(obj, JsonSerializerSettings(formatting));
        public static string ToJsonSettings(this object obj, JsonSerializerSettings settings) =>
            JsonConvert.SerializeObject(obj, settings);
        /// <summary>
        /// 获取json中的值 转字符串
        /// </summary>
        /// <param name="jt"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string SelectTokenToString(this JToken jt, string path)
        {
            if (jt == null)
            {
                return "";
            }
            if (jt.Type == JTokenType.Object) {
                var jToken = jt.SelectTokens(path).FirstOrDefault();
                if (jToken == null)
                {
                    return "";
                }
                return jToken.ToString();
            }
            return jt.ToString();
        }
        /// <summary>
        /// 获取json中的值 转字符串
        /// </summary>
        /// <param name="jt"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string SelectTokenToString(this JObject jt, string path)
        {
            var jToken = jt.SelectTokens(path).FirstOrDefault();
            if (jToken != null)
            {
                return jToken.ToString();
            }
            return "";
        }
        /// <summary>
        /// 获取json的字段数组集合
        /// </summary>
        /// <param name="jt"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static IEnumerable<JToken> SelectTokenChildren(this JObject jt, string path)
        {
            var jToken = jt.SelectTokens(path).FirstOrDefault()?.Children();
            
            return jToken;
        }
        public static bool SelectTokenIsNullOrEmpty(this JToken jt, string path)
        {
            if (jt == null)
            {
                return true;
            }
            return string.IsNullOrEmpty(SelectTokenToString(jt, path));
        }
        public static bool SelectTokenIsNullOrEmpty(this JObject jt, string path)
        {
            if (jt == null)
            {
                return true;
            }
            return string.IsNullOrEmpty(SelectTokenToString(jt, path));
        }
        public static Dictionary<string, string> GetValuesToDic(this JObject jObject, string[] keys)
        {
            var result = new Dictionary<string, string>();
            foreach (var key in keys)
            {
                var value = jObject.SelectToken(key)?.ToString();
                if (string.IsNullOrEmpty(value))
                {
                    result.Add(key, value);
                }
            }
            return result;
        }
        public static List<string> GetValues(this JObject jObject, string[] keys)
        {
            var result = new List<string>();
            foreach (var key in keys)
            {
                var value = jObject.SelectToken(key)?.ToString();
                if (!string.IsNullOrEmpty(value))
                {
                    result.Add(value);
                }
            }
            return result;
        }
        /// <summary>
        ///  向json添加字段
        /// </summary>
        /// <param name="jo"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static void AddOrUpdateProperty(this JObject jo, string key, object value)
        {
            if (jo == null||value==null)
            {
                return;
            }
            var propJobPost = jo.Property(key);
            if (propJobPost == null)
            {
                propJobPost = new JProperty(key, value);
                jo.Add(propJobPost);
            }
            else
            {
                propJobPost.Value = JToken.FromObject(value);
            }
        }
        public static void AddOrUpdateJToken(this JObject jo, string key, JToken value)
        {
            if (jo == null ||value==null) return;
            var jProperty = jo.Property(key);
            if (jProperty == null)
            {
                jProperty = new JProperty(key, value);
                jo.Add(jProperty);
            }
            else
            {
                jProperty.Value = value;
            }
        }
        public static void RemoveFromPath(this JObject jObject, string path)
        {
            if (jObject == null) return;
            var curJTokenArray = jObject.SelectTokens(path);
            foreach (var curJToken in curJTokenArray)
            {
                curJToken.Parent?.Remove();
            }
        }
        /// <summary>
        /// 新增或更新json某一个路径下的值
        /// 目前只支持新增通过.分路径的设置
        /// </summary>
        /// <param name="jObject"></param>
        /// <param name="path"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static void AddOrUpdatePathValue(this JObject jObject, string path, object value)
        {
            //数据为空
            if (jObject == null ||value==null) return;
            var curJtoken = jObject.SelectToken(path);
            if (curJtoken != null)
            {
                //更新
                curJtoken.Replace(JToken.FromObject(value));
                return;
            }
            //新增
            var keys = path.Split('.');
            if (keys.Length == 1)
            {
                var newItemJT = new JProperty(path, value);
                jObject.Add(newItemJT);
                return;
            }
            //最后节点的数据
            var lastKey = keys[keys.Length - 1];
            var lastItemJT = new JProperty(lastKey, value);
            var beforePath = path;//当前key之前的path
            var afterPath = lastKey;//当前key之后的path(包含当前key)
            JObject tempItemJt = null;
            for (var i = keys.Length - 1; i >= 0; i--)
            {
                if (i != keys.Length - 1)
                {
                    afterPath = keys[i] + "." + afterPath;
                }
                if (i == 0)
                {
                    jObject.Add(keys[i], tempItemJt);
                    break;
                }
                beforePath = path.Substring(0, path.Length - afterPath.Length - 1);
                var lastParentItemJt = jObject.SelectToken(beforePath);
                if (lastParentItemJt == null && tempItemJt == null)
                {
                    tempItemJt = new JObject();
                    tempItemJt.Add(lastItemJT);
                    continue;
                }
                if (lastParentItemJt == null && tempItemJt != null)
                {
                    var curTempItemJt = new JObject();
                    curTempItemJt.Add(keys[i], tempItemJt);
                    tempItemJt = curTempItemJt;
                    continue;
                }

                if (lastParentItemJt.Type == JTokenType.Object && tempItemJt == null)
                {
                    var lastParentItemJObject = (JObject)lastParentItemJt;
                    lastParentItemJObject.Add(keys[i], JToken.FromObject(value));
                    break;

                }
                if (lastParentItemJt.Type == JTokenType.Object && tempItemJt!= null)
                {
                    var lastParentItemJObject = (JObject)lastParentItemJt;
                    lastParentItemJObject.Add(keys[i], tempItemJt);
                    break;

                }

            }

        }
    }
}