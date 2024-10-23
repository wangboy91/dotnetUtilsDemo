using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using BouncyCastle.Cryptography.Utils;
using Common.Utils;
using Jose;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;

namespace Jwt.Utils.Test;

public class JoseJwtTest
{
    private string GetFilePath(string keyFileName)
    {
        var runPath = System.AppDomain.CurrentDomain.BaseDirectory;
        var parentDirectory = Directory.GetParent(runPath)?.Parent.Parent.Parent.Parent.FullName;

        var filePath =
            $"{parentDirectory}{Path.DirectorySeparatorChar}source{Path.DirectorySeparatorChar}openssl{Path.DirectorySeparatorChar}{keyFileName}";
        return filePath;
    }
    public static X509Certificate2 X509CertificateLoad(string cerFilePath, string password = "")
    {
        Pkcs12Store pkcs = new Pkcs12StoreBuilder().Build();
        using (Stream stream = File.OpenRead(cerFilePath))
        {
            pkcs.Load(stream, password.ToCharArray());
        }

        var keyAlias = pkcs.Aliases.Cast<string>().SingleOrDefault(a => pkcs.IsKeyEntry(a));

        var key = (RsaPrivateCrtKeyParameters) pkcs.GetKey(keyAlias).Key;
        var bouncyCertificate = pkcs.GetCertificate(keyAlias).Certificate;

        var cer1 = DotNetUtilities.ToX509Certificate(bouncyCertificate);

        var certificate = new X509Certificate2(cer1);
        
        var parameters = DotNetUtilities.ToRSAParameters(key);
        RSACryptoServiceProvider pirRsa = new RSACryptoServiceProvider();
        pirRsa.ImportParameters(parameters);

        certificate = RSACertificateExtensions.CopyWithPrivateKey(certificate, pirRsa);
        
        return certificate;
    }
    [Fact]
    public void GeneratorTokenRsa256()
    {
        try
        {
            var pfxFilePath = GetFilePath("certificate.pfx");
            //这里如果直接使用 new X509Certificate2(pfxFilePath) 只有linux 和windows 支持
            //mac 系统可能会强制校验密码 所以使用BouncyCastle来进行封装X509CertificateLoad
            //var x509Cert = new X509Certificate2(pfxFilePath);
            var x509Cert = X509CertificateLoad(pfxFilePath);
            var header = new Dictionary<string, object>()
            {
                // { "alg", "RS256" },
                // { "typ", "JWT" },
                {"x5t", "xxxx"}
            };
            var payload = new Dictionary<string, object>()
            {
                {"iss", "www.test.com"},
                {"prn", "test"},
                {"iat", DateTime.Now.DateTimeToTimeStamp()},
                {"exp", DateTime.Now.AddMinutes(5).DateTimeToTimeStamp()}
            };
            // RS256, RS384, RS512 and PS256, PS384, PS512 使用私钥

            //创建
            string token = Jose.JWT.Encode(payload, x509Cert.GetRSAPrivateKey(), JwsAlgorithm.RS256, header);
            //验证
            var payloadResult = Jose.JWT.Decode(token, x509Cert.GetRSAPublicKey());

            Assert.NotEmpty(payloadResult);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}