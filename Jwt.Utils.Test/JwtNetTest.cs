using System.Security.Cryptography.X509Certificates;
using BouncyCastle.Cryptography.Utils;
using Common.Utils;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;

namespace Jwt.Utils.Test;

public class JwtNetTest
{
    private string GetFilePath(string keyFileName)
    {
        var runPath = System.AppDomain.CurrentDomain.BaseDirectory;
        var parentDirectory = Directory.GetParent(runPath)?.Parent.Parent.Parent.Parent.FullName;

        var filePath =
            $"{parentDirectory}{Path.DirectorySeparatorChar}source{Path.DirectorySeparatorChar}openssl{Path.DirectorySeparatorChar}{keyFileName}";
        return filePath;
    }
    
    [Fact]
    public void GeneratorTokenRsa256()
    {
        try
        {
            var pfxFilePath = GetFilePath("certificate.pfx");
            //new X509Certificate2(pfxFilePath) 只有linux 和windows 支持
            // mac 系统报错 强制校验密码 所以使用BouncyCastle
            //var x509Cert = new X509Certificate2(pfxFilePath);
            var rsaResult = RSAUtil.X509CertificateLoadForRsa(pfxFilePath);
            var header = new Dictionary<string, object>()
            {
                // { "alg", "RS256" },
                // { "typ", "JWT" },
                { "x5t", "xxxx" }
            };
            var payload = new Dictionary<string, object>()
            {
                { "iss", "www.test.com" },
                { "prn", "xxxx" },
                { "iat", DateTime.Now.DateTimeToTimeStamp() },
                { "exp", DateTime.Now.AddMinutes(5).DateTimeToTimeStamp() }
            };
           
            var algorithm = new RS256Algorithm(rsaResult.Item2,rsaResult.Item1);
            //创建
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);
            //key  not needed if algorithm is asymmetric
            var token = encoder.Encode(header,payload, null);

            var s = $"Bearer {token}";
                
            //验证
            IDateTimeProvider provider = new UtcDateTimeProvider();
            IJwtValidator validator = new JwtValidator(serializer, provider);
            IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, algorithm);
            var payloadResult = decoder.Decode(token);
                
            Assert.NotEmpty(payloadResult);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}