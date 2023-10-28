#### oppenssl 说明

1 安装oppenssl 

生成私钥
openssl genrsa -out private.key 2048


生成一个X.509公钥证书文件

openssl req -new -x509 -key private.key -out publickey.cer -days 365

合并正式pfx文件（C# windows使用）
openssl pkcs12 -export -out certificate.pfx -inkey private.key -in publickey.cer

生成一个rsa公钥
openssl rsa -pubout -in private.key -out publickey.pem



输出文本查看
openssl x509 -in publickey.cer -text -noout

sha1 fingerprint 输出文本查看

openssl x509 -sha1 -in publickey.cer -noout -fingerprint

检索指纹后，您需要转换为 base64  echo 是windows不支持
echo "xx:xx:xx"|xxd -r -p | base64







