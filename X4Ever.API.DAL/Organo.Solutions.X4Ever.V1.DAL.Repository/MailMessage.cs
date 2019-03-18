using Organo.Solutions.X4Ever.V1.DAL.Repository.Model;
using System;
using System.Globalization;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Organo.Solutions.X4Ever.V1.DAL.Repository
{
    public class Message
    {
        private MailProperty mailProperty;

        public Message()
        {
            mailProperty = new MailProperty();
        }

        public bool SendMail(ref string message,string recipient, string cc, string bcc, string subject, string body, bool isHTML = false, MailPriority mailPriority = MailPriority.High)
        {
            return this.SendMail(ref message, mailProperty.SendFrom, recipient, cc, bcc, subject, body, isHTML, mailPriority);
        }

        public bool SendMail(ref string message, string sender, string recipient, string cc, string bcc, string subject,
            string body, bool isHTML = false, MailPriority mailPriority = MailPriority.High)
        {
            try
            {
                message = "";
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(sender);

                if ((recipient != null) && (recipient.Trim() != string.Empty))
                {
                    foreach (var address in recipient.Split(new[] {";"}, StringSplitOptions.RemoveEmptyEntries))
                    {
                        if (IsValidEmail(address) == true)
                        {
                            mailMessage.To.Add(new MailAddress(address.Trim()));
                        }
                    }
                }

                if ((cc != null) && (cc.Trim() != string.Empty))
                {
                    foreach (var address in cc.Split(new[] {";"}, StringSplitOptions.RemoveEmptyEntries))
                    {
                        if (IsValidEmail(address) == true)
                        {
                            mailMessage.CC.Add(new MailAddress(address.Trim()));
                        }
                    }
                }

                if (bcc != null && bcc.Trim() != string.Empty)
                {
                    foreach (var address in bcc.Split(new[] {";"}, StringSplitOptions.RemoveEmptyEntries))
                    {
                        if (IsValidEmail(address) == true)
                        {
                            mailMessage.Bcc.Add(new MailAddress(address.Trim()));
                        }
                    }
                }

                mailMessage.Subject = subject.Trim();
                mailMessage.Body = body.Trim();
                mailMessage.IsBodyHtml = isHTML;
                mailMessage.Priority = mailPriority;

                var smtpClient = new SmtpClient(mailProperty.SmtpSever, mailProperty.Port)
                {
                    UseDefaultCredentials = true,
                    Credentials = new NetworkCredential(mailProperty.Username, mailProperty.Password),
                    EnableSsl = mailProperty.SSL.Trim().Length > 0,
                };

                smtpClient.Send(mailMessage);
                return true;
                // To use Google SMTP, has to Turn ON Access for less secure apps at: https://www.google.com/settings/security/lesssecureapps
            }
            catch (Exception ex)
            {
                message = ex.Message.ToString();
                return false;
            }
        }

        private bool invalid = false;

        public bool IsValidEmail(string strIn)
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
    }
}