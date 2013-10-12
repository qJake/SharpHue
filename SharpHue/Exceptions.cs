using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpHue
{
    public class HueApiException : Exception
    {
        public int ErrorID {get;set;}
        public string RequestPath {get;set;}

        public HueApiException(string message, int ID, string path)
            : base(message)
        {
            ErrorID = ID;
            RequestPath = path;
        }
    }

    public class HubIPAddressNotFoundException : Exception
    {
        private const string DEFAULT_MESSAGE = "The Hue Hub could not be found automatically on your network. Please re-initialize the configuration and specify the IP address manually.";

        public HubIPAddressNotFoundException() : base(DEFAULT_MESSAGE) { }
        public HubIPAddressNotFoundException(string message) : base(message) { }
        public HubIPAddressNotFoundException(Exception innerException) : base(DEFAULT_MESSAGE, innerException) { }
        public HubIPAddressNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }
}
