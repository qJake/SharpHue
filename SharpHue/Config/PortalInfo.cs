using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpHue.Config
{
    /// <summary>
    /// Contains information about bridge / portal connectivity.
    /// </summary>
    [JsonObject]
    public class PortalInfo
    {
        /// <summary>
        /// Gets whether or not the bridge is signed in to the portal.
        /// </summary>
        [JsonProperty("signedon")]
        public bool IsSignedOn { get; private set; }
        
        /// <summary>
        /// Gets whether or not the downstream / incoming channel is enabled and functional.
        /// </summary>
        [JsonProperty("incoming")]
        public bool IsIncomingEnabled { get; private set; }

        /// <summary>
        /// Gets whether or not the upstream / outgoing channel is enabled and functional.
        /// </summary>
        [JsonProperty("outgoing")]
        public bool IsOutgoingEnabled { get; private set; }

        /// <summary>
        /// Gets or sets the communication state.
        /// </summary>
        /// <remarks>Available values are probably <c>"connected"</c> and <c>"disconnected"</c>.</remarks>
        [JsonProperty("communication")]
        public string CommunicationState { get; private set; }
    }
}
