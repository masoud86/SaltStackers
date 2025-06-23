using System.Security.Cryptography;
using System.Text;

namespace SaltStackers.Common.Helper
{
    public static class SecurityHelper
    {
        private const string initVector = "u4cqxnyhwj9p7doz";

        public static string Encrypt(this string text, string key)
        {
            var iv = Encoding.UTF8.GetBytes(initVector);
            var keyByte = Encoding.UTF8.GetBytes(key);
            using var aesAlg = Aes.Create();
            aesAlg.Padding = PaddingMode.Zeros;
            using var encryptor = aesAlg.CreateEncryptor(keyByte, iv);
            using var msEncrypt = new MemoryStream();
            using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
            using (var swEncrypt = new StreamWriter(csEncrypt))
            {
                swEncrypt.Write(text);
            }
            var decryptedContent = msEncrypt.ToArray();
            var result = new byte[iv.Length + decryptedContent.Length];
            Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
            Buffer.BlockCopy(decryptedContent, 0, result, iv.Length, decryptedContent.Length);
            return Convert.ToBase64String(result);
        }

        public static string Decrypt(this string cipherText, string key)
        {
            var fullCipher = Convert.FromBase64String(cipherText);
            var iv = Encoding.UTF8.GetBytes(initVector);
            var cipher = new byte[16];
            Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, iv.Length);
            var keyByte = Encoding.UTF8.GetBytes(key);
            using var aesAlg = Aes.Create();
            aesAlg.Padding = PaddingMode.Zeros;
            using var decryptor = aesAlg.CreateDecryptor(keyByte, iv);
            string result;
            using (var msDecrypt = new MemoryStream(cipher))
            {
                using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
                using var srDecrypt = new StreamReader(csDecrypt);
                result = srDecrypt.ReadToEnd();
            }
            return result.Replace("\0", string.Empty);
        }
    }
}
