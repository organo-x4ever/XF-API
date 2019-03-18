using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace Organo.Solutions.X4Ever.V1.DAL.API.PushNotification
{
    public class SSLClient
    {
        public delegate void OnSSLRequestHandler(SSLRequestEvent e);

        private string _remoteHost;
        private string[] _serverCertFilenames;
        private string[] _serverCertPasswords;

        public event OnSSLRequestHandler OnLoaded;

        public SSLClient(string remoteHost, string[] serverCertFilenames, string[] serverCertPasswords)
        {
            _remoteHost = remoteHost;
            _serverCertFilenames = serverCertFilenames;
            _serverCertPasswords = serverCertPasswords;
        }

        public bool ValidateServerCertificate(
            object sender,
            X509Certificate certificate,
            X509Chain chain,
            SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        public void Load(byte[] sendData, int port = 443)
        {
            // Create a TCP/IP client socket. 
            TcpClient client = new TcpClient(_remoteHost, port);

            // Create an SSL stream utilizing the TcpClient
            SslStream sslStream = new SslStream(
                client.GetStream(),
                false,
                new RemoteCertificateValidationCallback(ValidateServerCertificate),
                null
            );

            try
            {
                X509Certificate2Collection xc = new X509Certificate2Collection();

                for (int i = 0; i < _serverCertFilenames.Length; i++)
                {
                    X509Certificate2 x = new X509Certificate2(
                        _serverCertFilenames[i],
                        _serverCertPasswords[i]);

                    xc.Add(x);
                }

                sslStream.AuthenticateAsClient(_remoteHost, xc, SslProtocols.Default, true);
            }
            catch (AuthenticationException e)
            {
                client.Close();
                sslStream.Close();

                if (OnLoaded != null)
                    OnLoaded(new SSLRequestEvent(false, e.Message, null, this));

                return;
            }

            sslStream.Write(sendData);
            sslStream.Flush();

            // Read message from the server. 
            byte[] response = ReadMessage(sslStream);

            if (OnLoaded != null)
                OnLoaded(new SSLRequestEvent(true, "Success", response, this));

            // Close the client connection.
            client.Close();
        }

        private byte[] ReadMessage(SslStream sslStream)
        {
            MemoryStream ms = new MemoryStream();

            byte[] buffer = new byte[1024];

            int bytes = -1;
            do
            {
                bytes = sslStream.Read(buffer, 0, buffer.Length);

                ms.Write(buffer, 0, bytes);

            } while (bytes != 0);

            return ms.ToArray();
        }
    }
}