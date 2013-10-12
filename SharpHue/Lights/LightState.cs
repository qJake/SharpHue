using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpHue
{
    /* Model:
     {
        "hue": 50000,
        "on": true,
        "effect": "none",
        "alert": "none",
        "bri": 200,
        "sat": 200,
        "ct": 500,
        "xy": [0.5, 0.5],
        "reachable": true,
        "colormode": "hs"
    }
    */
    
    /// <summary>
    /// Stores information about the state of a light, including color information, current alert / effect settings, and more.
    /// </summary>
    public class LightState
    {
        /// <summary>
        /// Gets or sets whether or not this light is on.
        /// </summary>
        [JsonProperty("on")]
        public bool IsOn { get; set; }

        /// <summary>
        /// Gets or sets the Hue value of this light. The hue value is a wrapping value between 0 and 65535. Both 0 and 65535 are red, 25500 is green and 46920 is blue.
        /// </summary>
        [JsonProperty("hue")]
        public uint Hue { get; set; }

        /// <summary>
        /// Gets or sets the brightness of this light. 255 is the brightest, 0 is the darkest.
        /// </summary>
        [JsonProperty("bri")]
        public byte Brightness { get; set; }

        /// <summary>
        /// Gets or sets the saturation of this light. 255 is the most saturated, 0 is the least.
        /// </summary>
        [JsonProperty("sat")]
        public byte Saturation { get; set; }

        /// <summary>
        /// Gets or sets the Mired Color Temperature of this light. Currently, the acceptable range is 153 (6500K) to 500 (2000K).
        /// </summary>
        [JsonProperty("ct")]
        public uint ColorTemperature { get; set; }

        /// <summary>
        /// Gets or sets the X,Y CIE color space coordinates.
        /// </summary>
        [JsonProperty("xy")]
        public float[] ColorSpaceCoordinates { get; set; }

        /// <summary>
        /// Gets or sets the current effect of this light.
        /// </summary>
        [JsonProperty("effect")]
        public LightEffect Effect { get; set; }

        /// <summary>
        /// Gets or sets the current alert of this light.
        /// </summary>
        [JsonProperty("alert")]
        public LightAlert Alert { get; set; }

        /// <summary>
        /// Gets or sets whether this light is reachable by the Bridge.
        /// </summary>
        /// <remarks>Note: Always returns true, to be implemented in a future release of Hue, per Philips.</remarks>
        [JsonProperty("reachable")]
        public bool IsReachable { get; private set; }

        /// <summary>
        /// Gets the current color mode the light is using for color ("xy", "ct", "hs" for color coordinates, color temperature, and hue/saturation, respectively).
        /// </summary>
        [JsonProperty("colormode")]
        public string CurrentColorMode { get; private set; }

        /// <summary>
        /// Sets the current color state from a <see cref="System.Drawing.Color" /> object.
        /// </summary>
        public Color Color
        {
            set
            {
                HSBColor c = new HSBColor(value);
                Hue = (uint)(Math.Pow(c.H, 2));
                Saturation = (byte)c.S;
                Brightness = (byte)c.B;
            }
        }
    }
}
