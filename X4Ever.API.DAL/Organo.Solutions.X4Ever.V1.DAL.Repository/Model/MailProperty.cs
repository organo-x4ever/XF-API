namespace Organo.Solutions.X4Ever.V1.DAL.Repository.Model
{
    public class MailProperty
    {
        private Helper.IHelper _helper;

        public MailProperty() => _helper = new Helper.Helper();

        public string SmtpSever
        {
            get
            {
                return _helper.GetAppSetting("SmtpSever");
            }
        }

        public string SSL
        {
            get
            {
                return _helper.GetAppSetting("SSL");
            }
        }

        public string Mailer
        {
            get
            {
                return _helper.GetAppSetting("Mailer");
            }
        }

        public string Username
        {
            get
            {
                return _helper.GetAppSetting("Username");
            }
        }

        public string Password
        {
            get
            {
                return _helper.GetAppSetting("Password");
            }
        }

        public string ChecksumKey
        {
            get
            {
                return _helper.GetAppSetting("ChecksumKey");
            }
        }

        public string SendFrom
        {
            get
            {
                return _helper.GetAppSetting("SendFrom");
            }
        }

        public int Port
        {
            get
            {
                int.TryParse(_helper.GetAppSetting("Port"),out int port);
                return port;
            }
        }

        public bool SmtpAuth
        {
            get
            {
                return (bool)_helper.GetAppSetting("SmtpAuth", typeof(bool));
            }
        }
    }
}