using HBS.ImplementationAutomationConsole.Core.Models;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Cryptography;
using System.Text;

namespace HBS.ImplementationAutomationConsole.API.Security
{
    public static class Cryptography
    {
        public static AsymetricData Initiate()
        {
            //Generate asymetric public and private keys
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048))
            {
                return new AsymetricData()
                {
                    RSAPrivateKeyXML = rsa.ToXmlString(true),
                    RSAPublicKeyXML = rsa.ToXmlString(false),
                    RSAPublicKeyPEM = RsaUtil.ExportPublicKeyToPemString(rsa),
                    RSAPublicKeyPEMEncoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(RsaUtil.ExportPublicKeyToPemString(rsa)))
                };
            }
        }

        public static LoginDto Decrypt(IMemoryCache memoryCache, LoginDto loginDto)
        {
            var loginPassword = "";
            var loginUserID = "";
            //Generate asymetric public and private keys
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048))
            {
                if (memoryCache.Get("rsaPrivate") != null)
                {
                    string rsaPrivateKeyModulus = (string)memoryCache.Get("rsaPrivate");
                    rsa.FromXmlString(rsaPrivateKeyModulus);

                    var base64ToStringPassword = Convert.FromBase64String(loginDto.Password);
                    var dataPassword = rsa.Decrypt(base64ToStringPassword, false);
                    loginPassword = Encoding.UTF8.GetString(dataPassword);

                    var base64ToStringUser = Convert.FromBase64String(loginDto.UserID);
                    var dataUser = rsa.Decrypt(base64ToStringUser, false);
                    loginUserID = Encoding.UTF8.GetString(dataUser);
                }

                return new LoginDto()
                {
                    UserID = loginUserID,
                    Password = loginPassword,
                };
            }
        }

        public static LoginDto DecryptAfterAuth(IMemoryCache memoryCache, LoginDto loginDto)
        {
            var loginPassword = "";
            var loginUserID = "";
            //Generate asymetric public and private keys
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048))
            {
                if (memoryCache.Get("rsaPrivate_" + loginDto.UserID.Substring(0, 8)) != null)
                {
                    string rsaPrivateKeyModulus = (string)memoryCache.Get("rsaPrivate_" + loginDto.UserID.Substring(0, 8));
                    rsa.FromXmlString(rsaPrivateKeyModulus);

                    var base64ToStringPassword = Convert.FromBase64String(loginDto.Password);
                    var dataPassword = rsa.Decrypt(base64ToStringPassword, false);
                    loginPassword = Encoding.UTF8.GetString(dataPassword);

                    var base64ToStringUser = Convert.FromBase64String(loginDto.UserID);
                    var dataUser = rsa.Decrypt(base64ToStringUser, false);
                    loginUserID = Encoding.UTF8.GetString(dataUser);
                }

                return new LoginDto()
                {
                    UserID = loginUserID,
                    Password = loginPassword,
                };
            }
        }
    }
}