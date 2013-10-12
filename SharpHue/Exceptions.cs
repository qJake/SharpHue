using System;

namespace SharpHue
{
    /// <summary>
    /// Thrown when the response to a Bridge API call results in an error.
    /// Note: This exception is not thrown if there is a mixture of success and error responses.
    /// </summary>
    public class HueApiException : Exception
    {
        public int ErrorID { get; set; }
        public string RequestPath { get; set; }

        public HueApiException(string message, int ID, string path)
            : base(message)
        {
            ErrorID = ID;
            RequestPath = path;
        }
    }

    /// <summary>
    /// Thrown when the Hub discovery service cannot automatically locate a bridge device on your local network.
    /// </summary>
    public class HubIPAddressNotFoundException : Exception
    {
        private const string DEFAULT_MESSAGE = "The Hue Hub could not be found automatically on your network. Please re-initialize the configuration and specify the IP address manually.";

        public HubIPAddressNotFoundException() : base(DEFAULT_MESSAGE) { }
        public HubIPAddressNotFoundException(string message) : base(message) { }
        public HubIPAddressNotFoundException(Exception innerException) : base(DEFAULT_MESSAGE, innerException) { }
        public HubIPAddressNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }

    /// <summary>
    /// Thrown when the configuration is currently invalid, typically due to an API call requiring a username and/or a device IP.
    /// </summary>
    public class HueConfigurationException : Exception
    {
        public HueConfigurationException(string message) : base(message) { }
        public HueConfigurationException(string message, Exception innerException) : base(message, innerException) { }
    }

    /// <summary>
    /// Thrown when the light state builder is misconfigured.
    /// </summary>
    public class StateBuilderException : Exception
    {
        public StateBuilderException(string message) : base(message) { }
        public StateBuilderException(string message, Exception innerException) : base(message, innerException) { }
    }
}
