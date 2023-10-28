using System;
using System.Text;
using System.Web;

namespace Common.Utils
{
    public static class EncodeDecodeUtils
    {
        /// <summary>
        /// Url编码
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string UrlEncode(this string url)
        {
            return HttpUtility.UrlEncode(url);
        }
        /// <summary>
        /// Url解码
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string UrlDecode(this string url)
        {
            return HttpUtility.UrlDecode(url);
        }
        /// <summary>
        /// Html编码
        /// </summary>
        /// <param name="htmlString"></param>
        /// <returns></returns>
        public static string HtmlEncode(this string htmlString)
        {
            return  System.Web.HttpUtility.HtmlEncode(htmlString);
        }
        /// <summary>
        /// Html解码
        /// </summary>
        /// <param name="htmlString"></param>
        /// <returns></returns>
        public static string HtmlDecode(this string htmlString)
        {
           return  System.Web.HttpUtility.HtmlDecode(htmlString);
        }

        /// <summary>
        /// Base64 编码 Encoding
        /// </summary>
        /// <param name="str"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string ToBase64StringEncode(this string str, string encoding = "utf-8")
        {
            if (string.IsNullOrEmpty(str))
                return str;
            var bytes = Encoding.GetEncoding(encoding).GetBytes(str);
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// Base64 解码
        /// </summary>
        /// <param name="str"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string ToBase64StringDecode(this string str, string encoding = "utf-8")
        {
            if (string.IsNullOrEmpty(str))
                return str;
            byte[] bytes = Convert.FromBase64String(str);
            return Encoding.GetEncoding(encoding).GetString(bytes);
        }
    }
}