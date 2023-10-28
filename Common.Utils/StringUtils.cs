using System;
using System.Text;

namespace Common.Utils
{
    public static class StringUtils
    {
        #region 转换字符串
        
        public static byte[] GetBytes(this string data, string encoding = "utf-8")
        {
            return Encoding.GetEncoding(encoding).GetBytes(data);
        }

        public static string FromBytesToString(this byte[] data, string encoding = "utf-8")
        {
            return Encoding.GetEncoding(encoding).GetString(data);
        }
        /// <summary>
        /// 字符串转换UTF8字节数组
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] GetUtf8Bytes(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return default(byte[]);
            return Encoding.UTF8.GetBytes(str);
        }
        /// <summary>
        /// UTF8字节数组 转string
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ToUtf8BytesString(this byte[] bytes)
        {
            if (bytes==null||bytes.Length==0)
                return default(string);
            return Encoding.UTF8.GetString(bytes);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string ToHexString(this byte[] data)
        {
            StringBuilder result = new StringBuilder();
            if (data != null)
            {
                foreach (byte t in data)
                {
                    result.Append(t.ToString("X2"));
                }
            }

            return result.ToString();
        }
        #endregion

        
        #region 生成随机字符串

        /// <summary>
        /// 生成随机数字字符串
        /// </summary>
        /// <param name="codeCount"></param>
        /// <returns></returns>
        public static string GenerateRandomNumber(int codeCount)
        {
            var rep = 0;
            string str = string.Empty;
            long num2 = DateTime.Now.Ticks;
            rep++;
            Random random = new Random(((int)(((ulong)num2) & 0xffffffffL)) | ((int)(num2 >> rep)));
            for (int i = 0; i < codeCount; i++)
            {
                int num = random.Next();
                str = str + ((char)(0x30 + ((ushort)(num % 10)))).ToString();
            }

            return str;
        }

        public static string GenerateRandomCodeCase(int length = 6)
        {
            char[] constant =
            {
                '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
                'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u',
                'v', 'w', 'x', 'y', 'z',
                'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U',
                'V', 'W', 'X', 'Y', 'Z'
            };
            System.Text.StringBuilder newRandom = new System.Text.StringBuilder(62);
            Random rd = new Random();
            for (int i = 0; i < length; i++)
            {
                newRandom.Append(constant[rd.Next(62)]);
            }

            return newRandom.ToString();
        }

        public static string GenerateRandomCode(int length = 6)
        {
            char[] constant =
            {
                '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
                'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u',
                'v', 'w', 'x', 'y', 'z'
            };
            System.Text.StringBuilder newRandom = new System.Text.StringBuilder(36);
            Random rd = new Random();
            for (int i = 0; i < length; i++)
            {
                newRandom.Append(constant[rd.Next(36)]);
            }

            return newRandom.ToString();
        }

        #endregion
    }
}