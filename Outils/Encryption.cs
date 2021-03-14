using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
//using System.Threading.Tasks;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

//using Suly.Logging;

namespace Outils
{
    /// <summary>
    /// Encrypts and decrypts data.
    /// </summary>
    public class Encryption 
    {
        #region Constants

        private const int IterationEncryption = 3;
        //private const string ConnectionStringsSectionName = "connectionStrings";
        //private const string EntityFrameworkLowerString = "entityframework";

        #endregion

        #region Members

        private static readonly TraceSource ts = new TraceSource("Core");

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Encrypt the app.config section.
        /// </summary>
        /// <param name="pSectionName">The section name.</param>
        /// <returns>A value indicating if the section is protected.</returns>
        public static bool ProtectSection(string pSectionName)
        {
            // Open the app.config file.
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            // Get the section in the file.
            ConfigurationSection section = config.GetSection(pSectionName);
            // If the section exists and the section is not readonly, then protect the section.
            if (section != null)
            {
                if (!section.IsReadOnly() && !section.SectionInformation.IsProtected)
                {
                    // Protect the section.
                    section.SectionInformation.ProtectSection("DataProtectionConfigurationProvider");
                    section.SectionInformation.ForceSave = true;
                    // Save the change.
                    config.Save(ConfigurationSaveMode.Modified);
                    ConfigurationManager.RefreshSection(pSectionName);
                    return true;
                }

                return section.SectionInformation.IsProtected;
            }

            return false;
        }

        /// <summary>
   
        #endregion

        #region Private Methods

        /// <summary>
        /// Checks if a string is base64 encoded.
        /// </summary>
        /// <param name="base64String">The base64 encoded string.</param>
        /// <returns>A value indicating if the string is base64 encoded.</returns>
        public static bool IsBase64String(string pBase64String)
        {
            if (pBase64String == null) throw new ArgumentNullException("pBase64String");
            pBase64String = pBase64String.Trim();

            return (pBase64String.Length % 4 == 0) &&
                   Regex.IsMatch(pBase64String, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);
        }

        #region Rijndael Encryption

        /// <summary>
        /// Encrypt the given text and give the byte array back as a BASE64 string.
        /// </summary>
        /// <param name="pText">The text to encrypt.</param>
        /// <param name="pSalt">The password salt.</param>
        /// <param name="pKey">The application key.</param>
        /// <returns>The encrypted text.</returns>
        public static string EncryptRijndael(string pText, string pSalt, string pKey, int pIteration )
        {
            if (string.IsNullOrEmpty(pText)) throw new ArgumentNullException("pText");
            if (pSalt == null) throw new ArgumentNullException("pSalt");
            if (pKey == null) throw new ArgumentNullException("pKey");

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
        /// Decrypts the given text.
        /// </summary>
        /// <param name="pCipherText">The encrypted BASE64 text.</param>
        /// <param name="pSalt">The salt.</param>
        /// <param name="pKey">The key.</param>
        /// <param name="pIteration">The number of iteration.</param>
        /// <returns>The decrypted text.</returns>
        public static string DecryptRijndael(string pCipherText, string pSalt, string pKey, int pIteration)
        {
            if (string.IsNullOrEmpty(pCipherText)) throw new ArgumentNullException("pCipherText");
            if (pSalt == null) throw new ArgumentNullException("pSalt");
            if (pKey == null) throw new ArgumentNullException("pKey");
            if (!IsBase64String(pCipherText)) throw new ArgumentException("The cipherText input parameter is not base64 encoded");

            string text;

            var aesAlg = NewRijndaelManaged(pSalt, pKey);

            var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
            var cipher = Convert.FromBase64String(pCipherText);

            try
            {
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
            }
            catch (ArgumentNullException ex)
            {
                return null;
            }

            return text;
        }
        #endregion

        #region NewRijndaelManaged

        /// <summary>
        /// Create a new RijndaelManaged class and initialize it.
        /// </summary>
        /// <param name="pSalt">The salt.</param>
        /// <param name="pKey">The key.</param>
        /// <returns>The Rijndael Managed instance.</returns>
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

        #endregion

        #endregion
    }
}

