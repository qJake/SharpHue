using System;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SharpHue
{
    /// <summary>
    /// Represents a single light in a Hue system.
    /// </summary>
    [JsonObject]
    public class Light
    {
        /// <summary>
        /// Occurs when the state of the light changes locally (i.e. when using a LightStateBuilder).
        /// </summary>
        public event Action<object, LightState> StateChanged;

        /// <summary>
        /// Gets the numeric ID of this light.
        /// </summary>
        public int ID { get; internal set; }

        /// <summary>
        /// Gets the friendly name of this light.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; internal set; }

        /// <summary>
        /// Gets the state of this light.
        /// </summary>
        [JsonProperty("state")]
        public LightState State { get; internal set; }

        /// <summary>
        /// Gets the type of light (e.g. current Hue bulbs return "Extended color light").
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; private set; }

        /// <summary>
        /// Gets the model ID for this light.
        /// </summary>
        [JsonProperty("modelid")]
        public string ModelID { get; private set; }

        /// <summary>
        /// Gets the software version for this light.
        /// </summary>
        [JsonProperty("swversion")]
        public string SoftwareVersion { get; private set; }

        /// <summary>
        /// Initializes a new instance of the Light class.
        /// </summary>
        public Light()
        {
            State = new LightState();
        }

        /// <summary>
        /// Refreshes this light's state information.
        /// </summary>
        public void RefreshState(JToken localState = null)
        {
            if (localState == null)
            {
                localState = JsonClient.RequestSecure("/lights/" + ID);
                JsonConvert.PopulateObject(localState.ToString(), this);
            }
            else
            {
                JsonConvert.PopulateObject(localState.ToString(), State);
            }

            JsonConvert.PopulateObject(localState.ToString(), this);

            if (StateChanged != null)
            {
                StateChanged(this, State);
            }
        }

        /// <summary>
        /// Sets this light's state information using a LightStateBuilder object.
        /// </summary>
        /// <param name="stateBuilder">The LightStateBuilder containing the new state information.</param>
        public void SetState(LightStateBuilder stateBuilder)
        {
            SetState(stateBuilder.GetJson());
        }

        /// <summary>
        /// Sets this light's state information using a JSON object.
        /// </summary>
        /// <param name="newState">The Json object containing the new state information.</param>
        /// <remarks>For information on valid properties, see this API reference: http://developers.meethue.com/1_lightsapi.html#16_set_light_state </remarks>
        public void SetState(JObject newState)
        {
            JsonClient.RequestSecure(HttpMethod.Put, "/lights/" + ID + "/state", newState);

            RefreshState(newState);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
