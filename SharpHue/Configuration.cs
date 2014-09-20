using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
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
        public static string AppID { get; set; }

        /// <summary>
        /// Stores the maximum amount of lights that can be stored in the Bridge system.
        /// </summary>
        internal const int MAX_LIGHTS = 50;

        /// <summary>
        /// Stores the default App ID if none is specified.
        /// </summary>
        internal const string DEFAULT_APP_ID = "SharpHue";

        /// <summary>
        /// Initializes the Configuration class.
        /// </summary>
        static Configuration()
        {
            Username = null;
            DeviceIP = null;
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
            Username = user;
            DeviceIP = bridgeIP;
        }

        /// <summary>
        /// Adds a new user to the system. After this method returns, the <see cref="Username" /> property will contain the new username.
        /// NOTE: This method requires that you press the button on your Hue Bridge first!
        /// </summary>
        public static void AddUser()
        {
            AddUser(DEFAULT_APP_ID);
        }

        /// <summary>
        /// Adds a new user to the system with the specified App ID. After this method returns, the <see cref="Username" /> property will contain the new username.
        /// NOTE: This method requires that you press the button on your Hue bridge first!
        /// </summary>
        /// <param name="appId">The App ID to use when registering a new user in the system.</param>
        public static void AddUser(string appId)
        {
            AppID = string.Format("{0}#{1}", appId, Environment.MachineName);
            DiscoverBridgeIP();
            RegisterNewUser();
        }

        /// <summary>
        /// Retrieves the complete bridge configuration object, including the user whitelist.
        /// </summary>
        /// <returns>A <see cref="SharpHue.Config.Configuration" /> object representing the current configuration state of the bridge.</returns>
        public static Config.Configuration GetBridgeConfiguration()
        {
            RequireAuthentication();

            return ((JObject)JsonClient.RequestSecure("/config")).ToObject<Config.Configuration>();
        }

        /// <summary>
        /// Deletes the specified user from the whitelist.
        /// </summary>
        /// <param name="username">The user to delete.</param>
        /// <returns><c>true</c> on success, <c>false</c> otherwise.</returns>
        public static bool DeleteUser(string username)
        {
            RequireAuthentication();

            JArray response = (JArray)JsonClient.RequestSecure(HttpMethod.Delete, "/config/whitelist/" + username);

            try
            {
                return !string.IsNullOrWhiteSpace(response[0]["success"].Value<string>());
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Ensures that the configuration has been initialized with a valid IP address. Otherwise, throws an exception.
        /// </summary>
        internal static void RequireInitialization()
        {
            if (DeviceIP == null)
            {
                throw new HueConfigurationException("DeviceIP has not been initialized. Try calling Configuration.Initialize().");
            }
        }

        /// <summary>
        /// Ensures that the configuration has been initialized with a valid IP address and user. Otherwise, throws an exception.
        /// </summary>
        internal static void RequireAuthentication()
        {
            if (DeviceIP == null)
            {
                throw new HueConfigurationException("DeviceIP has not been initialized. Try calling Configuration.Initialize().");
            }
            if (Username == null)
            {
                throw new HueConfigurationException("Username has not been set. Try calling AddUser() or set the Username property to an existing user ID.");
            }
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
        /// <exception cref="HueConfigurationException">Thrown when this class has not been initialized (the <see cref="DeviceIP" /> is <c>null</c>).</exception>
        private static bool RegisterNewUser(string username = null)
        {
            RequireInitialization();

            dynamic data = new ExpandoObject();

            data.devicetype = AppID;

            if (username != null)
            {
                data.username = username;
            }

            JArray response = JsonClient.Request(HttpMethod.Post, "/api", data);

            try
            {
                if (response[0]["success"]["username"] != null)
                {
                    Username = response[0]["success"]["username"].ToString();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
