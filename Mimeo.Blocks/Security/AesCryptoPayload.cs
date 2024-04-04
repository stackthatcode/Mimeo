using Mimeo.Blocks.Helpers;

namespace Mimeo.Blocks.Security
{
    public class AesCryptoPayload
    {
        public string EncryptedMessage { get; set; }
        public string InitializationVector { get; set; }

        public const string CipherTextPrefix = "CIPHER_";

        public AesCryptoPayload()
        {
        }

        public AesCryptoPayload(string serializedPayload)
        {
            var output = serializedPayload
                .Replace(CipherTextPrefix, "")
                .SplitBy(":");

            EncryptedMessage = output[0];
            InitializationVector = output[1];
        }


        public static bool HasSerializedCipher(string input)
        {
            return input.StartsWith(CipherTextPrefix);
        }

        public string Serialize()
        {
            return $"{CipherTextPrefix}{EncryptedMessage}:{InitializationVector}";
        }

    }
}
