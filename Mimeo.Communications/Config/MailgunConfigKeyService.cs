using System.Security;
using Mimeo.Blocks.Security;

namespace Mimeo.Communications.Config
{
    public class MailgunConfigKeyService
    {
        public const string EnvironmentVariableName = "MIMEO_SETTING";

        public void SetApiKey(SecureString apiKey)
        {
            var encryptedData = apiKey.ToInsecureString().DpApiEncryptData();
            var base64Data = Convert.ToBase64String(encryptedData);
            Environment
                .SetEnvironmentVariable(
                    EnvironmentVariableName,
                    base64Data,
                    EnvironmentVariableTarget.Machine);
        }

        public SecureString GetApiKey()
        {
            var base64Data 
                = Environment.GetEnvironmentVariable(
                    EnvironmentVariableName, EnvironmentVariableTarget.Machine);

            var buffer = Convert.FromBase64String(base64Data);
            return buffer.DpApiDecryptData().ToSecureString();
        }
    }
}
