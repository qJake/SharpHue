using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SharpHue
{
    public class LightCollection : IReadOnlyCollection<Light>
    {
        private List<Light> Lights { get; set; }

        public LightCollection()
        {
            Contract.Requires(Configuration.Username != null);
            Contract.Requires(Configuration.DeviceIP != null);

            Lights = new List<Light>();

            Refresh();
        }

        public void Refresh()
        {
            JObject lights = JsonClient.Request(HttpMethod.Get, Configuration.GetAuthRequest("/lights")) as JObject;

            for (int i = 1; i <= Configuration.MAX_LIGHTS; i++)
            {
                if (lights[i.ToString()] != null)
                {
                    Light l = ((JObject)lights[i.ToString()]).ToObject<Light>();
                    l.ID = i;
                    Lights.Add(l);
                }
                else
                {
                    // Lights are sequential, break if we don't have a light with the specified index.
                    break;
                }
            }
        }

        #region ICollection<T> Implementation

        public int Count
        {
	        get { return Lights.Count; }
        }

        public IEnumerator<Light> GetEnumerator()
        {
 	        return Lights.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
 	        return Lights.GetEnumerator();
        }

        #endregion

    }
}