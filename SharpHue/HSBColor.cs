using System;
using System.Drawing;

namespace SharpHue
{
    /// <summary>
    /// Encapsulates a color using HSB values.
    /// </summary>
    /// <remarks>Source: http://www.codeproject.com/Articles/11340/Use-both-RGB-and-HSB-color-schemas-in-your-NET-app </remarks>
    public struct HSBColor
    {
        float h;
        float s;
        float b;
        int a;

        public HSBColor(float h, float s, float b)
        {
            this.a = 0xff;
            this.h = Math.Min(Math.Max(h, 0), 255);
            this.s = Math.Min(Math.Max(s, 0), 255);
            this.b = Math.Min(Math.Max(b, 0), 255);
        }

        public HSBColor(int a, float h, float s, float b)
        {
            this.a = a;
            this.h = Math.Min(Math.Max(h, 0), 255);
            this.s = Math.Min(Math.Max(s, 0), 255);
            this.b = Math.Min(Math.Max(b, 0), 255);
        }

        public HSBColor(Color color)
        {
            HSBColor temp = FromColor(color);
            this.a = temp.a;
            this.h = temp.h;
            this.s = temp.s;
            this.b = temp.b;
        }

        public HSBColor(int ColorTemperature) : this(FromColorTemperature(ColorTemperature)) { }

        public float H
        {
            get { return h; }
            set { value = h; }
        }

        public float S
        {
            get { return s; }
            set { value = s; }
        }

        public float B
        {
            get { return b; }
            set { value = b; }
        }

        public int A
        {
            get { return a; }
            set { value = a; }
        }

        public Color Color
        {
            get
            {
                return FromHSB(this);
            }
        }

        public static Color ShiftHue(Color c, float hueDelta)
        {
            HSBColor hsb = HSBColor.FromColor(c);
            hsb.h += hueDelta;
            hsb.h = Math.Min(Math.Max(hsb.h, 0), 255);
            return FromHSB(hsb);
        }

        public static Color ShiftSaturation(Color c, float saturationDelta)
        {
            HSBColor hsb = HSBColor.FromColor(c);
            hsb.s += saturationDelta;
            hsb.s = Math.Min(Math.Max(hsb.s, 0), 255);
            return FromHSB(hsb);
        }


        public static Color ShiftBrighness(Color c, float brightnessDelta)
        {
            HSBColor hsb = HSBColor.FromColor(c);
            hsb.b += brightnessDelta;
            hsb.b = Math.Min(Math.Max(hsb.b, 0), 255);
            return FromHSB(hsb);
        }

        public static Color FromHSB(HSBColor hsbColor)
        {
            float r = hsbColor.b;
            float g = hsbColor.b;
            float b = hsbColor.b;
            if (hsbColor.s != 0)
            {
                float max = hsbColor.b;
                float dif = hsbColor.b * hsbColor.s / 255f;
                float min = hsbColor.b - dif;

                float h = hsbColor.h * 360f / 255f;

                if (h < 60f)
                {
                    r = max;
                    g = h * dif / 60f + min;
                    b = min;
                }
                else if (h < 120f)
                {
                    r = -(h - 120f) * dif / 60f + min;
                    g = max;
                    b = min;
                }
                else if (h < 180f)
                {
                    r = min;
                    g = max;
                    b = (h - 120f) * dif / 60f + min;
                }
                else if (h < 240f)
                {
                    r = min;
                    g = -(h - 240f) * dif / 60f + min;
                    b = max;
                }
                else if (h < 300f)
                {
                    r = (h - 240f) * dif / 60f + min;
                    g = min;
                    b = max;
                }
                else if (h <= 360f)
                {
                    r = max;
                    g = min;
                    b = -(h - 360f) * dif / 60 + min;
                }
                else
                {
                    r = 0;
                    g = 0;
                    b = 0;
                }
            }

            return Color.FromArgb
                (
                    hsbColor.a,
                    (int)Math.Round(Math.Min(Math.Max(r, 0), 255)),
                    (int)Math.Round(Math.Min(Math.Max(g, 0), 255)),
                    (int)Math.Round(Math.Min(Math.Max(b, 0), 255))
                    );
        }

        public static HSBColor FromColor(Color color)
        {
            HSBColor ret = new HSBColor(0f, 0f, 0f);
            ret.a = color.A;

            float r = color.R;
            float g = color.G;
            float b = color.B;

            float max = Math.Max(r, Math.Max(g, b));

            if (max <= 0)
            {
                return ret;
            }

            float min = Math.Min(r, Math.Min(g, b));
            float dif = max - min;

            if (max > min)
            {
                if (g == max)
                {
                    ret.h = (b - r) / dif * 60f + 120f;
                }
                else if (b == max)
                {
                    ret.h = (r - g) / dif * 60f + 240f;
                }
                else if (b > g)
                {
                    ret.h = (g - b) / dif * 60f + 360f;
                }
                else
                {
                    ret.h = (g - b) / dif * 60f;
                }
                if (ret.h < 0)
                {
                    ret.h = ret.h + 360f;
                }
            }
            else
            {
                ret.h = 0;
            }

            ret.h *= 255f / 360f;
            ret.s = (dif / max) * 255f;
            ret.b = max;

            return ret;
        }

        public static Color FromColorTemperature(int temp)
        {
            var t = temp / 100;
    
            double red, green, blue;

            // Red:

            if (t <= 66)
            {
                red = 255;
            }
            else
            {
                red = t - 60;
                red = 329.698727446 * Math.Pow(red, -0.1332047592);
                if (red < 0) { red = 0; }
                if (red > 255) { red = 255; }
            }
    
            // Green:

            if (t <= 66)
            {
                green = t;
                green = 99.4708025861 * Math.Log(green) - 161.1195681661;
                if (green < 0) { green = 0; }
                if (green > 255) { green = 255; }
            }
            else
            {
                green = t - 60;
                green = 288.1221695283 * Math.Pow(green, -0.0755148492);
                if (green < 0) { green = 0; }
                if (green > 255) { green = 255; }
            }
    
            // Blue:

            if (t >= 66)
            {
                blue = 255;
            }
            else
            {
                if (t <= 19)
                {
                    blue = 0;
                }
                else
                {
                    blue = t - 10;
                    blue = 138.5177312231 * Math.Log(blue) - 305.0447927307;
                    if (blue < 0) { blue = 0; }
                    if (blue > 255) { blue = 255; }
                }
            }

            int r = (int)Math.Round(Math.Min(Math.Max(red, 0), 255));
            int g = (int)Math.Round(Math.Min(Math.Max(green, 0), 255));
            int b = (int)Math.Round(Math.Min(Math.Max(blue, 0), 255));

            return Color.FromArgb(r, g, b);
        }
    }
}