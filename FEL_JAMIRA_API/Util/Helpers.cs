using System;
using System.Security.Cryptography;

namespace FEL_JAMIRA_API.Util
{
    public static class Helpers
    {

        public static string CriarSenha(string senha, string auxSenha)
        {
            return Encrypt(senha + auxSenha);
        }

        public static string GenerateRandomString(int length = 5, string allowableChars = null)
        {
            if (string.IsNullOrEmpty(allowableChars))
                allowableChars = @"+_)(*&%$ÇQWERTY<>:";

            // Generate random data
            var rnd = new byte[length];
            using (var rng = new RNGCryptoServiceProvider())
                rng.GetBytes(rnd);

            // Generate the output string
            var allowable = allowableChars.ToCharArray();
            var l = allowable.Length;
            var chars = new char[length];
            for (var i = 0; i < length; i++)
                chars[i] = allowable[rnd[i] % l];

            return new string(chars);
        }

        /// <summary>
        /// Encrypt string
        /// </summary>
        /// <param name="DecryptedStr"></param>
        /// <returns></returns>
        public static string Encrypt(string DecryptedStr)
        {
            Byte[] b = System.Text.ASCIIEncoding.ASCII.GetBytes(DecryptedStr);
            string EncryptedStr = Convert.ToBase64String(b);
            return EncryptedStr;
        }

        /// <summary>
        /// Decrypt string
        /// </summary>
        /// <param name="CryptedStr"></param>
        /// <returns></returns>
        public static string Decrypt(string CryptedStr)
        {
            Byte[] b = Convert.FromBase64String(CryptedStr);
            string DecryptedStr = System.Text.ASCIIEncoding.ASCII.GetString(b);
            return DecryptedStr;
        }
    }
}