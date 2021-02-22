using System;
using CorePush.Apple;
using Newtonsoft.Json;

namespace GaryPortalAPI.Models
{
    public class Notification
    {
        public class ApsPayload
        {
            [JsonProperty("alert")]
            public APSAlert Alert { get; set; }
        }

        [JsonProperty("aps")]
        public ApsPayload Aps { get; set; }

        public static Notification CreateNotification(APSAlert alertContent)
        {
            return new Notification
            {
                Aps = new ApsPayload
                {
                    Alert = alertContent
                }
            };
        }
    }

    public class APSAlert
    {
        public string title { get; set; }
        public string subtitle { get; set; }
        public string body { get; set; }
    }

    public class APNSSettings
    {
        public string AppBundleIdentifier { get; set; }
        public string P8PrivateKey { get; set; }
        public string P8PrivateKeyId { get; set; }
        public string ServerType { get; set; }
        public string TeamId { get; set; }

        public ApnSettings ConvertToAPNSettings()
        {
            return new ApnSettings
            {
                AppBundleIdentifier = AppBundleIdentifier,
                P8PrivateKey = P8PrivateKey,
                P8PrivateKeyId = P8PrivateKeyId,
                ServerType = ServerType == "Production" ? ApnServerType.Production : ApnServerType.Development,
                TeamId = TeamId
            };
        }
    }
}
