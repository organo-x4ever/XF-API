using System;

namespace Organo.Solutions.X4Ever.V1.DAL.API.Models
{
    public class ForgotPassword
    {
        public string UserLogin { get; set; }
        public string UserEmail { get; set; }
    }

    public class PasswordDetail
    {
        public string RequestCode { get; set; }
        public string Password { get; set; }
    }

    public class PasswordChange
    {
        public Int64 UserID { get; set; }
        public string CurrentPassword { get; set; }
        public string Password { get; set; }
    }

    public class AccountDelete
    {
        public Int64 UserID { get; set; }
        public string Password { get; set; }
    }
}