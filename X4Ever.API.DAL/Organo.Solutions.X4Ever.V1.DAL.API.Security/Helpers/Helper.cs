using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Organo.Solutions.X4Ever.V1.API.Security.Helpers
{
    public sealed partial class Helper
    {
        public sealed class RegexUtilities
        {
            private bool invalid = false;

            public bool IsValidEmail(string strIn)
            {
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

            public bool ValidatePassword(string password, out string ErrorMessage)
            {
                var input = password;
                ErrorMessage = string.Empty;

                if (string.IsNullOrWhiteSpace(input))
                {
                    throw new Exception("Password should not be empty");
                }

                var hasNumber = new Regex(@"[0-9]+");
                var hasUpperChar = new Regex(@"[A-Z]+");
                var hasMiniMaxChars = new Regex(@".{8,8}");
                var hasLowerChar = new Regex(@"[a-z]+");
                var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

                if (!hasLowerChar.IsMatch(input))
                {
                    ErrorMessage = "Password should contain At least one lower case letter";
                    return false;
                }
                else if (!hasUpperChar.IsMatch(input))
                {
                    ErrorMessage = "Password should contain At least one upper case letter";
                    return false;
                }
                else if (!hasMiniMaxChars.IsMatch(input))
                {
                    ErrorMessage = "Password should not be less than or greater than 8 characters";
                    return false;
                }
                else if (!hasNumber.IsMatch(input))
                {
                    ErrorMessage = "Password should contain At least one numeric value";
                    return false;
                }
                else if (!hasSymbols.IsMatch(input))
                {
                    ErrorMessage = "Password should contain At least one special case characters";
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
    }
}