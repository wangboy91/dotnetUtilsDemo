using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Operators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;

namespace BouncyCastle.Cryptography.Utils
{
 /// <summary>
    /// KEY 
    /// </summary>
    public class RsaKey
    {
        /// <summary>
        /// 公钥
        /// </summary>
        public string PublicPemBase64String { get; set; }

        /// <summary>
        /// 私钥
        /// </summary>
        public string PrivateKeyBase64String { get; set; }
    }

    /// <summary>
    /// Rsa非对称加密算法
    /// RSA加密过程是使用接收者的公钥加密数据，然后使用接收者的私钥解密数据。这是非对称加密的基本原理
    /// https://github.com/bcgit/bc-csharp
    /// </summary>
    public static class RSAUtil
    {
        public static System.Security.Cryptography.RSA LoadCertificatePrivateKey(string keyBase64)
        {
            try
            {
                keyBase64 = keyBase64.Replace("\r", "").Replace("\n", "").Replace(" ", "");
                byte[] privateInfoByte = Convert.FromBase64String(keyBase64);
                AsymmetricKeyParameter priKey = PrivateKeyFactory.CreateKey(privateInfoByte);
                // 创建 RSA 参数
                RsaPrivateCrtKeyParameters privateKeyParams = (RsaPrivateCrtKeyParameters) priKey;

                // 创建 RSA 对象
                var rsa = System.Security.Cryptography.RSA.Create();
                RSAParameters rsaParams = new RSAParameters
                {
                    Modulus = privateKeyParams.Modulus.ToByteArrayUnsigned(),
                    Exponent = privateKeyParams.PublicExponent.ToByteArrayUnsigned(),
                    D = privateKeyParams.Exponent.ToByteArrayUnsigned(),
                    P = privateKeyParams.P.ToByteArrayUnsigned(),
                    Q = privateKeyParams.Q.ToByteArrayUnsigned(),
                    DP = privateKeyParams.DP.ToByteArrayUnsigned(),
                    DQ = privateKeyParams.DQ.ToByteArrayUnsigned(),
                    InverseQ = privateKeyParams.QInv.ToByteArrayUnsigned()
                };
                rsa.ImportParameters(rsaParams);
                return rsa;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static System.Security.Cryptography.RSA LoadCertificatePublicCer(string keyBase64)
        {
            try
            {
                byte[] certBytes = Convert.FromBase64String(keyBase64);
                Org.BouncyCastle.X509.X509Certificate cert = new Org.BouncyCastle.X509.X509Certificate(certBytes);
                // 从 X.509 证书中提取公钥
                AsymmetricKeyParameter publicKey = cert.GetPublicKey();
                RsaKeyParameters rsaKey = (RsaKeyParameters) publicKey;

                // 创建 RSA 对象
                var rsa = System.Security.Cryptography.RSA.Create();
                RSAParameters rsaParameters = new RSAParameters
                {
                    Modulus = rsaKey.Modulus.ToByteArrayUnsigned(),
                    Exponent = rsaKey.Exponent.ToByteArrayUnsigned()
                };
                rsa.ImportParameters(rsaParameters);
                return rsa;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static System.Security.Cryptography.RSA LoadCertificatePublicPem(string keyBase64)
        {
            try
            {
                keyBase64 = keyBase64.Replace("\r", "").Replace("\n", "").Replace(" ", "");
                byte[] publicInfoByte = Convert.FromBase64String(keyBase64);
                //Asn1Object pubKeyObj = Asn1Object.FromByteArray(publicInfoByte); //这里也可以从流中读取，从本地导入
                AsymmetricKeyParameter pubKey = PublicKeyFactory.CreateKey(publicInfoByte);

                RsaKeyParameters rsaKey = (RsaKeyParameters) pubKey;

                // 创建 RSA 对象
                var rsa = System.Security.Cryptography.RSA.Create();
                RSAParameters rsaParameters = new RSAParameters
                {
                    Modulus = rsaKey.Modulus.ToByteArrayUnsigned(),
                    Exponent = rsaKey.Exponent.ToByteArrayUnsigned()
                };
                rsa.ImportParameters(rsaParameters);
                return rsa;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static Encoding Encoding_UTF8 = Encoding.UTF8;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static (AsymmetricKeyParameter, AsymmetricKeyParameter) Generator()
        {
            //RSA密钥构造器的参数
            RsaKeyPairGenerator generator = new RsaKeyPairGenerator();
            generator.Init(new KeyGenerationParameters(new SecureRandom(), 2048));
            AsymmetricCipherKeyPair keyPair = generator.GenerateKeyPair();
            //获取公钥和密钥
            AsymmetricKeyParameter privateKey = keyPair.Private;
            AsymmetricKeyParameter publicKey = keyPair.Public;
            return (privateKey, publicKey);
        }

        public static RsaKey GeneratorPrivateAndPublicKeyString()
        {
            var keyPair = Generator();
            //获取公钥和密钥
            AsymmetricKeyParameter privateKey = keyPair.Item1;
            AsymmetricKeyParameter publicKey = keyPair.Item2;

            PrivateKeyInfo privateKeyInfo = PrivateKeyInfoFactory.CreatePrivateKeyInfo(privateKey);
            Asn1Object asn1ObjectPrivate = privateKeyInfo.ToAsn1Object();
            byte[] privateInfoByte = asn1ObjectPrivate.GetEncoded("UTF-8");

            SubjectPublicKeyInfo subjectPublicKeyInfo =
                SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(publicKey);
            Asn1Object asn1ObjectPublic = subjectPublicKeyInfo.ToAsn1Object();
            byte[] publicInfoByte = asn1ObjectPublic.GetEncoded("UTF-8");

            RsaKey item = new RsaKey()
            {
                PublicPemBase64String = Convert.ToBase64String(publicInfoByte).Replace("\r", "").Replace("\n", "")
                    .Replace(" ", ""),
                PrivateKeyBase64String = Convert.ToBase64String(privateInfoByte).Replace("\r", "").Replace("\n", "")
                    .Replace(" ", "")
            };
            return item;
        }

        public static (AsymmetricKeyParameter, System.Security.Cryptography.X509Certificates.X509Certificate2)
            GenerateRsaKeyPairAndCert()
        {
            var keyPair = Generator();
            //获取公钥和密钥
            AsymmetricKeyParameter privateKey = keyPair.Item1;
            AsymmetricKeyParameter publicKey = keyPair.Item2;

            // Create X.509 certificate
            var subjectDN = new X509Name("CN=test, O=www.test.cn");
            var issuerDN = subjectDN; // Self-signed certificate
            var serialNumber = new Org.BouncyCastle.Math.BigInteger(32, new SecureRandom());
            var notBefore = DateTime.UtcNow;
            var notAfter = notBefore.AddYears(1); // 过期时间

            var certificateGenerator = new X509V3CertificateGenerator();
            certificateGenerator.SetSerialNumber(serialNumber);
            certificateGenerator.SetSubjectDN(subjectDN);
            certificateGenerator.SetIssuerDN(issuerDN);
            certificateGenerator.SetNotBefore(notBefore);
            certificateGenerator.SetNotAfter(notAfter);

            certificateGenerator.SetPublicKey(publicKey);
            var signatureFactory = new Asn1SignatureFactory("SHA256WITHRSA", privateKey);
            var x509Certificate = certificateGenerator.Generate(signatureFactory);

            // Convert BouncyCastle certificate to X.509 certificate
            var bcCertBytes = x509Certificate.GetEncoded();
            var x509Cert = new System.Security.Cryptography.X509Certificates.X509Certificate2(bcCertBytes);

            return (privateKey, x509Cert);
        }

        public static void SaveFileByAsymmetricKeyParameter(AsymmetricKeyParameter key, string keyFilePath)
        {
            using (TextWriter textWriter = new StreamWriter(keyFilePath))
            {
                var pemWriter = new Org.BouncyCastle.OpenSsl.PemWriter(textWriter);
                pemWriter.WriteObject(key);
                pemWriter.Writer.Flush();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="privateKeyPath"></param>
        /// <returns></returns>
        public static AsymmetricKeyParameter LoadPrivateKeyFromFile(string privateKeyPath)
        {
            using (var streamReader = System.IO.File.OpenText(privateKeyPath))
            {
                var pemReader = new  Org.BouncyCastle.OpenSsl.PemReader(streamReader);
                AsymmetricCipherKeyPair keyPair = (AsymmetricCipherKeyPair) pemReader.ReadObject();
                return keyPair.Private;
            }
        }

        public static AsymmetricKeyParameter LoadPublicKeyFromPemFile(string publicKeyPath)
        {
            using (var streamReader = System.IO.File.OpenText(publicKeyPath))
            {
                var pemReader = new Org.BouncyCastle.OpenSsl.PemReader(streamReader);
                var publicKey = (SubjectPublicKeyInfo) pemReader.ReadObject();
                return PublicKeyFactory.CreateKey(publicKey);
            }
        }

        public static void SaveFileByX509Certificate2(
            System.Security.Cryptography.X509Certificates.X509Certificate2 certificate, string keyFilePath)
        {
            // 将 X509Certificate2 导出为 PEM 格式
            string pemString = "-----BEGIN CERTIFICATE-----\n";
            pemString += Convert.ToBase64String(certificate.Export(X509ContentType.Cert),
                Base64FormattingOptions.InsertLineBreaks);
            pemString += "\n-----END CERTIFICATE-----";
            File.WriteAllText(keyFilePath, pemString);
        }

        public static (System.Security.Cryptography.RSA, System.Security.Cryptography.RSA) X509CertificateLoadForRsa(string cerFilePath, string password = "")
        {
            Pkcs12Store pkcs = new Pkcs12StoreBuilder().Build();
            using (Stream stream = File.OpenRead(cerFilePath))
            {
                pkcs.Load(stream, password.ToCharArray());
            }

            var keyAlias = pkcs.Aliases.Cast<string>().SingleOrDefault(a => pkcs.IsKeyEntry(a));

            var pirKey = (RsaPrivateCrtKeyParameters) pkcs.GetKey(keyAlias).Key;
            var bouncyCertificate = pkcs.GetCertificate(keyAlias).Certificate;

            var publicCer = DotNetUtilities.ToX509Certificate(bouncyCertificate);
            var publicCertificate = new X509Certificate2(publicCer);

            var publicKeyRsa = publicCertificate.GetRSAPublicKey();
            
            var pirParameters = DotNetUtilities.ToRSAParameters(pirKey);
            RSACryptoServiceProvider privateRsa = new RSACryptoServiceProvider();
            privateRsa.ImportParameters(pirParameters);
            
            
            return (privateRsa,publicKeyRsa);
        }


        /// <summary>
        /// 私钥加密（公钥解密数据）
        /// </summary>
        /// <param name="plaintext">明文</param>
        /// <param name="keyBase64">私钥</param>
        /// <returns>返回Base64内容</returns>
        public static string EncryptWithPrivateKey(string plaintext, string keyBase64)
        {
            try
            {
                keyBase64 = keyBase64.Replace("\r", "").Replace("\n", "").Replace(" ", "");
                byte[] privateInfoByte = Convert.FromBase64String(keyBase64);
                AsymmetricKeyParameter priKey = PrivateKeyFactory.CreateKey(privateInfoByte);

                IAsymmetricBlockCipher engine = new Pkcs1Encoding(new RsaEngine());
                engine.Init(true, priKey);
                byte[] byteData = Encoding_UTF8.GetBytes(plaintext);
                var encryptedData = engine.ProcessBlock(byteData, 0, byteData.Length);
                var result = Convert.ToBase64String(encryptedData);
                Console.WriteLine($"Encrypted: {result}");
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 私钥解密数据（公钥加密数据）
        /// </summary>
        /// <param name="encryptedString">秘文</param>
        /// <param name="keyBase64">私钥</param>
        /// <returns></returns>
        public static string DecryptWithPrivateKey(string encryptedString, string keyBase64)
        {
            try
            {
                keyBase64 = keyBase64.Replace("\r", "").Replace("\n", "").Replace(" ", "");
                byte[] privateInfoByte = Convert.FromBase64String(keyBase64);
                AsymmetricKeyParameter priKey = PrivateKeyFactory.CreateKey(privateInfoByte);
                var engine = new Pkcs1Encoding(new RsaEngine());
                engine.Init(false, priKey);
                byte[] encryptedBytes = Convert.FromBase64String(encryptedString);
                byte[] decryptedBytes = engine.ProcessBlock(encryptedBytes, 0, encryptedBytes.Length);
                return Encoding.UTF8.GetString(decryptedBytes);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 公钥解密（私钥加密数据）
        /// </summary>
        /// <param name="encryptedString">待解密的内容</param>
        /// <param name="keyBase64">公钥</param>
        /// <returns>返回明文</returns>
        public static string DecryptWithPublicPem(string encryptedString, string keyBase64)
        {
            try
            {
                keyBase64 = keyBase64.Replace("\r", "").Replace("\n", "").Replace(" ", "");
                byte[] publicInfoByte = Convert.FromBase64String(keyBase64);
                //Asn1Object pubKeyObj = Asn1Object.FromByteArray(publicInfoByte); //这里也可以从流中读取，从本地导入
                AsymmetricKeyParameter pubKey = PublicKeyFactory.CreateKey(publicInfoByte);

                IAsymmetricBlockCipher engine = new Pkcs1Encoding(new RsaEngine());
                engine.Init(false, pubKey);
                byte[] encryptedDataBytes = Convert.FromBase64String(encryptedString);
                byte[] decryptedData = engine.ProcessBlock(encryptedDataBytes, 0, encryptedDataBytes.Length);
                string plaintext = System.Text.Encoding.UTF8.GetString(decryptedData);
                return plaintext;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 公钥加密数据(私钥解密数据)
        /// </summary>
        /// <param name="plaintext">明文</param>
        /// <param name="keyBase64"></param>
        /// <returns></returns>
        public static string EncryptWithPublicPem(string plaintext, string keyBase64)
        {
            try
            {
                keyBase64 = keyBase64.Replace("\r", "").Replace("\n", "").Replace(" ", "");
                byte[] publicInfoByte = Convert.FromBase64String(keyBase64);
                AsymmetricKeyParameter pubKey = PublicKeyFactory.CreateKey(publicInfoByte);

                var engine = new Pkcs1Encoding(new RsaEngine());
                engine.Init(true, pubKey);
                byte[] dataBytes = Encoding.UTF8.GetBytes(plaintext);
                byte[] encryptedBytes = engine.ProcessBlock(dataBytes, 0, dataBytes.Length);
                return Convert.ToBase64String(encryptedBytes);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 公钥解密
        /// </summary>
        /// <param name="plaintext">待解密的内容</param>
        /// <param name="cerText">公钥</param>
        /// <returns>返回明文</returns>
        public static string EncryptWithCer(string plaintext, string cerText)
        {
            try
            {
                byte[] certBytes = Convert.FromBase64String(cerText);
                Org.BouncyCastle.X509.X509Certificate cert = new Org.BouncyCastle.X509.X509Certificate(certBytes);
                // 获取接收者的公钥
                AsymmetricKeyParameter publicKey = cert.GetPublicKey();
                var engine = new Pkcs1Encoding(new RsaEngine());
                engine.Init(false, publicKey);
                byte[] data = Encoding.UTF8.GetBytes(plaintext);
                byte[] encryptedBytes = engine.ProcessBlock(data, 0, data.Length);
                return Convert.ToBase64String(encryptedBytes);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// cer证书解密
        /// </summary>
        /// <param name="encryptedString">待解密的内容</param>
        /// <param name="cerText">公钥</param>
        /// <returns>返回明文</returns>
        public static string DecryptWithCer(string encryptedString, string cerText)
        {
            try
            {
                byte[] certBytes = Convert.FromBase64String(cerText);
                Org.BouncyCastle.X509.X509Certificate cert = new Org.BouncyCastle.X509.X509Certificate(certBytes);
                // 从 X.509 证书中提取公钥
                AsymmetricKeyParameter publicKey = cert.GetPublicKey();
                IAsymmetricBlockCipher engine = new Pkcs1Encoding(new RsaEngine());
                engine.Init(false, publicKey);
                byte[] encryptedDataBytes = Convert.FromBase64String(encryptedString);
                byte[] decryptedData = engine.ProcessBlock(encryptedDataBytes, 0, encryptedDataBytes.Length);
                string plaintext = System.Text.Encoding.UTF8.GetString(decryptedData);
                return plaintext;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}