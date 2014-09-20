using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpHue.Config
{
    /// <summary>
    /// Represents the configuration state of the Hue Bridge.
    /// </summary>
    [JsonObject]
    public class Configuration
    {
        /// <summary>
        /// Gets the name of the Hue bridge.
        /// </summary>
        [JsonProperty("name")]
        public string BridgeName { get; private set; }

        /// <summary>
        /// Gets the Zigbee channel ID that the bridge is currently operating on.
        /// </summary>
        [JsonProperty("zigbeechannel")]
        public int ZigbeeChannel { get; private set; }

        /// <summary>
        /// Gets the MAC Address of the bridge.
        /// </summary>
        [JsonProperty("mac")]
        public string MacAddress { get; private set; }

        /// <summary>
        /// Gets whether or not DHCP is enabled on the bridge.
        /// </summary>
        [JsonProperty("dhcp")]
        public bool IsDhcpEnabled { get; private set; }

        /// <summary>
        /// Gets the IP Address of the bridge.
        /// </summary>
        [JsonProperty("ipaddress")]
        public string IpAddress { get; private set; }

        /// <summary>
        /// Gets the network mask of the bridge (e.g. 255.255.255.0).
        /// </summary>
        [JsonProperty("netmask")]
        public string NetMask { get; private set; }

        /// <summary>
        /// Gets the default gateway of the bridge (e.g. 192.168.0.1).
        /// </summary>
        [JsonProperty("gateway")]
        public string GatewayAddress { get; private set; }

        /// <summary>
        /// Gets the proxy address. If proxying is disabled, this is set to <c>"none"</c>.
        /// </summary>
        [JsonProperty("proxyaddress")]
        public string ProxyAddress { get; private set; }

        /// <summary>
        /// Gets the proxy port number. If proxying is disabled, this is set to <c>0</c>.
        /// </summary>
        [JsonProperty("proxyport")]
        public int ProxyPort { get; private set; }

        /// <summary>
        /// Gets the current time on the bridge, in UTC time.
        /// </summary>
        [JsonProperty("UTC")]
        public DateTime CurrentTimeUtc { get; private set; }

        /// <summary>
        /// Gets the current time on the bridge, in local time. Check the <see cref="TimeZone" /> property for the current timezone.
        /// </summary>
        [JsonProperty("localtime")]
        public DateTime CurrentTime { get; private set; }
        
        /// <summary>
        /// Gets the current timezone that the bridge is operating in.
        /// </summary>
        [JsonProperty("timezone")]
        public string TimeZone { get; private set; }

        /// <summary>
        /// Gets the current software version of the bridge.
        /// </summary>
        [JsonProperty("swversion")]
        public string SoftwareVersion { get; private set; }

        /// <summary>
        /// Gets the API version that the bridge is running.
        /// </summary>
        [JsonProperty("apiversion")]
        public string ApiVersion { get; private set; }

        /// <summary>
        /// Gets whether or not the link button has been pressed within the past 30 seconds.
        /// </summary>
        [JsonProperty("linkbutton")]
        public bool IsLinkButtonPressed { get; private set; }

        /// <summary>
        /// Gets whether or not portal services are enabled (whether or not the bridge is connected to the Hue Portal online).
        /// </summary>
        [JsonProperty("portalservices")]
        public bool IsPortalServicesEnabled { get; private set; }

        /// <summary>
        /// Gets the portal connection state.
        /// </summary>
        /// <remarks>I believe the only two valid values here are <c>"connected"</c> or <c>"disconnected"</c>.</remarks>
        [JsonProperty("portalconnection")]
        public string PortalConnectionState { get; private set; }

        /// <summary>
        /// Gets the current whitelist / user list of the bridge. The dictionary keys are the access IDs, and the values are <see cref="WhitelistItem" /> objects containing additional information.
        /// </summary>
        [JsonProperty("whitelist")]
        public Dictionary<string, WhitelistItem> Whitelist { get; private set; }

        /// <summary>
        /// Gets an <see cref="UpdateInfo" /> object representing whether or not the bridge is up to date.
        /// </summary>
        [JsonProperty("swupdate")]
        public UpdateInfo UpdateState { get; private set; }

        /// <summary>
        /// Gets a <see cref="PortalInfo" /> object representing whether or not the portal is connected, and whether or not upstream/downstream links are functional.
        /// </summary>
        [JsonProperty("portalstate")]
        public PortalInfo PortalState { get; private set; }

        /// <summary>
        /// Gets a string representation of this configuration.
        /// </summary>
        /// <returns>A string representing this configuration object.</returns>
        public override string ToString()
        {
            return "Configuration: " + BridgeName;
        }
    }
}