using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using PushSharp.Apple;
using PushSharp;
using PushSharp.Core;

namespace Organo.Solutions.PushNotification.Apple
{
    public class AppleNotification : INotification
    {
        public AppleNotification(string apnsCertificatePath, string apnsCertificatePass, bool isProduction)
        {
            APNSCertificatePath = apnsCertificatePath;
            APNSCertificatePass = apnsCertificatePass;
            IsProduction = isProduction;
        }

        public string APNSCertificatePath { get; set; } = string.Empty;
        public string APNSCertificatePass { get; set; } = string.Empty;
        public IList<string> Messages { get; set; } = new List<string>();
        private bool IsProduction { get; set; }

        public bool Send(string deviceToken, string message)
        {
            Messages= new List<string>();
            if (string.IsNullOrEmpty(deviceToken))
                Messages.Add("DeviceTokenInvalid");
            if (string.IsNullOrEmpty(APNSCertificatePath))
                Messages.Add("APNs Certificate Path is required");
            if (string.IsNullOrEmpty(APNSCertificatePass))
                Messages.Add("APNs Certificate Password is required");

            if (Messages.Count > 0)
                return false;
            deviceToken = deviceToken?.Replace(" ", "");
            var appleCert = System.IO.File.ReadAllBytes(APNSCertificatePath);

            // Configuration
            var apnsServerEnvironment = IsProduction
                ? ApnsConfiguration.ApnsServerEnvironment.Production
                : ApnsConfiguration.ApnsServerEnvironment.Sandbox;
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

            apnsBroker.OnNotificationSucceeded += (notification) =>
            {
                Messages.Add(notification.Payload.Root.ToString());
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

            return Messages.Count == 0;
        }

        public async Task<bool> SendAsync(string deviceToken, string message)
        {
            return await Task.Factory.StartNew(() =>
            {
                Messages = new List<string>();
                if (string.IsNullOrEmpty(deviceToken))
                    Messages.Add("DeviceTokenInvalid");
                if (string.IsNullOrEmpty(APNSCertificatePath))
                    Messages.Add("APNs Certificate Path is required");
                if (string.IsNullOrEmpty(APNSCertificatePass))
                    Messages.Add("APNs Certificate Password is required");

                if (Messages.Count > 0)
                    return false;
                deviceToken = deviceToken?.Replace(" ", "");
                var appleCert = System.IO.File.ReadAllBytes(APNSCertificatePath);

                // Configuration
                var apnsServerEnvironment = IsProduction
                    ? ApnsConfiguration.ApnsServerEnvironment.Production
                    : ApnsConfiguration.ApnsServerEnvironment.Sandbox;
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
                            var notificationException = (ApnsNotificationException)ex;

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

                apnsBroker.OnNotificationSucceeded += (notification) =>
                {
                    Messages.Add(notification.Payload.Root.ToString());
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

                return Messages.Count == 0;
            });
        }
    }
}