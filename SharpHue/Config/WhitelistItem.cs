using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpHue.Config
{
    /// <summary>
    /// Contains information about a single whitelist entry (or user).
    /// </summary>
    [JsonObject]
    public class WhitelistItem
    {
        /// <summary>
        /// Gets the application name / ID of this entry.
        /// </summary>
        [JsonProperty("name")]
        public string ApplicationID { get; private set; }

        /// <summary>
        /// Gets when this user ID was last used.
        /// </summary>
        [JsonProperty("last use date")]
        public DateTime LastUsed { get; private set; }

        /// <summary>
        /// Gets when this user ID was first created on the bridge.
        /// </summary>
        [JsonProperty("create date")]
        public DateTime Created { get; private set; }

        /// <summary>
        /// Gets a string representation of this whitelist entry.
        /// </summary>
        /// <returns>A string representing this whitelist entry.</returns>
        public override string ToString()
        {
            return ApplicationID;
        }
    }
}
