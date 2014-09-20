using System;
using System.IO;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SharpHue
{
    internal static class JsonClient
    {
        public static JToken Request(string apiPath)
        {
            return Request(HttpMethod.Get, apiPath, (string)null);
        }

        public static JToken Request(HttpMethod method, string apiPath)
        {
            return Request(method, apiPath, (string)null);
        }

        public static JToken Request(HttpMethod method, string apiPath, JObject data)
        {
            return Request(method, apiPath, data.ToString());
        }

        public static JToken Request(HttpMethod method, string apiPath, object data)
        {
            return Request(method, apiPath, JsonConvert.SerializeObject(data, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }));
        }

        public static JToken Request(HttpMethod method, string apiPath, string data)
        {
            if (Configuration.DeviceIP == null)
            {
                throw new HueConfigurationException("DeviceIP has not been initialized. Try calling Configuration.Initialize().");
            }

            string response = InvokeRequest(Configuration.DeviceIP, apiPath, data, method);
            JToken responseObject = JToken.Parse(response);

            // Check for a Hue error and throw an appropriate exception
            if (responseObject is JArray && ((JArray)responseObject).Count == 1)
            {
                JObject error = responseObject[0]["error"] as JObject;
                if (error != null)
                {
                    throw new HueApiException(error["description"].ToString(), error["type"].Value<int>(), error["address"].ToString());
                }
            }

            return responseObject;
        }

        public static JToken RequestSecure(string apiPath)
        {
            return Request(HttpMethod.Get, CreateAuthenticatedRequest(apiPath), (string)null);
        }

        public static JToken RequestSecure(HttpMethod method, string apiPath)
        {
            return Request(method, CreateAuthenticatedRequest(apiPath), (string)null);
        }

        public static JToken RequestSecure(HttpMethod method, string apiPath, JObject data)
        {
            return Request(method, CreateAuthenticatedRequest(apiPath), data.ToString());
        }

        public static JToken RequestSecure(HttpMethod method, string apiPath, object data)
        {
            return Request(method, CreateAuthenticatedRequest(apiPath), JsonConvert.SerializeObject(data, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }));
        }

        public static JToken RequestSecure(HttpMethod method, string apiPath, string data)
        {
            return Request(method, CreateAuthenticatedRequest(apiPath), data);
        }

        public static JToken RequestBroker()
        {
            WebRequest req = WebRequest.Create(new Uri("https://www.meethue.com/api/nupnp"));
            var resp = req.GetResponse();
            var respReader = new StreamReader(resp.GetResponseStream());
            var data = respReader.ReadToEnd();

            return JToken.Parse(data);
        }

        /// <summary>
        /// Builds an auth request by prepending /api/ and the username.
        /// </summary>
        /// <param name="apiPath">The path, after /api/{username}/.</param>
        /// <returns>The full API request string.</returns>
        internal static string CreateAuthenticatedRequest(string apiPath)
        {
            Configuration.RequireAuthentication();

            return "/api/" + Configuration.Username + (apiPath.StartsWith("/") ? "" : "/") + apiPath;
        }

        private static string InvokeRequest(IPAddress baseUrl, string page, string data, HttpMethod method)
        {
            WebRequest req = WebRequest.Create(new Uri(new Uri("http://" + baseUrl.ToString() + "/"), page));
            req.Method = method.Method;

            if (data != null)
            {
                var reqWriter = new StreamWriter(req.GetRequestStream());
                reqWriter.Write(data);
                reqWriter.Flush();
            }

            var resp = req.GetResponse();
            var respReader = new StreamReader(resp.GetResponseStream());
            var response = respReader.ReadToEnd();

            return response;
        }
    }
}
