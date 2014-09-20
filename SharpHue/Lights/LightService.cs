using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace SharpHue
{
    public static class LightService
    {
        /// <summary>
        /// Sets the state of all lights, using Group 0.
        /// </summary>
        /// <param name="newState">The new state to apply to every light.</param>
        public static void SetStateAll(JObject newState)
        {
            JsonClient.RequestSecure(HttpMethod.Put, "/groups/0/action", newState);
        }

        /// <summary>
        /// Instructs the bridge to discover new lights. A maximum of 15 new lights is added to the bridge configuration. If you are adding more than 15 lights, call this method once per 1 minute.
        /// </summary>
        public static void Discover()
        {
            JsonClient.RequestSecure(HttpMethod.Post, "/lights");
        }
    }
}
