using System.Text.RegularExpressions;

namespace Common.Utils
{
    public class StringValidateUtils
    {
        /// <summary>
        /// 验证邮箱正则表达式
        /// </summary>
        public const string EmailExpression = @"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";

        /// <summary>
        /// 验证手机正则表达式
        /// </summary>
        public const string MobileExpression = @"^(\+?[0-9]{2})?1[3|4|5|6|7|8|9|][0-9]{9}$";

        /// <summary>
        /// 验证身份证号码正则表达式
        /// </summary>
        public const string IdCardExpression = @"^(\d{15}$|^\d{18}$|^\d{17}(\d|X|x))$";
        /// <summary>
        /// 验证邮编正则表达式
        /// </summary>
        public const string ZipcodeExpression = @"\d{6}";
        /// <summary>
        /// 时间验证 YYYY/MM/DD| YY/MM/DD YY年MM月dd日
        /// </summary>
        public const string DateDayExpression = @"^(^(\d{4}|\d{2})(\-|\/|\.)\d{1,2}\3\d{1,2}$)|(^\d{4}年\d{1,2}月\d{1,2}日$)$";
        /// <summary>
        /// iPv4验证
        /// </summary>
        public const string Ipv4Expression =
            @"(?=(\b|\D))(((\d{1,2})|(1\d{1,2})|(2[0-4]\d)|(25[0-5]))\.){3}((\d{1,2})|(1\d{1,2})|(2[0-4]\d)|(25[0-5]))(?=(\b|\D))";
        /// <summary>
        /// 正整数
        /// </summary>
        public const string NumberExpression = @"^[0-9]\d*$";
        /// <summary>
        /// 正整数
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsPositiveInteger(string source)
        {
            return Regex.IsMatch(source, NumberExpression, RegexOptions.IgnoreCase);
        }
        /// <summary>
        /// 验证邮箱
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsEmail(string source)
        {
            return Regex.IsMatch(source, EmailExpression, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 验证手机号
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsMobile(string source)
        {
            return Regex.IsMatch(source, MobileExpression, RegexOptions.IgnoreCase);
        }
        /// <summary>
        /// 验证身份证
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsIdCard(string source)
        {
            return Regex.IsMatch(source, IdCardExpression, RegexOptions.IgnoreCase);
        }
        public static bool IsUserName(string source)
        {
            // Regex.IsMatch(source,)
            return false;
        }
        public static bool IsZipcode(string source)
        {
            return Regex.IsMatch(source, ZipcodeExpression, RegexOptions.IgnoreCase);
        }
        public static bool IsDateDay(string source)
        {
            return Regex.IsMatch(source, DateDayExpression, RegexOptions.IgnoreCase);
        }
        public static bool IsIpv4(string source)
        {
            return Regex.IsMatch(source, Ipv4Expression, RegexOptions.IgnoreCase);
        }
    }
}
