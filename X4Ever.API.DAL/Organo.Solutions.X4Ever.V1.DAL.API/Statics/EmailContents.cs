using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Organo.Solutions.X4Ever.V1.DAL.API.Models;
using Organo.Solutions.X4Ever.V1.DAL.Helper.Statics;

namespace Organo.Solutions.X4Ever.V1.DAL.API.Statics
{
    public interface IEmailContent
    {
        EmailDetail GetEmailDetail(string languageCode, EmailType emailType, string[] paramStrings);
        Task<EmailDetail> GetEmailDetailAsync(string languageCode, EmailType emailType, string[] paramStrings);

        string FooterContent(string languageCode);
    }

    public class EmailContent : IEmailContent
    {
        private bool IsRepeat = false;

        public EmailDetail GetEmailDetail(string languageCode, EmailType emailType, string[] paramStrings)
        {
            IsRepeat = false;
            return GetEmailBody(GetLanguageCode(languageCode), emailType, paramStrings);
        }

        private EmailDetail GetEmailBody(string languageCode, EmailType emailType, string[] paramStrings)
        {
            var emailDetail = new EmailDetail();
            var langCode = languageCode != null && languageCode.Trim().Length > 1
                ? (languageCode.Split('-'))[0]
                : "en";
            string[] filePath = FilePath(langCode, emailType);
            FileInfo fileInfo = new FileInfo(filePath[1]);
            if (fileInfo.Exists)
            {
                string[] pStrings = new string[paramStrings.Length + 1];
                pStrings[0] = FooterContent(langCode);
                for (int i = 0; i < paramStrings.Length; i++)
                {
                    pStrings[i + 1] = paramStrings[i];
                }

                string emailTemplate = File.ReadAllText(filePath[1]);
                emailDetail.Subject = filePath[0];
                emailDetail.Body = string.Format(emailTemplate, pStrings);
                return emailDetail;
            }
            else if (!IsRepeat)
            {
                IsRepeat = true;
                return GetEmailBody("en", emailType, paramStrings);
            }

            return null;
        }

        public async Task<EmailDetail> GetEmailDetailAsync(string languageCode, EmailType emailType,
            string[] paramStrings)
        {
            IsRepeat = false;
            return await GetEmailBodyAsync(GetLanguageCode(languageCode), emailType, paramStrings);
        }

        private async Task<EmailDetail> GetEmailBodyAsync(string languageCode, EmailType emailType,
            string[] paramStrings)
        {
            return await Task.Run(() => GetEmailBody(GetLanguageCode(languageCode), emailType, paramStrings));
        }

        string[] FilePath(string languageCode, EmailType emailType)
        {
            languageCode = GetLanguageCode(languageCode);
            string[] fileName = new string[2];
            fileName[0] = EmailSubject(emailType + "_SUBJECT_" + languageCode.ToUpper());
            fileName[1] =
                HttpContext.Current.Server.MapPath("~/EmailTemplates/" + emailType.ToString() + "_TEMPLATE_" +
                                                   languageCode.ToUpper() +
                                                   ".html");
            return fileName;
        }

        public string FooterContent(string languageCode)
        {
            var emailTemplate = "";
            var fileName =
                HttpContext.Current.Server.MapPath("~/EmailTemplates/FOOTER_TEMPLATE_" +
                                                   GetLanguageCode(languageCode).ToUpper() +
                                                   ".html");
            FileInfo fileInfo = new FileInfo(fileName);
            if (fileInfo.Exists)
            {
                emailTemplate = File.ReadAllText(fileName);
            }

            return emailTemplate;
        }

        string EmailSubject(string key)
        {
            return new Helper.Helper().GetAppSetting(key);
        }

        public string GetLanguageCode(string languageCode)
        {
            var languages = (languageCode ?? "en-US").Split('-');
            return languages[0];
        }
    }
}