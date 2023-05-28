using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FitLog.Implementation.Jwt
{
    public class TokenCryptor
    {
        public virtual Guid CreateCryptographicallySecureGuid()
        {
            var bytes = new byte[16];

            using var random = RandomNumberGenerator.Create();
            random.GetBytes(bytes);

            return new Guid(bytes);
        }

        public virtual string GetSha256Hash(string input)
        {
            using (var hashAlgorithm = new SHA256CryptoServiceProvider())
            {
                var bytes = Encoding.UTF8.GetBytes(input);
                var bytesHash = hashAlgorithm.ComputeHash(bytes);

                return Convert.ToBase64String(bytesHash);
            }
        }
    }
}
