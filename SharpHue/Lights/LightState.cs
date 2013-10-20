using System;
using System.Drawing;
using Newtonsoft.Json;

namespace SharpHue
{
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
        /// Gets the current color state represented as a Color object.
        /// </summary>
        public Color Color
        {
            get
            {
                switch (CurrentColorMode.ToLower())
                {
                    case "xy":
                        throw new InvalidOperationException("Converting from X,Y to a Color is not supported at this time.");
                    case "ct":
                        return new HSBColor(MathEx.TranslateValue(ColorTemperature, 137, 500, 2000, 11500, true)).Color;
                    case "hs":
                        return new HSBColor(Hue / 255, Saturation, Brightness).Color;
                    default:
                        return new Color();
                }
            }
        }
    }
}
