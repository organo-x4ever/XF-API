using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Organo.Solutions.X4Ever.V1.DAL.Helper.Statics
{
    public static class NotificationConstant
    {
        public static string PushNotificationPlatform { get; } = "pushNotificationPlatforms";
        public static string ApiEnvironment { get; } = "APIEnvironment";
        public static string CertificatePathDev { get; } = "apns_certificate_path_dev";
        public static string CertificatePassDev { get; } = "apns_certificate_password_dev";
        public static string CertificatePathProd { get; } = "apns_certificate_path_prod";
        public static string CertificatePassProd { get; } = "apns_certificate_password_prod";
    }
}