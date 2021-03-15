using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Tools.Security
{
    public static class Encryption
    {
        private const int IterationEncryption = 3;

        #region Rijndael Encryption

        /// <summary>
        /// Encrypt the given text and give the byte array back as a BASE64 string
        /// </summary>
        /// <param name="pText">The text to encrypt</param>
        /// <param name="pSalt">The password salt</param>
        /// <param name="pKey">The application key</param>
        /// <returns>The encrypted text</returns>
        public static string EncryptRijndael(string pText, string pSalt, string pKey, int pIteration)
        {
            if (string.IsNullOrEmpty(pText))
                throw new ArgumentNullException("pText");

            var aesAlg = NewRijndaelManaged(pSalt, pKey);

            var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
            var msEncrypt = new MemoryStream();
            using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
            using (var swEncrypt = new StreamWriter(csEncrypt))
            {
                swEncrypt.Write(pText);
            }

            string encrypted = Convert.ToBase64String(msEncrypt.ToArray());
            if (pIteration < IterationEncryption)
            {
                encrypted = EncryptRijndael(encrypted, pSalt, pKey, pIteration + 1);
            }

            return encrypted;
        }
        #endregion

        #region Rijndael Dycryption
        /// <summary>
        /// Checks if a string is base64 encoded
        /// </summary>
        /// <param name="base64String" />The base64 encoded string
        /// <returns>Base64 encoded stringt</returns>
        public static bool IsBase64String(string base64String)
        {
            base64String = base64String.Trim();
            return (base64String.Length % 4 == 0) &&
                   Regex.IsMatch(base64String, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);

        }

        /// <summary>
        /// Decrypts the given text
        /// </summary>
        /// <param name="cipherText" />The encrypted BASE64 text
        /// <param name="salt" />The pasword salt
        /// <returns>The decrypted text</returns>
        public static string DecryptRijndael(string pCipherText, string pSalt, string pKey, int pIteration)
        {
            if (string.IsNullOrEmpty(pCipherText))
                throw new ArgumentNullException("pCipherText");

            if (!IsBase64String(pCipherText))
                throw new Exception("The cipherText input parameter is not base64 encoded");

            string text;

            var aesAlg = NewRijndaelManaged(pSalt, pKey);

            var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
            var cipher = Convert.FromBase64String(pCipherText);

            using (var msDecrypt = new MemoryStream(cipher))
            {
                using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (var srDecrypt = new StreamReader(csDecrypt))
                    {
                        text = srDecrypt.ReadToEnd();
                    }
                }
            }

            if (pIteration < IterationEncryption)
            {
                text = DecryptRijndael(text, pSalt, pKey, pIteration + 1);
            }

            return text;
        }
        #endregion

        #region NewRijndaelManaged
        /// <summary>
        /// Create a new RijndaelManaged class and initialize it
        /// </summary>
        /// <param name="salt" />The pasword salt
        /// <returns></returns>
        private static RijndaelManaged NewRijndaelManaged(string pSalt, string pKey)
        {
            if (pSalt == null) throw new ArgumentNullException("pSalt");
            var saltBytes = Encoding.UTF8.GetBytes(pSalt);
            var key = new Rfc2898DeriveBytes(pKey, saltBytes);

            var aesAlg = new RijndaelManaged();
            aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
            aesAlg.IV = key.GetBytes(aesAlg.BlockSize / 8);

            return aesAlg;
        }
        #endregion
    }
}
