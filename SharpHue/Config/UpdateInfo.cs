using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpHue.Config
{
    /// <summary>
    /// Contains information about a bridge software update.
    /// </summary>
    [JsonObject]
    public class UpdateInfo
    {
        /// <summary>
        /// Gets the update status. This is 0 if there are no updates available.
        /// </summary>
        [JsonProperty("updatestate")]
        public int UpdateStatus { get; private set; }

        /// <summary>
        /// Gets the URL of the update.
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; private set; }

        /// <summary>
        /// Gets the message text to display to the user about the update.
        /// </summary>
        [JsonProperty("text")]
        public string Message { get; private set; }

        /// <summary>
        /// Gets whether or not a notification should be shown to the user that an update is available.
        /// </summary>
        [JsonProperty("notify")]
        public bool ShouldNotify { get; private set; }
    }
}
