using System;

namespace SharpHue
{
    public static class MathEx
    {
        /// <summary>
        /// On a scale from <paramref name="min" /> to <paramref name="max" />, translates the
        /// given <paramref name="value" /> into its value if the scale were 
        /// <paramref name="desiredMin" /> to <paramref name="desiredMax" /> instead.
        /// </summary>
        /// <param name="value">The value to translate. Must be between min and max.</param>
        /// <param name="min">The minimum end of the current scale.</param>
        /// <param name="max">The maximum end of the current scale.</param>
        /// <param name="desiredMin">The desired scale's minimum value.</param>
        /// <param name="desiredMax">The desired scale's maximum value.</param>
        public static int TranslateValue(double value, double min, int max, double desiredMin, double desiredMax, bool reverseValue = false)
        {
            if (value > max || value < min)
            {
                throw new ArgumentException("Value must be between min and max.", "value");
            }
            if (min >= max)
            {
                throw new ArgumentException("Min must be less than max.", "min");
            }
            if (desiredMin >= desiredMax)
            {
                throw new ArgumentException("Desired min must be less than desired max.", "desiredMin");
            }

            double normalizedValue = (value - min) / (max - min);
            double result = ((desiredMax - desiredMin) * normalizedValue) + desiredMin;

            if (reverseValue)
            {
                result = ((desiredMax - desiredMin) - (result - desiredMin)) + desiredMin;
            }
            return (int)result;
        }
    }
}
