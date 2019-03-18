using CryptSharp;
using System;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.Entity.Validation;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Organo.Solutions.X4Ever.V1.DAL.Helper
{
    public partial class Helper : IHelper
    {
        private static string itoa64 = "./0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        private AppSettingsReader settingsReader;

        public Helper()
        {
            settingsReader = new AppSettingsReader();
        }

        public string ComputeHash(string plainText)
        {
            string expected = "$P$B55D6LjfHDkINU5wF.v2BuuzO0/XPk/";
            return MD5Encode(plainText, expected);
        }

        private static string MD5Encode(string password, string hash)
        {
            string output = "*0";
            if (hash == null)
            {
                return output;
            }

            if (hash.StartsWith(output))
                output = "*1";

            string id = hash.Substring(0, 3);
            // We use "$P$", phpBB3 uses "$H$" for the same thing
            if (id != "$P$" && id != "$H$")
                return output;

            // get who many times will generate the hash
            int count_log2 = itoa64.IndexOf(hash[3]);
            if (count_log2 < 7 || count_log2 > 30)
                return output;

            int count = 1 << count_log2;

            string salt = hash.Substring(4, 8);
            if (salt.Length != 8)
                return output;

            byte[] hashBytes = { };
            using (MD5 md5Hash = MD5.Create())
            {
                hashBytes = md5Hash.ComputeHash(Encoding.ASCII.GetBytes(salt + password));
                byte[] passBytes = Encoding.ASCII.GetBytes(password);
                do
                {
                    hashBytes = md5Hash.ComputeHash(hashBytes.Concat(passBytes).ToArray());
                } while (--count > 0);
            }

            output = hash.Substring(0, 12);
            string newHash = Encode64(hashBytes, 16);

            return output + newHash;
        }

        private static string Encode64(byte[] input, int count)
        {
            StringBuilder sb = new StringBuilder();
            int i = 0;
            do
            {
                int value = (int) input[i++];
                sb.Append(itoa64[value & 0x3f]); // to uppercase
                if (i < count)
                    value = value | ((int) input[i] << 8);
                sb.Append(itoa64[(value >> 6) & 0x3f]);
                if (i++ >= count)
                    break;
                if (i < count)
                    value = value | ((int) input[i] << 16);
                sb.Append(itoa64[(value >> 12) & 0x3f]);
                if (i++ >= count)
                    break;
                sb.Append(itoa64[(value >> 18) & 0x3f]);
            } while (i < count);

            return sb.ToString();
        }

        public dynamic GetAppSetting(string key, Type returnType)
        {
            try
            {
                return (dynamic) settingsReader.GetValue(key, returnType);
            }
            catch
            {
                return null;
            }
        }

        public string GetAppSetting(string key)
        {
            try
            {
                return (string) settingsReader.GetValue(key, typeof(String));
            }
            catch
            {
                return null;
            }
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            try
            {
                return Crypter.CheckPassword(password, hashedPassword);
            }
            catch
            {
                return false;
            }
        }

        public string GetUniqueCode()
        {
            var code = Guid.NewGuid().ToString();
            var codes = code.Split('-');
            var codeCombined = "";
            foreach (var c in codes)
                codeCombined += c.ToUpper();

            return codeCombined;
        }

        public bool IsValidEmail(string email)
        {
            try
            {
                if (new EmailAddressAttribute().IsValid(email))
                {
                    var mailAddress = new MailAddress(email);
                    return mailAddress.Address == email;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        private bool invalid = false;

        public bool IsEmailAddressValid(string strIn)
        {
            invalid = false;
            if (String.IsNullOrEmpty(strIn))
                return false;

            // Use IdnMapping class to convert Unicode domain names.
            try
            {
                strIn = Regex.Replace(strIn, @"(@)(.+)$", this.DomainMapper,
                    RegexOptions.None, TimeSpan.FromMilliseconds(200));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }

            if (invalid)
                return false;

            // Return true if strIn is in valid e-mail format.
            try
            {
                return Regex.IsMatch(strIn,
                    @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        private string DomainMapper(Match match)
        {
            // IdnMapping class with default property values.
            IdnMapping idn = new IdnMapping();

            string domainName = match.Groups[2].Value;
            try
            {
                domainName = idn.GetAscii(domainName);
            }
            catch (ArgumentException)
            {
                invalid = true;
            }

            return match.Groups[1].Value + domainName;
        }

        /// <summary>
        /// Convert input string to SHA-512
        /// </summary>
        /// <param name="plainText">plain text string</param>
        /// <returns>Returns SHA-512 Encrypted string</returns>
        public string ConvertToSHA512(string plainText)
        {
            UnicodeEncoding UE = new UnicodeEncoding();
            byte[] message = UE.GetBytes(plainText);
            SHA512Managed hashString = new SHA512Managed();
            string hexNumber = "";
            byte[] hashValue = hashString.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hexNumber += $"{x:x2}";
            }

            return hexNumber;
        }

        private string ConvertToSHA512_WrongEncryption(string plainText)
        {
            byte[] data = Encoding.UTF8.GetBytes(plainText);
            byte[] result;
            SHA512 shaM = new SHA512Managed();
            result = shaM.ComputeHash(data);
            return Encoding.UTF8.GetString(result);
        }

        public bool CompareSHA512_WrongEncryption(string encryptedText, string plainText)
        {
            return encryptedText.Equals(ConvertToSHA512_WrongEncryption(plainText));
        }

        //private byte[] ConvertToSHA512(byte[] bytes)
        //{
        //    byte[] result;
        //    SHA512 shaM = new SHA512Managed();
        //    result = shaM.ComputeHash(bytes);
        //    return result;
        //}

        /// <summary>
        /// Compare encrypted text to plain text with SHA-512 encryption
        /// </summary>
        /// <param name="encryptedText">Encrypted text</param>
        /// <param name="plainText">Plain text</param>
        /// <returns>If plain text matches with encrypted text then it returns TRUE else FALSE.</returns>
        public bool CompareSHA512(string encryptedText, string plainText)
        {
            return encryptedText.Equals(ConvertToSHA512(plainText));
        }

        /// <summary>
        /// Compute hash for string encoded as UTF8
        /// </summary>
        /// <param name="s">String to be hashed</param>
        /// <returns>40-character hex string</returns>
        public string SHA1HashStringForUTF8String(string s)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(s);

            var sha1 = SHA1.Create();
            byte[] hashBytes = sha1.ComputeHash(bytes);

            return HexStringFromBytes(hashBytes);
        }

        /// <summary>
        /// Convert an array of bytes to a string of hex digits
        /// </summary>
        /// <param name="bytes">array of bytes</param>
        /// <returns>String of hex digits</returns>
        private string HexStringFromBytes(byte[] bytes)
        {
            var sb = new StringBuilder();
            foreach (byte b in bytes)
            {
                var hex = b.ToString("x2");
                sb.Append(hex);
            }

            return sb.ToString();
        }

        public bool CompareSHA1(string encryptedText, string plainText)
        {
            return encryptedText.Equals(SHA1HashStringForUTF8String(plainText));
        }

        public PhoneNumber PhoneNumber { get; set; }

        public void SaveLog(Exception ex, string className, string method = "")
        {
            try
            {
                var logFilePath = GetAppSetting("genericRepositoryError");
                // Today's file name
                var fileName = $"{DateTime.Now:yyyy-MM-dd}-gr-exception.log";
                var path = logFilePath + "\\" + fileName;
                // This text is added only once to the file.
                if (!File.Exists(path))
                {
                    // Create a file to write to.
                    using (StreamWriter sw = File.CreateText(path))
                    {
                        var dateString = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        sw.WriteLineAsync(dateString + " | UOW ERROR | LOG");
                        sw.WriteLineAsync(string.Format(
                            "{0}: Error of type \"{1}\" in class \"{2}\" and method \"{3}\" has the following validation errors:",
                            dateString, ex.GetType().Name, className, method));
                        if (ex.InnerException != null)
                        {
                            sw.WriteLineAsync(string.Format("- Error: \"{1}\"",
                                ex.InnerException.Message));
                            sw.WriteLineAsync("Date: " + DateTime.Now);
                            sw.WriteLineAsync(Environment.NewLine);
                        }
                    }
                }
                else
                {
                    // This text is always added, making the file longer over time
                    // if it is not deleted.
                    using (StreamWriter sw = File.AppendText(path))
                    {
                        var dateString = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        sw.WriteLineAsync(dateString + " | UOW ERROR | LOG");
                        sw.WriteLineAsync(string.Format(
                            "{0}: Error of type \"{1}\" in class \"{2}\" and method \"{3}\" has the following validation errors:",
                            dateString, ex.GetType().Name, className, method));
                        if (ex.InnerException != null)
                        {
                            sw.WriteLineAsync(string.Format("- Error: \"{1}\"",
                                ex.InnerException.Message));
                            sw.WriteLineAsync("Date: " + DateTime.Now);
                            sw.WriteLineAsync(Environment.NewLine);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                //
            }
        }

        public async Task SaveLogAsync(Exception ex, string className, string method = "")
        {
            try
            {
                var logFilePath = GetAppSetting("uowErrorLogs");
                // Today's file name
                var fileName = $"{DateTime.Now:yyyy-MM-dd}-uow-exception.log";
                var path = logFilePath + "\\" + fileName;
                // This text is added only once to the file.
                if (!File.Exists(path))
                {
                    // Create a file to write to.
                    using (StreamWriter sw = File.CreateText(path))
                    {
                        var dateString = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        await sw.WriteLineAsync(dateString + " | UOW ERROR | LOG");
                        await sw.WriteLineAsync(string.Format(
                            "{0}: Error of type \"{1}\" in class \"{2}\" and method \"{3}\" has the following validation errors:",
                            dateString, ex.GetType().Name, className, method));
                        if (ex.InnerException != null)
                        {
                            await sw.WriteLineAsync(string.Format("- Error: \"{1}\"",
                                ex.InnerException.Message));
                            await sw.WriteLineAsync("Date: " + DateTime.Now);
                            await sw.WriteLineAsync(Environment.NewLine);
                        }
                    }
                }
                else
                {
                    // This text is always added, making the file longer over time
                    // if it is not deleted.
                    using (StreamWriter sw = File.AppendText(path))
                    {
                        var dateString = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        await sw.WriteLineAsync(dateString + " | UOW ERROR | LOG");
                        await sw.WriteLineAsync(string.Format(
                            "{0}: Error of type \"{1}\" in class \"{2}\" and method \"{3}\" has the following validation errors:",
                            dateString, ex.GetType().Name, className, method));
                        if (ex.InnerException != null)
                        {
                            await sw.WriteLineAsync(string.Format("- Error: \"{1}\"",
                                ex.InnerException.Message));
                            await sw.WriteLineAsync("Date: " + DateTime.Now);
                            await sw.WriteLineAsync(Environment.NewLine);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                //
            }
        }

        public void SaveLog(DbEntityValidationResult eve)
        {
            try
            {
                var logFilePath = GetAppSetting("uowErrorLogs");
                // Today's file name
                var fileName = $"{DateTime.Now:yyyy-MM-dd}-uow-exception.log";
                var path = logFilePath + "\\" + fileName;
                // This text is added only once to the file.
                if (!File.Exists(path))
                {
                    // Create a file to write to.
                    using (StreamWriter sw = File.CreateText(path))
                    {
                        var dateString = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        sw.WriteLineAsync(dateString + " | UOW ERROR | LOG");
                        sw.WriteLineAsync(string.Format(
                            "{0}: Entity of type \"{1}\" in state \"{2}\" has the following validation errors:",
                            dateString, eve.Entry.Entity.GetType().Name, eve.Entry.State));
                        foreach (var ve in eve.ValidationErrors)
                        {
                            sw.WriteLineAsync(string.Format("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage));
                            sw.WriteLineAsync("Date: " + DateTime.Now);
                            sw.WriteLineAsync(Environment.NewLine);
                        }
                    }
                }
                else
                {
                    // This text is always added, making the file longer over time
                    // if it is not deleted.
                    using (StreamWriter sw = File.AppendText(path))
                    {
                        var dateString = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        sw.WriteLineAsync(dateString + " | UOW ERROR | LOG");
                        sw.WriteLineAsync(string.Format(
                            "{0}: Entity of type \"{1}\" in state \"{2}\" has the following validation errors:",
                            dateString, eve.Entry.Entity.GetType().Name, eve.Entry.State));
                        foreach (var ve in eve.ValidationErrors)
                        {
                            sw.WriteLineAsync(string.Format("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage));
                            sw.WriteLineAsync("Date: " + DateTime.Now);
                            sw.WriteLineAsync(Environment.NewLine);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                //
            }
        }

        public async Task SaveLogAsync(DbEntityValidationResult eve)
        {
            try
            {
                var logFilePath = GetAppSetting("uowErrorLogs");
                // Today's file name
                var fileName = $"{DateTime.Now:yyyy-MM-dd}-uow-exception.log";
                var path = logFilePath + "\\" + fileName;
                // This text is added only once to the file.
                if (!File.Exists(path))
                {
                    // Create a file to write to.
                    using (StreamWriter sw = File.CreateText(path))
                    {
                        var dateString = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        await sw.WriteLineAsync(dateString + " | UOW ERROR | LOG");
                        await sw.WriteLineAsync(string.Format(
                            "{0}: Entity of type \"{1}\" in state \"{2}\" has the following validation errors:",
                            dateString, eve.Entry.Entity.GetType().Name, eve.Entry.State));
                        foreach (var ve in eve.ValidationErrors)
                        {
                            await sw.WriteLineAsync(string.Format("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage));
                            await sw.WriteLineAsync("Date: " + DateTime.Now);
                            await sw.WriteLineAsync(Environment.NewLine);
                        }
                    }
                }
                else
                {
                    // This text is always added, making the file longer over time
                    // if it is not deleted.
                    using (StreamWriter sw = File.AppendText(path))
                    {
                        var dateString = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        await sw.WriteLineAsync(dateString + " | UOW ERROR | LOG");
                        await sw.WriteLineAsync(string.Format(
                            "{0}: Entity of type \"{1}\" in state \"{2}\" has the following validation errors:",
                            dateString, eve.Entry.Entity.GetType().Name, eve.Entry.State));
                        foreach (var ve in eve.ValidationErrors)
                        {
                            await sw.WriteLineAsync(string.Format("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage));
                            await sw.WriteLineAsync("Date: " + DateTime.Now);
                            await sw.WriteLineAsync(Environment.NewLine);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                //
            }
        }
    }
}