using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Net;
using System.Net.Http;

namespace SharpHue
{
    static class JsonClient
    {
        public static JArray Request(HttpMethod method, string apiPath, object data)
        {
            return Request(method, apiPath, JsonConvert.SerializeObject(data));
        }

        public static JArray Request(HttpMethod method, string apiPath, string data)
        {
            Contract.Requires(Configuration.DeviceIP != null);

            string response = InvokeRequest(Configuration.DeviceIP, apiPath, data, method);
            JArray responseObject = JArray.Parse(response);
            if (responseObject.Count == 1)
            {
                JObject error = responseObject[0]["error"] as JObject;
                if (error != null)
                {
                    throw new HueApiException(error["description"].ToString(), error["type"].Value<int>(), error["address"].ToString());
                }
            }

            return responseObject;
        }

        public static JArray RequestBroker()
        {
            WebRequest req = WebRequest.Create(new Uri("https://www.meethue.com/api/nupnp"));
            var resp = req.GetResponse();
            var respReader = new StreamReader(resp.GetResponseStream());
            var data = respReader.ReadToEnd();

            return JArray.Parse(data);
        }

        private static string InvokeRequest(IPAddress baseUrl, string page, string data, HttpMethod method)
        {
            WebRequest req = WebRequest.Create(new Uri(new Uri("http://" + baseUrl.ToString() + "/"), page));
            req.Method = method.Method;

            var reqWriter = new StreamWriter(req.GetRequestStream());
            reqWriter.Write(data);
            reqWriter.Flush();

            var resp = req.GetResponse();
            var respReader = new StreamReader(resp.GetResponseStream());
            var response = respReader.ReadToEnd();

            return response;
        }
    }
}
