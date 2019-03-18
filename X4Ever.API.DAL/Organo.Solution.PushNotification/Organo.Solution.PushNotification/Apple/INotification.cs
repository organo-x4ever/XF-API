using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Organo.Solutions.PushNotification.Apple
{
    public interface INotification
    {
        string APNSCertificatePath { get; set; }
        string APNSCertificatePass { get; set; }
        IList<string> Messages { get; set; }
        bool Send(string deviceToken, string message);
        Task<bool> SendAsync(string deviceToken, string message);
    }
}