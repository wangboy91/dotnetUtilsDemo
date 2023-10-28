using System;
using System.IO;

namespace BouncyCastle.Cryptography.Utils.Test;

    public class RSAUtilTest
    {
        [Fact]
        public void GeneratorTest()
        {
            var pairAndCert = RSAUtil.Generator();
            RSAUtil.SaveFileByAsymmetricKeyParameter(pairAndCert.Item1, "private.Key");
            RSAUtil.SaveFileByAsymmetricKeyParameter(pairAndCert.Item1, "public.pem");
        }

        [Fact]
        public void GenerateRsaKeyPairAndCertTest()
        {
            var pairAndCert = RSAUtil.GenerateRsaKeyPairAndCert();
            RSAUtil.SaveFileByAsymmetricKeyParameter(pairAndCert.Item1, "private.Key");
            RSAUtil.SaveFileByX509Certificate2(pairAndCert.Item2, "public.cer");
        }

        [Fact]
        public void GeneratorPrivateAndPublicKeyStringTest()
        {
            var key = RSAUtil.GeneratorPrivateAndPublicKeyString();

            Console.WriteLine(key.PrivateKeyBase64String);
            Console.WriteLine(key.PublicPemBase64String);

            File.WriteAllText("tempPrivateKey.txt", key.PrivateKeyBase64String);
            File.WriteAllText("tempPublicPem.txt", key.PublicPemBase64String);

            var encryptString1 = RSAUtil.EncryptWithPublicPem("test", key.PublicPemBase64String);
            var decryptString1 = RSAUtil.DecryptWithPrivateKey(encryptString1, key.PrivateKeyBase64String);
        }

        [Fact]
        public void EncryptDecryptTest()
        {
            var privateKey =
                "MIIEvgIBADANBgkqhkiG9w0BAQEFAASCBKgwggSkAgEAAoIBAQCbXCIASqIoroXYMMSurvXyN86HBMPsRlVEk7H0AWsc1eHtBMA7x7whq+ITOhqDpAzJlOolv93nLyZtaKM097ZiAbyhY8AQr+8hJxOdJpKGMW7o2wezgcD1OEEzkTjIE1IGFfeY57BaTz/5H27yymEPnoDLJ8V+3AscTRCrcF72nPZ9qie5aFjQMQznRIYGHRi94OyhBCRCgP9yZB61Ij2xFPLEh5AYMIP7GsJPXR3QK/kRAfVsp2WoWkQbrj9t4/ei7lS7oLoN0S8/mw+sPa68KxhHXS135wtPZ5RxWpj7L+RGN4IgB2PoI+xbAO86VeflAXYuXzC1of9gASM5mDV/AgMBAAECggEAFtYizqrUeJO5a4nfn1MV0UDlbsYDNpvatMsxATj5fFzuv176Es936/XOtBzPq5dVpIaV1G6w45FJXhO0yc26VWBZUIUlGk7XdlFpef7NfZmOlVPSJ09krvxR4+PuK3ALl9wVb7hpcnJBE7whxuEmBfG6j3eNJD9GstDxKQIfiLMB3hg9JW92+BjPta1/VUR9dTxrEMd6eqnmGa+/KnXPT8au/onNwDDzMOKHSYflUMGnpWIYrqkpSHacRdbCPMni8pl2aZkkMPt6EsrnBVW8e4p+ZfJtvbDLJrxHuHyuQXMvEa282QFuXtjGBQtwlyYXRusaUf4q4gR/EG9jzhX64QKBgQDMChEXas1B4C7R33KgVA8KjA7Xi8fj6+eHmMCAHjUUWIev8zgk0TB7AUFGMuyAeGdxouDTvxldevPlvB4I8MGNVdrE//nC+viUnt57V65ymW7aMHEZpSno+4MP1GJPZWOG2OBQIoh0TMAC9wbf7ksHLjhfAlt2cuW3suenc16kdQKBgQDC7IIxyB+Gj8ehXUlZTyrcS5DOwMR5QcFPOFYtQMyogavfOEGFlgyYN2K13VIjS798phoYsYJ7gKI65LWFccrweJyxoagWO7Vov9scB9P2DTg0cK+sW2fTXHJuTkN5Jlw1C+pHh/n5ahdIlbiAZlVBzsfse2foUWvIslllu0OjowKBgQCA6UNPJj00h61ND/3d6f3uzfp6mGfcSqlndEz7JRqoFh70PZiVOk2FCY/yCC6AJJJGq3+ciLo/43TBCw922pWO9FVZ7dghJmtzisRJ7WXcJbBxzfVHHKDUAEQX9jl+rDIrek6OkqLfx7XwqZ4AUQ+6I6ud62FZ/tHANBpEI5ICGQKBgQC+gErVwFIfPpHKANPYtuiamDeT+Q2LvFnixJpka7oYzXJeHCrdFcKFWUvdlcnauQMSHXvPulxdu1/R0wgcvROrZRUu/WC7KeunzimkAXqENHNDvQXTmxmjlew4JcEHvBfNuDFla11BBxBuXfUNd/XG9QKq+giZIkoGwVdhd10ppQKBgG73VnUAvns3bj68FW2/dV01MAHwseYgt6KfgGmCYROEJt8PzeVgdiiM/78Nq9ZdPEzDFlKoi4EerSO48fMwWEL1fJXPHRy4r6MN4ZblN6yJZeLww1B/0NrPi0P5hdStr24pn+7Rjozxbar35EaH/xdODyG1U/8IMo1PH77UvJBY";
            
            var publicKey =
                "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAm1wiAEqiKK6F2DDErq718jfOhwTD7EZVRJOx9AFrHNXh7QTAO8e8IaviEzoag6QMyZTqJb/d5y8mbWijNPe2YgG8oWPAEK/vIScTnSaShjFu6NsHs4HA9ThBM5E4yBNSBhX3mOewWk8/+R9u8sphD56AyyfFftwLHE0Qq3Be9pz2faonuWhY0DEM50SGBh0YveDsoQQkQoD/cmQetSI9sRTyxIeQGDCD+xrCT10d0Cv5EQH1bKdlqFpEG64/beP3ou5Uu6C6DdEvP5sPrD2uvCsYR10td+cLT2eUcVqY+y/kRjeCIAdj6CPsWwDvOlXn5QF2Ll8wtaH/YAEjOZg1fwIDAQAB";
            
            var encryptString1 = RSAUtil.EncryptWithPublicPem("test", publicKey);
            var decryptString1 = RSAUtil.DecryptWithPrivateKey(encryptString1, privateKey);
        }

        private string GetFilePath(string keyFileName)
        {
            var runPath = System.AppDomain.CurrentDomain.BaseDirectory;
            var parentDirectory = Directory.GetParent(runPath)?.Parent.Parent.Parent.Parent.FullName;
            
            var filePath =
                $"{parentDirectory}{Path.DirectorySeparatorChar}source{Path.DirectorySeparatorChar}openssl{Path.DirectorySeparatorChar}{keyFileName}";
            return filePath;
        }

        [Fact]
        public void EncryptByPublicKeyAndDecryptByPrivateKeyTest()
        {
            var plaintext = "test";
            var publicPemFilePath = GetFilePath("publickey.pem");
            var publicPemContent = File.ReadAllText(publicPemFilePath);
            publicPemContent = publicPemContent
                .Replace("-----BEGIN PUBLIC KEY-----", "")
                .Replace("-----END PUBLIC KEY-----", "").Replace(" ", "").Replace("\n", "")
                .Replace("\r", "")
                .Replace(Environment.NewLine, "");

            var encryptString = RSAUtil.EncryptWithPublicPem(plaintext, publicPemContent);

            var privateKeyFilePath = GetFilePath("private.key");
            var privateKeyContent = File.ReadAllText(privateKeyFilePath);
            privateKeyContent = privateKeyContent
                .Replace("-----BEGIN PRIVATE KEY-----", "")
                .Replace("-----END PRIVATE KEY-----", "")
                .Replace(" ", "").Replace("\n", "")
                .Replace("\r", "")
                .Replace(Environment.NewLine, "");

            var plaintext2 = RSAUtil.DecryptWithPrivateKey(encryptString, privateKeyContent);

            Assert.True(plaintext == plaintext2);
        }

        [Fact]
        public void EncryptByPrivateKeyAndDecryptByCerTest()
        {
            var plaintext = "test";

            var privateKeyFilePath = GetFilePath("private.key");
            var privateKeyContent = File.ReadAllText(privateKeyFilePath);
            privateKeyContent = privateKeyContent
                .Replace("-----BEGIN PRIVATE KEY-----", "")
                .Replace("-----END PRIVATE KEY-----", "")
                .Replace(" ", "").Replace("\n", "")
                .Replace("\r", "")
                .Replace(Environment.NewLine, "");
            var encryptString = RSAUtil.EncryptWithPrivateKey(plaintext, privateKeyContent);

            var publicCerPath = GetFilePath("publickey.cer");
            var publicCerContent = File.ReadAllText(publicCerPath);
            publicCerContent = publicCerContent
                .Replace("-----BEGIN CERTIFICATE-----", "")
                .Replace("-----END CERTIFICATE-----", "").Replace(" ", "").Replace("\n", "")
                .Replace("\r", "")
                .Replace(Environment.NewLine, "");

            var plaintext2 = RSAUtil.DecryptWithCer(encryptString, publicCerContent);

            Assert.True(plaintext == plaintext2);
        }

        [Fact]
        public void EncryptByPrivateKeyAndDecryptByPublicKeyTest()
        {
            
            var privateKeyFilePath = GetFilePath("private.key");
            var privateKeyContent = File.ReadAllText(privateKeyFilePath);
            privateKeyContent = privateKeyContent
                .Replace("-----BEGIN PRIVATE KEY-----", "")
                .Replace("-----END PRIVATE KEY-----", "")
                .Replace(" ", "").Replace("\n", "")
                .Replace("\r", "")
                .Replace(Environment.NewLine, "");
            var encryptString = RSAUtil.EncryptWithPrivateKey("test", privateKeyContent);

            var publicPemFilePath = GetFilePath("publickey.pem");
            var publicPemContent = File.ReadAllText(publicPemFilePath);
            publicPemContent = publicPemContent
                .Replace("-----BEGIN PUBLIC KEY-----", "")
                .Replace("-----END PUBLIC KEY-----", "").Replace(" ", "").Replace("\n", "")
                .Replace("\r", "")
                .Replace(Environment.NewLine, "");

            var plaintext1 = RSAUtil.DecryptWithPublicPem(encryptString, publicPemContent);

            var publicCerPath =
                $"{System.AppDomain.CurrentDomain.BaseDirectory}openssl{Path.DirectorySeparatorChar}publickey.cer";
            var publicCerContent = File.ReadAllText(publicCerPath);
            publicCerContent = publicCerContent
                .Replace("-----BEGIN CERTIFICATE-----", "")
                .Replace("-----END CERTIFICATE-----", "").Replace(" ", "").Replace("\n", "")
                .Replace("\r", "")
                .Replace(Environment.NewLine, "");

            var plaintext2 = RSAUtil.DecryptWithCer(encryptString, publicCerContent);

            var decryptString2 = RSAUtil.EncryptWithCer(plaintext2, publicCerContent);
            var plaintext3 = RSAUtil.DecryptWithCer(decryptString2, publicCerContent);
        }
    }