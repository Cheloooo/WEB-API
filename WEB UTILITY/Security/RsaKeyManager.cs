using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WEB_UTILITY.Security
{ //sealed means cannot be inherited, no other class can derive from this class
    public sealed class RsaKeyManager
    {
        private static readonly Lazy<RsaKeyManager> _instance =
            new (() => new RsaKeyManager());

        public static RsaKeyManager Instance => _instance.Value;
        public RSA Rsa { get; private set; }

        private RsaKeyManager()
        {
            Rsa = RSA.Create(2048);
        }

        public void LoadPublicKey(string base64PublicKey)
        {
            Rsa.ImportRSAPublicKey(Convert.FromBase64String(base64PublicKey), out _);
        }

        public void LoadPrivateKey(string base64PrivateKey)
        {
            Rsa.ImportRSAPrivateKey(Convert.FromBase64String(base64PrivateKey), out _);
        }

        internal byte[] Encrypt(byte[] data, RSAEncryptionPadding oaepSHA256)
        {
            throw new NotImplementedException();
        }

        public static implicit operator RsaKeyManager(RSA v)
        {
            throw new NotImplementedException();
        }
    }
}

/*There are three reason to seal a class
 * - prevent modifying behavior through inheritance
 * - slight performance improvements
 * - design decision */