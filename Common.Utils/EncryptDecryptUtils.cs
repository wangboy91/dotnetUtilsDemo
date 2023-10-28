using System;
using System.Security.Cryptography;
using System.Text;

namespace Common.Utils
{
    public static class EncryptDecryptUtils
    {
        /// <summary>
        /// Md5加密
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string MD5Encrypt(this string source)
        {
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            byte[] hashedDataBytes = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(source));
            StringBuilder tmp = new StringBuilder();
            foreach (byte i in hashedDataBytes)
            {
                tmp.Append(i.ToString("x2"));
            }
            return tmp.ToString();
        }
        /// <summary>
        /// SHA1 加密
        /// </summary>
        /// <param name="source"></param>
        /// <param name="isReplace"></param>
        /// <param name="isToLower"></param>
        /// <returns></returns>
        public static string SHA1Encrypt(this string source, bool isReplace = true, bool isToLower = false)
        {
            SHA1 sha1 = SHA1.Create();
            byte[] hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(source));
            string shaStr = BitConverter.ToString(hash);
            if (isReplace)
            {
                shaStr = shaStr.Replace("-", "");
            }
            if (isToLower)
            {
                shaStr = shaStr.ToLower();
            }
            return shaStr;
        }
        /// <summary>
        /// AES 加密 appSecret必须要求16位字符串长度
        /// </summary>
        /// <param name="plaintext"></param>
        /// <param name="appSecret"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static string AESEncrypt(this string plaintext, string appSecret)
        {
            if (string.IsNullOrWhiteSpace(appSecret))
            {
                throw new ArgumentException("appSecret is null");
            }
            // 判断密钥是否为16位
            if (appSecret.Length != 16)
            {
                throw new ArgumentException("appSecret长度不是16位");
            }
            byte[] raw = Encoding.UTF8.GetBytes(appSecret);
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = raw;
                aesAlg.Mode = CipherMode.ECB;
                aesAlg.Padding = PaddingMode.PKCS7;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                byte[] encryptedBytes = null;

                using (var msEncrypt = new System.IO.MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        byte[] plainBytes = Encoding.UTF8.GetBytes(plaintext);
                        csEncrypt.Write(plainBytes, 0, plainBytes.Length);
                        csEncrypt.FlushFinalBlock();
                        encryptedBytes = msEncrypt.ToArray();
                    }
                }

                string str = Convert.ToBase64String(encryptedBytes);
                return str.Replace(Environment.NewLine, "");
            }
        }
        /// <summary>
        /// AES 解密 appSecret必须要求16位字符串长度
        /// </summary>
        /// <param name="ciphertext"></param>
        /// <param name="appSecret"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static string AESDecrypt(this string ciphertext, string appSecret)
        {
            if (string.IsNullOrWhiteSpace(appSecret))
            {
                throw new ArgumentException("appSecret is null");
            }
            // 判断密钥是否为16位
            if (appSecret.Length != 16)
            {
                throw new ArgumentException("appSecret长度不是16位");
            }
            byte[] raw = Encoding.UTF8.GetBytes(appSecret);
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = raw;
                aesAlg.Mode = CipherMode.ECB;
                aesAlg.Padding = PaddingMode.PKCS7;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor();

                byte[] cipherBytes = Convert.FromBase64String(ciphertext);
                byte[] decryptedBytes = null;

                using (var msDecrypt = new System.IO.MemoryStream(cipherBytes))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new System.IO.StreamReader(csDecrypt))
                        {
                            string decryptedString = srDecrypt.ReadToEnd();
                            decryptedBytes = Encoding.UTF8.GetBytes(decryptedString);
                        }
                    }
                }

                return Encoding.UTF8.GetString(decryptedBytes);
            }
        }
        
    }
}