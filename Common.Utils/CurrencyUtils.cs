using System;

namespace Common.Utils
{
    /// <summary>
    /// 货币运行帮助
    /// </summary>
    public static class CurrencyUtils
    {
        private const int C_CURRENCYUnit = 100;
        private const int C_CURRENCDegree = 2;
        /// <summary>
        /// int转货币
        /// </summary>
        /// <param name="intRedeem">分为单位的int</param>
        /// <returns></returns>
        public static double ConvertToCurrency(this int intRedeem)
        {
            return Math.Round(intRedeem / (double)C_CURRENCYUnit, C_CURRENCDegree);
        }
        /// <summary>
        /// int转货币 字符串显示类型
        /// </summary>
        /// <param name="intRedeem"></param>
        /// <returns></returns>
        public static string ConvertToCurrencyString(this int intRedeem)
        {
            return ConvertToCurrency(intRedeem).ToFString();
        }
        public static double? ConvertToCurrency(this int? intRedeem)
        {
            if (intRedeem.HasValue)
            {
                return intRedeem.Value.ConvertToCurrency();
            }
            return null;
        }
        public static int ConvertCurrencyToInt(this double doubleRedeem)
        {
            return (int)((decimal)doubleRedeem * C_CURRENCYUnit);
        }
        public static int ConvertCurrencyToInt(this decimal decimalRedeem)
        {
            return (int)(decimalRedeem * C_CURRENCYUnit);
        }
        /// <summary>
        /// 四舍五入 处理
        /// </summary>
        /// <param name="value"></param>
        /// <param name="precision"></param>
        /// <returns></returns>
        public static double ConvertMathRound(this double value, int precision = 2)
        {
            return Math.Round(value, precision,MidpointRounding.AwayFromZero);
            // if (value < 0) 
            // { 
            //     return Math.Round(value + 5 / Math.Pow(10, precision + 1), precision, MidpointRounding.AwayFromZero); 
            // }
        }
        /// <summary>
        /// 四舍五入 处理
        /// </summary>
        /// <param name="value"></param>
        /// <param name="precision"></param>
        /// <returns></returns>
        public static decimal ConvertMathRound(this decimal value, int precision = 2)
        {
            return Math.Round(value, precision,MidpointRounding.AwayFromZero);
            // if (value < 0m)
            // {
            //     var temp = 5 / Math.Pow(10, precision + 1);
            //     return Math.Round(value + (decimal)temp, precision, MidpointRounding.AwayFromZero); 
            // } 
        }
        /// <summary>
        /// 转换成字符串 四舍五入 添加千分位分割
        /// </summary>
        /// <param name="number"></param>
        /// <param name="precision"></param>
        /// <returns></returns>
        public static string ToNString(this double number, int precision = 2)
        {
            return number.ToString("N" + precision);
        }
        
        /// <summary>
        ///  转换成字符串 四舍五入
        /// </summary>
        /// <param name="number"></param>
        /// <param name="precision"></param>
        /// <returns></returns>
        public static string ToFString(this double number, int precision = 2)
        {
            return number.ToString("F" + precision);
        }
        /// <summary>
        /// 转换成字符串 四舍五入 添加千分位分割
        /// </summary>
        /// <param name="number"></param>
        /// <param name="precision"></param>
        /// <returns></returns>
        public static string ToNString(this decimal number, int precision = 2)
        {
            return number.ToString("N" + precision);
        }
        /// <summary>
        /// 转换成字符串 四舍五入
        /// </summary>
        /// <param name="number"></param>
        /// <param name="precision"></param>
        /// <returns></returns>
        public static string ToFString(this decimal number, int precision = 2)
        {
            return number.ToString("F" + precision);
        }
        /// <summary>
        /// 转换成字符串 四舍五入 添加千分位分割
        /// </summary>
        /// <param name="number"></param>
        /// <param name="precision"></param>
        /// <returns></returns>
        public static string ToNString(this float number, int precision = 2)
        {
            return number.ToString("N" + precision);
        }
        /// <summary>
        /// 转换成字符串 四舍五入
        /// </summary>
        /// <param name="number"></param>
        /// <param name="precision"></param>
        /// <returns></returns>
        public static string ToFString(this float number, int precision = 2)
        {
            return number.ToString("F" + precision);
        }
    }
}