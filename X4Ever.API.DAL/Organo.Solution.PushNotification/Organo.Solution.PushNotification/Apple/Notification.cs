using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using PushSharp.Apple;

namespace Organo.Solutions.PushNotification.Apple
{
    public class Notification : INotification
    {
        public Notification(string apnsCertificatePath, string apnsCertificatePass)
        {
            APNSCertificatePath = apnsCertificatePath;
            APNSCertificatePass = apnsCertificatePass;
        }

        public string APNSCertificatePath { get; set; } = string.Empty;
        public string APNSCertificatePass { get; set; } = string.Empty;
        public IList<string> Messages { get; set; } = new List<string>();

        public bool Send(string deviceToken, string message)
        {
            if (string.IsNullOrEmpty(APNSCertificatePath))
                Messages.Add("APNs Certificate Path is required");

            if (string.IsNullOrEmpty(APNSCertificatePass))
                Messages.Add("APNs Certificate Password is required");

            if (Messages.Count > 0)
                return false;

            var appleCert = System.IO.File.ReadAllBytes(APNSCertificatePath);

            // Configuration
            var apnsServerEnvironment = ApnsConfiguration.ApnsServerEnvironment.Production;
            var config = new ApnsConfiguration(apnsServerEnvironment, appleCert, APNSCertificatePass);

            // Create a new broker
            var apnsBroker = new ApnsServiceBroker(config);

            // Wire up events
            apnsBroker.OnNotificationFailed += (notification, aggregateEx) =>
            {
                aggregateEx.Handle(ex =>
                {
                    // See what kind of exception it was to further diagnose
                    if (ex is ApnsNotificationException)
                    {
                        var notificationException = (ApnsNotificationException) ex;

                        // Deal with the failed notification
                        var apnsNotification = notificationException.Notification;
                        var statusCode = notificationException.ErrorStatusCode;

                        Messages.Add($"Apple Notification Failed: ID={apnsNotification.Identifier}, Code={statusCode}");
                    }
                    else
                    {
                        // Inner exception might hold more useful information like an ApnsConnectionException
                        Messages.Add($"Apple Notification Failed for some unknown reason : {ex.InnerException}");
                    }

                    // Mark it as handled
                    return true;
                });
            };

            apnsBroker.OnNotificationSucceeded += (notification) => { Console.WriteLine("Apple Notification Sent!"); };

            var fbs = new FeedbackService(config);
            fbs.FeedbackReceived += (string token, DateTime timestamp) => { };

            // Start the broker
            apnsBroker.Start();

            // Queue a notification to send
            apnsBroker.QueueNotification(new ApnsNotification
            {
                DeviceToken = deviceToken,
                //Payload = JObject.Parse("{\"aps\":{\"badge\":7}}")
                Payload = JObject.Parse(("{\"aps\":{\"badge\":1,\"sound\":\"oven.caf\",\"alert\":\"" +
                                         (message + "\"}}")))
            });

            // Stop the broker, wait for it to finish   
            // This isn't done after every message, but after you're
            // done with the broker
            apnsBroker.Stop();

            return false;
        }

        public async Task<bool> SendAsync(string deviceToken, string message)
        {
            return await Task.Factory.StartNew(() =>
            {
                if (string.IsNullOrEmpty(APNSCertificatePath))
                    Messages.Add("APNs Certificate Path is required");

                if (string.IsNullOrEmpty(APNSCertificatePass))
                    Messages.Add("APNs Certificate Password is required");

                if (Messages.Count > 0)
                    return false;

                var appleCert = System.IO.File.ReadAllBytes(APNSCertificatePath);

                // Configuration
                var apnsServerEnvironment = ApnsConfiguration.ApnsServerEnvironment.Production;
                var config = new ApnsConfiguration(apnsServerEnvironment, appleCert, APNSCertificatePass);

                // Create a new broker
                var apnsBroker = new ApnsServiceBroker(config);

                // Wire up events
                apnsBroker.OnNotificationFailed += (notification, aggregateEx) =>
                {
                    aggregateEx.Handle(ex =>
                    {
                        // See what kind of exception it was to further diagnose
                        if (ex is ApnsNotificationException)
                        {
                            var notificationException = (ApnsNotificationException) ex;

                            // Deal with the failed notification
                            var apnsNotification = notificationException.Notification;
                            var statusCode = notificationException.ErrorStatusCode;
                            Messages.Add(
                                $"Apple Notification Failed: ID={apnsNotification.Identifier}, Code={statusCode}");
                        }
                        else
                        {
                            // Inner exception might hold more useful information like an ApnsConnectionException            
                            Messages.Add($"Apple Notification Failed for some unknown reason : {ex.InnerException}");
                        }

                        // Mark it as handled
                        return true;
                    });
                };

                apnsBroker.OnNotificationSucceeded += (notification) =>
                {
                    Console.WriteLine("Apple Notification Sent!");
                };

                var fbs = new FeedbackService(config);
                fbs.FeedbackReceived += (string token, DateTime timestamp) => { };

                // Start the broker
                apnsBroker.Start();

                // Queue a notification to send
                apnsBroker.QueueNotification(new ApnsNotification
                {
                    DeviceToken = deviceToken,
                    //Payload = JObject.Parse("{\"aps\":{\"badge\":7}}")
                    Payload = JObject.Parse(("{\"aps\":{\"badge\":1,\"sound\":\"oven.caf\",\"alert\":\"" +
                                             (message + "\"}}")))
                });

                // Stop the broker, wait for it to finish   
                // This isn't done after every message, but after you're
                // done with the broker
                apnsBroker.Stop();
                return false;
            });
        }
    }
}