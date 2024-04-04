using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Mimeo.Blocks.Security
{
    public static class DataProtectionExtensions
    {
        public static byte[] DpApiEncryptData(this string dataToEncrypt)
        {
            byte[] data = Encoding.Unicode.GetBytes(dataToEncrypt);
            byte[] encryptedData = ProtectedData.Protect(data, null, DataProtectionScope.CurrentUser);
            return encryptedData;
        }

        public static string DpApiDecryptData(this byte[] dataToDecrypt)
        {
            byte[] decryptedData = ProtectedData.Unprotect(dataToDecrypt, null, DataProtectionScope.CurrentUser);
            return Encoding.Unicode.GetString(decryptedData);
        }
    }
}
