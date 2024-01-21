using System.Security.Cryptography;
using System.Text;

namespace WebApi.Implementation.Jwt
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
