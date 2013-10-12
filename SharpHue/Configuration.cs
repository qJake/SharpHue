using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SharpHue
{
    public static class Configuration
    {
        public static string Username { get; set; }
        public static IPAddress DeviceIP { get; set; }

        internal const string APP_ID = "SharpHue";
        internal const int MAX_LIGHTS = 50;

        static Configuration()
        {
            Username = null;
            DeviceIP = null;
        }

        public static void AddUser()
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

        internal static string GetAuthRequest(string apiPath)
        {
            return "/api/" + Username + (apiPath.StartsWith("/") ? "" : "/") + apiPath;
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

        private static void RegisterNewUser(string username = null)
        {
            Contract.Requires(DeviceIP != null);

            dynamic data = new ExpandoObject();

            data.devicetype = APP_ID;

            if (username != null)
            {
                data.username = username;
            }
            
            JArray response = JsonClient.Request(HttpMethod.Post, "/api", data);

            if (response[0]["success"]["username"] != null)
            {
                Username = response[0]["success"]["username"].ToString();
            }
        }

        public static void Register()
        {

        }
    }
}
