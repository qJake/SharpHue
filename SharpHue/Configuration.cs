using System;
using System.Dynamic;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace SharpHue
{
    /// <summary>
    /// Represents common configuration information for a Hue system.
    /// </summary>
    public static class Configuration
    {
        /// <summary>
        /// Gets or sets the username used to query the bridge with.
        /// </summary>
        public static string Username { get; set; }

        /// <summary>
        /// Gets or sets the IP address used to query the brige with.
        /// </summary>
        public static IPAddress DeviceIP { get; set; }

        /// <summary>
        /// Gets or sets the App ID used for registering a new user.
        /// </summary>
        public static string AppId { get; set; }

        /// <summary>
        /// Stores the maximum amount of lights that can be stored in the Bridge system.
        /// </summary>
        internal const int MAX_LIGHTS = 50;

        /// <summary>
        /// Initializes the Configuration class.
        /// </summary>
        static Configuration()
        {
            Username = null;
            DeviceIP = null;
        }

        /// <summary>
        /// Adds a new user to the system. After this method returns, the <see cref="Username" /> property will contain the new username.
        /// NOTE: This method requires that you press the button on your Hue Bridge first!
        /// </summary>
        public static void AddUser()
        {
            //The Default name is none is specified.
            AddUser("SharpHue");
        }

        public static void AddUser(string appId)
        {
            AppId = appId;
            DiscoverBridgeIP();
            RegisterNewUser();
        }

        /// <summary>
        /// Initializes the Hue configuration with the specified existing username.
        /// </summary>
        /// <param name="user">The Hue username to use when querying the bridge.</param>
        public static void Initialize(string user)
        {
            Username = user;
            DiscoverBridgeIP();
        }

        /// <summary>
        /// Initializes the Hue configuration with the specified existing username, and bridge IP address.
        /// </summary>
        /// <param name="user">The Hue username to use when querying the bridge.</param>
        /// <param name="bridgeIP">The IP address of the bridge device.</param>
        public static void Initialize(string user, IPAddress bridgeIP)
        {
            Username = null;
            DeviceIP = bridgeIP;
        }

        /// <summary>
        /// Builds an auth request by prepending /api/ and the username.
        /// </summary>
        /// <param name="apiPath">The path, after /api/{username}/.</param>
        /// <returns>The full API request string.</returns>
        internal static string GetAuthRequest(string apiPath)
        {
            return "/api/" + Username + (apiPath.StartsWith("/") ? "" : "/") + apiPath;
        }

        /// <summary>
        /// Attempts to discover the bridge IP address by invoking a Philips discovery broker API.
        /// </summary>
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

        /// <summary>
        /// Registers a new user with the bridge device.
        /// </summary>
        /// <param name="username">The new username to register.</param>
        private static void RegisterNewUser(string username = null)
        {
            if (DeviceIP == null)
            {
                throw new HueConfigurationException("DeviceIP has not been initialized. Try calling Configuration.Initialize().");
            }

            dynamic data = new ExpandoObject();

            data.devicetype = AppId;

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
    }
}
