namespace SharpHue
{
    /// <summary>
    /// The type of effect on a light.
    /// </summary>
    public enum LightEffect
    {
        /// <summary>
        /// No effect is running.
        /// </summary>
        None,

        /// <summary>
        /// The light cycles through all hues at the current brightness and saturation levels.
        /// </summary>
        ColorLoop
    }
}
