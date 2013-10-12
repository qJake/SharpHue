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
        private JObject stateObject;

        /// <summary>
        /// Initializes a new instance of the LightStateBuilder class.
        /// </summary>
        public LightStateBuilder()
        {
            stateObject = new JObject();
        }

        /// <summary>
        /// When this state is sent to the bridge, turns the light on.
        /// </summary>
        /// <returns>This LightStateBuilder instance, for method chaining.</returns>
        public LightStateBuilder TurnOn()
        {
            stateObject.Add(new JProperty("on", true));
            AddOrUpdateProperty("on", true);
            return this;
        }

        /// <summary>
        /// When this state is sent to the bridge, turns the light off.
        /// </summary>
        /// <returns>This LightStateBuilder instance, for method chaining.</returns>
        public LightStateBuilder TurnOff()
        {
            stateObject.Add(new JProperty("on", false));
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
            if (effect != LightEffect.None)
            {
                stateObject.Add(new JProperty("effect", effect.ToString().ToLower()));
                AddOrUpdateProperty("effect", effect.ToString().ToLower());
            }
            return this;
        }

        /// <summary>
        /// When this state is sent to the bridge, sets the current alert of the light.
        /// </summary>
        /// <returns>This LightStateBuilder instance, for method chaining.</returns>
        public LightStateBuilder Alert(LightAlert alert)
        {
            if (alert != LightAlert.None)
            {
                AddOrUpdateProperty("alert", alert.ToString().ToLower());
            }
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
