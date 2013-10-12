using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpHue
{
    public class LightStateBuilder
    {
        private JObject stateObject;

        public LightStateBuilder()
        {
            stateObject = new JObject();
        }

        public LightStateBuilder TurnOn()
        {
            stateObject.Add(new JProperty("on", true));
            AddOrUpdateProperty("on", true);
            return this;
        }

        public LightStateBuilder TurnOff()
        {
            stateObject.Add(new JProperty("on", false));
            AddOrUpdateProperty("on", false);
            return this;
        }

        public LightStateBuilder Color(Color c)
        {
            HSBColor hsb = new HSBColor(c);
            AddOrUpdateProperty("hue", (ushort)Math.Round(hsb.H * 256));
            AddOrUpdateProperty("sat", (byte)Math.Round(hsb.S));
            AddOrUpdateProperty("bri", (byte)Math.Round(hsb.B));
            return this;
        }

        public LightStateBuilder Hue(ushort hue)
        {
            AddOrUpdateProperty("hue", hue);
            return this;
        }

        public LightStateBuilder Saturation(byte saturation)
        {
            AddOrUpdateProperty("sat", saturation);
            return this;
        }

        public LightStateBuilder Brightness(byte brightness)
        {
            AddOrUpdateProperty("bri", brightness);
            return this;
        }

        public LightStateBuilder Effect(LightEffect effect)
        {
            if (effect != LightEffect.None)
            {
                stateObject.Add(new JProperty("effect", effect.ToString().ToLower()));
                AddOrUpdateProperty("effect", effect.ToString().ToLower());
            }
            return this;
        }

        public LightStateBuilder Alert(LightAlert alert)
        {
            if (alert != LightAlert.None)
            {
                AddOrUpdateProperty("alert", alert.ToString().ToLower());
            }
            return this;
        }

        public LightStateBuilder TransitionTime(ushort time)
        {
            AddOrUpdateProperty("transitiontime", time);
            return this;
        }

        public string Finish()
        {
            return stateObject.ToString();
        }

        public override string ToString()
        {
            return stateObject.ToString();
        }

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
