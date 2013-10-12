using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpHue
{
    /// <summary>
    /// Assists with building a new light state.
    /// </summary>
    public class LightStateBuilder
    {
        /// <summary>
        /// Stores the current state.
        /// </summary>
        private JObject stateObject { get; set; }

        /// <summary>
        /// Gets or sets the associated lights to apply this new state to.
        /// </summary>
        private List<Light> associatedLights { get; set; }

        /// <summary>
        /// Gets or sets whether or not to apply this state to all lights (using group 0).
        /// </summary>
        private bool applyAll { get; set; }

        /// <summary>
        /// Initializes a new instance of the LightStateBuilder class.
        /// </summary>
        public LightStateBuilder()
        {
            stateObject = new JObject();
            associatedLights = new List<Light>();
        }

        /// <summary>
        /// When <see cref="Apply" /> is called, applies the state information stored in this LightStateBuilder to each of the lights passed in to this method.
        /// </summary>
        /// <param name="lights">The light(s) for which this new state applies to. To actually apply the new state, call <see cref="Apply" />.</param>
        /// <returns>This LightStateBuilder instance, for method chaining.</returns>
        public LightStateBuilder For(params Light[] lights)
        {
            foreach (var l in lights)
            {
                associatedLights.Add(l);
            }
            return this;
        }

        /// <summary>
        /// When <see cref="Apply" /> is called, applies the state information stored in this LightStateBuilder to each of the lights passed in to this method.
        /// </summary>
        /// <param name="lights">The light(s) for which this new state applies to. To actually apply the new state, call <see cref="Apply" />.</param>
        /// <returns>This LightStateBuilder instance, for method chaining.</returns>
        public LightStateBuilder For(IEnumerable<Light> lights)
        {
            foreach (var l in lights)
            {
                associatedLights.Add(l);
            }
            return this;
        }

        /// <summary>
        /// When <see cref="Apply" /> is called, applies the state information stored in this LightStateBuilder to every light (using Group 0).
        /// </summary>
        /// <returns>This LightStateBuilder instance, for method chaining.</returns>
        public LightStateBuilder ForAll()
        {
            applyAll = true;
            return this;
        }

        /// <summary>
        /// When this state is sent to the bridge, turns the light on.
        /// </summary>
        /// <returns>This LightStateBuilder instance, for method chaining.</returns>
        public LightStateBuilder TurnOn()
        {
            AddOrUpdateProperty("on", true);
            return this;
        }

        /// <summary>
        /// When this state is sent to the bridge, turns the light off.
        /// </summary>
        /// <returns>This LightStateBuilder instance, for method chaining.</returns>
        public LightStateBuilder TurnOff()
        {
            AddOrUpdateProperty("on", false);
            return this;
        }

        /// <summary>
        /// When this state is sent to the bridge, sets the light's color using a common <see cref="Color" /> object.
        /// </summary>
        /// <returns>This LightStateBuilder instance, for method chaining.</returns>
        public LightStateBuilder Color(Color c)
        {
            HSBColor hsb = new HSBColor(c);
            AddOrUpdateProperty("hue", (ushort)Math.Round(hsb.H * 256));
            AddOrUpdateProperty("sat", (byte)Math.Round(hsb.S));
            AddOrUpdateProperty("bri", (byte)Math.Round(hsb.B));
            return this;
        }

        /// <summary>
        /// When this state is sent to the bridge, sets the current hue of the light (0 to 65535).
        /// </summary>
        /// <returns>This LightStateBuilder instance, for method chaining.</returns>
        public LightStateBuilder Hue(ushort hue)
        {
            AddOrUpdateProperty("hue", hue);
            return this;
        }

        /// <summary>
        /// When this state is sent to the bridge, sets the current saturation level of the light (0 to 255).
        /// </summary>
        /// <returns>This LightStateBuilder instance, for method chaining.</returns>
        public LightStateBuilder Saturation(byte saturation)
        {
            AddOrUpdateProperty("sat", saturation);
            return this;
        }

        /// <summary>
        /// When this state is sent to the bridge, sets the current brightness of the light (0 to 255).
        /// </summary>
        /// <returns>This LightStateBuilder instance, for method chaining.</returns>
        public LightStateBuilder Brightness(byte brightness)
        {
            AddOrUpdateProperty("bri", brightness);
            return this;
        }

        /// <summary>
        /// When this state is sent to the bridge, sets the current color temperature of the light (153 to 500).
        /// </summary>
        /// <returns>This LightStateBuilder instance, for method chaining.</returns>
        public LightStateBuilder ColorTemperature(byte temp)
        {
            AddOrUpdateProperty("ct", temp);
            return this;
        }

        /// <summary>
        /// When this state is sent to the bridge, sets the CIY color space X,Y coordinates of the light.
        /// </summary>
        /// <returns>This LightStateBuilder instance, for method chaining.</returns>
        public LightStateBuilder XYCoordinates(double x, double y)
        {
            AddOrUpdateProperty("xy", new float[] { (float)x, (float)y });
            return this;
        }

        /// <summary>
        /// When this state is sent to the bridge, sets the current effect of the light.
        /// </summary>
        /// <returns>This LightStateBuilder instance, for method chaining.</returns>
        public LightStateBuilder Effect(LightEffect effect)
        {
            stateObject.Add(new JProperty("effect", effect.ToString().ToLower()));
            AddOrUpdateProperty("effect", effect.ToString().ToLower());
            return this;
        }

        /// <summary>
        /// When this state is sent to the bridge, sets the current alert of the light.
        /// </summary>
        /// <returns>This LightStateBuilder instance, for method chaining.</returns>
        public LightStateBuilder Alert(LightAlert alert)
        {
            AddOrUpdateProperty("alert", alert.ToString().ToLower());
            return this;
        }

        /// <summary>
        /// When this state is sent to the bridge, sets the transition time for this state update.
        /// </summary>
        /// <returns>This LightStateBuilder instance, for method chaining.</returns>
        public LightStateBuilder TransitionTime(ushort time)
        {
            AddOrUpdateProperty("transitiontime", time);
            return this;
        }

        /// <summary>
        /// Applies the state contained within this LightStateBuilder to the lights specified in any <see cref="For" /> method calls.
        /// </summary>
        public void Apply()
        {
            if (applyAll)
            {
                LightCollection.SetStateAll(GetJson());
            }
            else
            {
                foreach (var l in associatedLights)
                {
                    l.SetState(GetJson());
                }
            }
        }

        /// <summary>
        /// Returns the underlying Json object after the state has been built.
        /// </summary>
        /// <returns>The underlying Json object, typically used to send the new state to one or more lights.</returns>
        public JObject GetJson()
        {
            return stateObject;
        }

        /// <summary>
        /// Returns the underlying Json object, as a Json string.
        /// </summary>
        /// <returns>The underlying Json object containing the new state information, as a string.</returns>
        public override string ToString()
        {
            return stateObject.ToString();
        }

        /// <summary>
        /// Helper method to add or update an existing property on the Json object.
        /// </summary>
        /// <param name="propName">The name of the property.</param>
        /// <param name="value">The value.</param>
        private void AddOrUpdateProperty(string propName, object value)
        {
            if (stateObject[propName] != null)
            {
                stateObject.Remove(propName);
            }
            stateObject.Add(new JProperty(propName, value));
        }
    }
}
