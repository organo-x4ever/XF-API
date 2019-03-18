using System;
using System.Data.Entity.Validation;
using System.Threading.Tasks;

namespace Organo.Solutions.X4Ever.V1.DAL.Helper
{
    public interface IHelper
    {
        string GetAppSetting(string key);

        dynamic GetAppSetting(string key, Type returnType);

        string ComputeHash(string plainText);

        bool VerifyPassword(string password, string hashedPassword);

        string GetUniqueCode();

        bool IsEmailAddressValid(string email);

        /// <summary>
        /// Convert input string to SHA-512
        /// </summary>
        /// <param name="plainText">plain text string</param>
        /// <returns>Returns SHA-512 Encrypted string</returns>
        string ConvertToSHA512(string plainText);

        /// <summary>
        /// Compare encrypted text to plain text with SHA-512 encryption
        /// </summary>
        /// <param name="encryptedText">Encrypted text</param>
        /// <param name="plainText">Plain text</param>
        /// <returns>If plain text matches with encrypted text then it returns TRUE else FALSE.</returns>
        bool CompareSHA512(string encryptedText, string plainText);

        bool CompareSHA512_WrongEncryption(string encryptedText, string plainText);

        /// <summary>
        /// Compute hash for string encoded as UTF8
        /// </summary>
        /// <param name="s">String to be hashed</param>
        /// <returns>40-character hex string</returns>
        string SHA1HashStringForUTF8String(string s);

        /// <summary>
        /// Compare encrypted bytes to plain bytes with SHA-1 encryption
        /// </summary>
        /// <param name="encryptedText">Encrypted bytes</param>
        /// <param name="plainText">Plain bytes</param>
        /// <returns>If plain bytes matches with encrypted bytes then it returns TRUE else FALSE.</returns>
        bool CompareSHA1(string encryptedText, string plainText);

        void SaveLog(Exception ex, string className, string method = "");
        Task SaveLogAsync(Exception ex, string className, string method = "");

        void SaveLog(DbEntityValidationResult eve);

        Task SaveLogAsync(DbEntityValidationResult eve);
    }
}