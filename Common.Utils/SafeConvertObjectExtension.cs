using System;

namespace Common.Utils
{
    public static class SafeConvertObjectExtension
    {
        #region Long
        /// <summary>
        /// 多租赁的 object 转int
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defualtValue"></param>
        /// <returns></returns>
        public static long SafeToLong(this object value, long defualtValue = 0L)
        {
            if (value == null)
                return defualtValue;

            if (long.TryParse(value.ToString(), out var i))
                return i;
            else
                return defualtValue;
        }
        /// <summary>
        /// string 转int
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defualtValue"></param>
        /// <returns></returns>
        public static long SafeToLong(this string value, long defualtValue = 0L)
        {
            if (value == null)
                return defualtValue;
            if (long.TryParse(value, out var i))
                return i;
            else
                return defualtValue;
        }
        

        #endregion
        
        #region Int
        /// <summary>
        /// 多租赁的 object 转int
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defualtValue"></param>
        /// <returns></returns>
        public static int SafeToInt(this object value, int defualtValue = 0)
        {
            if (value == null)
                return defualtValue;

            if (int.TryParse(value.ToString(), out var i))
                return i;
            else
                return defualtValue;
        }
        /// <summary>
        /// string 转int
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defualtValue"></param>
        /// <returns></returns>
        public static int SafeToInt(this string value, int defualtValue = 0)
        {
            if (value == null)
                return defualtValue;
            var i = 0;
            if (int.TryParse(value, out i))
                return i;
            else
                return defualtValue;
        }
        /// <summary>
        /// 多租赁的 object 转int
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defualtValue"></param>
        /// <returns></returns>
        public static int? SafeToIntNullable(this object value, int? defualtValue = null)
        {
            if (value == null)
                return defualtValue;

            if (int.TryParse(value.ToString(), out var i))
                return i;
            else
                return defualtValue;
        }
        /// <summary>
        ///  string 转int
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defualtValue"></param>
        /// <returns></returns>
        public static int? SafeToIntNullable(this string value, int? defualtValue=null)
        {
            if (value == null)
                return defualtValue;
            var i = 0;
            if (int.TryParse(value, out i))
                return i;
            else
                return defualtValue;
        }

        #endregion

        #region Boolean
        public static bool SafeIsBooleanType(this object value)
        {
            if (value == null)
                return false;
            string lower = value.ToString().ToLower();
            return lower == "0" || lower == "1" || lower == "false" || lower == "true";
        }
        /// <summary>
        /// 转换成bool
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static bool SafeToBoolean(this object value, bool defaultValue=false)
        {
            if (value == null)
                return defaultValue;
            if (value.ToString() == "1" || value.ToString() == "0")
            {
                return value.ToString() != "0";
            }
            else
            {
                var b = false;
                bool.TryParse(value.ToString(), out b);
                return b;
            }
        }
        #endregion

        #region String
        /// <summary>
        /// 转成字符串
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defualtValue"></param>
        /// <returns></returns>
        public static string SafeToString(this object value, string defualtValue="")
        {
            if (value == null)
                return defualtValue;
            return value.ToString();
        }
        #endregion

        #region Float
        /// <summary>
        /// 转成float
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defualtValue"></param>
        /// <returns></returns>
        public static float SafeToFloat(this object value, float defualtValue = 0.0f)
        {
            if (value == null)
                return defualtValue;

            float i = 0;
            if (float.TryParse(value.ToString(), out i))
                return i;
            else
                return defualtValue;
        }
        /// <summary>
        /// 转成float
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defualtValue"></param>
        /// <returns></returns>
        public static float SafeToFloat(this string value, float defualtValue = 0.0f)
        {
            if (value == null)
                return defualtValue;
            float i = 0;
            if (float.TryParse(value, out i))
                return i;
            else
                return defualtValue;
        }
        /// <summary>
        /// 转成float
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defualtValue"></param>
        /// <returns></returns>
        public static float? SafeToFloatNullable(this object value, float? defualtValue=null)
        {
            if (value == null)
                return defualtValue;

            float i = 0;
            if (float.TryParse(value.ToString(), out i))
                return i;
            else
                return defualtValue;
        }
        /// <summary>
        /// 转成float
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defualtValue"></param>
        /// <returns></returns>
        public static float? SafeToFloatNullable(this string value, float? defualtValue=null)
        {
            if (value == null)
                return defualtValue;
            float i = 0;
            if (float.TryParse(value, out i))
                return i;
            else
                return defualtValue;
        }

        #endregion

        #region Double
        /// <summary>
        ///  转换成 double
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defualtValue"></param>
        /// <returns></returns>
        public static double SafeToDouble(this object value, double defualtValue = 0.0d)
        {
            if (value == null)
                return defualtValue;

            double i = 0;
            if (double.TryParse(value.ToString(), out i))
                return i;
            else
                return defualtValue;
        }
        /// <summary>
        ///  转换成 double
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defualtValue"></param>
        /// <returns></returns>
        public static double SafeToDouble(this string value, double defualtValue = 0.0d)
        {
            if (value == null)
                return defualtValue;
            double i = 0;
            if (double.TryParse(value, out i))
                return i;
            else
                return defualtValue;
        }
        /// <summary>
        /// 转换成 double
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defualtValue"></param>
        /// <returns></returns>
        public static double? SafeToDoubleNullable(this object value, double? defualtValue=null)
        {
            if (value == null)
                return defualtValue;

            double i = 0;
            if (double.TryParse(value.ToString(), out i))
                return i;
            else
                return defualtValue;
        }

        /// <summary>
        ///  转换成 double
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defualtValue"></param>
        /// <returns></returns>
        public static double? SafeToDoubleNullable(this string value, double? defualtValue=null)
        {
            if (value == null)
                return defualtValue;
            double i = 0;
            if (double.TryParse(value, out i))
                return i;
            else
                return defualtValue;
        }


        #endregion

        #region decimal
        /// <summary>
        /// 转换成 decimal
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defualtValue"></param>
        /// <returns></returns>
        public static decimal SafeToDecimal(this string value, decimal defualtValue = 0.0m)
        {
            if (value == null)
                return defualtValue;
            decimal i;
            if (decimal.TryParse(value, out i))
                return i;
            else
                return defualtValue;
        }
        /// <summary>
        /// 转换成 decimal
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defualtValue"></param>
        /// <returns></returns>
        public static decimal SafeToDecimal(this object value, decimal defualtValue = 0.0m)
        {
            if (value == null)
                return defualtValue;
            decimal i;
            if (decimal.TryParse(value.ToString(), out i))
                return i;
            else
                return defualtValue;
        }
        /// <summary>
        /// 保留小数位数 按四色五入方式
        /// </summary>
        /// <param name="value"></param>
        /// <param name="currencyDegree"></param>
        /// <returns></returns>
        public static decimal SafeToCurrency(this decimal value, int currencyDegree = 2)
        {
            return Math.Round(value, currencyDegree, MidpointRounding.AwayFromZero);
        }
        /// <summary>
        ///  转换成 decimal
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defualtValue"></param>
        /// <returns></returns>
        public static decimal? SafeToDecimalNullable(this string value, decimal? defualtValue=null)
        {
            if (value == null)
                return defualtValue;
            decimal i;
            if (decimal.TryParse(value, out i))
                return i;
            else
                return defualtValue;
        }

        /// <summary>
        /// 转换成 decimal
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defualtValue"></param>
        /// <returns></returns>
        public static decimal? SafeToDecimalNullable(this object value, decimal? defualtValue=null)
        {
            if (value == null)
                return defualtValue;
            decimal i;
            if (decimal.TryParse(value.ToString(), out i))
                return i;
            else
                return defualtValue;
        }


        #endregion

        #region Guid
        /// <summary>
        /// 转换成 Guid
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Guid SafeToGuid(this object value)
        {
            if (value == null)
                return default(Guid);

            Guid i;
            if (Guid.TryParse(value.ToString(), out i))
                return i;
            else
                return default(Guid);
        }
        public static Guid? SafeToGuidNullable(this object value)
        {
            if (value == null)
                return null;

            if (Guid.TryParse(value.ToString(), out var i))
                return i;
            else
                return null;
        }
        #endregion

        #region DateTime
        /// <summary>
        /// 多租赁的 object 转ToDateTime
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defualtValue"></param>
        /// <returns></returns>
        public static DateTime? SafeToDateTimeNullable(this object value, DateTime? defualtValue = null)
        {
            if (value == null)
                return defualtValue;

            DateTime i;
            if (DateTime.TryParse(value.ToString(), out i))
                return i;
            else
                return defualtValue;
        }
        /// <summary>
        /// 多租赁的 object 转ToDateTime
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defualtValue"></param>
        /// <returns></returns>
        public static DateTime SafeToDateTime(this object value)
        {
            if (value == null)
                return DateTime.MinValue;

            DateTime i;
            if (DateTime.TryParse(value.ToString(), out i))
                return i;
            else
                return DateTime.MinValue;
        }
        #endregion
    }

}
