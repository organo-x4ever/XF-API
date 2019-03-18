using System;
using System.Text.RegularExpressions;

namespace Organo.Solutions.X4Ever.V1.DAL.Helper
{
    public class PhoneNumber
    {
        public PhoneNumber()
        {
        }

        public PhoneNumber(string value)
        {
            if (String.IsNullOrEmpty(value))
                throw new ArgumentNullException("numberString", "PhoneNumberIsNullOrEmpty");

            var match = new Regex(@"\+(\w+) \((\w+)\) (\w+)", RegexOptions.Compiled).Match(value);
            if (match.Success)
            {
                ushort countryCode = 0;
                ushort localCode = 0;
                int number = 0;

                if (UInt16.TryParse(match.Result("$1"), out countryCode) &&
                    UInt16.TryParse(match.Result("$2"), out localCode) &&
                    Int32.TryParse(match.Result("$3"), out number))
                {
                    this.CountryCode = countryCode;
                    this.LocalCode = localCode;
                    this.Number = number;
                }
            }
            else
            {
                throw new ArgumentNullException("numberString", "PhoneNumberInvalid");
            }
        }

        public PhoneNumber(int countryCode, int localCode, int number)
        {
            if (countryCode == 0)
                throw new ArgumentOutOfRangeException("countryCode", "PhoneNumberIsNullOrEmpty");
            else if (localCode == 0)
                throw new ArgumentOutOfRangeException("localCode", "PhoneNumberIsNullOrEmpty");
            else if (number == 0)
                throw new ArgumentOutOfRangeException("number", "PhoneNumberIsNullOrEmpty");

            this.CountryCode = countryCode;
            this.LocalCode = localCode;
            this.Number = number;
        }

        public int CountryCode { get; set; }

        public int LocalCode { get; set; }

        public int Number { get; set; }

        public override string ToString()
        {
            return String.Format(System.Globalization.CultureInfo.CurrentCulture, "+{0} ({1}) {2}", CountryCode, LocalCode, Number);
        }

        public static bool Validate(string value)
        {
            return new Regex(@"\+\w+ \(\w+\) \w+", RegexOptions.Compiled).IsMatch(value);
        }

        public static bool Validate(string countryCode, string localCode, string number, out PhoneNumber phoneNumber)
        {
            var valid = false;
            phoneNumber = null;
            try
            {
                ushort uCountryCode = 0;
                ushort uLocalCode = 0;
                int iNumber = 0;

                // match only if all three numbers have been parsed successfully
                valid = UInt16.TryParse(countryCode, out uCountryCode) &&
                        UInt16.TryParse(localCode, out uLocalCode) &&
                        Int32.TryParse(number, out iNumber);

                if (valid)
                    phoneNumber = new PhoneNumber(uCountryCode, uLocalCode, iNumber);
            }
            catch (ArgumentException)
            {
                // still not match
            }
            return valid;
        }
    }
}