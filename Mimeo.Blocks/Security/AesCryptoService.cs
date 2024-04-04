using System;
using System.IO;
using System.Security;
using System.Security.Cryptography;
using System.Text;


namespace Mimeo.Blocks.Security
{
    // *** Important - preserving the exact succession of "using" statements is critical;
    //  ... things will break if you do not. Investigation of this is pending...
    //
    public class AesCryptoService
    {
        public byte[] GenerateKey()
        {
            using (var aes = new AesManaged())
            {
                aes.Padding = PaddingMode.PKCS7;
                aes.GenerateKey();
                return aes.Key;
            }
        }

        public AesCryptoPayload Encrypt(SecureString cipherKey, string input)
        {
            using (Aes aes = new AesManaged())
            {
                aes.Padding = PaddingMode.PKCS7;
                var encryptionKey = Convert.FromBase64String(cipherKey.ToInsecureString());
                
                aes.Key = encryptionKey;
                aes.GenerateIV();

                byte[] cipher = null;

                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        byte[] rawPlaintext = System.Text.Encoding.Unicode.GetBytes(input);
                        cs.Write(rawPlaintext, 0, rawPlaintext.Length);
                    }

                    cipher = ms.ToArray();
                }

                var encryptedText = Convert.ToBase64String(cipher);
                var initializationVector = Convert.ToBase64String(aes.IV);

                return new AesCryptoPayload
                {
                    EncryptedMessage = encryptedText,
                    InitializationVector = initializationVector,
                };
            }
        }

        public SecureString Decrypt(SecureString cipherKey, AesCryptoPayload input)
        {
            byte[] cipherText = Convert.FromBase64String(input.EncryptedMessage);
            byte[] plainText = null;

            using (Aes aes = new AesManaged())
            {
                aes.Padding = PaddingMode.PKCS7;
                var encryptionKey = Convert.FromBase64String(cipherKey.ToInsecureString()); 
                aes.Key = encryptionKey;

                var initializationVector = Convert.FromBase64String(input.InitializationVector);
                aes.IV = initializationVector;

                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherText, 0, cipherText.Length);
                    }

                    plainText = ms.ToArray();
                }

                var s = Encoding.Unicode.GetString(plainText);
                return s.ToSecureString();
            }
        }
    }
}
