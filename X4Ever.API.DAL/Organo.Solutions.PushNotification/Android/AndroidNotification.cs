using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organo.Solutions.PushNotification.Android
{
    public class AndroidNotification //: INotification
    {
        public IList<string> Messages { get; set; } = new List<string>();
        public string APNSCertificatePath { get; set; }
        public string APNSCertificatePass { get; set; }

        public bool Send(string deviceToken, string message, bool production = false)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SendAsync(string deviceToken, string message)
        {
            throw new NotImplementedException();
        }
    }
}