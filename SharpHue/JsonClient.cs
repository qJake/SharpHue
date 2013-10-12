using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SharpHue
{
    static class JsonClient
    {
        public static object Request(string apiPath, object data, HttpMethod method)
        {
            Contract.Requires(Configuration.DeviceIP != null);
            string request = JsonConvert.SerializeObject(data);

            string response = InvokeRequest(Configuration.DeviceIP, apiPath, request, method);

            return response;
        }

        public static object RequestBroker()
        {
            WebRequest req = WebRequest.Create(new Uri("https://www.meethue.com/api/nupnp"));
            var resp = req.GetResponse();
            var respReader = new StreamReader(resp.GetResponseStream());
            var data = respReader.ReadToEnd();

            return JsonConvert.DeserializeObject(data);
        }

        private static string InvokeRequest(IPAddress baseUrl, string page, string data, HttpMethod method)
        {
            WebRequest req = WebRequest.Create(new Uri(new Uri("http://" + baseUrl.ToString() + "/"), page));
            var resp = req.GetResponse();
            var respReader = new StreamReader(resp.GetResponseStream());
            var response = respReader.ReadToEnd();
            return response;
        }
    }
}
