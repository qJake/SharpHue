using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SharpHue
{
    [JsonObject]
    public class Light
    {
        public int ID { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("state")]
        public LightState State { get; set; }

        [JsonProperty("type")]
        public string Type { get; private set; }

        [JsonProperty("modelid")]
        public string ModelID { get; private set; }

        [JsonProperty("swversion")]
        public string SoftwareVersion { get; private set; }

        public void RefreshState()
        {
            var state = JsonClient.Request(Configuration.GetAuthRequest("/lights/" + ID));
            JsonConvert.PopulateObject(state.ToString(), this);
        }

        public void SetState(LightStateBuilder stateBuilder)
        {
            SetState(stateBuilder.Finish());
        }

        public void SetState(string newState)
        {
            JsonClient.Request(HttpMethod.Put, Configuration.GetAuthRequest("/lights/" + ID + "/state"), newState);
        }
    }
}
