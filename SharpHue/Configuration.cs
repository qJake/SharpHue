using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SharpHue
{
    public static class Configuration
    {
        public static string Username { get; set; }
        public static IPAddress DeviceIP { get; set; }

        static Configuration()
        {
            Username = null;
            DeviceIP = null;
        }

        public static void Initialize()
        {
            DiscoverBridgeIP();
            RegisterNewUser();
        }

        public static void Initialize(string user)
        {
            Username = user;
            DiscoverBridgeIP();
        }

        public static void Initialize(string user, IPAddress bridgeIP)
        {
            Username = null;
            DeviceIP = bridgeIP;
        }

        private static void DiscoverBridgeIP()
        {
            try
            {
                var brokerInfo = JsonClient.RequestBroker() as JArray;
                DeviceIP = IPAddress.Parse(((JObject)brokerInfo[0])["internalipaddress"].Value<string>());
            }
            catch (Exception ex)
            {
                throw new HubIPAddressNotFoundException(ex);
            }
        }

        private static void RegisterNewUser()
        {

        }

        public static void Register()
        {

        }
    }
}
